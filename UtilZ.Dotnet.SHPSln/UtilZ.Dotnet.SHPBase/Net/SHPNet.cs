using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Exceptions;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPBase.Net
{
    public class SHPNet : ISHPNet
    {
        private readonly SHPConfigBase _config;
        private readonly TransferChannel _transferChannel;
        public SHPNet(TransferChannel transferChannel, SHPConfigBase config)
        {
            this._transferChannel = transferChannel;
            this._config = config;
        }

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">目的地主机IP</param>
        /// <returns>命令执行结果</returns>
        public byte[] SendInteractiveCommandHost(SHPTransferCommand transferCommand, string ip)
        {
            return this.SendInteractiveCommand(transferCommand, new IPEndPoint(IPAddress.Parse(ip), this._config.AgentPort));
        }

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        /// <returns>命令执行结果</returns>
        public byte[] SendInteractiveCommandDevOps(SHPTransferCommand transferCommand, string ip)
        {
            return this.SendInteractiveCommand(transferCommand, new IPEndPoint(IPAddress.Parse(ip), this._config.DevOpsPort));
        }

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        /// <returns>命令执行结果</returns>
        public byte[] SendInteractiveCommand(SHPTransferCommand transferCommand, string ip, int port)
        {
            return this.SendInteractiveCommand(transferCommand, new IPEndPoint(IPAddress.Parse(ip), port));
        }

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ipEndPoint">运控主机</param>
        /// <returns>命令执行结果</returns>
        public byte[] SendInteractiveCommand(SHPTransferCommand transferCommand, IPEndPoint ipEndPoint)
        {
            var policy = TransferPolicyManager.Instance.GetTransferPolicy(ipEndPoint);
            return this.SendInteractiveCommand(transferCommand, policy);
        }

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="policy">发送策略</param>
        /// <returns>命令执行结果</returns>
        public byte[] SendInteractiveCommand(SHPTransferCommand transferCommand, TransferPolicy policy)
        {
            var timeout = transferCommand.Timeout;
            var contextId = transferCommand.ContextId;
            var evetnHandleInfo = AutoEventWaitHandleManager.CreateEventWaitHandle(contextId, timeout * 2);
            try
            {
                var buffer = transferCommand.GenerateBuffer();
                this._transferChannel.SendData(buffer, policy);
                if (evetnHandleInfo.EventWaitHandle.WaitOne(timeout))
                {
                    var result = evetnHandleInfo.Tag as SHPResult;
                    if (result.Result == SHPCommandExcuteResult.Sucess)
                    {
                        return result.Data;
                    }

                    throw new SHPOperationException(result);
                }
                else
                {
                    throw new SHPOperationException(SHPCommandExcuteResult.Timeout, "执行命令超时");
                }
            }
            finally
            {
                AutoEventWaitHandleManager.RemoveEventWaitHandle(contextId);
                try
                {
                    evetnHandleInfo.EventWaitHandle.Dispose();
                }
                catch (NullReferenceException)
                { }
                catch (ObjectDisposedException)
                { }
            }
        }

        public void SyncResNotify(SHPTransferCommand transferCommand)
        {
            var evetnHandleInfo = AutoEventWaitHandleManager.GetEventWaitHandleInfo(transferCommand.ContextId);
            if (evetnHandleInfo != null)
            {
               var tag = SerializeEx.BinaryDeserialize<SHPResult>(transferCommand.Data);
                evetnHandleInfo.TrySet(tag);
            }
        }

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">目的地主机IP</param>
        public void SendCommandHost(SHPTransferCommand transferCommand, string ip)
        {
            this.SendCommandTo(transferCommand, new IPEndPoint(IPAddress.Parse(ip), this._config.AgentPort));
        }

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        public void SendCommandDevOps(SHPTransferCommand transferCommand, string ip)
        {
            this.SendCommandTo(transferCommand, new IPEndPoint(IPAddress.Parse(ip), this._config.DevOpsPort));
        }

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        public void SendCommand(SHPTransferCommand transferCommand, string ip, int port)
        {
            this.SendCommandTo(transferCommand, new IPEndPoint(IPAddress.Parse(ip), port));
        }

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        public void SendCommandTo(SHPTransferCommand transferCommand, IPEndPoint ipEndPoint)
        {
            var policy = TransferPolicyManager.Instance.GetTransferPolicy(ipEndPoint);
            this.SendCommand(transferCommand, policy);
        }

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="policy">发送策略</param>
        public void SendCommand(SHPTransferCommand transferCommand, TransferPolicy policy)
        {
            var buffer = transferCommand.GenerateBuffer();
            this._transferChannel.SendData(buffer, policy);
        }

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {

        }
    }
}
