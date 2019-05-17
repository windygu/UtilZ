using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.DevOpsMigrateNotify, CommandExcutorType.Asyn)]
    public class DevOpsMigrateNotifyCommandExcutor : AgentCommandExcutorBase<DevOpsMigrateNotifyCommand>
    {
        public DevOpsMigrateNotifyCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, DevOpsMigrateNotifyCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            try
            {
                var devOpsInfo = this.BLL._devOpsInfoCollection.Where(t => { return t.Id == cmd.DOId; }).FirstOrDefault();
                if (devOpsInfo != null)
                {
                    devOpsInfo.DevOpsIPEndPoint = transferCommand.SrcEndPoint;
                }

                var resTransferCommand = new SHPTransferCommand(transferCommand.ContextId, SHPConstant.SHP_PLUGINID, new DevOpsMigrateNotifyResCommand(cmd.HostId), transferCommand.Timeout);
                base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
                Loger.Info($"响应{cmdName}命令结果完成...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            Loger.Info($"处理{cmdName}命令完成...");
        }
    }
}
