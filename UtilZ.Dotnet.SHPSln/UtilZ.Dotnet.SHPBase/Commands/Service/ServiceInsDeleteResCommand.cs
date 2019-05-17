using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPBase.Commands.Service
{
    public class ServiceInsDeleteResCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public ServiceRouteRemoveResPara ResPara { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ServiceInsDeleteResCommand(ServiceRouteRemoveResPara para)
            : base(SHPCommandDefine.SERVICE_INS_DELETE_RES)
        {
            this.ResPara = para;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ServiceInsDeleteResCommand()
        {

        }        
    }
}
