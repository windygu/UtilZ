using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Commands.Service;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.RouteCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.SERVICE_INS_DELETE_REQ, CommandExcutorType.Asyn)]
    public class ServiceInsDeleteReqCommandExcutor : AgentCommandExcutorBase<ServiceInsDeleteReqCommand>
    {
        public ServiceInsDeleteReqCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ServiceInsDeleteReqCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            ServiceRouteRemoveResPara resPara;

            try
            {
                resPara = this.BLL.ServiceInstanceManager.ProRemoveServiceInstance(cmd);
                Loger.Info($"处理{cmdName}命令完成...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                resPara = new ServiceRouteRemoveResPara(ex.Message);
            }

            var resTransferCommand = new SHPTransferCommand(transferCommand.ContextId, SHPConstant.SHP_PLUGINID, new ServiceInsDeleteResCommand(resPara), transferCommand.Timeout);
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应{cmdName}命令结果完成...");
        }
    }
}
