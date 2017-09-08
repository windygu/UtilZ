using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Base.Extend;
using System.IO;

namespace UtilZ.Lib.WinformEx.PageGrid
{
    /// <summary>
    /// 分页数据显示控件
    /// </summary>
    public partial class PageGridControl : System.Windows.Forms.UserControl, IPageGridControl
    {
        /// <summary>
        /// weifenluo表格控件窗口
        /// </summary>
        private readonly FGrid _fgrid = null;

        /// <summary>
        /// DX分页显示表格控件显示列设置窗口
        /// </summary>
        private readonly FColumnDisplaySetting _fColumnDisplaySetting = new FColumnDisplaySetting();

        /// <summary>
        /// 上次本控件所属于的窗体
        /// </summary>
        private Form _lastParentForm = null;

        /// <summary>
        /// 列标题映射字典[key:列名;value:列标题
        /// </summary>
        private Dictionary<string, string> _colHeadInfos = null;

        /// <summary>
        /// 允许编辑的列集合
        /// </summary>
        private IEnumerable<string> _allowEditColumns = null;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PageGridControl()
        {
            InitializeComponent();

            if (this.DesignMode)
            {
                return;
            }

            //初始化数据展示表格控件
            this._fgrid = new FGrid(this._fColumnDisplaySetting.ListBoxSettingCols);

            //数据表格窗口
            WFLHelper.AddDock<FGrid>(this._fgrid, wflDockPanel, "数据", DockStyle.Fill, WeifenLuo.WinFormsUI.Docking.DockState.Document);

            //显示列设置窗口
            WFLHelper.AddDock<FColumnDisplaySetting>(this._fColumnDisplaySetting, wflDockPanel, PageGridControlCommon.DisplayColSettingFormText, DockStyle.Right, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);

            //初始化列设置存放目录
            this.SettingDirectory = PageGridControlCommon.GetDefaultSettingDirectory();
            this.Load += new System.EventHandler(this.PageGridControl_Load);

            this._fgrid.QueryAssignPageData += OnRaiseQueryAssignPageData;
            this._fgrid.GridView.SelectionChanged += GridView_SelectionChanged;
            this._fgrid.GridView.MouseClick += GridView_MouseClick;
            this._fgrid.GridView.MouseDoubleClick += GridView_MouseDoubleClick;
            this._fgrid.GridView.DataBindingComplete += GridView_DataBindingComplete;
            this._fgrid.GridView.MouseDown += GridView_MouseDown;
        }

        /// <summary>
        /// 单击列标题时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 1)
                {
                    DataGridView.HitTestInfo hitTestInfo = this._fgrid.GridView.HitTest(e.X, e.Y);
                    if (hitTestInfo != null && hitTestInfo.Type == DataGridViewHitTestType.ColumnHeader)
                    {
                        //当单击列标题时可能会对数据进行排序,在排序前先把显示设置保存,否则排序后显示是在数据显示时的设置
                        this._fgrid.SaveCurrentColumnsSetting(this.SettingDirectory, this.DataSourceName);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 数据绑定完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                //设置列标题
                this.SetColumnHead(this._colHeadInfos, this._allowEditColumns);

                //加载列设置
                this._fgrid.LoadColumnsSetting(this.SettingDirectory, this.DataSourceName);

                //设置最后一个可见列填充
                DataGridViewColumn col;
                for (int i = this._fgrid.GridView.Columns.Count - 1; i >= 0; i--)
                {
                    col = this._fgrid.GridView.Columns[i];
                    if (col.Visible)
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 表格控件鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            object row = null;
            int rowIndex = -1;
            //DataGridView.HitTestInfo hitTestInfo = this._fgrid.GridView.HitTest(e.X, e.Y);
            //if (hitTestInfo != null)
            //{
            //    rowIndex = hitTestInfo.RowIndex;
            //    row = this._fgrid.GridView.Rows[rowIndex].DataBoundItem;
            //}

            if (this._fgrid.GridView.CurrentRow != null)
            {
                rowIndex = this._fgrid.GridView.CurrentRow.Index;
                row = this._fgrid.GridView.CurrentRow.DataBoundItem;
            }

            this.OnRaiseDataRowDoubleClick(sender, row, rowIndex);
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
            if (this._fgrid.GridView.CurrentRow != null)
            {
                rowIndex = this._fgrid.GridView.CurrentRow.Index;
                row = this._fgrid.GridView.CurrentRow.DataBoundItem;
            }

            this.OnRaiseDataRowClick(sender, row, rowIndex);
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

                this._prevSelectedRow = this._fgrid.GridView.CurrentRow;
                if (this._fgrid.GridView.CurrentRow != null)
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

                this.SetShowPageInfo(rowHandle == -1 ? 0 : rowHandle + 1);
                this.OnRaiseSelectionChanged(rowHandle, prevRowHandle, row, prevRow, selectedRowDatas);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// PageGridControl_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageGridControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                this.Controls.Add(this.wflDockPanel);

                //注册窗口关闭事件,在关闭时保存设置的列                
                this.ParentChanged += PageGridControl_ParentChanged;
                this.PageGridControl_ParentChanged(sender, e);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 本控件的父控件更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageGridControl_ParentChanged(object sender, EventArgs e)
        {
            try
            {
                //如果更改后的窗体不是上次注册事件时的窗体,则取消之前窗体注册的FormClosing事件,再对当前窗体重新注册FormClosing事件
                if (this._lastParentForm == this.ParentForm)
                {
                    return;
                }

                if (this._lastParentForm != null)
                {
                    this._lastParentForm.FormClosing -= this._lastParentForm_FormClosing;
                }

                this._lastParentForm = this.ParentForm;
                if (this._lastParentForm != null)
                {
                    this._lastParentForm.FormClosing += this._lastParentForm_FormClosing;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 控件所在窗口关闭事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _lastParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._fgrid.SaveCurrentColumnsSetting(this.SettingDirectory, this.DataSourceName);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        #region IPageGridControl接口
        #region 事件
        /// <summary>
        /// 数据行单击事件
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("数据行单击事件")]
        public event EventHandler<DataRowDoubleClickArgs> DataRowClick;

        /// <summary>
        /// 触发数据行单击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="row">双击的数据行</param>
        /// <param name="rowIndex">行索引</param>
        private void OnRaiseDataRowClick(object sender, object row, int rowIndex)
        {
            if (this.DataRowClick != null)
            {
                this.DataRowClick(sender, new DataRowDoubleClickArgs(row, rowIndex));
            }
        }

        /// <summary>
        /// 数据行双击事件
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("数据行双击事件")]
        public event EventHandler<DataRowDoubleClickArgs> DataRowDoubleClick;

        /// <summary>
        /// 触发数据行双击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="row">双击的数据行</param>
        /// <param name="rowIndex">行索引</param>
        private void OnRaiseDataRowDoubleClick(object sender, object row, int rowIndex)
        {
            if (this.DataRowDoubleClick != null)
            {
                this.DataRowDoubleClick(sender, new DataRowDoubleClickArgs(row, rowIndex));
            }
        }

        /// <summary>
        /// 选中行改变事件
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("选中行改变事件")]
        public event EventHandler<SelectionChangedArgs> SelectionChanged;

        /// <summary>
        /// 触发选中行改变事件
        /// </summary>
        /// <param name="rowHandle">当前选中行索引</param>
        /// <param name="prevRowHandle">上次选中行索引</param>
        /// <param name="row">当前行数据</param>
        /// <param name="prevRow">上次行数据</param>
        /// <param name="selectedRows">选中行数据集合</param>
        private void OnRaiseSelectionChanged(int rowHandle, int prevRowHandle, object row, object prevRow, List<object> selectedRows)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, new SelectionChangedArgs(rowHandle, prevRowHandle, row, prevRow, selectedRows));
            }
        }

        /// <summary>
        /// 查询指定页数据事件
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("查询指定页数据事件")]
        public event EventHandler<QueryAssignPageDataArgs> QueryAssignPageData;

        /// <summary>
        /// 触发查询指定页数据事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">参数</param>
        private void OnRaiseQueryAssignPageData(object sender, QueryAssignPageDataArgs e)
        {
            if (this.QueryAssignPageData != null)
            {
                this.QueryAssignPageData(sender, e);
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取UI是否处于设计器模式
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("UI是否处于设计器模式")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UIDesignMode
        {
            get { return this.DesignMode; }
        }

        /// <summary>
        /// 获取数据源名称
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取数据源名称")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataSourceName
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取或设置用户设置数据存放目录
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取或设置用户设置数据存放目录")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SettingDirectory { get; set; }

        /// <summary>
        /// 获取数据源名称
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取数据源名称")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get { return this._fgrid.GridView.DataSource; }
        }

        /// <summary>
        /// 获取或设置表中的数据是否只读
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置表中的数据是否只读")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DataReadOnly
        {
            get { return this._fgrid.GridView.ReadOnly; }
            set
            {
                if (this._fgrid.GridView.ReadOnly == value)
                {
                    return;
                }

                this._fgrid.GridView.ReadOnly = value;
            }
        }

        /// <summary>
        /// 获取GridControl
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取GridControl")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object GridControl
        {
            get { return this._fgrid.GridView; }
        }

        /// <summary>
        /// 获取或设置分页栏是否可见
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置分页栏是否可见")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool PagingVisible
        {
            get { return this._fgrid.PagingVisible; }
            set { this._fgrid.PagingVisible = value; }
        }

        /// <summary>
        /// 获取或设置焦点行索引
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取或设置分页栏是否可见")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FocusedRowHandle
        {
            get
            {
                if (this._fgrid.GridView.CurrentRow != null)
                {
                    return this._fgrid.GridView.CurrentRow.Index;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (value > this._fgrid.GridView.RowCount - 1)
                {
                    throw new ArgumentOutOfRangeException("value", "焦点行索引值超出行数范围");
                }

                foreach (DataGridViewRow row in this._fgrid.GridView.SelectedRows)
                {
                    row.Selected = false;
                }

                if (value >= 0)
                {
                    this._fgrid.GridView.Rows[value].Selected = true;
                }
            }
        }

        /// <summary>
        /// 获取选中行集合
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("获取选中行集合")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] SelectedRows
        {
            get
            {
                Dictionary<int, DataGridViewRow> dicSelectedRows = new Dictionary<int, DataGridViewRow>();
                foreach (DataGridViewCell cell in this._fgrid.GridView.SelectedCells)
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

        /// <summary>
        /// 列设置是否可见
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列设置是否可见")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ColumnSettingVisible
        {
            get
            {
                return WFLHelper.GetDockContentVisible(this._fColumnDisplaySetting);
            }
            set
            {
                WFLHelper.SetDockContentVisible(this._fColumnDisplaySetting, value);
            }
        }

        /// <summary>
        /// 获取或设置与数据表格控件关联的 System.Windows.Forms.ContextMenuStrip
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置与数据表格控件关联的 System.Windows.Forms.ContextMenuStrip")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContextMenuStrip GridContextMenuStrip
        {
            get { return this._fgrid.GridView.ContextMenuStrip; }
            set { this._fgrid.GridView.ContextMenuStrip = value; }
        }

        /// <summary>
        /// 获取或设置是否允许多选行
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置是否允许多选行")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool MuiltSelect
        {
            get { return this._fgrid.GridView.MultiSelect; }
            set { this._fgrid.GridView.MultiSelect = value; }
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        [Browsable(false)]
        public PageInfo PageInfo
        {
            get { return this._fgrid.PageInfo; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 状态栏控件启用或是禁用状态设置
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="enable">状态[true:启用;false:禁用]</param>
        public void EnableStatusControl(string name, bool enable)
        {
            this._fgrid.EnableStatusControl(name, enable);
        }

        /// <summary>
        /// 获取状态栏控件启用或是禁用状态设置[true:启用;false:禁用]
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>状态[true:启用;false:禁用]</returns>
        public bool GetStatusControlEnable(string name)
        {
            return this._fgrid.GetStatusControlEnable(name);
        }

        /// <summary>
        /// 获取数据表格中显示列列名集合
        /// </summary>
        /// <returns>数据表格中显示列列名集合</returns>
        public Dictionary<string, string> GetShowGridColumns()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[注:当需要有列可编辑时,需要设置DataReadOnly属性值为false,如果该值为true,则会在此方法内自动设置;默认为null]</param>
        public void ShowData(string dataSourceName, object dataSource, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    this.SetShowData(dataSourceName, dataSource, colHeadInfos, allowEditColumns);
                }));
            }
            else
            {
                this.SetShowData(dataSourceName, dataSource, colHeadInfos, allowEditColumns);
            }
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="dataSourceName">数据源名称</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[注:当需要有列可编辑时,需要设置DataReadOnly属性值为false,如果该值为true,则会在此方法内自动设置;默认为null]</param>
        private void SetShowData(string dataSourceName, object dataSource, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null)
        {
            try
            {
                if (string.IsNullOrEmpty(dataSourceName))
                {
                    throw new ArgumentNullException("dataSourceName", "数据源名称不能为空或null");
                }

                if (dataSource != null && this._fgrid.GridView.DataSource == dataSource)
                {
                    //如果和上次所查询的数据不是同一个数据源
                    if (!dataSourceName.Equals(this.DataSourceName))
                    {
                        //保存上次查询表的显示设置样式
                        this._fgrid.SaveCurrentColumnsSetting(this.SettingDirectory, this.DataSourceName);
                        //记录数据源名称
                        this.DataSourceName = dataSourceName;
                        //加载列设置
                        this._fgrid.LoadColumnsSetting(this.SettingDirectory, this.DataSourceName);
                    }

                    return;
                }

                //保存上次查询表的显示设置样式
                this._fgrid.SaveCurrentColumnsSetting(this.SettingDirectory, this.DataSourceName);

                ////设置显示数据
                //if (!dataSourceName.Equals(this.DataSourceName))
                //{
                //    //保存上次查询表的显示设置样式
                //    this._fgrid.SaveCurrentColumnsSetting(this.SettingDirectory, this.DataSourceName);
                //}

                this._colHeadInfos = colHeadInfos;
                this._allowEditColumns = allowEditColumns;
                if (dataSource == null)
                {
                    this.DataSourceName = string.Empty;
                }
                else
                {
                    //记录数据源名称
                    this.DataSourceName = dataSourceName;
                }

                //更新数据
                this._fgrid.GridView.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                throw new Exception("设置显示数据失败", ex);
            }
        }

        /// <summary>
        /// 设置列标题
        /// </summary>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合</param>
        private void SetColumnHead(Dictionary<string, string> colHeadInfos, IEnumerable<string> allowEditColumns)
        {
            if (colHeadInfos == null)
            {
                colHeadInfos = new Dictionary<string, string>();
            }

            if (allowEditColumns == null)
            {
                allowEditColumns = new List<string>();
            }

            //设置数据是否为只读
            this.DataReadOnly = allowEditColumns == null || allowEditColumns.Count() == 0;
            var dt = this.DataSource as DataTable;
            string caption = null;
            string fieldName = null;
            foreach (DataGridViewColumn gridColumn in this._fgrid.GridView.Columns)
            {
                //获取字段名
                fieldName = gridColumn.Name;
                //设置为可编辑性
                gridColumn.ReadOnly = !allowEditColumns.Contains(fieldName);

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

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            this._fgrid.GridView.DataSource = null;
            this._fgrid.GridView.Columns.Clear();
        }

        /// <summary>
        /// 触发查询指定页数据事件
        /// </summary>
        /// <param name="args">参数</param>
        public void OnRaiseQueryAssignPageData(QueryAssignPageDataArgs args)
        {
            this.OnRaiseQueryAssignPageData(this, args);
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        public void SetPageInfo(PageInfo pageInfo)
        {
            this._fgrid.SetPageInfo(pageInfo);
        }

        /// <summary>
        /// 设置选中记录提示信息
        /// </summary>
        /// <param name="recordIndex">选中记录索引</param>
        public void SetShowPageInfo(int recordIndex)
        {
            this._fgrid.SetShowPageInfo(recordIndex);
        }
        #endregion
        #endregion
    }
}
