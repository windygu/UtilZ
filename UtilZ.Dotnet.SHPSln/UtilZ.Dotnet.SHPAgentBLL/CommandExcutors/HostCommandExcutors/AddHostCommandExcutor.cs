using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.ADD_HOST_REQ, CommandExcutorType.Asyn)]
    public class AddHostCommandExcutor : AgentCommandExcutorBase<AddHostCommand>
    {
        public AddHostCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, AddHostCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            SHPCommandExcuteResult result;
            string info;

            try
            {
                var devOpsInfo = new DevOpsInfo(transferCommand.SrcEndPoint, cmd);
                var existDevOpsInfos = this.BLL._devOpsInfoCollection.Where(t => { return t.Id == devOpsInfo.Id; }).ToArray();
                if (existDevOpsInfos.Length > 0)
                {
                    foreach (var existDevOpsInfo in existDevOpsInfos)
                    {
                        this.BLL._devOpsInfoCollection.Remove(existDevOpsInfo);
                    }
                }

                this.BLL._devOpsInfoCollection.Add(devOpsInfo);
                this.BLL._dal.AddDevOps(devOpsInfo);
                Loger.Info($"处理{cmdName}命令完成...");
                result = SHPCommandExcuteResult.Sucess;
                info = cmd.HostId.ToString();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                result = SHPCommandExcuteResult.Exception;
                info = ex.Message;
            }

            var resTransferCommand = new SHPTransferCommand(transferCommand, SHPCommandDefine.ADD_HOST_RES, result, SHPResult.GetBytes(info));
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应{cmdName}命令结果完成...");
        }
    }
}
