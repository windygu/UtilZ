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
    [CommandExcutorAttribute(SHPCommandDefine.SERVICE_LISTEN_CHANGED_NOTIFY, CommandExcutorType.Asyn)]
    public class ServiceListenInfoChangeNoifyCommandExcutor : DevOpsAsynCommandExcutorBase<ServiceListenInfoChangeNoifyCommand>
    {
        public ServiceListenInfoChangeNoifyCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ServiceListenInfoChangeNoifyCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            this.BLL.RouteManager.ProServiceInsListenChangedNotify(cmd);
            Loger.Info($"处理{cmdName}命令成功...");
        }
    }
}
