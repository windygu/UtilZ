using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [DataContract]
    public class ServiceInsListenInfo
    {
        /// <summary>
        /// 运控id
        /// </summary>
        [DataMember]
        public long DOID { get; set; }

        /// <summary>
        /// 服务实例id
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int ListenPort { get; set; }

        [DataMember]
        public int ServiceRestfullServiceListenPort { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public ServiceInsListenInfo()
        {

        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="doId">运控id</param>
        /// <param name="id">服务实例id</param>
        /// <param name="listenPort"></param>
        public ServiceInsListenInfo(long doId, long id, int listenPort, int serviceRestfullServiceListenPort)
        {
            this.DOID = doId;
            this.Id = id;
            this.ListenPort = listenPort;
            this.ServiceRestfullServiceListenPort = serviceRestfullServiceListenPort;
        }
    }
}
