using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    [ServiceContract(Name = nameof(SHPServiceRestfullService))]
    public interface ISHPServiceRestfullService
    {
        [OperationContract]
        [WebGet(UriTemplate = AgentServiceMethodNameConstant.CLOSE_SERVICE_NOTIFY + "{contextId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ApiResult CloseServiceNotify(string contextId);
    }
}
