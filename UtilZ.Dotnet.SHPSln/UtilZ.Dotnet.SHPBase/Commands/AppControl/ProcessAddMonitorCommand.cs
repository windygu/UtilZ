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
    public class ProcessAddMonitorCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public int ProcessId { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ProcessAddMonitorCommand(int processId)
            : base(SHPCommandDefine.PROCESS_ADD_TO_MONITOR)
        {
            this.ProcessId = processId;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ProcessAddMonitorCommand()
        {

        }
    }
}
