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
    /// 单个服务下的所有服务实例状态管理类
    /// </summary>
    public class ServiceStatusManager
    {
        private readonly long _serviceId;
        /// <summary>
        /// [key:服务实例id]
        /// </summary>
        private readonly ConcurrentDictionary<long, ServiceStatusInfoCollection> _serviceStatusInfoCollectionDic = new ConcurrentDictionary<long, ServiceStatusInfoCollection>();

        public ServiceStatusManager(long serviceId)
        {
            this._serviceId = serviceId;
        }

        public void AddServiceInsStatus(long hostId, ServiceStatusInfo serviceStatusInfo)
        {
            ServiceStatusInfoCollection serviceStatusInfoCollection;
            if (!this._serviceStatusInfoCollectionDic.TryGetValue(serviceStatusInfo.Id, out serviceStatusInfoCollection))
            {
                serviceStatusInfoCollection = new ServiceStatusInfoCollection(hostId, serviceStatusInfo, Config.Instance.ServiceInsOffLineMillisecondsTimeout);
                this._serviceStatusInfoCollectionDic.TryAdd(serviceStatusInfo.Id, serviceStatusInfoCollection);
            }

            serviceStatusInfoCollection.AddServiceInsStatusInfo(serviceStatusInfo);
        }

        public ServiceStatusInfo[] GetServiceInsStatusById(long id)
        {
            ServiceStatusInfoCollection serviceStatusInfoCollection;
            if (this._serviceStatusInfoCollectionDic.TryGetValue(id, out serviceStatusInfoCollection))
            {
                return serviceStatusInfoCollection.GetServiceStatusInfoArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 分析服务实例负载
        /// </summary>
        internal List<long> AnalyzeServiceInsPaload(List<long> offLineHostIdList)
        {
            List<long> offLineServiceInsIdList = new List<long>();
            try
            {
                ServiceStatusInfoCollection[] arr = this._serviceStatusInfoCollectionDic.Values.ToArray();
                var serviceInsPayloadAnalyzeResultList = new List<ServiceInsPayloadAnalyzeResult>();

                foreach (var serviceStatusInfoCollection in arr)
                {
                    if (offLineHostIdList.Contains(serviceStatusInfoCollection.HostId))
                    {
                        //主机离线,服务实例也离线
                        offLineServiceInsIdList.Add(serviceStatusInfoCollection.Id);
                    }
                    else
                    {
                        if (serviceStatusInfoCollection.IsTimeout())
                        {
                            //服务实例检测到离线
                            offLineServiceInsIdList.Add(serviceStatusInfoCollection.Id);
                        }
                        else
                        {
                            //正常在线,分析负载
                            var result = serviceStatusInfoCollection.AnalyzeServiceInsPayload();
                            serviceInsPayloadAnalyzeResultList.Add(result);
                        }
                    }
                }

                this.AnalyzeProAbility(serviceInsPayloadAnalyzeResultList);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, $"服务[ID:{this._serviceId}]分析发生异常");
            }

            return offLineServiceInsIdList;
        }

        /// <summary>
        /// 核心重点分析,策略很严峻啊
        /// </summary>
        /// <param name="serviceInsPayloadAnalyzeResultList"></param>
        private void AnalyzeProAbility(List<ServiceInsPayloadAnalyzeResult> serviceInsPayloadAnalyzeResultList)
        {

            int avgPayloadValue = (int)serviceInsPayloadAnalyzeResultList.Average(t => { return t.PayloadValue; });
            int totalDenyCount = serviceInsPayloadAnalyzeResultList.Sum(t => { return t.DenyCount; });
            if (avgPayloadValue >= Config.Instance.ServiceInsAvgMaxPayloadValue || totalDenyCount >= Config.Instance.DenyCount)
            {
                //增加一个服务实例
                LRPCCore.RemoteCallA(DevOpsConstant.LRPC_ADD_SERVICE_INS_CHANNEL, this._serviceId);
                return;
            }

            if (avgPayloadValue <= Config.Instance.ServiceInsAvgMinPayloadValue || totalDenyCount <= 0)
            {
                //减少一个服务实例
                var preRemoveServiceIns = serviceInsPayloadAnalyzeResultList.OrderBy(t => { return t.PayloadValue; }).FirstOrDefault();
                LRPCCore.RemoteCallA(DevOpsConstant.LRPC_DELETE_SERVICE_INS_CHANNEL, preRemoveServiceIns.Id);
                return;
            }

        }
    }
}
