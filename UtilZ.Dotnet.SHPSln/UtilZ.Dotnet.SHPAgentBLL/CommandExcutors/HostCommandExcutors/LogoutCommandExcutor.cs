using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.LOGOUT, CommandExcutorType.Asyn)]
    public class LogoutCommandExcutor : AgentCommandExcutorBase<LogoutCommand>
    {
        public LogoutCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, LogoutCommand cmd)
        {
            ProcessEx.SynExcuteCmd("shutdown", "/l");
            Loger.Info($"处理注销主机命令完成...");
        }
    }
}
