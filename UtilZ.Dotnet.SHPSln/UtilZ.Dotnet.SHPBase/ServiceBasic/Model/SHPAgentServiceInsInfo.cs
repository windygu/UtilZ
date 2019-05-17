using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [Serializable]
    public class SHPAgentServiceInsInfo : SHPBasicServiceInfo
    {
        public string FilePath { get; set; }

        public int ListenPort { get; set; }

        public int ServiceRestfullServiceListenPort { get; set; }

        public SHPAgentServiceInsInfo()
            : base()
        {

        }

        public SHPAgentServiceInsInfo(SHPBasicServiceInfo shpBasicServiceInfo)
              : base(shpBasicServiceInfo)
        {

        }

        public void Update(ServiceInsListenInfo para)
        {
            this.ListenPort = para.ListenPort;
            this.ServiceRestfullServiceListenPort = para.ServiceRestfullServiceListenPort;
        }

        public void Update(SHPAgentServiceInsInfo shpAgentServiceInsInfo)
        {
            this.ListenPort = shpAgentServiceInsInfo.ListenPort;
            this.ServiceRestfullServiceListenPort = shpAgentServiceInsInfo.ServiceRestfullServiceListenPort;
        }
    }
}
