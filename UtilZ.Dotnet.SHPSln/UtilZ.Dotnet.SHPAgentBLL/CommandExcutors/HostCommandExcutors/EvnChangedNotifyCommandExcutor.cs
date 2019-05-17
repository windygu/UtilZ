using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.EVN_CHANGED_NOTIFY, CommandExcutorType.Asyn)]
    public class EvnChangedNotifyCommandExcutor : AgentCommandExcutorBase<EvnChangedNotifyCommand>
    {
        public EvnChangedNotifyCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, EvnChangedNotifyCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            this.BLL._devOpsInfoCollection.Update(cmd);
            var hostHardInfo = HostHardInfoHelper.GetHostHardInfo();
            hostHardInfo.Id = cmd.HostId;
            var hostHardInfoResCommand = new EvnChangedNotifyResCommand(hostHardInfo);
            var resTransferCommand = new SHPTransferCommand(transferCommand.ContextId, SHPConstant.SHP_PLUGINID, hostHardInfoResCommand, transferCommand.Timeout);
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"处理{cmdName}命令完成...");
        }
    }
}
