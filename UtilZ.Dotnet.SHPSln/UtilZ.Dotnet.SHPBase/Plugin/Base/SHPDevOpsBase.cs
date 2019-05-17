using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    public abstract class SHPDevOpsBase : SHPPluginBase, ISHPDevOpsBase
    {
        public SHPDevOpsBase()
        {

        }

        public void RegisterSHPResultCommand(int cmd)
        {
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(cmd);
        }

        public void RegisterSHPResultCommand(IEnumerable<int> cmds)
        {
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(cmds);
        }

        public virtual ISHPNet Net { get; set; }

        public void ExcuteCommand(SHPTransferCommand transferCommand)
        {
            if (SHPSyncCommandResultExcutor.ContainsCmd(transferCommand.Cmd))
            {
                SHPSyncCommandResultExcutor.Instance.ProCommand(transferCommand);
            }
            else
            {
                this.PrimitiveExcuteCommand(transferCommand);
            }
        }

        protected abstract void PrimitiveExcuteCommand(SHPTransferCommand transferCommand);
    }
}
