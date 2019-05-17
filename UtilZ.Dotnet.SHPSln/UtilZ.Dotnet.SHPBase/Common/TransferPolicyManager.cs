using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.Common
{
    public class TransferPolicyManager
    {
        private TransferPolicyManager()
        {

        }

        private readonly static TransferPolicyManager _instance = new TransferPolicyManager();

        public static TransferPolicyManager Instance
        {
            get { return _instance; }
        }


        //[key:IPEndPoint;value:TransferPolicy]
        private readonly ConcurrentDictionary<string, TransferPolicy> _policyDic = new ConcurrentDictionary<string, TransferPolicy>();
        private readonly object _policyDicLock = new object();

        public TransferPolicy GetTransferPolicy(IPEndPoint ipEndPoint, Func<IPEndPoint, TransferPolicy> createPolicyCallback)
        {
            if (createPolicyCallback == null)
            {
                throw new ArgumentNullException(nameof(createPolicyCallback));
            }

            return PrimitiveGetTransferPolicy(ipEndPoint, createPolicyCallback);
        }

        public TransferPolicy GetTransferPolicy(IPEndPoint ipEndPoint)
        {
            return PrimitiveGetTransferPolicy(ipEndPoint, null);
        }

        private TransferPolicy PrimitiveGetTransferPolicy(IPEndPoint ipEndPoint, Func<IPEndPoint, TransferPolicy> createPolicyCallback)
        {
            if (ipEndPoint == null)
            {
                throw new ArgumentNullException(nameof(ipEndPoint));
            }

            TransferPolicy transferPolicy;
            var key = ipEndPoint.Address.ToString();
            if (!this._policyDic.TryGetValue(key, out transferPolicy))
            {
                lock (this._policyDicLock)
                {
                    if (!this._policyDic.TryGetValue(key, out transferPolicy))
                    {
                        if (createPolicyCallback != null)
                        {
                            transferPolicy = createPolicyCallback(ipEndPoint);
                        }
                        else
                        {
                            transferPolicy = new TransferPolicy(ipEndPoint, 0, 5000, 3);
                        }

                        if (!this._policyDic.TryAdd(key, transferPolicy))
                        {
                            Loger.Warn("if(!this._policyDic.TryAdd(key,transferPolicy))失败");
                        }
                    }
                }
            }

            return transferPolicy;
        }
    }
}
