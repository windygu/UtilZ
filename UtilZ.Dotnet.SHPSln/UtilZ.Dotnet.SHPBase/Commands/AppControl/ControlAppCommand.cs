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
    public class ControlAppCommand : CommandBase
    {
        [TTLVAttribute(2)]
        public string AppName { get; set; }

        [TTLVAttribute(3)]
        public AppControlType ControlType { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ControlAppCommand(string appName, AppControlType controlType)
            : base(SHPCommandDefine.CONTROL_APP)
        {
            this.AppName = appName;
            this.ControlType = controlType;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ControlAppCommand()
        {

        }
    }
}
