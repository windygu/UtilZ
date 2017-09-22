using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base.DataStruct.UIBinding;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Winform.PageGrid.Interface;
using UtilZ.Lib.Winform.Extend;
using System.IO;
using System.Xml.Linq;
using UtilZ.Lib.Base.Extend;
using System.Dynamic;

namespace UtilZ.Lib.Winform.PageGrid
{
    /// <summary>
    /// 分页数据表格
    /// </summary>
    public partial class UCPageGridControl : UserControl, IPageDataGrid<DataGridViewRow, ZDataGridView>
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

        /// <summary>
        /// 获取或设置列设置区域宽度
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列设置区域宽度")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public int ColumnSettingWidth
        {
            get { return panelColVisibleSetting.Width; }
            set { panelColVisibleSetting.Width = value; }
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

        private bool _columnSettingVisible = true;

        /// <summary>
        /// 获取或设置列设置是否可见
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("列设置是否可见[true:可见;false:隐藏]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public bool ColumnSettingVisible
        {
            get { return _columnSettingVisible; }
            set
            {
                if (_columnSettingVisible == value)
                {
                    return;
                }

                _columnSettingVisible = value;
                if (!_columnSettingVisible && this.ColumnSettingStatus == PageGridColumnSettingStatus.Float)
                {
                    this._fPageGridColumnsSetting.SwitchColumnSettingStatus(PageGridColumnSettingStatus.Hiden);
                }

                panelColVisibleSetting.Visible = value;
            }
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
                    this.dataGridView.RowPostPaint += DataGridView_RowPostPaint;
                }
                else
                {
                    this.dataGridView.RowPostPaint -= DataGridView_RowPostPaint;
                }
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
            var rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridView.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 获取表格控件
        /// </summary>
        [Description("表格控件")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("分页数据显示控件")]
        public ZDataGridView GridControl
        {
            get { return dataGridView; }
        }

        /// <summary>
        /// 状态栏控件
        /// </summary>
        [Browsable(false)]
        public Control StatusControl
        {
            get { throw new NotImplementedException(); }
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
            get { return dataGridView.DataSource; }
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
                if (this.dataGridView.CurrentRow != null)
                {
                    return this.dataGridView.CurrentRow.Index;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (value > this.dataGridView.RowCount - 1)
                {
                    throw new ArgumentOutOfRangeException("value", "焦点行索引值超出行数范围");
                }

                foreach (DataGridViewRow row in this.dataGridView.SelectedRows)
                {
                    row.Selected = false;
                }

                if (value >= 0)
                {
                    this.dataGridView.Rows[value].Selected = true;
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
                foreach (DataGridViewCell cell in this.dataGridView.SelectedCells)
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
            NExtendDataGridView.DataBinding(this.dataGridView, dataSource, hidenColumns, colHeadInfos, allowEditColumns);
            if (string.Equals(this._dataSourceName, dataSourceName))
            {
                return;
            }

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
        /// 触发查询数据事件
        /// </summary>
        /// <param name="e">查询参数</param>
        public void OnRaiseQueryData(QueryDataArgs e)
        {

        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        public void SetPageInfo(PageInfo pageInfo)
        {

        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            this.dataGridView.DataSource = null;
            this.dataGridView.Columns.Clear();
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
                   this.dataGridView.Columns.Count == 0)
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
                int count = int.Parse(NXmlHelper.GetXElementAttributeValue(rootEle, "Count"));
                if (this.dataGridView.Columns.Count != count)
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
                        item.Name = NXmlHelper.GetXElementAttributeValue(itemEle, "Name");
                        item.Width = int.Parse(NXmlHelper.GetXElementAttributeValue(itemEle, "Width"));
                        item.DisplayIndex = int.Parse(NXmlHelper.GetXElementAttributeValue(itemEle, "DisplayIndex"));
                        item.Visible = bool.Parse(NXmlHelper.GetXElementAttributeValue(itemEle, "Visible"));
                        items.Add(item);

                        if (!this.dataGridView.Columns.Contains(item.Name))
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
                    DataGridViewColumn col = this.dataGridView.Columns[item.Name];
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
                    this.dataGridView.Columns.Count == 0)
                {
                    return;
                }

                string columnSettingFilePath = PageGridControlCommon.GetGridColumnSettingFilePath(settingDirectory, dataSourceName);
                var xdoc = new XDocument();
                var rootEle = new XElement("DGVColumnsLayout");//NXmlHelper.
                rootEle.Add(new XAttribute("Count", this.dataGridView.Columns.Count));

                foreach (DataGridViewColumn col in this.dataGridView.Columns)
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

        /// <summary>
        /// 隐藏列绑定列表
        /// </summary>
        private readonly UIBindingList<DataGridViewColumn> _hidenCols = new UIBindingList<DataGridViewColumn>();

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


            //初始化列设置存放目录
            this._settingDirectory = PageGridControlCommon.GetDefaultSettingDirectory();
            this._fPageGridColumnsSetting = new FPageGridColumnsSetting(this.panelColVisibleSetting, this.labelTitle, this._hidenCols);
            this._fPageGridColumnsSetting.SaveColumnDisplaySetting += _fPageGridColumnsSetting_SaveColumnDisplaySetting;

            this.dataGridView.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_ColumnDisplayIndexChanged);
            this.dataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseClick);
            this.RowNumVisible = true;
        }

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

            this._fPageGridColumnsSetting.Show();
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
                   this._columnSettingVisible)
                {
                    this._targetCol = this.dataGridView.Columns[e.ColumnIndex];
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
        /// 显示隐藏列列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiShowHidenColList_Click(object sender, EventArgs e)
        {
            panelColVisibleSetting.Visible = true;
        }

        /// <summary>
        /// 隐藏隐藏列列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiHidenHidenColList_Click(object sender, EventArgs e)
        {
            panelColVisibleSetting.Visible = false;
        }

        /// <summary>
        /// 列隐藏列表隐藏时的标题单击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelTitle_Click(object sender, EventArgs e)
        {
            this._fPageGridColumnsSetting.SwitchColumnSettingStatus(PageGridColumnSettingStatus.Dock);
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
    }
}
