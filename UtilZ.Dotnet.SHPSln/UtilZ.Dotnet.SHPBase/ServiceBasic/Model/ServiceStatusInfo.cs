using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [DataContract]
    public class ServiceStatusInfo : ServiceStatus
    {
        /// <summary>
        /// 运控id
        /// </summary>
        [DataMember]
        public long DOID { get; set; }

        /// <summary>
        /// 服务实例Id
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// 拒绝接收数据次数
        /// </summary>
        [DataMember]
        public int DenyCount { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public ServiceStatusInfo()
            : base()
        {

        }

        /// <summary>
        /// 创建默认
        /// </summary>
        /// <param name="doId">运控id</param>
        /// <param name="id">服务实例Id</param>
        public ServiceStatusInfo(long doId, long id)
            : base()
        {
            this.DOID = doId;
            this.Id = id;
        }
    }
}
