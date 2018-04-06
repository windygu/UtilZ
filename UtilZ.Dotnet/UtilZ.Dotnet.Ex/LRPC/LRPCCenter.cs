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
        /// 创建或替换已存在的本地远程调用通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="pro">通道回调</param>
        public static void CreateOrReplaceChannel(string channelName, Func<object, object> pro)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentNullException("channelName", "本地远程调用通道名称不能为空或全空格");
            }

            if (pro == null)
            {
                throw new ArgumentNullException("pro", "回调委托不能为null");
            }
            
            lock (_htChannel.SyncRoot)
            {
                _htChannel[channelName] = new LRPCChannel(channelName, pro);
            }
        }

        /// <summary>
        /// 创建本地远程调用通道[返回值:true:创建成功;false:创建失败,该通道已存在]
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="pro">通道回调</param>
        /// <returns>创建结果</returns>
        public static bool TryCreateChannel(string channelName, Func<object, object> pro)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentNullException("channelName", "本地远程调用通道名称不能为空或全空格");
            }

            if (pro == null)
            {
                throw new ArgumentNullException("pro", "回调委托不能为null");
            }

            lock (_htChannel.SyncRoot)
            {
                if (_htChannel.ContainsKey(channelName))
                {
                    return false;
                }

                _htChannel.Add(channelName, new LRPCChannel(channelName, pro));
                return true;
            }
        }

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
                throw new ArgumentNullException("pro", "回调委托不能为null");
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
        /// 是否存在本地远程调用通道[存在返回true;不存在返回false]
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <returns>存在返回true;不存在返回false</returns>
        public static bool ExistChannel(string channelName)
        {
            return _htChannel.ContainsKey(channelName);
        }

        /// <summary>
        /// 获取已创建的本地远程调用通道名称列表
        /// </summary>
        /// <returns>已创建的本地远程调用通道名称列表</returns>
        public static List<string> GetChannelNames()
        {
            var channelNames = new List<string>();
            lock (_htChannel.SyncRoot)
            {
                foreach (string channelName in _htChannel.Keys)
                {
                    channelNames.Add(channelName);
                }
            }

            return channelNames;
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
        /// 本地远程调用[如果通道未创建则会抛出]NotFoundLRPCChannelException
        /// </summary>
        /// <param name="channelName">远程通道名称</param>
        /// <param name="obj">远程调用参数</param>
        /// <returns>远程调用输出结果</returns>
        public static object RemoteCall(string channelName, object obj)
        {
            LRPCChannel channel = _htChannel[channelName] as LRPCChannel;
            if (channel == null)
            {
                throw new NotFoundLRPCChannelException(string.Format("名称为:{0}的远程调用通道未创建", channelName));
            }

            return channel.OnRaisePro(obj);
        }

        /// <summary>
        /// 尝试本地远程调用[返回值:true:调用成功;false:调用失败]
        /// </summary>
        /// <param name="channelName">远程通道名称</param>
        /// <param name="obj">远程调用参数</param>
        /// <param name="result">远程调用输出结果</param>
        /// <returns>远程调用结果</returns>
        public static bool TryRemoteCall(string channelName, object obj, out object result)
        {
            bool callResult;
            LRPCChannel channel = _htChannel[channelName] as LRPCChannel;
            if (channel == null)
            {
                result = null;
                callResult = false;
            }
            else
            {
                result = channel.OnRaisePro(obj);
                callResult = true;
            }

            return callResult;
        }
    }
}
