using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.LRPC
{
    /// <summary>
    /// 远程调用通道
    /// </summary>
    internal class LRPCChannel
    {
        /// <summary>
        /// 本地远程调用通道名称
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// 本地远程调用回调
        /// </summary>
        public Func<object, object> Pro { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channelName">本地远程调用通道名称</param>
        /// <param name="pro">本地远程调用回调</param>
        public LRPCChannel(string channelName, Func<object, object> pro)
        {
            this.ChannelName = channelName;
            this.Pro = pro;
        }
    }
}
