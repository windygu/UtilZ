using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPDevOpsModel
{
    /// <summary>
    /// 所有服务以及服务下的所有服务实例状态管理类
    /// </summary>
    public class ServiceStatusManagerCollection
    {
        /// <summary>
        /// [key:ServiceId;]
        /// </summary>
        private readonly ConcurrentDictionary<long, ServiceStatusManager> _serviceStatusManagerDic = new ConcurrentDictionary<long, ServiceStatusManager>();
        public ServiceStatusManagerCollection()
        {

        }

        public void AddServiceInsStatus(HostStatusInfo hostStatus)
        {
            SHPServiceInsInfo serviceInsInfo;
            ServiceStatusManager serviceStatus;

            foreach (var serviceStatusInfo in hostStatus.ServiceStatusInfoList)
            {
                serviceInsInfo = (SHPServiceInsInfo)LRPCCore.RemoteCallF(DevOpsConstant.LRPC_GET_SERVICE_INS_INFO_CHANNEL, serviceStatusInfo.Id);
                if (serviceInsInfo == null)
                {
                    Loger.Warn("内部错误,获取服务实例信息失败");
                    continue;
                }

                if (!this._serviceStatusManagerDic.TryGetValue(serviceInsInfo.ServiceId, out serviceStatus))
                {
                    Loger.Warn($"获取服务[ID:{serviceInsInfo.ServiceId}]状态管理对象失败");
                    continue;
                }

                serviceStatus.AddServiceInsStatus(hostStatus.Id, serviceStatusInfo);
            }

            hostStatus.ServiceStatusInfoList = null;
        }

        public void AnalyzeServiceInsStatus(List<long> offLineHostIdList)
        {
            List<long> offLineServiceInsIdList = new List<long>();
            List<long> offLineServiceInsIdListTmp;
            var serviceStatusManagerArr = this._serviceStatusManagerDic.Values.ToArray();
            foreach (var serviceStatusManager in serviceStatusManagerArr)
            {
                offLineServiceInsIdListTmp = serviceStatusManager.AnalyzeServiceInsPaload(offLineHostIdList);
                if (offLineServiceInsIdListTmp != null && offLineServiceInsIdListTmp.Count > 0)
                {
                    offLineServiceInsIdList.AddRange(offLineServiceInsIdListTmp);
                }
            }

            //服务实例离线通知
            if (offLineServiceInsIdList.Count > 0)
            {
                LRPCCore.TryRemoteCallA(DevOpsConstant.LRPC_SERVICE_INS_OFF_LINE_CHANNEL, offLineServiceInsIdList);
            }
        }

        public ServiceStatusInfo[] GetServiceInsStatus(SHPServiceInsInfo serviceInsInfo)
        {
            ServiceStatusManager serviceStatusManager;
            if (this._serviceStatusManagerDic.TryGetValue(serviceInsInfo.ServiceId, out serviceStatusManager))
            {
                return serviceStatusManager.GetServiceInsStatusById(serviceInsInfo.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
