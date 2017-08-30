using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Log;

namespace UtilZ.Lib.Base.LocalMeseageQueue
{
    /// <summary>
    /// 订阅项
    /// </summary>
    [Serializable]
    public class SubscibeItem : LMQBase
    {
        /// <summary>
        /// 消息通知委托
        /// </summary>
        public Action<LMQDataMessage> MessageNotify;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topic">主题</param>
        public SubscibeItem(string topic)
            : base(topic)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messageNotify">消息通知委托</param>
        public SubscibeItem(string topic, Action<LMQDataMessage> messageNotify)
            : base(topic)
        {
            this.MessageNotify = messageNotify;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">数据消息</param>
        internal void Publish(object message)
        {
            try
            {
                var handler = this.MessageNotify;
                if (handler != null)
                {
                    handler(message as LMQDataMessage);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
