using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    [DataContract]
    public class ServiceRouteInfo
    {
        /// <summary>
        /// 服务实例Id
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public TransferProtocal TransferProtocal { get; set; }

        public ServiceRouteInfo()
        {

        }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="id">服务实例Id</param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="transferProtocal"></param>
        public ServiceRouteInfo(long id, string ip, int port, TransferProtocal transferProtocal)
        {
            this.Id = id;
            this.Ip = ip;
            this.Port = port;
            this.TransferProtocal = transferProtocal;
        }
    }
}
