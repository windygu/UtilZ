using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.LRPC
{
    /// <summary>
    /// 本地远程调用中心
    /// </summary>
    public class LRPCCenter
    {
        /// <summary>
        /// 远程调用Hashtable集合[key:通道名称(string);value:通道(LRPCChannel)]
        /// </summary>
        private static readonly Hashtable _htChannel = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 创建本地远程调用通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="pro">通道回调</param>
        public static void CreateChannel(string channelName, Func<object, object> pro)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentNullException("channelName", "本地远程调用通道名称不能为空或全空格");
            }

            if (pro == null)
            {
                throw new ArgumentNullException("pro", "通道回调不能为null");
            }

            lock (_htChannel.SyncRoot)
            {
                if (_htChannel.ContainsKey(channelName))
                {
                    throw new ArgumentException(string.Format("已存在名称为:{0}的本地远程调用通道", channelName));
                }

                _htChannel.Add(channelName, new LRPCChannel(channelName, pro));
            }
        }

        /// <summary>
        /// 删除本地远程调用通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        public static void DeleteChannel(string channelName)
        {
            lock (_htChannel.SyncRoot)
            {
                if (_htChannel.ContainsKey(channelName))
                {
                    _htChannel.Remove(channelName);
                }
            }
        }

        /// <summary>
        /// 清空所有通道
        /// </summary>
        public static void ClearChannel()
        {
            lock (_htChannel.SyncRoot)
            {
                _htChannel.Clear();
            }
        }

        /// <summary>
        /// 本地远程调用
        /// </summary>
        /// <param name="channelName">远程通道名称</param>
        /// <param name="obj">远程调用参数</param>
        /// <returns>远程调用结果</returns>
        public static object LRPCCall(string channelName, object obj)
        {
            LRPCChannel channel = _htChannel[channelName] as LRPCChannel;
            if (channel == null)
            {
                lock (_htChannel.SyncRoot)
                {
                    channel = _htChannel[channelName] as LRPCChannel;
                }
            }

            if (channel != null)
            {
                return channel.Pro(obj);
            }
            else
            {
                return null;
            }
        }
    }
}
