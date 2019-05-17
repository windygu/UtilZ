using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    public class HostStatusInfoManager
    {
        //[key:Hostid]
        private readonly ConcurrentDictionary<long, HostStatusInfoCollection> _hostStatusInfoDic = new ConcurrentDictionary<long, HostStatusInfoCollection>();
        private readonly object _hostStatusInfoDicLock = new object();
        private readonly ServiceStatusManagerCollection _serviceStatusManagerCollection = new ServiceStatusManagerCollection();

        public HostStatusInfoManager()
        {

        }

        #region 主机
        private void PrimitiveAddHostInfo(HostInfo hostInfo)
        {
            var hostStatusInfoCollection = new HostStatusInfoCollection(hostInfo);
            if (!this._hostStatusInfoDic.TryAdd(hostInfo.Id, hostStatusInfoCollection))
            {
                Loger.Warn($"主机[{hostInfo.Name}]添加到主机状态字典集合失败,原因未知");
            }
        }

        public void AddHostInfo(HostInfo hostInfo)
        {
            lock (this._hostStatusInfoDicLock)
            {
                this.PrimitiveAddHostInfo(hostInfo);
            }
        }

        public void AddRangeHostInfo(IEnumerable<HostInfo> hostInfos)
        {
            lock (this._hostStatusInfoDicLock)
            {
                foreach (var hostInfo in hostInfos)
                {
                    this.PrimitiveAddHostInfo(hostInfo);
                }
            }
        }

        public void RemoveHostInfo(HostInfo hostInfo)
        {
            lock (this._hostStatusInfoDicLock)
            {
                HostStatusInfoCollection hostStatusInfoCollection;
                this._hostStatusInfoDic.TryRemove(hostInfo.Id, out hostStatusInfoCollection);
            }
        }

        public List<HostInfo> GetHostInfoList()
        {
            lock (this._hostStatusInfoDicLock)
            {
                return this._hostStatusInfoDic.Values.Select(t => { return t.HostInfo; }).ToList();
            }
        }

        public HostInfo GetHostInfoByHostId(long hostId)
        {
            HostStatusInfoCollection hostStatusInfoCollection;
            if (this._hostStatusInfoDic.TryGetValue(hostId, out hostStatusInfoCollection))
            {
                return hostStatusInfoCollection.HostInfo;
            }
            else
            {
                throw new ArgumentException($"不存在id为[{hostId}]的主机");
            }
        }
        #endregion

        #region 状态
        public void AddHostStatusInfo(HostStatusInfo hostStatus)
        {
            HostStatusInfoCollection hostStatusInfoCollection;
            if (this._hostStatusInfoDic.TryGetValue(hostStatus.Id, out hostStatusInfoCollection))
            {
                //添加到服务
                this._serviceStatusManagerCollection.AddServiceInsStatus(hostStatus);

                hostStatusInfoCollection.Add(hostStatus);
                if (hostStatusInfoCollection.HostInfo.Status == HostStatus.OffLine)
                {
                    hostStatusInfoCollection.HostInfo.Status = HostStatus.OnLine;
                    LRPCCore.RemoteCallF(DevOpsConstant.LRPC_HOST_ON_LINE_NOTIFY_CHANNEL, hostStatusInfoCollection.HostInfo);
                    this.OnRaiseHostStatusChanged(hostStatusInfoCollection.HostInfo);
                }
            }
            else
            {
                Loger.Warn($"无效的主机[{hostStatus.Id}]");
            }
        }

        public HostStatusInfo[] GetHostStatusInfoByHostId(long hostId)
        {
            HostStatusInfoCollection hostStatusInfoCollection;
            if (this._hostStatusInfoDic.TryGetValue(hostId, out hostStatusInfoCollection))
            {
                lock (hostStatusInfoCollection.SynRoot)
                {
                    return hostStatusInfoCollection.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        public ServiceStatusInfo[] GetServiceInsStatus(SHPServiceInsInfo serviceInsInfo)
        {
            return this._serviceStatusManagerCollection.GetServiceInsStatus(serviceInsInfo);
        }
        #endregion

        #region 状态分析
        private ThreadEx _statusAnalyzeThread;
        /// <summary>
        /// bool:true:上线;false:离线
        /// </summary>
        private Action<HostInfo> _hostStatusChanged;


        private void OnRaiseHostStatusChanged(HostInfo hostInfo)
        {
            var handler = this._hostStatusChanged;
            if (handler != null)
            {
                handler(hostInfo);
            }
        }

        public void StartStatusAnalyze(Action<HostInfo> hostStatusChanged)
        {
            this._hostStatusChanged = hostStatusChanged;
            this._statusAnalyzeThread = new ThreadEx(this.StatusAnalyzeThreadMethod, "主机离线检测线程", true);
            this._statusAnalyzeThread.Start();
        }

        private void StatusAnalyzeThreadMethod(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(Config.Instance.HostOffLineCheckInterval);

                    //主机状态分析
                    var offLineHostIdList = AnalyzeHost();

                    //分析服务实例
                    this._serviceStatusManagerCollection.AnalyzeServiceInsStatus(offLineHostIdList);
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, "主机离线检测发生异常");
                }
            }
        }

        private List<long> AnalyzeHost()
        {
            List<long> offLineHostIdList = new List<long>();
            var hostStatusInfoCollectionArr = this._hostStatusInfoDic.Values.ToArray();

            foreach (var hostStatusInfoCollection in hostStatusInfoCollectionArr)
            {
                var hostInfo = hostStatusInfoCollection.HostInfo;
                if (hostInfo.Status == HostStatus.OnLine && hostStatusInfoCollection.IsTimeout())
                {
                    //主机超时处理
                    hostInfo.Status = HostStatus.OffLine;
                    offLineHostIdList.Add(hostInfo.Id);
                    this.OnRaiseHostStatusChanged(hostInfo);
                }
            }

            return offLineHostIdList;
        }
        #endregion

        internal HostInfo GetDeployServiceHostInfoByHostTypeId(ServiceInfo serviceInfo)
        {
            HostStatusInfoCollection[] hostStatusInfoCollections;
            lock (this._hostStatusInfoDicLock)
            {
                hostStatusInfoCollections = (from t in this._hostStatusInfoDic.Values where t.HostInfo.HostTypeId == serviceInfo.HostTypeId && t.HostInfo.Status == HostStatus.OnLine select t).ToArray();
            }

            if (hostStatusInfoCollections.Length == 0)
            {
                //throw new InvalidOperationException($"服务[{serviceInfo.Name}]没有可用的主机,服务所需要主机类型[{serviceInfo.HostTypeText}]");
                var hostInfo = this._hostStatusInfoDic.Values.ElementAt(0).HostInfo;
                throw new InvalidOperationException($"服务[{serviceInfo.Name}]没有可用的主机,服务所需要主机类型[{serviceInfo.HostTypeId}],当前主机类型[{hostInfo.HostTypeId}],状态[{hostInfo.Status.ToString()}]");
            }

            //todo..各种策略选择主机,以后现说,先默认第一个
            return hostStatusInfoCollections[0].HostInfo;
        }
    }
}
