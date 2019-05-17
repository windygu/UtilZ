using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Commands.AppControl;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.ControlAppCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.CONTROL_APP, CommandExcutorType.Asyn)]
    public class StartAppCommandExcutor : AgentCommandExcutorBase<ControlAppCommand>
    {
        public StartAppCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ControlAppCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            AppMonitorItem[] appMonitorItems;
            lock (this.BLL.AppMonitorItemManager.AppMonitorList.SyncRoot)
            {
                appMonitorItems = this.BLL.AppMonitorItemManager.AppMonitorList.Where(t => { return string.Equals(t.AppName, cmd.AppName); }).ToArray();
            }

            SHPCommandExcuteResult result;
            string info;
            try
            {
                if (appMonitorItems.Length > 0)
                {
                    foreach (var appMonitorItem in appMonitorItems)
                    {
                        switch (cmd.ControlType)
                        {
                            case AppControlType.Start:
                                this.BLL.AppMonitorItemManager.StartMonitorItem(appMonitorItem);
                                break;
                            case AppControlType.Stop:
                                this.BLL.AppMonitorItemManager.StopMonitorItem(appMonitorItem);
                                break;
                            case AppControlType.Restart:
                                this.BLL.AppMonitorItemManager.RestartMonitorItem(appMonitorItem);
                                break;
                        }
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
            var resTransferCommand = new SHPTransferCommand(transferCommand, SHPCommandDefine.CONTROL_APP_RES, result, SHPResult.GetBytes(info));
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应{cmdName}命令结果完成...");
        }
    }
}
