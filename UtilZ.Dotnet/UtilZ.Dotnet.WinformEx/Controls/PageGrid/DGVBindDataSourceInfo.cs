using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.WinformEx.Controls.PageGrid
{
    /// <summary>
    /// 表格绑定数据源信息
    /// </summary>
    public class DGVBindDataSourceInfo
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public object DataSource { get; private set; }

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string DataSourceName { get; private set; }

        /// <summary>
        /// 隐藏列集合
        /// </summary>
        public IEnumerable<string> HidenColumns { get; private set; }

        /// <summary>
        /// 列标题映射字典[key:列名;value:列标题;默认为null]
        /// </summary>
        public Dictionary<string, string> ColHeadInfos { get; private set; }

        /// <summary>
        /// 允许编辑的列集合[当为null或空时,全部列都可编辑;默认为null]
        /// </summary>
        public IEnumerable<string> AllowEditColumns { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="hidenColumns">隐藏列集合</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[当为null或空时,全部列都可编辑;默认为null]</param>
        public DGVBindDataSourceInfo(object dataSource, string dataSourceName, IEnumerable<string> hidenColumns,
            Dictionary<string, string> colHeadInfos, IEnumerable<string> allowEditColumns)
        {
            this.DataSource = dataSource;
            this.DataSourceName = dataSourceName;
            this.HidenColumns = hidenColumns;
            this.ColHeadInfos = colHeadInfos;
            this.AllowEditColumns = allowEditColumns;
        }
    }
}
