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
    public class ServiceListenInfoChangeNoifyCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public ServiceListenInfo ServiceListenInfo { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ServiceListenInfoChangeNoifyCommand(ServiceListenInfo serviceListenInfo)
            : base(SHPCommandDefine.SERVICE_LISTEN_CHANGED_NOTIFY)
        {
            this.ServiceListenInfo = serviceListenInfo;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ServiceListenInfoChangeNoifyCommand()
        {

        }
    }
}
