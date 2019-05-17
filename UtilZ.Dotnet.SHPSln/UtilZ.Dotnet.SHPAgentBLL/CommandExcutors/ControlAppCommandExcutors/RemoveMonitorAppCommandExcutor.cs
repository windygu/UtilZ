using System;
using System.Collections.Generic;
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
    [CommandExcutorAttribute(SHPCommandDefine.REMOVE_MONITOR_APP, CommandExcutorType.Asyn)]
    public class RemoveMonitorAppCommandExcutor : AgentCommandExcutorBase<RemoveMonitorAppCommand>
    {
        public RemoveMonitorAppCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, RemoveMonitorAppCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            AppMonitorItem[] appMonitorItems;

            SHPCommandExcuteResult result;
            string info;
            try
            {
                appMonitorItems = this.BLL.AppMonitorItemManager.AppMonitorList.Where(t => { return string.Equals(t.AppName, cmd.AppName); }).ToArray();
                if (appMonitorItems.Length > 0)
                {
                    //判断是否可以移除
                    foreach (var appMonitorItem in appMonitorItems)
                    {
                        this.BLL.AssertRemoveMonitor(appMonitorItem);
                    }

                    //执行移除操作
                    foreach (var appMonitorItem in appMonitorItems)
                    {
                        this.BLL.AppMonitorItemManager.RemoveMonitorItem(appMonitorItem);
                    }

                    result = SHPCommandExcuteResult.Sucess;
                    info = "成功";
                }
                else
                {
                    info = $"应用[{cmd.AppName}不存在]";
                    result = SHPCommandExcuteResult.Fail;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                info = ex.Message;
                result = SHPCommandExcuteResult.Exception;
            }

            Loger.Info($"执行{cmdName}命令完成...");
            base.SendSyncResponse(transferCommand, SHPCommandDefine.PROCESS_ADD_TO_MONITOR_RES, result, SHPResult.GetBytes(info));
            Loger.Info($"响应{cmdName}命令结果完成...");
        }
    }
}
