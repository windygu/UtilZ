using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.EVN_CHANGED_NOTIFY_RES, CommandExcutorType.Asyn)]
    public class EvnChangedNotifyResCommandExcutor : DevOpsAsynCommandExcutorBase<EvnChangedNotifyResCommand>
    {
        public EvnChangedNotifyResCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, EvnChangedNotifyResCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            var hostHardInfo = SerializeEx.BinaryDeserialize<HostHardInfo>(cmd.Data);

            var hostManager = this.BLL.HostManager;
            if (!hostManager.IsValidateHost(hostHardInfo.Id))
            {
                hostManager.DeleteHost(hostHardInfo.Id, transferCommand.SrcEndPoint);
                return;
            }

            hostManager.RemoveGetHostHardInfo(hostHardInfo.Id);
            hostManager.UpdateHostHardInfo(hostHardInfo);
            object lrpcResult;
            LRPCCore.TryRemoteCallF(DevOpsConstant.LRPC_REV_HOST_HARD_INFO_CHANNEL, hostHardInfo, out lrpcResult);
            Loger.Info($"处理{cmdName}命令成功,运控[{transferCommand.SrcEndPoint.Address.ToString()}]...");
        }
    }
}
