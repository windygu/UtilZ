using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.LRR
{
    /// <summary>
    /// 本地消息响应
    /// </summary>
    [Serializable]
    public class LMRes : LMReqResBase
    {
        /// <summary>
        /// 响应委托
        /// </summary>
        public Func<object, object> Res { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LMRes() : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="res">响应委托</param>
        public LMRes(int id, Func<object, object> res) : base(id)
        {
            this.Res = res;
        }
    }
}
