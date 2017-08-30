using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.LocalMeseageQueue
{
    /// <summary>
    /// 本地消息队列数据消息
    /// </summary>
    public class LMQDataMessage : LMQBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="data">数据</param>
        public LMQDataMessage(string topic, object data)
            : base(topic)
        {
            this.Data = data;
        }
    }
}
