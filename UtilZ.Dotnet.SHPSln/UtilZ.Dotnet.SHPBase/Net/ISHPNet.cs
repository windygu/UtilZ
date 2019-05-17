using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Net
{
    /// <summary>
    /// 平台传输接口
    /// </summary>
    public interface ISHPNet : IDisposable
    {
        /// <summary>
        /// 发送交互式命令到主机,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">目的地主机IP</param>
        /// <returns>命令执行结果</returns>
        byte[] SendInteractiveCommandHost(SHPTransferCommand transferCommand, string ip);

        /// <summary>
        /// 发送交互式命令到运控,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        /// <returns>命令执行结果</returns>
        byte[] SendInteractiveCommandDevOps(SHPTransferCommand transferCommand, string ip);

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        /// <returns>命令执行结果</returns>
        byte[] SendInteractiveCommand(SHPTransferCommand transferCommand, string ip, int port);

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ipEndPoint">运控主机</param>
        /// <returns>命令执行结果</returns>
        byte[] SendInteractiveCommand(SHPTransferCommand transferCommand, IPEndPoint ipEndPoint);

        /// <summary>
        /// 发送交互式命令,需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="policy">发送策略</param>
        /// <returns>命令执行结果</returns>
        byte[] SendInteractiveCommand(SHPTransferCommand transferCommand, TransferPolicy policy);

        /// <summary>
        /// 同步命令收到应答通知
        /// </summary>
        /// <param name="transferCommand">应用命令</param>
        void SyncResNotify(SHPTransferCommand transferCommand);

        /// <summary>
        /// 发送命令到主机,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">目的地主机IP</param>
        void SendCommandHost(SHPTransferCommand transferCommand, string ip);

        /// <summary>
        /// 发送命令到运控,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        void SendCommandDevOps(SHPTransferCommand transferCommand, string ip);

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        void SendCommand(SHPTransferCommand transferCommand, string ip, int port);

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="ip">运控主机IP</param>
        void SendCommandTo(SHPTransferCommand transferCommand, IPEndPoint ipEndPoint);

        /// <summary>
        /// 发送命令,不需要应答
        /// </summary>
        /// <param name="transferCommand">要发送命令</param>
        /// <param name="policy">发送策略</param>
        void SendCommand(SHPTransferCommand transferCommand, TransferPolicy policy);
    }
}
