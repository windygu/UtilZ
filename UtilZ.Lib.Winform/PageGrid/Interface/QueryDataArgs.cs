using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PageGrid.Interface
{
    /// <summary>
    /// 查询数据参数
    /// </summary>
    public class QueryDataArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="totalCount">总数据记录数</param>
        /// <param name="pageCount">查询的数据总页数</param>
        /// <param name="pageIndex">当前要查询页数</param>
        public QueryDataArgs(long totalCount, int pageCount, int pageIndex)
        {
            this.TotalCount = totalCount;
            this.PageCount = pageCount;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 总数据记录数
        /// </summary>
        public long TotalCount { get; private set; }

        /// <summary>
        /// 获取查询的数据总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 获取当前要查询页数
        /// </summary>
        public int PageIndex { get; private set; }
    }
}
