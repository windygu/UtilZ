using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [DataContract]
    public class GetServiceRoutePara
    {
        /// <summary>
        /// 内部参数,外部忽略
        /// </summary>
        [DataMember]
        public List<long> SendFailServiceInsIdList { get; set; }

        /// <summary>
        /// 数据路由编码
        /// </summary>
        [DataMember]
        public int DataCode { get; set; }

        /// <summary>
        /// 目地地一致性key
        /// </summary>
        [DataMember]
        public string DestinationConsistentKey { get; set; }

        /// <summary>
        /// 目地地一致性数据个数
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        /// <summary>
        /// 一致性数据发送超时时长
        /// </summary>
        [DataMember]
        public int MillisecondsTimeout { get; set; }

        public GetServiceRoutePara()
        {

        }

        public GetServiceRoutePara(List<long> sendFailServiceInsIdList, TransferBasicPolicy transferBasicPolicy)
        {
            this.SendFailServiceInsIdList = sendFailServiceInsIdList;
            this.DataCode = transferBasicPolicy.DataCode;
            this.DestinationConsistentKey = transferBasicPolicy.DestinationConsistentKey;
            this.Count = transferBasicPolicy.DestinationConsistentCount;
            this.MillisecondsTimeout = transferBasicPolicy.ConsistentMillisecondsTimeout;
        }
    }
}
