using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [DataContract]
    public class ServiceStatus
    {
        /// <summary>
        /// 负载百分比
        /// </summary>
        [DataMember]
        public int PayloadValue { get; set; }

        public ServiceStatus()
        {

        }

        public void UpdateServiceStatus(ServiceStatus serviceStatus)
        {
            this.PayloadValue = serviceStatus.PayloadValue;
        }
    }
}
