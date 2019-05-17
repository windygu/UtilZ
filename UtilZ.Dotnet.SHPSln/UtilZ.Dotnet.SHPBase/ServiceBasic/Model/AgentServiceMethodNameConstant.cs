using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    public class AgentServiceMethodNameConstant
    {
        public const string AGENT_SERVICE_BASE_URL = "http://localhost:20011/";

        /// <summary>
        /// QuerySHPBasicServiceInfo/para
        /// </summary>
        public const string QUERY_SHP_BASIC_SERVICE_INFO = "QuerySHPBasicServiceInfo/para";

        /// <summary>
        /// UploadServiceInsListenInfo/para
        /// </summary>
        public const string UPLOAD_SERVICE_INS_LISTEN_INFO = "UploadServiceInsListenInfo/para";

        /// <summary>
        /// UploadServiceStatus/para
        /// </summary>
        public const string UPLOAD_SERVICE_INS_Status = "UploadServiceStatus/para";



        public const string CLOSE_SERVICE_NOTIFY = "CloseServiceNotify/";

        public static string GetServiceRestfullServiceUrl(int port)
        {
            return $"http://localhost:{port}/";
        }
    }
}
