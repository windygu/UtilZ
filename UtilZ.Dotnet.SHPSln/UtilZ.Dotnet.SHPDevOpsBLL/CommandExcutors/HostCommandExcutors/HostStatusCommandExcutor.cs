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
    [CommandExcutorAttribute(SHPCommandDefine.UP_HOST_STATUS, CommandExcutorType.Asyn)]
    public class HostStatusCommandExcutor : DevOpsAsynCommandExcutorBase<HostStatusInfoUploadCommand>
    {
        public HostStatusCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, HostStatusInfoUploadCommand cmd)
        {
            var hostStatusInfo = cmd.StatusInfo;
            var hostManager = this.BLL.HostManager;
            if (!hostManager.IsValidateHost(hostStatusInfo.Id))
            {
                hostManager.DeleteHost(hostStatusInfo.Id, transferCommand.SrcEndPoint);
                return;
            }

            base.BLL.HostStatusInfoManager.AddHostStatusInfo(hostStatusInfo);
            object lrpcResult;
            LRPCCore.TryRemoteCallF(DevOpsConstant.LRPC_REV_HOST_STATUS_INFO_CHANNEL, hostStatusInfo, out lrpcResult);
        }
    }
}
