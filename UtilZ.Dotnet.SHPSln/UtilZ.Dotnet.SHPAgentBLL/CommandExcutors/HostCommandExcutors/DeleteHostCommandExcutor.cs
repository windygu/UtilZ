using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.DELETE_HOST, CommandExcutorType.Asyn)]
    public class DeleteHostCommandExcutor : AgentCommandExcutorBase<DeleteHostCommand>
    {
        public DeleteHostCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, DeleteHostCommand cmd)
        {
            var deleDevOpsList = this.BLL._devOpsInfoCollection.RemoveByHostId(cmd.HostId);
            this.BLL._dal.DeleteDevOps(deleDevOpsList);
            Loger.Info($"删除主机成功,运控[{transferCommand.SrcEndPoint.Address.ToString()}]...");
        }
    }
}
