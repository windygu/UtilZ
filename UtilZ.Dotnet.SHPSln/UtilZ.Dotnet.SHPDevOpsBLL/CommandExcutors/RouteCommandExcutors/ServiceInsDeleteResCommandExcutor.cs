using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Commands.Service;

namespace UtilZ.Dotnet.SHPDevOpsBLL.CommandExcutors.RouteCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.SERVICE_INS_DELETE_RES, CommandExcutorType.Asyn)]
    public class ServiceInsDeleteResCommandExcutor : DevOpsAsynCommandExcutorBase<ServiceInsDeleteResCommand>
    {
        public ServiceInsDeleteResCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ServiceInsDeleteResCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            this.BLL.RouteManager.ProServiceInsRemoveResCommand(cmd);
            Loger.Info($"处理{cmdName}命令成功...");
        }
    }
}
