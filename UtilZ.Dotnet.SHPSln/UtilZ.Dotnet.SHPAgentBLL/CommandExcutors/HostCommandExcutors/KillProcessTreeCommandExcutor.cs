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
    [CommandExcutorAttribute(SHPCommandDefine.KILL_PROCESS_TREE, CommandExcutorType.Asyn)]
    public class KillProcessTreeCommandExcutor : AgentCommandExcutorBase<KillProcessTreeCommand>
    {
        public KillProcessTreeCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, KillProcessTreeCommand cmd)
        {
            try
            {
                ProcessEx.KillProcessTreeById(cmd.ProcessId);
            }
            catch
            { }

            Loger.Info($"处理结束进程命令完成...");

            var shpResult = new SHPResult(SHPCommandExcuteResult.Sucess, null);
            var shpData = shpResult.ToBytes();
            var resTransferCommand = new SHPTransferCommand(transferCommand.ContextId, SHPConstant.SHP_PLUGINID, SHPCommandDefine.KILL_PROCESS_RES, shpData, transferCommand.Timeout);
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应结束进程树结果完成...");
        }
    }
}
