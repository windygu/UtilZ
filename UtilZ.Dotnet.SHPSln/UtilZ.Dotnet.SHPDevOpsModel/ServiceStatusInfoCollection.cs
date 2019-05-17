using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPDevOpsModel
{
    /// <summary>
    /// 单个服务实例状态分析类
    /// </summary>
    public class ServiceStatusInfoCollection : TimeoutBase
    {
        /// <summary>
        /// 服务实例Id
        /// </summary>
        public long Id { get; private set; }

        public long HostId { get; private set; }

        private readonly List<ServiceStatusInfo> _serviceStatusInfoList = new List<ServiceStatusInfo>();
        private readonly object _serviceStatusInfoListLock = new object();
        private bool _isTimeout = false;
        private int _totalDenyCount = 0;

        public ServiceStatusInfoCollection(long hostId, ServiceStatusInfo serviceStatusInfo, int millisecondsTimeout)
            : base(millisecondsTimeout)
        {
            this.HostId = hostId;
            this.Id = serviceStatusInfo.Id;
            this.AddServiceInsStatusInfo(serviceStatusInfo);
        }

        public void AddServiceInsStatusInfo(ServiceStatusInfo serviceStatusInfo)
        {
            lock (this._serviceStatusInfoListLock)
            {
                if (serviceStatusInfo.DenyCount > 0)
                {
                    this._totalDenyCount += serviceStatusInfo.DenyCount;
                }

                this._serviceStatusInfoList.Add(serviceStatusInfo);

                var serviceInsPaloadAnalyzeObjectCount = Config.Instance.ServiceInsStatusInfoCacheCount;
                if (this._serviceStatusInfoList.Count > serviceInsPaloadAnalyzeObjectCount)
                {
                    int count = this._serviceStatusInfoList.Count - serviceInsPaloadAnalyzeObjectCount;
                    this._serviceStatusInfoList.RemoveRange(0, count);
                }
            }

            base.UpdateLastAccessTimestamp();
            if (this._isTimeout)
            {
                LRPCCore.TryRemoteCallA(DevOpsConstant.LRPC_SERVICE_INS_ON_LINE_CHANNEL, serviceStatusInfo);
                this._isTimeout = false;
            }
        }

        public ServiceStatusInfo[] GetServiceStatusInfoArray()
        {
            lock (this._serviceStatusInfoListLock)
            {
                return this._serviceStatusInfoList.ToArray();
            }
        }

        public override bool IsTimeout()
        {
            this._isTimeout = base.IsTimeout();
            return this._isTimeout;
        }

        /// <summary>
        /// 分析负载
        /// </summary>
        internal ServiceInsPayloadAnalyzeResult AnalyzeServiceInsPayload()
        {
            ServiceStatusInfo[] serviceStatusInfoArr = this.GetServiceStatusInfoArray();
            int serviceInsPaloadAnalyzeObjectCount = Config.Instance.ServiceInsPaloadAnalyzeObjectCount;
            if (serviceStatusInfoArr.Length > serviceInsPaloadAnalyzeObjectCount)
            {
                serviceStatusInfoArr = serviceStatusInfoArr.Take(serviceStatusInfoArr.Length - serviceInsPaloadAnalyzeObjectCount).ToArray();

            }

            int avgPayloadValue = (int)serviceStatusInfoArr.Average((t) => { return t.PayloadValue; });
            int totalDenyCount = (int)serviceStatusInfoArr.Sum(t => { return t.DenyCount; });
            return new ServiceInsPayloadAnalyzeResult(this.Id, avgPayloadValue, totalDenyCount);
        }
    }
}
