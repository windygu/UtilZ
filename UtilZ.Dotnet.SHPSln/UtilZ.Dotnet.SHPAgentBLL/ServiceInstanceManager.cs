using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.FileTransfer;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPAgentDAL;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Commands.Service;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Command;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    internal class ServiceInstanceManager : IDisposable
    {
        public readonly AgentDAL _dal;
        private readonly IAppMonitorManagement _appMonitorItemManager;
        private readonly DevOpsInfoCollection _devOpsInfoCollection;
        private readonly TransferChannel _net;
        private readonly string _serviceDeployBaseDir;

        /// <summary>
        /// 服务部署器列表
        /// </summary>
        private readonly List<ServiceDeployer> _serviceDeployerList = new List<ServiceDeployer>();

        /// <summary>
        /// 服务实例信息列表
        /// </summary>
        private readonly List<SHPAgentServiceInsInfo> _serviceInsInfoList = new List<SHPAgentServiceInsInfo>();
        private readonly object _lock = new object();

        private readonly Dictionary<string, ServiceStatusInfo> _serviceInsStatusDic = new Dictionary<string, ServiceStatusInfo>();
        private readonly object _serviceInsStatusDicLock = new object();

        public ServiceInstanceManager(AgentDAL dal, IAppMonitorManagement appMonitorItemManager,
            DevOpsInfoCollection devOpsInfoCollection, TransferChannel net)
        {
            this._dal = dal;
            this._appMonitorItemManager = appMonitorItemManager;
            this._devOpsInfoCollection = devOpsInfoCollection;
            this._net = net;

            this._serviceDeployBaseDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Services");

            //还原数据
            this._serviceInsInfoList.AddRange(this._dal.QueryAllServiceInsInfo());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doId">运控id</param>
        /// <param name="id">服务实例Id,服务路由id</param>
        /// <returns></returns>
        private string CreateServiceInsStatusKey(long doId, long id)
        {
            return $"{doId}_{id}";
        }

        internal int UploadServiceStatus(ServiceStatusInfo serviceStatus)
        {
            try
            {
                lock (this._serviceInsStatusDicLock)
                {
                    //存储服务实例状态
                    string key = this.CreateServiceInsStatusKey(serviceStatus.DOID, serviceStatus.Id);
                    this._serviceInsStatusDic[key] = serviceStatus;
                }

                //获取最新的上报间隔
                var serviceInsInfo = this._serviceInsInfoList.Where(t => { return t.DOID == serviceStatus.DOID && t.Id == serviceStatus.Id; }).FirstOrDefault();
                if (serviceInsInfo != null)
                {
                    return serviceInsInfo.StatusUploadIntervalMilliseconds;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            return -1;
        }

        internal void UploadServiceInsListenInfo(ServiceInsListenInfo para)
        {
            try
            {
                lock (this._lock)
                {
                    //更新监听参数
                    var serviceInsInfo = this._serviceInsInfoList.Where(t => { return t.DOID == para.DOID && t.Id == para.Id; }).FirstOrDefault();
                    if (serviceInsInfo != null)
                    {
                        serviceInsInfo.Update(para);
                        this._dal.UpdateServiceInsInfoListenPort(serviceInsInfo);
                    }
                    else
                    {
                        throw new ApplicationException($"服务实例不存在");
                    }

                    var serviceDeployer = this._serviceDeployerList.Where(t => { return t.ServiceDeployPara.DOId == para.DOID && t.ServiceDeployPara.Id == para.Id; }).FirstOrDefault();
                    if (serviceDeployer == null)
                    {
                        //上报运控服务实例监听信息变更(服务重启)
                        var devOpsInfo = this._devOpsInfoCollection.Where(t => { return t.Id == para.DOID; }).FirstOrDefault();
                        if (devOpsInfo != null)
                        {
                            var serviceListenInfoChangeNoifyCommand = new ServiceListenInfoChangeNoifyCommand(new ServiceListenInfo(para.Id, para.ListenPort));
                            var resTransferCommand = new SHPTransferCommand(SHPTransferCommand.DefaultContextId, SHPConstant.SHP_PLUGINID, serviceListenInfoChangeNoifyCommand, Config.Instance.HostStatusUploadTimeout);
                            var buffer = resTransferCommand.GenerateBuffer();
                            var policy = TransferPolicyManager.Instance.GetTransferPolicy(devOpsInfo.DevOpsIPEndPoint);
                            this._net.SendData(buffer, policy);
                        }
                    }
                    else
                    {
                        //通知服务部署器,服务实例启动完成(服务部署第一次启动)
                        serviceDeployer.UploadServiceInsListenInfo(para);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        internal SHPBasicServiceInfo QueryServiceInsInfo(GetServiceInsInfoPara para)
        {
            string filePath = para.FilePath;
            SHPBasicServiceInfo shpBasicServiceInfo;
            lock (this._lock)
            {
                //根据文件路径查找
                shpBasicServiceInfo = this._serviceInsInfoList.Where(t => { return string.Equals(t.FilePath, filePath, StringComparison.OrdinalIgnoreCase); }).FirstOrDefault();
                if (shpBasicServiceInfo != null)
                {
                    return shpBasicServiceInfo;
                }

                var serviceDeployer = this._serviceDeployerList.Where(t => { return t.IsDeploying(filePath); }).FirstOrDefault();
                if (serviceDeployer == null)
                {
                    string err = $"内部逻辑错误,服务实例启动程序[{filePath}]对应的服务实例信息不存在,部署器也不存在";
                    Loger.Error(err);
                    throw new InvalidOperationException(err);
                }

                ServiceDeployPara serviceDeployPara = serviceDeployer.ServiceDeployPara;
                shpBasicServiceInfo = new SHPAgentServiceInsInfo();
                shpBasicServiceInfo.Id = serviceDeployPara.Id;//服务实例id,服务路由id
                shpBasicServiceInfo.DOID = serviceDeployPara.DOId;//运控id
                shpBasicServiceInfo.TransferProtocal = serviceDeployPara.TransferProtocal;
                shpBasicServiceInfo.HostNotAllocatePortList = serviceDeployPara.NotAllocatePortList;
                shpBasicServiceInfo.RouteServiceUrl = serviceDeployPara.RouteServiceUrl;// @"http://192.168.10.96:20001/"
                shpBasicServiceInfo.StatusUploadIntervalMilliseconds = serviceDeployPara.StatusUploadIntervalMilliseconds;
                shpBasicServiceInfo.ServiceInsName = serviceDeployer.ServiceInsName;

                var serviceInsInfo = new SHPAgentServiceInsInfo(shpBasicServiceInfo);
                serviceInsInfo.FilePath = filePath;
                this._dal.AddServiceInsInfo(serviceInsInfo);
                this._serviceInsInfoList.Add(serviceInsInfo);

                return shpBasicServiceInfo;
            }
        }

        internal ServiceListenInfo DeployServiceInstance(ServiceDeployPara serviceDeployPara)
        {
            using (var serviceDeployer = new ServiceDeployer(this._serviceDeployBaseDir, serviceDeployPara, this._appMonitorItemManager))
            {
                lock (this._lock)
                {
                    this._serviceDeployerList.Add(serviceDeployer);
                    Loger.Info($"服务[{serviceDeployPara.ServiceInsName}]部署器添加到列表中...");
                }

                try
                {
                    return serviceDeployer.Deploy();
                }
                finally
                {
                    lock (this._lock)
                    {
                        this._serviceDeployerList.Remove(serviceDeployer);
                        Loger.Info($"服务[{serviceDeployPara.ServiceInsName}]部署器从列表中中移除...");
                    }
                }
            }
        }

        internal List<ServiceStatusInfo> GetServiceStatusInfoListByDOID(long doId)
        {
            lock (this._serviceInsStatusDicLock)
            {
                return this._serviceInsStatusDic.Values.Where(t => { return t.DOID == doId; }).ToList();
            }
        }

        internal ServiceRouteRemoveResPara ProRemoveServiceInstance(ServiceInsDeleteReqCommand cmd)
        {
            var doId = cmd.DOID;
            var idList = cmd.IdList;

            //服务实例列表删除
            lock (this._lock)
            {
                //找出从删除服务实例
                var deleteServiceInsArr = this._serviceInsInfoList.Where(t => { return t.DOID == doId && idList.Contains(t.Id); }).ToArray();

                //移除监视,停止服务,删除服务目录
                lock (this._appMonitorItemManager.AppMonitorListLock)
                {
                    //服务实例列表中移除
                    foreach (var deleteServiceIns in deleteServiceInsArr)
                    {
                        try
                        {
                            var deleteMonitorItem = this._appMonitorItemManager.AppMonitorList.Where(t => { return t.ServiceInsInfoIsMonitorItem(deleteServiceIns); }).FirstOrDefault();
                            if (deleteMonitorItem != null)
                            {
                                //移除并停止监视
                                this._appMonitorItemManager.RemoveMonitorItem(deleteMonitorItem);

                                //通知服务停止
                                this.NotifyServiceClose(cmd, deleteServiceIns);

                                //停止应用
                                deleteMonitorItem.StopApp();

                                //删除服务目录
                                DirectoryInfo dirInfo = Directory.GetParent(deleteMonitorItem.AppExeFilePath);
                                try
                                {
                                    dirInfo.Delete(true);
                                    Loger.Info($"删除服务实例ID[{deleteServiceIns.Id}]的目录成功");
                                }
                                catch (Exception exi)
                                {
                                    Loger.Error(exi, $"删除服务实例ID[{deleteServiceIns.Id}]的目录[{dirInfo.FullName}]发生异常");
                                }
                            }

                            this._serviceInsInfoList.Remove(deleteServiceIns);
                        }
                        catch (Exception ex)
                        {
                            Loger.Error(ex, $"删除服务实例ID[{deleteServiceIns.Id}]发生异常");
                        }
                    }
                }

                //服务实例数据删除
                this._dal.DeleteServiceInsInfo(doId, idList);
            }

            //服务实例状态删除
            foreach (var id in idList)
            {
                lock (this._serviceInsStatusDicLock)
                {
                    string key = this.CreateServiceInsStatusKey(doId, id);
                    if (this._serviceInsStatusDic.ContainsKey(key))
                    {
                        this._serviceInsStatusDic.Remove(key);
                    }
                }
            }

            return new ServiceRouteRemoveResPara(idList);
        }

        private void NotifyServiceClose(ServiceInsDeleteReqCommand cmd, SHPAgentServiceInsInfo deleteServiceIns)
        {
            string eventHandlerId = Guid.NewGuid().GetHashCode().ToString();
            var eventWaitHandleInfo = AutoEventWaitHandleManager.CreateEventWaitHandle(eventHandlerId, cmd.MillisecondsTimeout * 2);
            try
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        string url = AgentServiceMethodNameConstant.GetServiceRestfullServiceUrl(deleteServiceIns.ServiceRestfullServiceListenPort) + AgentServiceMethodNameConstant.CLOSE_SERVICE_NOTIFY + eventHandlerId;
                        string json = RestFullClientHelper.Get(url);
                        var apiResult = SerializeEx.WebScriptJsonDeserializeObject<ApiResult>(json);
                        if (apiResult.Status == ApiResultStatus.Succes)
                        {
                            try
                            {
                                var eventHandler = AutoEventWaitHandleManager.GetEventWaitHandleInfo(apiResult.Data);
                                if (eventHandler != null)
                                {
                                    eventHandler.Set();
                                }
                            }
                            catch (ObjectDisposedException)
                            { }
                        }
                    }
                    catch (Exception)
                    { }
                });

                eventWaitHandleInfo.EventWaitHandle.WaitOne(cmd.MillisecondsTimeout);
            }
            finally
            {
                eventWaitHandleInfo.EventWaitHandle.Dispose();
                AutoEventWaitHandleManager.RemoveEventWaitHandle(eventHandlerId);
            }
        }

        internal bool MonitorIsServiceIns(AppMonitorItem appMonitorItem)
        {
            lock (this._lock)
            {
                var arr = this._serviceInsInfoList.Where(t => { return appMonitorItem.ServiceInsInfoIsMonitorItem(t); }).ToArray();
                return arr.Length > 0;
            }
        }

        public void Dispose()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
