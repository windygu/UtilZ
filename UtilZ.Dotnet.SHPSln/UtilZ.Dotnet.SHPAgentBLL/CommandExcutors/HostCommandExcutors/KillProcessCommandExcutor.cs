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
    [CommandExcutorAttribute(SHPCommandDefine.KILL_PROCESS, CommandExcutorType.Asyn)]
    public class KillProcessCommandExcutor : AgentCommandExcutorBase<KillProcessCommand>
    {
        public KillProcessCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, KillProcessCommand cmd)
        {
            try
            {
                var pro = Process.GetProcessById(cmd.ProcessId);
                pro.Kill();
            }
            catch
            { }

            Loger.Info($"处理结束进程命令完成...");

            var shpResult = new SHPResult(SHPCommandExcuteResult.Sucess, null);
            var shpData = shpResult.ToBytes();
            var resTransferCommand = new SHPTransferCommand(transferCommand.ContextId, SHPConstant.SHP_PLUGINID, SHPCommandDefine.KILL_PROCESS_RES, shpData, transferCommand.Timeout);
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应结束进程结果完成...");
        }
    }
}
