using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPBase.Commands.AppControl
{
    public class RemoveMonitorAppCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public string AppName { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public RemoveMonitorAppCommand(string appName)
            : base(SHPCommandDefine.REMOVE_MONITOR_APP)
        {
            this.AppName = appName;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public RemoveMonitorAppCommand()
        {

        }
    }
}
