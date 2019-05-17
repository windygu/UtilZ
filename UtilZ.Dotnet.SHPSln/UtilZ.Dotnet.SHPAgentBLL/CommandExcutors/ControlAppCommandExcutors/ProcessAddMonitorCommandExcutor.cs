using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Commands.AppControl;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.ControlAppCommandExcutors
{

    [CommandExcutorAttribute(SHPCommandDefine.PROCESS_ADD_TO_MONITOR, CommandExcutorType.Asyn)]
    public class ProcessAddMonitorCommandExcutor : AgentCommandExcutorBase<ProcessAddMonitorCommand>
    {
        public ProcessAddMonitorCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ProcessAddMonitorCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            SHPCommandExcuteResult result;
            string info;
            try
            {
                var pro = Process.GetProcessById(cmd.ProcessId);
                var appMonitorItem = new AppMonitorItem(Path.GetFileNameWithoutExtension(pro.MainModule.FileName), pro.MainModule.FileName, pro.MainModule.FileName, string.Empty);
                this.BLL.AppMonitorItemManager.AddMonitorItem(appMonitorItem);
                result = SHPCommandExcuteResult.Sucess;
                info = "成功";
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                info = ex.Message;
                result = SHPCommandExcuteResult.Exception;
            }

            Loger.Info($"执行{cmdName}命令完成...");
            var resTransferCommand = new SHPTransferCommand(transferCommand, SHPCommandDefine.PROCESS_ADD_TO_MONITOR_RES, result, SHPResult.GetBytes(info));
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应{cmdName}命令结果完成...");
        }
    }
}
