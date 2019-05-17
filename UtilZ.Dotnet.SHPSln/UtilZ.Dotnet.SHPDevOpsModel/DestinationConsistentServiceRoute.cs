using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPDevOpsModel
{
    public class DestinationConsistentServiceRoute
    {
        private int _totalCount;
        private int _count = 0;


        public ServiceRouteInfo ServiceRoute { get; private set; }

        public string HashKey { get; private set; }

        public DestinationConsistentServiceRoute(int totalCount, ServiceRouteInfo serviceRoute, string hashKey)
        {
            this._totalCount = totalCount;
            this.ServiceRoute = serviceRoute;
            this.HashKey = hashKey;
        }

        public bool Increment()
        {
            lock (this)
            {
                this._count += 1;
                return this._count >= this._totalCount;
            }
        }
    }
}
