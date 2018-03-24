using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.LRR
{
    /// <summary>
    /// 本地消息请求
    /// </summary>
    [Serializable]
    public class LMReq : LMReqResBase
    {
        /// <summary>
        /// 请求参数
        /// </summary>
        public object ReqPara { get; set; }

        /// <summary>
        /// 响应结果
        /// </summary>
        public object ResResult { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LMReq() : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">消息编号</param>
        /// <param name="reqPara">请求参数</param>
        public LMReq(int id, object reqPara) : base(id)
        {
            this.ReqPara = reqPara;
        }
    }
}
