using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPBase.Commands.Service
{
    /// <summary>
    /// 因为部署服务为同步操作,所以没有对应的响应命令
    /// </summary>
    public class ServiceDeployReqCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public ServiceDeployPara ServiceDeployPara { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ServiceDeployReqCommand(ServiceDeployPara para)
            : base(SHPCommandDefine.SERVICE_DEPLOY_REQ)
        {
            this.ServiceDeployPara = para;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ServiceDeployReqCommand()
        {

        }
    }
}
