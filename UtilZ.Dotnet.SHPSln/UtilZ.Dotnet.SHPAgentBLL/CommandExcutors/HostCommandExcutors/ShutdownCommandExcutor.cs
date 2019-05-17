using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.SHUTDOWN, CommandExcutorType.Asyn)]
    public class ShutdownCommandExcutor : AgentCommandExcutorBase<ShutdownCommand>
    {
        public ShutdownCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ShutdownCommand cmd)
        {
            ProcessEx.SynExcuteCmd("shutdown", "/s /t 0");
            Loger.Info($"处理关机主机命令完成...");
        }
    }
}
