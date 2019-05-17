using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Commands.AppControl;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPDevOpsDAL;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    public class HostManager : IDisposable
    {
        private readonly ICommandSender _sender;
        private readonly SHPPluginManager<ISHPDDevOps, ISHPHardDisplay> _pluginManager;
        private readonly RouteManager _routeManager;
        private readonly HostStatusInfoManager _hostStatusInfoManager;

        private readonly HostManagerDAL _dal = new HostManagerDAL();

        private readonly ConcurrentDictionary<long, HostInfo> _hostInfoDic = new ConcurrentDictionary<long, HostInfo>();
        private readonly object _hostInfoDicLock = new object();

        private readonly Dictionary<long, HostTypeItem> _hostTypeDic = new Dictionary<long, HostTypeItem>();
        private readonly BindingCollection<HostTypeItem> _hostTypeList;
        public BindingCollection<HostTypeItem> HostTypeList
        {
            get { return _hostTypeList; }
        }

        private readonly List<HostInfo> _getHostHardInfoList = new List<HostInfo>();
        private readonly object _getHostHardInfoListLock = new object();
        private readonly AutoResetEvent _getHostHardInfoListEventHandle = new AutoResetEvent(false);
        private readonly ThreadEx _getHostHardInfoThread;


        internal HostManager(ICommandSender sender, SHPPluginManager<ISHPDDevOps, ISHPHardDisplay> pluginManager,
            RouteManager routeManager, HostStatusInfoManager hostStatusInfoManager, ICollectionOwner owner)
        {
            this._sender = sender;
            this._pluginManager = pluginManager;
            this._routeManager = routeManager;
            this._hostStatusInfoManager = hostStatusInfoManager;
            this._dal = new SHPDevOpsDAL.HostManagerDAL();
            this._hostTypeList = new BindingCollection<HostTypeItem>(owner);

            this._getHostHardInfoThread = new ThreadEx(this.GetHostHardInfoThreadMethod, "获取主机硬件信息线程", true);

            ServiceInfo.GetHostTypeById = this.GetHostTypeById;
            SHPServiceInsInfo.GetHostInfoByIdFunc = this.GetHostInfo;
            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_HOST_ON_LINE_NOTIFY_CHANNEL, this.HostLine);
        }

        private object HostLine(object obj)
        {
            try
            {
                var hostInfo = (HostInfo)obj;
                this.AddToGetHostHardInfoList(hostInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            return null;
        }

        private HostInfo GetHostInfo(long hostId)
        {
            HostInfo hostInfo;
            this._hostInfoDic.TryGetValue(hostId, out hostInfo);
            return hostInfo;
        }

        private HostTypeItem GetHostTypeById(long hostTypeId)
        {
            if (this._hostTypeDic.ContainsKey(hostTypeId))
            {
                return this._hostTypeDic[hostTypeId];
            }

            return null;
        }

        private void GetHostHardInfoThreadMethod(CancellationToken token)
        {
            const int checkInterval = 60 * 1000;
            HostInfo[] hostInfos;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    this._getHostHardInfoListEventHandle.WaitOne(checkInterval);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    lock (this._getHostHardInfoListLock)
                    {
                        hostInfos = this._getHostHardInfoList.ToArray();
                    }

                    if (hostInfos.Length == 0)
                    {
                        continue;
                    }

                    foreach (var hostInfo in hostInfos)
                    {
                        this._sender.SendCommandHost(new EvnChangedNotifyCommand(Config.Instance.DevOpsId, hostInfo.Id, Config.Instance.StatusUploadIntervalMilliseconds), hostInfo.Ip);
                    }
                }
                catch (ObjectDisposedException)
                { }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        public void UpdateHostHardInfo(HostHardInfo hostHardInfo)
        {
            this._dal.UpdateHostHardInfo(hostHardInfo);
        }

        public void RemoveGetHostHardInfo(long hostId)
        {
            lock (this._getHostHardInfoListLock)
            {
                var removeItems = this._getHostHardInfoList.Where(t => { return t.Id == hostId; }).ToArray();
                foreach (var removeItem in removeItems)
                {
                    this._getHostHardInfoList.Remove(removeItem);
                }
            }
        }

        #region 主机管理
        public void AddHostGroup(HostGroup hostGroup)
        {
            this._dal.AddHostGroup(hostGroup);
        }

        public void ModifyHostGroup(HostGroup hostGroup)
        {
            this._dal.ModifyHostGroup(hostGroup);
        }

        public void DeleteHostGroup(HostGroup hostGroup)
        {
            this._dal.DeleteHostGroupById(hostGroup.Id);

            //通知ui
            object result;
            LRPCCore.TryRemoteCallF(DevOpsConstant.LRPC_HOST_HROUP_DELETE_CHANNEL, hostGroup, out result);
        }

        public void AddHost(HostInfo hostInfo)
        {
            this._dal.AddHost(hostInfo);
            this._hostStatusInfoManager.AddHostInfo(hostInfo);

            try
            {
                this.SendAddHostCommand(hostInfo);
            }
            catch
            {
                this._hostStatusInfoManager.RemoveHostInfo(hostInfo);
                this._dal.DeleteHost(hostInfo);
                throw;
            }

            lock (this._hostInfoDicLock)
            {
                this._hostInfoDic[hostInfo.Id] = hostInfo;
            }

            this.AddToGetHostHardInfoList(hostInfo);
        }

        private void SendAddHostCommand(HostInfo hostInfo)
        {
            var notAllocatePortList = hostInfo.HostDisablePortInfoList.Select(t => { return t.Port; }).ToList();
            byte[] data = this._sender.SendInteractiveCommandHost(new AddHostCommand(Config.Instance.DevOpsId, hostInfo.Id, notAllocatePortList),
                hostInfo.Ip, Config.Instance.SendCommandDefaultTimeout);
        }

        private void AddToGetHostHardInfoList(HostInfo hostInfo)
        {
            lock (this._getHostHardInfoListLock)
            {
                this._getHostHardInfoList.Add(hostInfo);
                this._getHostHardInfoListEventHandle.Set();
            }
        }

        public void ModifyHost(HostInfo hostInfo, bool ipChanged)
        {
            if (ipChanged)
            {
                this.SendAddHostCommand(hostInfo);
            }

            this._dal.ModifyHost(hostInfo);
            lock (this._hostInfoDicLock)
            {
                this._hostInfoDic[hostInfo.Id] = hostInfo;
            }
        }

        public void DeleteHost(HostInfo hostInfo)
        {
            //先删该主机上的服务实例
            this._routeManager.DeleteServiceInsByHostId(hostInfo.Id);

            //删除主机
            this._dal.DeleteHost(hostInfo);
            lock (this._hostInfoDicLock)
            {
                HostInfo hostInfo2;
                this._hostInfoDic.TryRemove(hostInfo.Id, out hostInfo2);
                this._hostStatusInfoManager.RemoveHostInfo(hostInfo);

                //通知ui
                object result;
                LRPCCore.TryRemoteCallF(DevOpsConstant.LRPC_HOST_DELETE_CHANNEL, hostInfo, out result);
            }
        }

        public List<HostInfo> QueryHostList(HostGroup hostGroup)
        {
            return this._dal.QueryHostListByHostGroupID(hostGroup.Id);
        }

        public List<HostGroup> QueryHostGroupList()
        {
            return this._dal.QueryHostGroupList();
        }

        public void KillProcess(HostInfo hostInfo, int processId)
        {
            this._sender.SendInteractiveCommandHost(new KillProcessCommand(processId), hostInfo.Ip, Config.Instance.SendCommandDefaultTimeout);
        }

        public void KillTreeProcess(HostInfo hostInfo, int processId)
        {
            this._sender.SendInteractiveCommandHost(new KillProcessTreeCommand(processId), hostInfo.Ip, Config.Instance.SendCommandDefaultTimeout);
        }

        public void ControlHost<T>(HostInfo hostInfo) where T : ICommand, new()
        {
            if (hostInfo == null)
            {
                return;
            }

            var command = new T();
            this._sender.SendCommandHost(command, hostInfo.Ip);
            Loger.Info($"发送{SHPCommandDefine.GetCommandNameByCommand(command.Cmd)}命令完成");
        }
        #endregion

        public void DeleteHost(long hostId, IPEndPoint hostEndPoint)
        {
            HostInfo hostInfo;
            if (this._hostInfoDic.TryGetValue(hostId, out hostInfo))
            {
                this.DeleteHost(hostInfo);
            }

            this._sender.SendCommandHost(new DeleteHostCommand(hostId), hostEndPoint.Address.ToString());
        }

        public bool IsValidateHost(long hostId)
        {
            return this._hostInfoDic.ContainsKey(hostId);
        }

        public Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection GetDHostLoadPlugins()
        {
            return this._pluginManager.GetTHPlugins();
        }

        public HostStatusInfo[] GetHostStatusInfo(HostInfo hostInfo)
        {
            return this._hostStatusInfoManager.GetHostStatusInfoByHostId(hostInfo.Id);
        }

        public void Start()
        {
            this.RecoverHostTypeList();
            this.RecoverGetHostHardInfoList();
            this._getHostHardInfoThread.Start();
        }

        private void RecoverHostTypeList()
        {
            var items = this._dal.QueryAllHostTypeItem();
            this._hostTypeList.AddRange(items);
            foreach (var item in items)
            {
                this._hostTypeDic[item.Id] = item;
            }
        }

        /// <summary>
        /// 恢复获取主机硬件信息列表
        /// </summary>
        private void RecoverGetHostHardInfoList()
        {
            var hostInfoList = this._dal.QueryAllHostinfo();
            this._hostStatusInfoManager.AddRangeHostInfo(hostInfoList);
            var hostInfoDic = hostInfoList.ToDictionary(t => { return t.Id; });
            var hostHardInfoDic = this._dal.QueryAllHostHardInfo().ToDictionary(t => { return t.Id; });

            //查找出不存在主机的硬件信息,之后删除
            var delHostHardInfoList = new List<HostHardInfo>();
            foreach (var kv in hostHardInfoDic)
            {
                if (!hostInfoDic.ContainsKey(kv.Key))
                {
                    delHostHardInfoList.Add(kv.Value);
                }
            }

            //查找出没有硬件信息的主机,将其信息添加到获取硬件信息集合中,由发送获取硬件信息命令线程处理
            foreach (var kv in hostInfoDic)
            {
                this._hostInfoDic.TryAdd(kv.Key, kv.Value);

                if (!hostHardInfoDic.ContainsKey(kv.Key))
                {
                    this._getHostHardInfoList.Add(kv.Value);
                }
            }

            if (this._getHostHardInfoList.Count > 0)
            {
                this._getHostHardInfoListEventHandle.Set();
            }

            this._dal.DeleteHostHardInfo(delHostHardInfoList);
        }

        public HostHardInfo GetHostHardInfoByHostId(long hostId)
        {
            return this._dal.GetHostHardInfoByHostId(hostId);
        }

        #region 监视应用相关
        /// <summary>
        /// 控制监视应用
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="appMonitorItem"></param>
        /// <param name="hostInfo"></param>
        public void ControlApp(AppControlType controlType, AppMonitorItem appMonitorItem, HostInfo hostInfo)
        {
            this._sender.SendInteractiveCommandHost(new ControlAppCommand(appMonitorItem.AppName, controlType), hostInfo.Ip, Config.Instance.SendCommandDefaultTimeout);
        }

        public void AddMonitor(HostInfo hostInfo, int processId)
        {
            this._sender.SendInteractiveCommandHost(new ProcessAddMonitorCommand(processId), hostInfo.Ip, Config.Instance.SendCommandDefaultTimeout);
        }

        public void RemoveMonitor(HostInfo hostInfo, AppMonitorItem appMonitorItem)
        {
            this._sender.SendInteractiveCommandHost(new RemoveMonitorAppCommand(appMonitorItem.AppName), hostInfo.Ip, Config.Instance.SendCommandDefaultTimeout);
        }
        #endregion

        #region 主机类型
        public void AddHostType(HostTypeItem hostTypeItem)
        {
            this._dal.AddHostType(hostTypeItem);
            this._hostTypeList.Add(hostTypeItem);
            this._hostTypeDic[hostTypeItem.Id] = hostTypeItem;
        }

        public void RemoveHostType(HostTypeItem hostTypeItem)
        {
            this._dal.DeleteHostType(hostTypeItem);
            this._hostTypeList.Remove(hostTypeItem);

            if (this._hostTypeDic.ContainsKey(hostTypeItem.Id))
            {
                this._hostTypeDic.Remove(hostTypeItem.Id);
            }
        }

        public void ModifyHostType(HostTypeItem hostTypeItem)
        {
            this._dal.ModifyHostType(hostTypeItem);
        }

        public void ClearHostType()
        {
            this._dal.DeleteAllHostType();
            this._hostTypeList.Clear();
            this._hostTypeDic.Clear();
        }
        #endregion

        #region 脚本
        public void ExcuteScript(ScriptType type, string content, int millisecondsTimeout, HostInfo hostInfo)
        {
            byte[] data = this._sender.SendInteractiveCommandHost(new ExcuteScriptCommand(type, content), hostInfo.Ip, millisecondsTimeout);
            Loger.Info($"主机[{hostInfo.Name}]脚本执行完成,结果信息如下:");
            string retStr;
            if (data != null)
            {
                retStr = SHPResult.GetString(data);
            }
            else
            {
                retStr = string.Empty;
            }

            Loger.Info(retStr);
        }
        #endregion


        #region 运控迁移
        private ThreadEx _devOpsMigratedHostNotifyThread = null;
        public void DevOpsMigrate()
        {
            if (this._devOpsMigratedHostNotifyThread == null)
            {
                this._devOpsMigratedHostNotifyThread = new ThreadEx(this.DevOpsMigratedHostNotifyThreadMethod, "devOpsMigratedHostNotifyThread ", true);
            }
            else
            {
                this._devOpsMigratedHostNotifyThread.Stop();
            }

            this._devOpsMigratedHostNotifyThread.Start();
        }

        private readonly List<HostInfo> _waitHostRevDevOpsMigratedNotifyResList = new List<HostInfo>();
        private readonly object _waitHostRevDevOpsMigratedNotifyResListLock = new object();
        private void DevOpsMigratedHostNotifyThreadMethod(CancellationToken token)
        {
            lock (this._waitHostRevDevOpsMigratedNotifyResListLock)
            {
                this._waitHostRevDevOpsMigratedNotifyResList.Clear();
                this._waitHostRevDevOpsMigratedNotifyResList.AddRange(this._hostInfoDic.Values);
            }

            HostInfo[] hostInfos;
            const int interval = 5000;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    lock (this._waitHostRevDevOpsMigratedNotifyResListLock)
                    {
                        hostInfos = this._waitHostRevDevOpsMigratedNotifyResList.ToArray();
                    }

                    if (hostInfos.Length == 0)
                    {
                        break;
                    }

                    foreach (var hostInfo in hostInfos)
                    {
                        try
                        {
                            this._sender.SendCommandHost(new DevOpsMigrateNotifyCommand(Config.Instance.DevOpsId, hostInfo.Id), hostInfo.Ip);
                        }
                        catch (Exception exi)
                        {
                            Loger.Warn(exi, $"发送迁移通知到主机[{hostInfo.Name}]异常");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }

                Thread.Sleep(interval);
            }
        }

        internal void ProDevOpsMigrateNotifyRes(DevOpsMigrateNotifyResCommand devOpsMigrateNotifyResCommand)
        {
            lock (this._waitHostRevDevOpsMigratedNotifyResListLock)
            {
                var items = this._waitHostRevDevOpsMigratedNotifyResList.Where(t => { return t.Id == devOpsMigrateNotifyResCommand.HostId; }).ToArray();
                foreach (var item in items)
                {
                    this._waitHostRevDevOpsMigratedNotifyResList.Remove(item);
                }
            }
        }
        #endregion

        public void Dispose()
        {
            try
            {
                this._getHostHardInfoThread.Stop();
                this._getHostHardInfoListEventHandle.Set();
                this._getHostHardInfoThread.Dispose();
                this._getHostHardInfoListEventHandle.Dispose();

                if (this._devOpsMigratedHostNotifyThread != null)
                {
                    this._devOpsMigratedHostNotifyThread.Stop();
                    this._devOpsMigratedHostNotifyThread.Dispose();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
