using System;
using System.Collections.Generic;
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
    [CommandExcutorAttribute(SHPCommandDefine.RESTART, CommandExcutorType.Asyn)]
    public class RestartCommandExcutor : AgentCommandExcutorBase<RestartCommand>
    {
        public RestartCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, RestartCommand cmd)
        {
            ProcessEx.SynExcuteCmd("shutdown", "/r /t 0");
            Loger.Info($"处理重启主机命令完成...");
        }
    }
}
