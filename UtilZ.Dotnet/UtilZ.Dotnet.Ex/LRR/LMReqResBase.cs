using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.LRR
{
    /// <summary>
    /// 本地消息请求响应基类
    /// </summary>
    [Serializable]
    public abstract class LMReqResBase
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LMReqResBase()
        {

        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">消息编号</param>
        public LMReqResBase(int id)
        {
            this.ID = id;
        }
    }
}
