using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.WindowEx.Winform.Interface;

namespace UtilZ.Dotnet.IWindowEx.Winform.PageGrid
{
    /// <summary>
    /// 分页数据显示接口
    /// </summary>
    public interface IPageGridControl : IControl
    {
        #region 事件
        /// <summary>
        /// 查询指定页数据事件
        /// </summary>
        event EventHandler<QueryAssignPageDataArgs> QueryAssignPageData;

        /// <summary>
        /// 数据行单击事件
        /// </summary>
        event EventHandler<DataRowDoubleClickArgs> DataRowClick;

        /// <summary>
        /// 数据行双击事件
        /// </summary>
        event EventHandler<DataRowDoubleClickArgs> DataRowDoubleClick;

        /// <summary>
        /// 选中行改变事件
        /// </summary>
        event EventHandler<SelectionChangedArgs> SelectionChanged;
        #endregion

        #region 属性
        /// <summary>
        /// 获取分页信息
        /// </summary>
        PageInfo PageInfo { get; }

        /// <summary>
        /// 获取数据源名称
        /// </summary>
        string DataSourceName { get; }

        /// <summary>
        /// 获取或设置用户设置数据存放目录
        /// </summary>
        string SettingDirectory { get; set; }

        /// <summary>
        /// 获取数据源
        /// </summary>
        object DataSource { get; }

        /// <summary>
        /// 获取或设置表中的数据是否只读
        /// </summary>
        bool DataReadOnly { get; set; }

        /// <summary>
        /// 获取GridControl
        /// </summary>
        object GridControl { get; }

        /// <summary>
        /// 获取或设置分页栏是否可见
        /// </summary>
        bool PagingVisible { get; set; }

        /// <summary>
        /// 获取或设置焦点行索引
        /// </summary>
        int FocusedRowHandle { get; set; }

        /// <summary>
        /// 获取选中行集合
        /// </summary>
        object[] SelectedRows { get; }

        /// <summary>
        /// 列设置是否可见
        /// </summary>
        bool ColumnSettingVisible { get; set; }

        /// <summary>
        /// 获取或设置与数据表格控件关联的 System.Windows.Forms.ContextMenuStrip
        /// </summary>
        ContextMenuStrip GridContextMenuStrip { get; set; }

        /// <summary>
        /// 获取或设置是否允许多选行
        /// </summary>
        bool MuiltSelect { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 触发查询指定页数据事件
        /// </summary>
        /// <param name="args">参数</param>
        void OnRaiseQueryAssignPageData(QueryAssignPageDataArgs args);

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        void SetPageInfo(PageInfo pageInfo);

        /// <summary>
        /// 设置选中记录提示信息
        /// </summary>
        /// <param name="recordIndex">选中记录索引</param>
        void SetShowPageInfo(int recordIndex);

        /// <summary>
        /// 状态栏控件启用或是禁用状态设置
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="enable">状态[true:启用;false:禁用]</param>
        void EnableStatusControl(string name, bool enable);

        /// <summary>
        /// 获取状态栏控件启用或是禁用状态设置[true:启用;false:禁用]
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>状态[true:启用;false:禁用]</returns>
        bool GetStatusControlEnable(string name);

        /// <summary>
        /// 获取数据表格中显示列列名集合
        /// </summary>
        /// <returns>数据表格中显示列列名集合</returns>
        Dictionary<string, string> GetShowGridColumns();

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[注:当需要有列可编辑时,需要设置DataReadOnly属性值为false,如果该值为true,则会在此方法内自动设置;默认为null]</param>
        void ShowData(string dataSourceName, object dataSource, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null);

        /// <summary>
        /// 清空数据
        /// </summary>
        void Clear();
        #endregion
    }
}
