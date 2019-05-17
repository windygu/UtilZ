using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.Commands.Service;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.RouteCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.SERVICE_DEPLOY_REQ, CommandExcutorType.Asyn)]
    public class ServiceDeployReqCommandExcutor : AgentCommandExcutorBase<ServiceDeployReqCommand>
    {
        public ServiceDeployReqCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ServiceDeployReqCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            SHPCommandExcuteResult result;
            byte[] data;

            try
            {
                ServiceListenInfo serviceListenInfo = this.BLL.ServiceInstanceManager.DeployServiceInstance(cmd.ServiceDeployPara);
                data = SerializeEx.BinarySerialize(serviceListenInfo);
                result = SHPCommandExcuteResult.Sucess;
                Loger.Info($"处理{cmdName}命令完成...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                result = SHPCommandExcuteResult.Exception;
                data = SHPResult.GetBytes(ex.Message);
            }

            var resTransferCommand = new SHPTransferCommand(transferCommand, SHPCommandDefine.SERVICE_DEPLOY_RES, result, data);
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应{cmdName}命令结果完成...");
        }
    }
}
