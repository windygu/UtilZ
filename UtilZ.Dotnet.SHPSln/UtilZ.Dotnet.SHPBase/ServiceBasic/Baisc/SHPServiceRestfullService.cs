using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using UtilZ.Dotnet.Ex.RestFullBase;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
      ConcurrencyMode = ConcurrencyMode.Single,
      IncludeExceptionDetailInFaults = true,
      AddressFilterMode = AddressFilterMode.Any)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    internal class SHPServiceRestfullService : ISHPServiceRestfullService
    {
        private readonly Action _closeServiceNotifyFun;
        public SHPServiceRestfullService(Action closeServiceNotifyFun)
        {
            this._closeServiceNotifyFun = closeServiceNotifyFun;
        }

        public ApiResult CloseServiceNotify(string contextId)
        {
            try
            {
                this._closeServiceNotifyFun();
                return new ApiResult(ApiResultStatus.Succes, contextId);
            }
            catch (Exception ex)
            {
                return new ApiResult(ApiResultStatus.Exception, ex.Message);
            }
        }
    }
}
