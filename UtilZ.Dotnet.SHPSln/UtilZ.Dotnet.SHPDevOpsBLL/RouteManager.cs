using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.AsynWait;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Commands.Service;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;
using UtilZ.Dotnet.SHPDevOpsDAL;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    public class RouteManager : IDisposable
    {
        private readonly BindingCollection<DataRouteInfo> _dataRouteInfoList;
        public BindingCollection<DataRouteInfo> DataRouteInfoList
        {
            get { return _dataRouteInfoList; }
        }

        private readonly BindingCollection<SHPServiceInsInfo> _serviceInsList;
        private readonly object _serviceInsListLock = new object();
        public BindingCollection<SHPServiceInsInfo> ServiceInsList
        {
            get { return _serviceInsList; }
        }

        private readonly ICommandSender _commandSender;
        private readonly HostStatusInfoManager _hostStatusInfoManager;
        private readonly object _createServiceRouteIdLock = new object();
        private readonly RouteDAL _dal;

        public RouteManager(ICommandSender sender, HostStatusInfoManager hostStatusInfoManager, ICollectionOwner owner)
        {
            this._commandSender = sender;
            this._hostStatusInfoManager = hostStatusInfoManager;

            this._dataRouteInfoList = new BindingCollection<DataRouteInfo>(owner);
            this._serviceInsList = new BindingCollection<SHPServiceInsInfo>(owner);

            this._dal = new RouteDAL();
            this._deleteServiceInsInfoThread = new ThreadEx(this.DeleteServiceInsInfoThreadMethod, "删除服务实例及服务实例线程", true);

            this.RegisteServiceInsStatusChangeNotifyCallback();
        }

        #region 服务实例状态相关远程调用
        private void RegisteServiceInsStatusChangeNotifyCallback()
        {
            LRPCCore.CreateChannelA(DevOpsConstant.LRPC_SERVICE_INS_OFF_LINE_CHANNEL, this.ServiceInsOffLineNotify);
            LRPCCore.CreateChannelA(DevOpsConstant.LRPC_SERVICE_INS_ON_LINE_CHANNEL, this.ServiceInsOnLineNotify);
            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_GET_SERVICE_INS_INFO_CHANNEL, this.GetServiceInsInfo);
            LRPCCore.CreateChannelA(DevOpsConstant.LRPC_DELETE_SERVICE_INS_CHANNEL, this.DeleteServiceIns);
            LRPCCore.CreateChannelA(DevOpsConstant.LRPC_ADD_SERVICE_INS_CHANNEL, this.AddServiceIns);
        }

        private void AddServiceIns(object obj)
        {
            ServiceInfo serviceInfo = (ServiceInfo)LRPCCore.RemoteCallF(DevOpsConstant.LRPC_GET_SERVICE_INFO_CHANNEL, obj);
            if (serviceInfo == null)
            {
                throw new InvalidOperationException($"服务ID[{obj}]对应的服务不存在");
            }

            this.DeployServiceIns(serviceInfo);
        }

        private void DeleteServiceIns(object obj)
        {
            long serviceInsId = (long)obj;
            this.PrimitiveDeleteServiceIns(t => { return t.Id == serviceInsId; });
        }

        private object GetServiceInsInfo(object obj)
        {
            long serviceInsId = (long)obj;
            lock (this._serviceInsList.SyncRoot)
            {
                return this._serviceInsList.Where(t => { return t.Id == serviceInsId; }).FirstOrDefault();
            }
        }

        private void ServiceInsOnLineNotify(object obj)
        {
            var statusInfo = (ServiceStatusInfo)obj;
            lock (this._serviceInsList.SyncRoot)
            {
                var serviceInsInfo = this._serviceInsList.Where(t => { return t.Id == statusInfo.Id; }).FirstOrDefault();
                if (serviceInsInfo != null)
                {
                    serviceInsInfo.Status = SHPBase.Model.ServiceInsStatus.OnLine;
                }
            }
        }

        private void ServiceInsOffLineNotify(object obj)
        {
            var offLineServiceInsIdList = (List<long>)obj;
            lock (this._serviceInsList.SyncRoot)
            {
                var serviceInsInfoArr = this._serviceInsList.Where(t => { return offLineServiceInsIdList.Contains(t.Id); }).ToArray();
                foreach (var serviceInsInfo in serviceInsInfoArr)
                {
                    serviceInsInfo.Status = SHPBase.Model.ServiceInsStatus.OffLine;
                }
            }
        }
        #endregion

        private Hashtable _htDestinationConsistentServiceRoute = Hashtable.Synchronized(new Hashtable());
        internal ServiceRouteInfo GetServiceRouteInfo(GetServiceRoutePara para)
        {
            ServiceRouteInfo serviceRoute;
            if (string.IsNullOrWhiteSpace(para.DestinationConsistentKey))
            {
                serviceRoute = this.GetServiceRoute(para);
            }
            else
            {
                serviceRoute = this.GetDestinationConsistentServiceRoute(para);
            }

            return serviceRoute;
        }

        private ServiceRouteInfo GetDestinationConsistentServiceRoute(GetServiceRoutePara para)
        {
            string hashKey = $"DestinationConsistentServiceRoute_{para.DataCode}_{para.DestinationConsistentKey}";

            var destinationConsistentServiceRoute = this._htDestinationConsistentServiceRoute[hashKey] as DestinationConsistentServiceRoute;
            if (destinationConsistentServiceRoute == null)
            {
                lock (this._htDestinationConsistentServiceRoute.SyncRoot)
                {
                    destinationConsistentServiceRoute = this._htDestinationConsistentServiceRoute[hashKey] as DestinationConsistentServiceRoute;
                    if (destinationConsistentServiceRoute == null)
                    {
                        var serviceRoute = this.GetServiceRoute(para);
                        destinationConsistentServiceRoute = new DestinationConsistentServiceRoute(para.Count, serviceRoute, hashKey);
                        this._htDestinationConsistentServiceRoute[hashKey] = destinationConsistentServiceRoute;
                        MemoryCacheEx.Set(hashKey, destinationConsistentServiceRoute, para.MillisecondsTimeout, this.DestinationConsistentServiceRouteCacheEntryRemovedCallback);
                    }
                }
            }

            if (destinationConsistentServiceRoute.Increment())
            {
                this._htDestinationConsistentServiceRoute.Remove(hashKey);
                MemoryCacheEx.Remove(hashKey);
            }

            return destinationConsistentServiceRoute.ServiceRoute;
        }
        private void DestinationConsistentServiceRouteCacheEntryRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            try
            {
                if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
                {
                    var destinationConsistentServiceRoute = arguments.CacheItem.Value as DestinationConsistentServiceRoute;
                    if (destinationConsistentServiceRoute != null)
                    {
                        this._htDestinationConsistentServiceRoute.Remove(destinationConsistentServiceRoute.HashKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "目的地一致性服务路由过期处理异常");
            }
        }

        /// <summary>
        /// 获取最佳服务路由
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="denyServiceRouteIdList"></param>
        /// <returns></returns>
        private ServiceRouteInfo GetServiceRoute(GetServiceRoutePara para)
        {
            var dataRouteInfo = this._dataRouteInfoList.Where(t => { return t.DataCode == para.DataCode; }).FirstOrDefault();
            if (dataRouteInfo == null)
            {
                throw new InvalidOperationException($"不存在数据编码[{para.DataCode}]的数据路由");
            }

            ServiceInfo serviceInfo = dataRouteInfo.GetServiceInfo();
            if (serviceInfo == null)
            {
                throw new Exception($"数据编码[{dataRouteInfo.DataCode}]对应的服务不存在");
            }

            SHPServiceInsInfo shpServiceInsInfo = this.GetOptimumServiceIns(para, dataRouteInfo, serviceInfo);
            var hostInfo = this._hostStatusInfoManager.GetHostInfoByHostId(shpServiceInsInfo.HostId);
            return new ServiceRouteInfo(shpServiceInsInfo.Id, hostInfo.Ip, shpServiceInsInfo.EndPointPort, serviceInfo.TransferProtocal);
        }

        /// <summary>
        /// 获取最佳服务实例
        /// </summary>
        /// <param name="para"></param>
        /// <param name="dataRouteInfo"></param>
        /// <param name="serviceInfo"></param>
        /// <returns></returns>
        private SHPServiceInsInfo GetOptimumServiceIns(GetServiceRoutePara para, DataRouteInfo dataRouteInfo, ServiceInfo serviceInfo)
        {
            SHPServiceInsInfo[] shpServiceInsInfoArr = null;
            lock (this._serviceInsListLock)
            {
                //选择同数据路由且在线的服务实例
                shpServiceInsInfoArr = this._serviceInsList.
                    Where(t => { return t.DataRouteIdList.Contains(dataRouteInfo.Id); }).
                    OrderBy(s => { return s.PayloadValue; }).
                    ToArray();
            }

            int deployedServiceInsCount = shpServiceInsInfoArr.Length;
            SHPServiceInsInfo shpServiceInsInfo;
            if (deployedServiceInsCount == 0)
            {
                //没有部署过服务,部署服务
                shpServiceInsInfo = this.DeployServiceIns(serviceInfo);
            }
            else
            {
                var onLineServiceRouteInfoArr = shpServiceInsInfoArr.
                    Where(t => { return t.Status == SHPBase.Model.ServiceInsStatus.OnLine; }).
                    OrderBy(s => { return s.PayloadValue; }).ToArray();
                if (onLineServiceRouteInfoArr.Length == 0)
                {
                    if (serviceInfo.AllowDeployNewServiceIns(deployedServiceInsCount))
                    {
                        //没有可用的服务实例,新部署服务实例
                        shpServiceInsInfo = this.DeployServiceIns(serviceInfo);
                    }
                    else
                    {
                        throw new InvalidOperationException($"没有可用的服务实例且部署的服务实例个数已达到上限");
                    }
                }
                else
                {
                    //选择同数据路由且在线的服务实例
                    shpServiceInsInfo = this.GetServiceInsInfoByServicePayloadInfo(onLineServiceRouteInfoArr,
                        para.SendFailServiceInsIdList, serviceInfo, deployedServiceInsCount);
                }
            }

            return shpServiceInsInfo;
        }

        public void DeployServiceIns(ServiceInfo serviceInfo, int serviceInsCount, IPartAsynWait asynWait)
        {
            var serviceInsList = new List<SHPServiceInsInfo>();
            for (int i = 0; i < serviceInsCount; i++)
            {
                if (asynWait.IsCanceled)
                {
                    Loger.Warn($"正在部署第[{i + 1}/{serviceInsCount}]个服务实例时取消操作");
                    break;
                }

                asynWait.Hint = $"正在部署第[{i + 1}/{serviceInsCount}]个服务实例";
                this.DeployServiceIns(serviceInfo);
            }

            if (asynWait.IsCanceled)
            {
                for (int i = 0; i < serviceInsList.Count; i++)
                {
                    asynWait.Hint = $"正在移除第[{i + 1}/{serviceInsList.Count}]个服务实例";
                    this.DeleteServiceIns(serviceInsList[i]);
                }
            }
        }

        private SHPServiceInsInfo DeployServiceIns(ServiceInfo serviceInfo)
        {
            try
            {
                var shpServiceInsInfo = this.PrimitiveDeployServiceIns(serviceInfo);
                lock (this._serviceInsListLock)
                {
                    this._dal.AddServiceIns(shpServiceInsInfo);
                    this._serviceInsList.Add(shpServiceInsInfo);
                }

                return shpServiceInsInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("部署服务实例发生异常", ex);
            }
        }

        private SHPServiceInsInfo PrimitiveDeployServiceIns(ServiceInfo serviceInfo)
        {
            long id;//服务实例id,同一运控下不可重复,所以加锁创建
            lock (this._createServiceRouteIdLock)
            {
                id = TimeEx.GetTimestamp();
            }

            var config = Config.Instance;
            string serviceInsName = $"{serviceInfo.Name}.{config.DevOpsId}.{id}";

            Loger.Info($"部署服务实例[{serviceInsName}]开始...");
            ServiceMirrorInfo mirrorInfo = serviceInfo.GetServiceMirrorInfo();
            if (mirrorInfo == null)
            {
                throw new InvalidOperationException($"服务实例[{serviceInsName}]所属服务[{serviceInfo.Name}]未上传服务镜像");
            }

            HostInfo hostInfo = this._hostStatusInfoManager.GetDeployServiceHostInfoByHostTypeId(serviceInfo);
            var serviceDeployPara = this.CreateServiceDeployPara(config, id, serviceInsName,
                serviceInfo, mirrorInfo, mirrorInfo.DeployMillisecondsTimeout, hostInfo);
            var cmd = new ServiceDeployReqCommand(serviceDeployPara);

            byte[] data = this._commandSender.SendInteractiveCommandHost<ServiceDeployReqCommand>(cmd, hostInfo.Ip, mirrorInfo.DeployMillisecondsTimeout);
            var serviceListenInfo = SerializeEx.BinaryDeserialize<SHPBase.Model.ServiceListenInfo>(data);

            Loger.Info($"创建服务实例[{serviceInsName}]开始...");
            var shpServiceInsInfo = new SHPServiceInsInfo();
            shpServiceInsInfo.Id = serviceListenInfo.Id;

            lock (this._dataRouteInfoList.SyncRoot)
            {
                var dataRouteIdArr = (from t in this._dataRouteInfoList where t.ServiceInfoId == serviceInfo.Id select t.Id).ToArray();
                shpServiceInsInfo.DataRouteIdList.AddRange(dataRouteIdArr);
            }

            shpServiceInsInfo.Name = serviceInsName;
            shpServiceInsInfo.ServiceId = serviceInfo.Id;
            shpServiceInsInfo.HostId = hostInfo.Id;
            shpServiceInsInfo.EndPointPort = serviceListenInfo.ServiceInsListenPort;
            Loger.Info($"部署服务实例[{serviceInsName}]完成...");
            return shpServiceInsInfo;
        }

        private ServiceDeployPara CreateServiceDeployPara(Config config, long id, string serviceInsName,
            ServiceInfo serviceInfo, ServiceMirrorInfo mirrorInfo, int millisecondsTimeout, HostInfo hostInfo)
        {
            var serviceDeployPara = new ServiceDeployPara();
            serviceDeployPara.DOId = config.DevOpsId;
            serviceDeployPara.Id = id;
            serviceDeployPara.ServiceInsName = serviceInsName;
            serviceDeployPara.TransferProtocal = serviceInfo.TransferProtocal;
            serviceDeployPara.ServiceMirrorType = mirrorInfo.ServiceMirrorType;
            serviceDeployPara.Arguments = mirrorInfo.Arguments;
            serviceDeployPara.AppProcessFilePath = mirrorInfo.AppProcessFilePath;
            serviceDeployPara.AppExeFilePath = mirrorInfo.AppExeFilePath;
            serviceDeployPara.BaseUrl = config.ServiceMirrorFtpUrl;
            serviceDeployPara.MirrorFilePath = mirrorInfo.MirrorFilePath;
            serviceDeployPara.FileServiceUsername = config.FileServiceUserName;
            serviceDeployPara.FileServicePassword = config.FileServicePassword;
            serviceDeployPara.MillisecondsTimeout = millisecondsTimeout;
            serviceDeployPara.StatusUploadIntervalMilliseconds = config.StatusUploadIntervalMilliseconds;
            serviceDeployPara.RouteServiceUrl = config.ServiceRouteUrl;
            serviceDeployPara.NotAllocatePortList = hostInfo.HostDisablePortInfoList.Select(t => { return t.Port; }).ToList();
            return serviceDeployPara;
        }

        internal void ProServiceInsListenChangedNotify(ServiceListenInfoChangeNoifyCommand cmd)
        {
            lock (this._serviceInsListLock)
            {
                var serviceRouteInfo = this._serviceInsList.Where(t => { return t.Id == cmd.ServiceListenInfo.Id; }).FirstOrDefault();
                serviceRouteInfo.EndPointPort = cmd.ServiceListenInfo.ServiceInsListenPort;
                this._dal.UpdateServiceIns(serviceRouteInfo);
            }
        }

        #region 删除服务实例及服务实例
        private readonly List<SHPServiceInsInfo> _deleteServiceInsInfoList = new List<SHPServiceInsInfo>();
        private readonly object _deleteServiceInsInfoListLock = new object();
        private readonly ThreadEx _deleteServiceInsInfoThread;
        private readonly AutoResetEvent _deleteServiceInsInfoListEventHandler = new AutoResetEvent(false);

        private void DeleteServiceInsInfoThreadMethod(CancellationToken token)
        {
            const int millisecondsTimeout = 30000;//todo 服务停止超时时长 ,先写死

            while (!token.IsCancellationRequested)
            {
                try
                {
                    try
                    {
                        this._deleteServiceInsInfoListEventHandler.WaitOne(millisecondsTimeout);
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }

                    if (this._deleteServiceInsInfoList.Count == 0)
                    {
                        continue;
                    }

                    IEnumerable<IGrouping<long, SHPServiceInsInfo>> groups = this._deleteServiceInsInfoList.GroupBy(t => { return t.HostId; });
                    foreach (IGrouping<long, SHPServiceInsInfo> group in groups)
                    {
                        try
                        {
                            var idList = group.Select(t => { return t.Id; }).ToList();
                            var hostInfo = this._hostStatusInfoManager.GetHostInfoByHostId(group.Key);
                            var cmd = new ServiceInsDeleteReqCommand(Config.Instance.DevOpsId, millisecondsTimeout, idList);
                            this._commandSender.SendCommandHost<ServiceInsDeleteReqCommand>(cmd, hostInfo.Ip);
                        }
                        catch (Exception exi)
                        {
                            Loger.Error(exi);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        internal void DeleteServiceInsByServiceInfoId(long serviceInfoId)
        {
            this.PrimitiveDeleteServiceIns(t => { return t.ServiceId == serviceInfoId; });
        }

        internal void DeleteServiceInsByHostId(long hostId)
        {
            this.PrimitiveDeleteServiceIns(t => { return t.HostId == hostId; });
        }

        private void PrimitiveDeleteServiceIns(Func<SHPServiceInsInfo, bool> predicate)
        {
            SHPServiceInsInfo[] serviceRouteInfoArr;
            lock (this._serviceInsListLock)
            {
                serviceRouteInfoArr = this._serviceInsList.Where(predicate).ToArray();
                this._serviceInsList.RemoveArrange(serviceRouteInfoArr);
            }

            lock (this._deleteServiceInsInfoListLock)
            {
                this._dal.AddDeleteServiceInsInfo(serviceRouteInfoArr);
                this._deleteServiceInsInfoList.AddRange(serviceRouteInfoArr);
                this._deleteServiceInsInfoListEventHandler.Set();
            }
        }

        internal void ProServiceInsRemoveResCommand(ServiceInsDeleteResCommand cmd)
        {
            try
            {
                var resPara = cmd.ResPara;
                if (resPara.Result)
                {
                    lock (this._deleteServiceInsInfoListLock)
                    {
                        this._deleteServiceInsInfoList.RemoveAll((t) => { return resPara.IdList.Contains(t.Id); });
                    }

                    this._dal.RemoveDeleteServiceIns(resPara.IdList);
                }
                else
                {
                    Loger.Warn(resPara.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "处理删除服务响应异常");
            }
        }

        public void DeleteServiceIns(SHPServiceInsInfo serviceInsInfo)
        {
            this.PrimitiveDeleteServiceIns(t => { return t.Id == serviceInsInfo.Id; });
        }
        #endregion

        private SHPServiceInsInfo GetServiceInsInfoByServicePayloadInfo(SHPServiceInsInfo[] serviceRouteInfoArr,
            List<long> sendFailServiceInsIdList, ServiceInfo serviceInfo, int deployedServiceInsCount)
        {
            SHPServiceInsInfo serviceInsInfo;
            if (sendFailServiceInsIdList != null && sendFailServiceInsIdList.Count > 0)
            {
                //未发送失败的服务实例数据
                var noSendFailSerivceInsArr = serviceRouteInfoArr.Where(t => { return !sendFailServiceInsIdList.Contains(t.Id); }).ToArray();
                if (noSendFailSerivceInsArr.Length < 1)
                {
                    //当前已部署的所有服务实例全部都拒绝提供服务
                    if (serviceInfo.AllowDeployNewServiceIns(deployedServiceInsCount))
                    {
                        //全都被发送失败,且可部署的服务实例数未达到上限,部署新的服务实例
                        serviceInsInfo = this.DeployServiceIns(serviceInfo);
                    }
                    else
                    {
                        //全都被发送失败,且可部署的服务实例数达到上限,硬着头皮选择负载最低的服务实例,进行死磕
                        serviceInsInfo = serviceRouteInfoArr[0];
                    }
                }
                else
                {
                    //已按服务负载升序排列,所以选择第一项(负载最低)实例即可
                    serviceInsInfo = noSendFailSerivceInsArr[0];
                }
            }
            else
            {
                //没有服务实例拒绝过服务,选择负载最低的服务实例
                serviceInsInfo = serviceRouteInfoArr[0];
            }

            return serviceInsInfo;
        }

        public void Start()
        {
            Loger.Info("正在启动路由服务...");
            this._dataRouteInfoList.AddRange(this._dal.QueryAllDataRoute());
            lock (this._serviceInsListLock)
            {
                this._serviceInsList.AddRange(this._dal.QueryAllServiceInsInfo());
            }

            lock (this._deleteServiceInsInfoListLock)
            {
                this._deleteServiceInsInfoList.AddRange(this._dal.QueryAllDeleteServiceInsInfo());
            }

            this._deleteServiceInsInfoThread.Start();
            Loger.Info("启动路由服务成功...");
        }

        #region 数据路由
        public void AddDataRoute(DataRouteInfo dataRouteItem)
        {
            this._dal.AddDataRoute(dataRouteItem);
            this._dataRouteInfoList.Add(dataRouteItem);
        }

        public void RemoveDataRoute(DataRouteInfo dataRouteItem)
        {
            lock (this._serviceInsListLock)
            {
                foreach (var serviceIns in this._serviceInsList)
                {
                    if (serviceIns.DataRouteIdList.Contains(dataRouteItem.Id))
                    {
                        serviceIns.DataRouteIdList.Remove(dataRouteItem.Id);
                    }
                }

                this._dal.RemoveDataRoute(dataRouteItem);
                this._dataRouteInfoList.Remove(dataRouteItem);
            }
        }

        public void ModifyDataRoute(DataRouteInfo dataRouteItem)
        {
            this._dal.ModifyDataRoute(dataRouteItem);
        }

        public void ClearDataRoute()
        {
            lock (this._serviceInsListLock)
            {
                foreach (var serviceIns in this._serviceInsList)
                {
                    serviceIns.DataRouteIdList.Clear();
                }

                this._dal.ClearDataRoute();
                this._dataRouteInfoList.Clear();
            }
        }

        public bool MonitorAppIsSerivice(HostInfo hostInfo, AppMonitorItem appMonitorItem)
        {
            lock (this._serviceInsListLock)
            {
                return this._serviceInsList.Where(t => { return t.HostId == hostInfo.Id && AppMonitorHelper.Equals(t, appMonitorItem); }).Count() > 0;
            }
        }
        #endregion

        public void Dispose()
        {
            try
            {
                Loger.Info("正在退出路由服务...");
                this._deleteServiceInsInfoThread.Stop();
                this._deleteServiceInsInfoThread.Dispose();
                this._deleteServiceInsInfoListEventHandler.Dispose();
                Loger.Info("退出路由服务成功...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "退出路由服务异常");
            }
        }
    }
}
