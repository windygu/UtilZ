using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPBase.BaseCommandExcutors
{
    public abstract class CommandExcutorBase<T> : ICommandExcutor
    where T : ICommand, new()
    {
        public CommandExcutorBase()
        {

        }

        public CommandExcutorType ExcutorType { get; set; }

        public ISHPNet Net { get; set; }

        public void ProCommand(SHPTransferCommand transferCommand)
        {
            if (this.ExcutorType == CommandExcutorType.Sync)
            {
                this.Net.SyncResNotify(transferCommand);
            }
            else
            {
                if (transferCommand.Cmd == SHPCommandDefine.UP_HOST_STATUS)
                {
                    //主机状态命令频率太高,不输出日志
                    var cmd = new T();
                    cmd.Parse(transferCommand);
                    this.ProAsynCommand(transferCommand, cmd);
                }
                else
                {
                    string cmdName = this.GetCommandName(transferCommand.Cmd);
                    Loger.Info($"收到{cmdName}命令,开始解析...");
                    var cmd = new T();
                    cmd.Parse(transferCommand);
                    Loger.Info($"解析{cmdName}命令成功...");
                    this.ProAsynCommand(transferCommand, cmd);
                }
            }
        }

        protected virtual void ProAsynCommand(SHPTransferCommand transferCommand, T cmd)
        {

        }

        protected string GetCommandName(int cmd)
        {
            return SHPCommandDefine.GetCommandNameByCommand(cmd);
        }

        protected void SendSyncResponse(SHPTransferCommand transferCommand, int cmd, SHPCommandExcuteResult result, byte[] data)
        {
            var resTransferCommand = new SHPTransferCommand(transferCommand, cmd, result, data);
            this.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
        }

        protected void SendAsynResponse(SHPTransferCommand transferCommand, ICommand cmd)
        {
            var resTransferCommand = new SHPTransferCommand(transferCommand, cmd);
            this.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
        }

        protected void SendResponse(SHPTransferCommand transferCommand, IPEndPoint targetEndPoint)
        {
            var policy = TransferPolicyManager.Instance.GetTransferPolicy(targetEndPoint);
            this.Net.SendCommand(transferCommand, policy);
        }
    }
}
