using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    public class ServiceRouteTransferPolicy
    {
        public ServiceRouteInfo ServiceRouteInfo { get; private set; }
        public TransferPolicy TransferPolicy { get; private set; }

        public ServiceRouteTransferPolicy(ServiceRouteInfo serviceRouteInfo, TransferPolicy transferPolicy)
        {
            this.ServiceRouteInfo = serviceRouteInfo;
            this.TransferPolicy = transferPolicy;
        }
    }
}
