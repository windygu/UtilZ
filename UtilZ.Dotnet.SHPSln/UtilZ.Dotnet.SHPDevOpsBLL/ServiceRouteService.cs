using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    [ServiceContract(Name = nameof(ServiceRouteService))]
    public interface IServiceRouteService
    {
        [OperationContract]
        [WebGet(UriTemplate = "test/{code}",
           BodyStyle = WebMessageBodyStyle.Bare,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        ApiResult Test(string code);

        [OperationContract]
        [WebInvoke(Method = WebRequestMethods.Http.Post,
            UriTemplate = ServiceRouteServiceMethodNameConstant.QUERY_SERVICE_ROUTE,
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ApiResult QueryServiceRoute(GetServiceRoutePara para);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
       ConcurrencyMode = ConcurrencyMode.Multiple,
       IncludeExceptionDetailInFaults = true,
       AddressFilterMode = AddressFilterMode.Any)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceRouteService : IServiceRouteService
    {
        private readonly Func<GetServiceRoutePara, ServiceRouteInfo> _getServiceRouteInfo;
        public ServiceRouteService(Func<GetServiceRoutePara, ServiceRouteInfo> getServiceRouteInfo)
        {
            this._getServiceRouteInfo = getServiceRouteInfo;
        }

        public ApiResult Test(string code)
        {
            return new ApiResult(ApiResultStatus.Succes, DateTime.Now.ToString());
        }

        public ApiResult QueryServiceRoute(GetServiceRoutePara para)
        {
            try
            {
                ServiceRouteInfo serviceRouteInfo = this._getServiceRouteInfo(para);
                var json = SerializeEx.WebScriptJsonSerializerObject(serviceRouteInfo);
                return new ApiResult(ApiResultStatus.Succes, json);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "查询服务路由异常");
                return new ApiResult(ApiResultStatus.Exception, $"查询服务路由异常,{ex.Message}");
            }
        }
    }
}
