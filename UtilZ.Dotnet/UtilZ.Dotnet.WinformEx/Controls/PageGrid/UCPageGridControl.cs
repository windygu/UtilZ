using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.WinformEx.Controls.PageGrid.Interface;
using System.IO;
using System.Xml.Linq;
using System.Dynamic;
using System.Collections.ObjectModel;
using UtilZ.Dotnet.WinformEx.Base;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.WinformEx.Controls.PageGrid
{
    /// <summary>
    /// 分页数据表格
    /// </summary>
    public partial class UCPageGridControl : UserControl, IPageDataGrid<DataGridViewRow, DataGridViewEx>
    {
        #region IPageDataGrid接口
        #region 事件
        /// <summary>
        /// 查询数据事件
        /// </summary>
        [Description("查询数据事件")]
        [Category("分页数据显示控件")]
        public event EventHandler<QueryDataArgs> QueryData;

        /// <summary>
        /// 分页大小改变事件
        /// </summary>
        [Description("分页大小改变事件")]
        [Category("分页数据显示控件")]
        public event EventHandler<PageSizeChangedArgs> PageSizeChanged;

        /// <summary>
        /// 数据行单击事件
        /// </summary>
        [Description("数据行单击事件")]
        [Category("分页数据显示控件")]
        public event EventHandler<DataRowClickArgs> DataRowClick;

        /// <summary>
        /// 数据行双击事件
        /// </summary>
        [Description("数据行双击事件")]
        [Category("分页数据显示控件")]
        public event EventHandler<DataRowClickArgs> DataRowDoubleClick;

        /// <summary>
        /// 选中行改变事件
        /// </summary>
        [Description("选中行改变事件")]
        [Category("分页数据显示控件")]
        public event EventHandler<DataRowSelectionChangedArgs> DataRowSelectionChanged;
        #endregion

        #region 属性
        /// <summary>
        /// 获取UI是否处于设计器模式
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UIDesignMode
        {
            get { return this.DesignMode; }
        }

        private readonly ReadOnlyCollection<Control> _pageControls;

        private bool _alignDirection = true;

        /// <summary>
        /// 获取或设置分页控件对齐方向[true:靠左;false:靠右]
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("分页控件对齐方向")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool AlignDirection
        {
            get { return _alignDirection; }
            set
            {
                if (_alignDirection == value)
                {
                    return;
                }

                _alignDirection = value;

                Control[] pageControls;
                FlowDirection flowDirection;
                if (_alignDirection)
                {
                    pageControls = _pageControls.ToArray();
                    flowDirection = FlowDirection.LeftToRight;

                }
                else
                {
                    pageControls = _pageControls.Reverse().ToArray();
                    flowDirection = FlowDirection.RightToLeft;
                }

                panelPage.Controls.Clear();
                panelPage.FlowDirection = flowDirection;
                panelPage.Controls.AddRange(pageControls);
            }
        }

        /// <summary>
        /// 获取或设置列设置区域宽度
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列设置区域宽度")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public int ColumnSettingWidth
        {
            get { return _fPageGridColumnsSetting.Width; }
            set { _fPageGridColumnsSetting.Width = value; }
        }

        /// <summary>
        /// 获取或设置分页栏是否可见
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("分页栏是否可见[true:可见;false:隐藏]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool PagingVisible
        {
            get { return panelPage.Visible; }
            set { panelPage.Visible = value; }
        }

        /// <summary>
        /// 获取或设置列设置是否可见
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列设置是否可见[true:可见;false:隐藏]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool ColumnSettingVisible
        {
            get { return this._fPageGridColumnsSetting.Visible; }
            set { this._fPageGridColumnsSetting.Visible = value; }
        }

        /// <summary>
        /// 列显示高级设置是否可见
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列显示高级设置是否可见")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool AdvanceSettingVisible
        {
            get { return this._fPageGridColumnsSetting.AdvanceSettingVisible; }
            set { this._fPageGridColumnsSetting.AdvanceSettingVisible = value; }
        }

        /// <summary>
        /// 获取或设置列设置控件状态
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列设置控件状态")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public PageGridColumnSettingStatus ColumnSettingStatus
        {
            get { return _fPageGridColumnsSetting.ColumnSettingStatus; }
            set { _fPageGridColumnsSetting.ColumnSettingStatus = value; }
        }

        /// <summary>
        /// 行号是否显示
        /// </summary>
        private bool _rowNumVisible = false;

        /// <summary>
        /// 获取或设置行号是否显示
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("行号是否显示[true:显示行号;false:不显示行号]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool RowNumVisible
        {
            get { return _rowNumVisible; }
            set
            {
                if (_rowNumVisible == value)
                {
                    return;
                }

                _rowNumVisible = value;
                if (_rowNumVisible)
                {
                    this._dataGridView.RowPostPaint += DataGridView_RowPostPaint;
                }
                else
                {
                    this._dataGridView.RowPostPaint -= DataGridView_RowPostPaint;
                }
            }
        }

        private bool _pageSizeVisible = true;

        /// <summary>
        /// 获取或设置分页大小是否可见
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("行号是否显示[true:显示行号;false:不显示行号]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool PageSizeVisible
        {
            get { return _pageSizeVisible; }
            set
            {
                if (_pageSizeVisible == value)
                {
                    return;
                }

                _pageSizeVisible = value;
                this.SwitchPageSizeVisible(_pageSizeVisible);
            }
        }

        /// <summary>
        /// 切换分页大小可见性
        /// </summary>
        /// <param name="visible">分页大小可见性</param>
        private void SwitchPageSizeVisible(bool visible)
        {
            label2.Visible = visible;
            label4.Visible = visible;
            numPageSize.Visible = visible;
        }

        /// <summary>
        /// 获取或设置用户可选择的最大分页大小
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("行号是否显示[true:显示行号;false:不显示行号]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public int PageSizeMaximum
        {
            get { return (int)numPageSize.Maximum; }
            set
            {
                if (numPageSize.Maximum == value)
                {
                    return;
                }

                numPageSize.Maximum = value;
            }
        }

        /// <summary>
        /// 最后一列显示模式是否默认Fill[true:Fill;false:系统默认;默认值:false]
        /// </summary>
        private bool _isLastColumnAutoSizeModeFill = false;

        /// <summary>
        /// 获取或设置最后一列显示模式是否默认Fill[true:Fill;false:系统默认;默认值:true]
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("最后一列显示模式是否默认Fill[true:Fill;false:系统默认;默认值:true]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool IsLastColumnAutoSizeModeFill
        {
            get { return _isLastColumnAutoSizeModeFill; }
            set { _isLastColumnAutoSizeModeFill = value; }
        }

        /// <summary>
        /// 绘制表格行号
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, _dataGridView.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), _dataGridView.RowHeadersDefaultCellStyle.Font, rectangle,
                _dataGridView.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 获取表格控件
        /// </summary>
        [Description("表格控件")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public DataGridViewEx GridControl
        {
            get { return _dataGridView; }
        }

        /// <summary>
        /// 用户设置数据存放目录
        /// </summary>
        private string _settingDirectory;

        /// <summary>
        /// 获取或设置用户设置数据存放目录
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取或设置用户设置数据存放目录")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("分页数据显示控件")]
        public string SettingDirectory
        {
            get { return _settingDirectory; }
            set { _settingDirectory = value; }
        }

        /// <summary>
        /// 
        /// 获取
        /// </summary>
        private string _dataSourceName = null;

        /// <summary>
        /// 获取当前数据源名称
        /// </summary>
        [Browsable(false)]
        public string DataSourceName
        {
            get { return _dataSourceName; }
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        [Browsable(false)]
        public object DataSource
        {
            get { return _dataGridView.DataSource; }
        }

        /// <summary>
        /// 当前分页信息
        /// </summary>
        private PageInfo _pageInfo = null;

        /// <summary>
        /// 获取当前分页信息
        /// </summary>
        [Browsable(false)]
        public PageInfo PageInfo
        {
            get { return _pageInfo; }
        }

        /// <summary>
        /// 获取或设置焦点行索引
        /// </summary>
        [Browsable(false)]
        public int FocusedRowIndex
        {
            get
            {
                if (this._dataGridView.CurrentRow != null)
                {
                    return this._dataGridView.CurrentRow.Index;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (value > this._dataGridView.RowCount - 1)
                {
                    throw new ArgumentOutOfRangeException("value", "焦点行索引值超出行数范围");
                }

                foreach (DataGridViewRow row in this._dataGridView.SelectedRows)
                {
                    row.Selected = false;
                }

                if (value >= 0)
                {
                    this._dataGridView.Rows[value].Selected = true;
                }
            }
        }

        /// <summary>
        /// 获取选中行集合
        /// </summary>
        [Description("获取选中行集合")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public DataGridViewRow[] SelectedRows
        {
            get
            {
                Dictionary<int, DataGridViewRow> dicSelectedRows = new Dictionary<int, DataGridViewRow>();
                foreach (DataGridViewCell cell in this._dataGridView.SelectedCells)
                {
                    if (dicSelectedRows.ContainsKey(cell.RowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        dicSelectedRows.Add(cell.RowIndex, cell.OwningRow);
                    }
                }

                return dicSelectedRows.Values.ToArray();
            }
        }
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
        public void ShowData(object dataSource, string dataSourceName = null, IEnumerable<string> hidenColumns = null, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null)
        {
            DataGridViewEx.DataBinding(this._dataGridView, dataSource, hidenColumns, colHeadInfos, allowEditColumns);
            this.LoadColumnsSetting(this._settingDirectory, dataSourceName);
            this._fPageGridColumnsSetting.UpdateAdvanceSetting(this._dataGridView.Columns);
            this._dataSourceName = dataSourceName;

            if (this._isLastColumnAutoSizeModeFill)
            {
                //设置最后一个可见列填充
                DataGridViewColumn col;
                for (int i = this._dataGridView.Columns.Count - 1; i >= 0; i--)
                {
                    col = this._dataGridView.Columns[i];
                    if (col.Visible)
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                    }
                }
            }
        }

        /*
        private DGVBindDataSourceInfo _currentDGVBindDataSourceInfo = null;
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="hidenColumns">隐藏列集合</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[当为null或空时,全部列都可编辑;默认为null]</param>
        public void ShowData(object dataSource, string dataSourceName = null, IEnumerable<string> hidenColumns = null, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null)
        {
            if (dataGridView.DataSource == dataSource)
            {
                return;
            }

            this._currentDGVBindDataSourceInfo = new DGVBindDataSourceInfo(dataSource, dataSourceName, hidenColumns, colHeadInfos, allowEditColumns);
            if (dataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect ||
                dataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
            {
                var srcSelectionMode = dataGridView.SelectionMode;
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataSource;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    if (col.SortMode == DataGridViewColumnSortMode.Automatic)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }

                dataGridView.SelectionMode = srcSelectionMode;
            }
            else
            {
                dataGridView.DataSource = dataSource;
            }
        }

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this._currentDGVBindDataSourceInfo == null)
            {
                return;
            }

            var dataSourceName = this._currentDGVBindDataSourceInfo.DataSourceName;
            if (string.Equals(this._dataSourceName, dataSourceName))
            {
                return;
            }

            this.SettingCol(this._currentDGVBindDataSourceInfo.DataSource, this._currentDGVBindDataSourceInfo.HidenColumns, this._currentDGVBindDataSourceInfo.ColHeadInfos, this._currentDGVBindDataSourceInfo.AllowEditColumns);

            this.LoadColumnsSetting(this._settingDirectory, dataSourceName);
            this._fPageGridColumnsSetting.UpdateAdvanceSetting(this.dataGridView.Columns);
            this._dataSourceName = dataSourceName;

            if (this._isLastColumnAutoSizeModeFill)
            {
                //设置最后一个可见列填充
                DataGridViewColumn col;
                for (int i = this.dataGridView.Columns.Count - 1; i >= 0; i--)
                {
                    col = this.dataGridView.Columns[i];
                    if (col.Visible)
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// DataGridView绑定数据
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="hidenColumns">隐藏列集合</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[当为null或空时,全部列都可编辑;默认为null]</param>
        private void SettingCol(object dataSource, IEnumerable<string> hidenColumns, Dictionary<string, string> colHeadInfos, IEnumerable<string> allowEditColumns)
        {
            if (dataSource == null)
            {
                return;
            }

            if (hidenColumns == null)
            {
                hidenColumns = new List<string>();
            }

            if (colHeadInfos == null)
            {
                colHeadInfos = new Dictionary<string, string>();
            }

            if (allowEditColumns == null)
            {
                allowEditColumns = new List<string>();
            }

            string caption = null;
            string fieldName = null;
            bool isReadOnly;
            var dt = dataGridView.DataSource as System.Data.DataTable;
            dataGridView.ReadOnly = allowEditColumns.Count() == 0;

            foreach (DataGridViewColumn gridColumn in dataGridView.Columns)
            {
                //获取字段名
                fieldName = gridColumn.Name;
                if (hidenColumns.Contains(fieldName))
                {
                    gridColumn.Visible = false;
                    break;
                }

                isReadOnly = !allowEditColumns.Contains(fieldName);
                //设置为可编辑性
                if (isReadOnly != gridColumn.ReadOnly)
                {
                    gridColumn.ReadOnly = isReadOnly;
                }

                //设置显示标题
                if (colHeadInfos.ContainsKey(fieldName))
                {
                    caption = colHeadInfos[fieldName];
                }
                else if (dt != null && dt.Columns.Contains(fieldName))
                {
                    caption = dt.Columns[fieldName].Caption;
                }

                if (!string.IsNullOrEmpty(caption))
                {
                    gridColumn.HeaderText = caption;
                    caption = null;
                }
            }
        }
        */

        /// <summary>
        /// 触发查询数据事件
        /// </summary>
        /// <param name="e">查询参数</param>
        public void OnRaiseQueryData(QueryDataArgs e)
        {
            var handler = this.QueryData;
            if (handler != null)
            {
                this.QueryData(this, e);
            }
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        public void SetPageInfo(PageInfo pageInfo)
        {
            if (pageInfo == null || pageInfo.PageCount < 1)
            {
                labelPageCount.Text = "0页|共0条记录";
            }
            else
            {
                labelPageCount.Text = string.Format("{0}页|共{1}条记录", pageInfo.PageCount, pageInfo.TotalCount);
            }

            this.InnerSetPageInfo(pageInfo);

            //外部更改查询页，触发分页查询事件
            if (pageInfo != null && pageInfo.PageCount > 0)
            {
                this.OnRaiseQueryData(new QueryDataArgs(1, pageInfo.PageSize));
            }
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        private void InnerSetPageInfo(PageInfo pageInfo)
        {
            this.numPageIndex.ValueChanged -= this.numPageIndex_ValueChanged;

            try
            {
                //记录当前分页信息
                this._pageInfo = pageInfo;

                //设置显示信息
                if (pageInfo == null || pageInfo.PageCount <= 0)
                {
                    //设置页数选择控件值
                    numPageIndex.Minimum = 0;
                    numPageIndex.Maximum = 0;
                    numPageIndex.Value = 0;
                    numPageIndex.Enabled = false;
                    numPageSize.Enabled = false;

                    //禁用跳转按钮
                    btnFirstPage.Enabled = false;
                    btnPrePage.Enabled = false;
                    btnNextPage.Enabled = false;
                    btnLastPage.Enabled = false;
                }
                else
                {
                    if (this.numPageSize.Maximum < pageInfo.PageSize)
                    {
                        this.numPageSize.Maximum = pageInfo.PageSize;
                    }

                    if (this.numPageSize.Value != pageInfo.PageSize)
                    {
                        this.numPageSize.Value = pageInfo.PageSize;
                    }

                    numPageIndex.Minimum = 1;
                    numPageIndex.Maximum = pageInfo.PageCount;
                    numPageIndex.Value = pageInfo.PageIndex;
                    numPageIndex.Enabled = true;
                    numPageSize.Enabled = true;

                    //启用跳转按钮
                    btnFirstPage.Enabled = false;
                    btnPrePage.Enabled = false;
                    btnNextPage.Enabled = false;
                    btnLastPage.Enabled = false;

                    //如果当前页为小于等于1,则禁首页和用前一页
                    if (pageInfo.PageIndex <= 1)
                    {
                        btnFirstPage.Enabled = false;
                        btnPrePage.Enabled = false;
                    }
                    else
                    {
                        btnFirstPage.Enabled = true;
                        btnPrePage.Enabled = true;
                    }

                    //如果当前页大于等于总页数,则禁用最后一页和下一页
                    if (pageInfo.PageIndex >= pageInfo.PageCount)
                    {
                        btnNextPage.Enabled = false;
                        btnLastPage.Enabled = false;
                    }
                    else
                    {
                        btnNextPage.Enabled = true;
                        btnLastPage.Enabled = true;
                    }
                }
            }
            finally
            {
                this.numPageIndex.ValueChanged += this.numPageIndex_ValueChanged;
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            this._dataGridView.DataSource = null;
            this._dataGridView.Columns.Clear();
        }
        #endregion
        #endregion

        #region 私有方法
        /// <summary>
        /// 加载列设置
        /// </summary>
        /// <param name="settingDirectory">用户设置数据存放目录</param>
        /// <param name="dataSourceName">数据源名称</param>
        private void LoadColumnsSetting(string settingDirectory, string dataSourceName)
        {
            try
            {
                if (string.IsNullOrEmpty(dataSourceName) ||
                   string.IsNullOrEmpty(settingDirectory) ||
                   this._dataGridView.Columns.Count == 0)
                {
                    return;
                }

                string columnSettingFilePath = PageGridControlCommon.GetGridColumnSettingFilePath(settingDirectory, dataSourceName);
                if (!File.Exists(columnSettingFilePath))
                {
                    return;
                }

                var xdoc = XDocument.Load(columnSettingFilePath);
                var rootEle = xdoc.Root;
                int count = int.Parse(XmlEx.GetXElementAttributeValue(rootEle, "Count"));
                if (this._dataGridView.Columns.Count != count)
                {
                    //项数不同,同数据源名称,但是内容有变,不加载
                    return;
                }

                //加载数据
                List<dynamic> items = new List<dynamic>();
                try
                {
                    foreach (var itemEle in rootEle.Elements("Item"))
                    {
                        dynamic item = new ExpandoObject();
                        item.Name = XmlEx.GetXElementAttributeValue(itemEle, "Name");
                        item.Width = int.Parse(XmlEx.GetXElementAttributeValue(itemEle, "Width"));
                        item.DisplayIndex = int.Parse(XmlEx.GetXElementAttributeValue(itemEle, "DisplayIndex"));
                        item.Visible = bool.Parse(XmlEx.GetXElementAttributeValue(itemEle, "Visible"));
                        items.Add(item);

                        if (!this._dataGridView.Columns.Contains(item.Name))
                        {
                            //不包含该列,同数据源名称,但是内容有变,不加载
                            return;
                        }
                    }
                }
                catch (Exception exi)
                {
                    //数据有错误
                    Loger.Error(exi);
                    return;
                }

                this._hidenCols.Clear();
                foreach (dynamic item in items)
                {
                    DataGridViewColumn col = this._dataGridView.Columns[item.Name];
                    col.Width = item.Width;
                    col.DisplayIndex = item.DisplayIndex;
                    col.Visible = item.Visible;

                    //如果该列不显示则添加到隐藏列集合中
                    if (!col.Visible)
                    {
                        this._hidenCols.Add(col);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 保存当前数据源显示表的的设置信息
        /// </summary>
        /// <param name="settingDirectory">用户设置数据存放目录</param>
        /// <param name="dataSourceName">数据源名称</param>
        private void SaveCurrentColumnsSetting(string settingDirectory, string dataSourceName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dataSourceName) ||
                    string.IsNullOrWhiteSpace(settingDirectory) ||
                    this._dataGridView.Columns.Count == 0)
                {
                    return;
                }

                string columnSettingFilePath = PageGridControlCommon.GetGridColumnSettingFilePath(settingDirectory, dataSourceName);
                var xdoc = new XDocument();
                var rootEle = new XElement("DGVColumnsLayout");//NXmlHelper.
                rootEle.Add(new XAttribute("Count", this._dataGridView.Columns.Count));

                foreach (DataGridViewColumn col in this._dataGridView.Columns)
                {
                    var itemEle = new XElement("Item");
                    itemEle.Add(new XAttribute("Name", col.Name));
                    itemEle.Add(new XAttribute("Width", col.Width));
                    itemEle.Add(new XAttribute("DisplayIndex", col.DisplayIndex));
                    itemEle.Add(new XAttribute("Visible", col.Visible));
                    rootEle.Add(itemEle);
                }

                xdoc.Add(rootEle);

                string dir = Path.GetDirectoryName(columnSettingFilePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                xdoc.Save(columnSettingFilePath);
            }
            catch (IOException ioex)
            {
                MessageBox.Show(ioex.Message);
                Loger.Error(ioex);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion

        private readonly DataGridViewEx _dataGridView;

        /// <summary>
        /// 隐藏列绑定列表
        /// </summary>
        private readonly BindingListEx<DataGridViewColumn> _hidenCols = new BindingListEx<DataGridViewColumn>();

        /// <summary>
        /// 隐藏列窗口
        /// </summary>
        private readonly FPageGridColumnsSetting _fPageGridColumnsSetting;

        /// <summary>
        /// 目标列
        /// </summary>
        private DataGridViewColumn _targetCol = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCPageGridControl()
        {
            InitializeComponent();

            this._dataGridView = this.CreateDataGridView();
            this._fPageGridColumnsSetting = this.CreateColSetting();

            this.panelContent.Controls.Add(this._dataGridView);
            this.panelContent.Controls.Add(this._fPageGridColumnsSetting);

            this._dataGridView.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_ColumnDisplayIndexChanged);
            this._dataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseClick);
            this._dataGridView.SelectionChanged += GridView_SelectionChanged;
            this._dataGridView.MouseClick += GridView_MouseClick;
            this._dataGridView.MouseDoubleClick += GridView_MouseDoubleClick;

            this.SetPageInfo(null);

            var pageControls = new List<Control>();
            foreach (Control control in panelPage.Controls)
            {
                pageControls.Add(control);
            }

            this._pageControls = new ReadOnlyCollection<Control>(pageControls);
            //初始化列设置存放目录
            this._settingDirectory = PageGridControlCommon.GetDefaultSettingDirectory();
            this.numPageIndex.ValueChanged += this.numPageIndex_ValueChanged;
            this.RowNumVisible = true;
        }

        private FPageGridColumnsSetting CreateColSetting()
        {
            var fPageGridColumnsSetting = new FPageGridColumnsSetting(this.panelContent, this._hidenCols);
            fPageGridColumnsSetting.Dock = DockStyle.Right;
            fPageGridColumnsSetting.SaveColumnDisplaySetting += _fPageGridColumnsSetting_SaveColumnDisplaySetting;
            fPageGridColumnsSetting.TopLevel = false;
            fPageGridColumnsSetting.Show();
            return fPageGridColumnsSetting;
        }

        private DataGridViewEx CreateDataGridView()
        {
            var dgv = new DataGridViewEx();
            dgv.AllowDrop = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv.Location = new System.Drawing.Point(0, 0);
            dgv.MouseRightButtonChangeSelectedRow = false;
            dgv.Name = "dataGridView";
            dgv.ReadOnly = true;
            dgv.RowTemplate.Height = 23;
            dgv.Size = new System.Drawing.Size(225, 86);
            dgv.TabIndex = 2;
            dgv.VirtualMode = true;
            return dgv;
        }

        #region 接口事件调用
        /// <summary>
        /// 表格控件鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            object row = null;
            int rowIndex = -1;
            //DataGridView.HitTestInfo hitTestInfo = this.dataGridView.HitTest(e.X, e.Y);
            //if (hitTestInfo != null)
            //{
            //    rowIndex = hitTestInfo.RowIndex;
            //    row = this.dataGridView.Rows[rowIndex].DataBoundItem;
            //}

            if (this._dataGridView.CurrentRow != null)
            {
                rowIndex = this._dataGridView.CurrentRow.Index;
                row = this._dataGridView.CurrentRow.DataBoundItem;
            }

            var handler = this.DataRowDoubleClick;
            if (handler != null)
            {
                this.DataRowDoubleClick(sender, new DataRowClickArgs(row, rowIndex));
            }
        }

        /// <summary>
        /// 表格控件鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_MouseClick(object sender, MouseEventArgs e)
        {
            object row = null;
            int rowIndex = -1;
            if (this._dataGridView.CurrentRow != null)
            {
                rowIndex = this._dataGridView.CurrentRow.Index;
                row = this._dataGridView.CurrentRow.DataBoundItem;
            }

            var handler = this.DataRowClick;
            if (handler != null)
            {
                this.DataRowClick(sender, new DataRowClickArgs(row, rowIndex));
            }
        }

        /// <summary>
        /// 前次选中的行
        /// </summary>
        private DataGridViewRow _prevSelectedRow = null;

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_SelectionChanged(object sender, EventArgs e)
        {
            int rowHandle = -1;//当前选中行索引
            int prevRowHandle = -1;//上次选中行索引
            object row = null;//当前行数据
            object prevRow = null;//上次行数据

            try
            {
                //前一次选中行信息
                if (this._prevSelectedRow != null)
                {
                    prevRowHandle = this._prevSelectedRow.Index;
                    prevRow = this._prevSelectedRow.DataBoundItem;
                }

                this._prevSelectedRow = this._dataGridView.CurrentRow;
                if (this._dataGridView.CurrentRow != null)
                {
                    row = this._prevSelectedRow.DataBoundItem;
                    rowHandle = this._prevSelectedRow.Index;
                }

                //当前选中行数据集合
                List<object> selectedRowDatas = new List<object>();
                object[] selectedRows = this.SelectedRows;
                foreach (DataGridViewRow selectedRow in selectedRows)
                {
                    selectedRowDatas.Add(selectedRow.DataBoundItem);
                }

                var handler = this.DataRowSelectionChanged;
                if (handler != null)
                {
                    handler(this, new DataRowSelectionChangedArgs(rowHandle, prevRowHandle, row, prevRow, selectedRows.ToList()));
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }
        #endregion 

        #region 显示列设置
        /// <summary>
        /// 列设置保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _fPageGridColumnsSetting_SaveColumnDisplaySetting(object sender, EventArgs e)
        {
            try
            {
                this.SaveCurrentColumnsSetting(this._settingDirectory, this._dataSourceName);
                this._fPageGridColumnsSetting.ColumnDisplaySettingChanged = false;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCPageGridControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
        }

        /// <summary>
        /// 表格列头右键单击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && e.Clicks == 1 &&
                   this.ColumnSettingVisible)
                {
                    this._targetCol = this._dataGridView.Columns[e.ColumnIndex];
                    //cms.Show(MousePosition.X, MousePosition.Y);
                    this.cmsColVisibleSetting.Show(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 隐藏列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiHidenCol_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._targetCol != null)
                {
                    this._targetCol.Visible = false;
                    this._hidenCols.Add(this._targetCol);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 列显示顺序改变事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this._fPageGridColumnsSetting.ColumnDisplaySettingChanged = true;
        }
        #endregion

        #region 分页
        private void PageQuery(int pageIndex)
        {
            this.InnerSetPageInfo(new PageInfo(this._pageInfo.PageCount, this._pageInfo.PageSize, pageIndex, this._pageInfo.TotalCount));
            this.OnRaiseQueryData(new QueryDataArgs(pageIndex, this._pageInfo.PageSize));
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (this._pageInfo == null)
            {
                return;
            }

            int pageIndex = 1;
            if (pageIndex == this._pageInfo.PageIndex)
            {
                return;
            }

            this.PageQuery(pageIndex);
        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            if (this._pageInfo == null)
            {
                return;
            }

            int pageIndex = this._pageInfo.PageIndex - 1;
            if (pageIndex < 1)
            {
                return;
            }

            this.PageQuery(pageIndex);
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (this._pageInfo == null)
            {
                return;
            }

            int pageIndex = this._pageInfo.PageIndex + 1;
            if (pageIndex > this._pageInfo.PageCount)
            {
                return;
            }

            this.PageQuery(pageIndex);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (this._pageInfo == null)
            {
                return;
            }

            int pageIndex = this._pageInfo.PageCount;
            if (this._pageInfo.PageIndex == pageIndex)
            {
                return;
            }

            this.PageQuery(pageIndex);
        }

        private void numPageIndex_ValueChanged(object sender, EventArgs e)
        {
            if (this._pageInfo == null)
            {
                return;
            }

            int pageIndex = (int)numPageIndex.Value;
            if (pageIndex == this._pageInfo.PageIndex)
            {
                return;
            }

            this.PageQuery(pageIndex);
        }

        private void numPageSize_ValueChanged(object sender, EventArgs e)
        {
            var handler = this.PageSizeChanged;
            if (handler != null)
            {
                handler(this, new PageSizeChangedArgs((int)numPageSize.Value));
            }
        }
        #endregion
    }
}
