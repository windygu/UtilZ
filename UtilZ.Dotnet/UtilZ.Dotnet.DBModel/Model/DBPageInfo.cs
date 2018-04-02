using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBModel.Model
{
    /// <summary>
    /// 页信息
    /// </summary>
    [Serializable]
    public class DBPageInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageCount">页数</param>
        /// <param name="totalCount">总数据记录数</param>
        /// <param name="pageSize">页大小</param>
        public DBPageInfo(int pageCount, long totalCount, int pageSize)
        {
            this.PageCount = pageCount;
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 总数据记录数
        /// </summary>
        public long TotalCount { get; private set; }

        /// <summary>
        /// 数据页大小
        /// </summary>
        public int PageSize { get; private set; }
    }
}
