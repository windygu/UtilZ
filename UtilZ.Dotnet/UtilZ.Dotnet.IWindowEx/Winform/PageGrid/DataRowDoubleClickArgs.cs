using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.IWindowEx.Winform.PageGrid
{
    /// <summary>
    /// 历史数据行双击事件参数
    /// </summary>
    public class DataRowDoubleClickArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row">双击的数据行</param>
        /// <param name="rowIndex">行索引</param>
        public DataRowDoubleClickArgs(object row, int rowIndex)
        {
            this.Row = row;
            this.RowIndex = rowIndex;
        }

        /// <summary>
        /// 获取双击的数据行
        /// </summary>
        public object Row { get; private set; }

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; private set; }
    }
}
