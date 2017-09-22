using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PageGrid.Interface
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageCount">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        public PageInfo(int pageCount, int pageSize, int pageIndex)
        {
            this.PageCount = pageCount;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// 分总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; private set; }
    }
}
