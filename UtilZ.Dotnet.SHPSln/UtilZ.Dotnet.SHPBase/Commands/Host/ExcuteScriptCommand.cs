using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class ExcuteScriptCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public ScriptType ScriptType { get; set; }

        [TTLVAttribute(102)]
        public string Content { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ExcuteScriptCommand(ScriptType scriptType, string content)
            : base(SHPCommandDefine.EXCUTE_SCRPT_REQ)
        {
            this.ScriptType = scriptType;
            this.Content = content;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ExcuteScriptCommand()
        {

        }
    }
}
