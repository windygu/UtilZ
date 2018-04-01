using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.WinformEx.Interface;

namespace UtilZ.Dotnet.WinformEx.Controls.PageGrid.Interface
{
    /// <summary>
    /// 分页数据表格接口
    /// </summary>
    /// <typeparam name="T">控件数据行类型</typeparam>
    /// <typeparam name="G">表格控件类型</typeparam>
    public interface IPageDataGrid<T, G> : IControl
    {
        #region 事件
        /// <summary>
        /// 查询数据事件
        /// </summary>
        event EventHandler<QueryDataArgs> QueryData;

        /// <summary>
        /// 分页大小改变事件
        /// </summary>
        event EventHandler<PageSizeChangedArgs> PageSizeChanged;

        /// <summary>
        /// 数据行单击事件
        /// </summary>
        event EventHandler<DataRowClickArgs> DataRowClick;

        /// <summary>
        /// 数据行双击事件
        /// </summary>
        event EventHandler<DataRowClickArgs> DataRowDoubleClick;

        /// <summary>
        /// 选中行改变事件
        /// </summary>
        event EventHandler<DataRowSelectionChangedArgs> DataRowSelectionChanged;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置分页控件对齐方向
        /// </summary>
        bool AlignDirection { get; set; }

        /// <summary>
        /// 获取或设置列设置区域宽度
        /// </summary>
        int ColumnSettingWidth { get; set; }

        /// <summary>
        /// 获取或设置分页栏是否可见
        /// </summary>
        bool PagingVisible { get; set; }

        /// <summary>
        /// 列设置是否可见
        /// </summary>
        bool ColumnSettingVisible { get; set; }

        /// <summary>
        /// 列显示高级设置是否可见
        /// </summary>
        bool AdvanceSettingVisible { get; set; }

        /// <summary>
        /// 列设置控件状态
        /// </summary>
        PageGridColumnSettingStatus ColumnSettingStatus { get; set; }

        /// <summary>
        /// 行号是否显示
        /// </summary>
        bool RowNumVisible { get; set; }

        /// <summary>
        /// 获取或设置分页大小是否可见
        /// </summary>
        bool PageSizeVisible { get; set; }

        /// <summary>
        /// 获取或设置用户可选择的最大分页大小
        /// </summary>
        int PageSizeMaximum { get; set; }

        /// <summary>
        /// 最后一列显示模式是否默认Fill[true:Fill;false:系统默认;默认值:false]
        /// </summary>
        bool IsLastColumnAutoSizeModeFill { get; set; }

        /// <summary>
        /// 获取表格控件
        /// </summary>
        G GridControl { get; }

        /// <summary>
        /// 获取或设置用户设置数据存放目录
        /// </summary>
        string SettingDirectory { get; set; }

        /// <summary>
        /// 获取当前数据源名称
        /// </summary>
        string DataSourceName { get; }

        /// <summary>
        /// 获取数据源
        /// </summary>
        object DataSource { get; }

        /// <summary>
        /// 获取当前分页信息
        /// </summary>
        PageInfo PageInfo { get; }

        /// <summary>
        /// 获取或设置焦点行索引
        /// </summary>
        int FocusedRowIndex { get; set; }

        /// <summary>
        /// 获取选中行集合
        /// </summary>
        T[] SelectedRows { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="hidenColumns">隐藏列集合</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[当为null或空时,全部列都可编辑;默认为null]</param>
        void ShowData(object dataSource, string dataSourceName = null, IEnumerable<string> hidenColumns = null, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null);

        /// <summary>
        /// 触发查询数据事件
        /// </summary>
        /// <param name="e">参数</param>
        void OnRaiseQueryData(QueryDataArgs e);

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        void SetPageInfo(PageInfo pageInfo);

        /// <summary>
        /// 清空数据
        /// </summary>
        void Clear();
        #endregion
    }
}
