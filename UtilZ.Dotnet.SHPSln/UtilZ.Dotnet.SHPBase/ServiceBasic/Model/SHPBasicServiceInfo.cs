using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [Serializable]
    [DataContract]
    public class SHPBasicServiceInfo
    {
        /// <summary>
        /// 路由id,同时也是服务实例id
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// 运控id
        /// </summary>
        [DataMember]
        public long DOID { get; set; }

        [DataMember]
        public TransferProtocal TransferProtocal { get; set; }

        [DataMember]
        public List<int> HostNotAllocatePortList { get; set; }

        [DataMember]
        public string RouteServiceUrl { get; set; }

        /// <summary>
        /// 上报服务实例负载状态时间间隔
        /// </summary>
        [DataMember]
        public int StatusUploadIntervalMilliseconds { get; set; }

        /// <summary>
        /// 服务实例名称
        /// </summary>
        [DataMember]
        public string ServiceInsName { get; set; }

        public SHPBasicServiceInfo()
        {

        }

        public SHPBasicServiceInfo(SHPBasicServiceInfo shpBasicServiceInfo)
        {
            this.Id = shpBasicServiceInfo.Id;
            this.DOID = shpBasicServiceInfo.DOID;
            this.TransferProtocal = shpBasicServiceInfo.TransferProtocal;
            var hostNotAllocatePortList = shpBasicServiceInfo.HostNotAllocatePortList;
            if (hostNotAllocatePortList != null)
            {
                hostNotAllocatePortList = hostNotAllocatePortList.ToList();
            }

            this.HostNotAllocatePortList = hostNotAllocatePortList;
            this.RouteServiceUrl = shpBasicServiceInfo.RouteServiceUrl;
            this.StatusUploadIntervalMilliseconds = shpBasicServiceInfo.StatusUploadIntervalMilliseconds;
            this.ServiceInsName = shpBasicServiceInfo.ServiceInsName;
        }
    }
}
