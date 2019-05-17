using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPDevOpsBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.DevOpsMigrateNotifyRes, CommandExcutorType.Asyn)]
    public class DevOpsMigrateNotifyResCommandExcutor : DevOpsAsynCommandExcutorBase<DevOpsMigrateNotifyResCommand>
    {
        public DevOpsMigrateNotifyResCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, DevOpsMigrateNotifyResCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            var hostManager = this.BLL.HostManager;
            if (!hostManager.IsValidateHost(cmd.HostId))
            {
                return;
            }

            hostManager.ProDevOpsMigrateNotifyRes(cmd);
        }
    }
}
