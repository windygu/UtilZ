using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
      ConcurrencyMode = ConcurrencyMode.Single,
      IncludeExceptionDetailInFaults = true,
      AddressFilterMode = AddressFilterMode.Any)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    internal class AgentService : IAgentService
    {
        private readonly ServiceInstanceManager _serviceInstanceManager;

        public AgentService(ServiceInstanceManager serviceInstanceManager)
        {
            this._serviceInstanceManager = serviceInstanceManager;
        }

        public ApiResult QueryServiceInsInfo(GetServiceInsInfoPara para)
        {
            try
            {
                var serviceInsInfo = this._serviceInstanceManager.QueryServiceInsInfo(para);
                var json = SerializeEx.WebScriptJsonSerializerObject(serviceInsInfo);
                return new ApiResult(ApiResultStatus.Succes, json);
            }
            catch (Exception ex)
            {
                return new ApiResult(ApiResultStatus.Exception, $"查询服务路由异常,{ex.Message}");
            }
        }

        public ApiResult UploadServiceInsListenInfo(ServiceInsListenInfo para)
        {
            try
            {
                this._serviceInstanceManager.UploadServiceInsListenInfo(para);
                return new ApiResult(ApiResultStatus.Succes, string.Empty);
            }
            catch (Exception ex)
            {
                return new ApiResult(ApiResultStatus.Exception, $"查询服务路由异常,{ex.Message}");
            }
        }

        public ApiResult UploadServiceStatus(ServiceStatusInfo para)
        {
            try
            {
                int uploadStatusInterval = this._serviceInstanceManager.UploadServiceStatus(para);
                return new ApiResult(ApiResultStatus.Succes, uploadStatusInterval.ToString());
            }
            catch (Exception ex)
            {
                return new ApiResult(ApiResultStatus.Exception, $"查询服务路由异常,{ex.Message}");
            }
        }
    }

    [ServiceContract(Name = nameof(AgentService))]
    internal interface IAgentService
    {
        [OperationContract]
        [WebInvoke(Method = WebRequestMethods.Http.Post,
            UriTemplate = AgentServiceMethodNameConstant.QUERY_SHP_BASIC_SERVICE_INFO,
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ApiResult QueryServiceInsInfo(GetServiceInsInfoPara para);


        [OperationContract]
        [WebInvoke(Method = WebRequestMethods.Http.Post,
            UriTemplate = AgentServiceMethodNameConstant.UPLOAD_SERVICE_INS_LISTEN_INFO,
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ApiResult UploadServiceInsListenInfo(ServiceInsListenInfo para);

        [OperationContract]
        [WebInvoke(Method = WebRequestMethods.Http.Post,
            UriTemplate = AgentServiceMethodNameConstant.UPLOAD_SERVICE_INS_Status,
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ApiResult UploadServiceStatus(ServiceStatusInfo para);
    }
}
