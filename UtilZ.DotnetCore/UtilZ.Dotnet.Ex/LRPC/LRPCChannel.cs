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
        private Func<object, object> _pro;

        /// <summary>
        /// 调用本地远程调用回调
        /// </summary>
        /// <param name="obj">调用参数</param>
        /// <returns>调用结果</returns>
        public object OnRaisePro(object obj)
        {
            return this._pro(obj);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channelName">本地远程调用通道名称</param>
        /// <param name="pro">本地远程调用回调</param>
        public LRPCChannel(string channelName, Func<object, object> pro)
        {
            this.ChannelName = channelName;
            this._pro = pro;
        }
    }
}
