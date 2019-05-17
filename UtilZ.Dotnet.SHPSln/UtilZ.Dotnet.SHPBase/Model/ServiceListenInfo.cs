using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class ServiceListenInfo
    {
        /// <summary>
        /// 服务实例Id
        /// </summary>
        [TTLVAttribute(101)]
        public long Id { get; set; }

        [TTLVAttribute(102)]
        public int ServiceInsListenPort { get; set; }

        public ServiceListenInfo()
        {

        }

        public ServiceListenInfo(long id, int serviceInsListenPort)
        {
            this.Id = id;
            this.ServiceInsListenPort = serviceInsListenPort;
        }
    }
}
