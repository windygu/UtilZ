using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using WeifenLuo.WinFormsUI.Docking;

namespace UtilZ.Dotnet.IWinformEx
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public partial class FGrid : DockContent
    {
        /// <summary>
        /// 获取表格控件
        /// </summary>
        internal DataGridView GridView
        {
            get { return this.dataGridView; }
        }

        /// <summary>
        /// 保存当前数据源显示表的的设置信息
        /// </summary>
        /// <param name="settingDirectory">用户设置数据存放目录</param>
        /// <param name="dataSourceName">数据源名称</param>
        internal void SaveCurrentColumnsSetting(string settingDirectory, string dataSourceName)
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

        /// <summary>
        /// 加载列设置
        /// </summary>
        /// <param name="settingDirectory">用户设置数据存放目录</param>
        /// <param name="dataSourceName">数据源名称</param>
        internal void LoadColumnsSetting(string settingDirectory, string dataSourceName)
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
                int count = int.Parse(XmlEx.GetXElementAttributeValue(rootEle, "Count"));
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
                        item.Name = XmlEx.GetXElementAttributeValue(itemEle, "Name");
                        item.Width = int.Parse(XmlEx.GetXElementAttributeValue(itemEle, "Width"));
                        item.DisplayIndex = int.Parse(XmlEx.GetXElementAttributeValue(itemEle, "DisplayIndex"));
                        item.Visible = bool.Parse(XmlEx.GetXElementAttributeValue(itemEle, "Visible"));
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
        /// 获取或设置分页栏是否可见
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置分页栏是否可见")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool PagingVisible
        {
            get { return this.panelPage.Visible; }
            set
            {
                if (value == this.panelPage.Visible)
                {
                    return;
                }

                this.panelPage.Visible = value;
            }
        }

        /// <summary>
        /// 状态栏外部添加的控件字典集合[key:控件名称;value:控件]
        /// </summary>
        private readonly Dictionary<string, Control> _dicStatusControls = new Dictionary<string, Control>();

        /// <summary>
        /// 状态栏控件启用或是禁用状态设置
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="enable">状态[true:启用;false:禁用]</param>
        public void EnableStatusControl(string name, bool enable)
        {
            if (!this._dicStatusControls.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("不存在名称为{0}的控件", name));
            }

            this._dicStatusControls[name].Enabled = enable;
        }

        /// <summary>
        /// 获取状态栏控件启用或是禁用状态设置[true:启用;false:禁用]
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>状态[true:启用;false:禁用]</returns>
        public bool GetStatusControlEnable(string name)
        {
            if (!this._dicStatusControls.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("不存在名称为{0}的控件", name));
            }

            return this._dicStatusControls[name].Enabled;
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        private PageInfo _pageInfo = null;

        /// <summary>
        /// 获取分页信息
        /// </summary>
        [Browsable(false)]
        public PageInfo PageInfo
        {
            get { return this._pageInfo; }
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        public void SetPageInfo(PageInfo pageInfo)
        {
            //记录当前分页信息
            this._pageInfo = pageInfo;

            //设置显示信息
            labelPageInfo.Text = PageGridControlCommon.GetPageInfoStr(pageInfo);
            this.SetShowPageInfo(0);

            if (pageInfo == null || pageInfo.PageCount == 0)
            {
                //设置页数选择控件值
                numPage.Minimum = 0;
                numPage.Maximum = 0;
                numPage.Value = 0;
                numPage.ReadOnly = true;

                //禁用三个跳转按钮
                btnGo.Enabled = false;
                btnNextPage.Enabled = false;
                btnProPage.Enabled = false;
                return;
            }
            else
            {
                numPage.Minimum = 1;
                numPage.Maximum = pageInfo.PageCount;
                numPage.Value = pageInfo.PageIndex;
                numPage.ReadOnly = false;

                //启用三个跳转按钮
                btnGo.Enabled = true;
                btnNextPage.Enabled = true;
                btnProPage.Enabled = true;

                //如果当前页为小于等于1,则禁用前一页
                if (pageInfo.PageIndex <= 1)
                {
                    btnProPage.Enabled = false;
                }
                else
                {
                    btnProPage.Enabled = true;
                }

                //如果当前页大于等于总页数,则禁用后一页
                if (pageInfo.PageIndex >= pageInfo.PageCount)
                {
                    btnNextPage.Enabled = false;
                }
                else
                {
                    btnNextPage.Enabled = true;
                }

                //如果总页数小于等于1,则禁用跳转
                if (pageInfo.PageCount <= 1)
                {
                    btnGo.Enabled = false;
                }
                else
                {
                    btnGo.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 设置选中记录提示信息
        /// </summary>
        /// <param name="recordIndex">选中记录索引</param>
        public void SetShowPageInfo(int recordIndex)
        {
            labelRecordInfo.Text = PageGridControlCommon.GetRecordInfoStr(this._pageInfo, recordIndex);
        }

        /// <summary>
        /// 查询指定页数据事件
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("查询指定页数据事件")]
        internal event EventHandler<QueryAssignPageDataArgs> QueryAssignPageData;

        /// <summary>
        /// 触发查询指定页数据事件
        /// </summary>
        ///  <param name="totalCount">总数据记录数</param>
        /// <param name="pageCount">查询的数据总页数</param>
        /// <param name="pageIndex">当前要查询页数</param>
        private void OnRaiseQueryAssignPageData(long totalCount, int pageCount, int pageIndex)
        {
            try
            {
                if (this.QueryAssignPageData != null)
                {
                    this.QueryAssignPageData(this, new QueryAssignPageDataArgs(totalCount, pageCount, pageIndex));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 显示列设置控件
        /// </summary>
        private readonly ListBox _listBoxSettingCols = null;

        /// <summary>
        /// 表格标题右键菜单
        /// </summary>
        private readonly ContextMenuStrip _cmsColumnHeader = new ContextMenuStrip();

        /// <summary>
        /// 隐藏列绑定列表
        /// </summary>
        private readonly BindingListEx<DataGridViewColumn> _hidenCols = new BindingListEx<DataGridViewColumn>();

        /// <summary>
        /// 目标列
        /// </summary>
        private DataGridViewColumn _targetCol = null;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="listBoxSettingCols">显示列设置控件</param>
        public FGrid(ListBox listBoxSettingCols)
            : this()
        {
            try
            {
                //初始化页显示
                this.SetPageInfo(null);

                //隐藏列显示列表
                this._listBoxSettingCols = listBoxSettingCols;
                this._listBoxSettingCols.DataSource = this._hidenCols;
                this._listBoxSettingCols.DisplayMember = "HeaderText";
                this._listBoxSettingCols.MouseDoubleClick += _listBoxSettingCols_MouseDoubleClick;

                //右键菜单
                this.dataGridView.ColumnHeaderMouseClick += dataGridView_ColumnHeaderMouseClick;
                this.dataGridView.CellMouseClick += dataGridView_CellMouseClick;

                ToolStripMenuItem tsmiHidenCol = new ToolStripMenuItem("隐藏");
                tsmiHidenCol.Click += tsmiHidenCol_Click;
                this._cmsColumnHeader.Items.Add(tsmiHidenCol);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 表格数据区域单击事件->右键菜单显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && e.Clicks == 1)
                {
                    this.contextMenuStrip.Show(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 表格标题单击事件->右键菜单显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && e.Clicks == 1)
                {
                    this._targetCol = this.dataGridView.Columns[e.ColumnIndex];
                    //cms.Show(MousePosition.X, MousePosition.Y);
                    this._cmsColumnHeader.Show(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 标题右键菜单隐藏列
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
        /// 隐藏列显示控件双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _listBoxSettingCols_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridViewColumn targetCol = this._listBoxSettingCols.SelectedItem as DataGridViewColumn;
                if (targetCol == null)
                {
                    return;
                }

                targetCol.Visible = true;
                this._hidenCols.Remove(targetCol);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// FGrid_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FGrid_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProPage_Click(object sender, EventArgs e)
        {
            try
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

                this.OnRaiseQueryAssignPageData(this._pageInfo.TotalCount, this._pageInfo.PageCount, pageIndex);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            try
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

                this.OnRaiseQueryAssignPageData(this._pageInfo.TotalCount, this._pageInfo.PageCount, pageIndex);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 跳转到指定页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this._pageInfo == null)
                {
                    return;
                }

                int pageIndex = Convert.ToInt32(numPage.Value);
                this.OnRaiseQueryAssignPageData(this._pageInfo.TotalCount, this._pageInfo.PageCount, pageIndex);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.btnGo_LinkClicked(sender, null);
            }
        }

        #region 右键菜单
        /// <summary>
        /// 获取单元格值文本
        /// </summary>
        /// <returns>单元格值文本</returns>
        private string GetCellValueStr()
        {
            if (this.dataGridView.CurrentCell == null)
            {
                return null;
            }

            return this.GetCellValueStr(this.dataGridView.CurrentCell.OwningColumn.Name);
        }

        /// <summary>
        /// 获取单元格值文本
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>单元格值文本</returns>
        private string GetCellValueStr(string colName)
        {
            DataGridViewCell cell = this.dataGridView.CurrentRow.Cells[colName];
            return cell.FormattedValue != null ? cell.FormattedValue.ToString() : string.Empty;
        }

        /// <summary>
        /// 右键菜单打开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (this.dataGridView.DataSource == null //数据null
                    || this.dataGridView.RowCount == 0)//数据行数为0
                {
                    e.Cancel = true;
                    return;
                }

                if (this.dataGridView.CurrentRow == null)//没有选中行
                {
                    tsmiCopy.Enabled = false;
                    tsmiVisibleCopy.Enabled = false;
                    tsmiCopyRow.Enabled = false;
                }
                else
                {
                    tsmiVisibleCopy.Enabled = true;//启用复制可见列
                    tsmiCopyRow.Enabled = true;//启用复制整行
                    string copyText = this.GetCellValueStr();
                    if (string.IsNullOrEmpty(copyText))
                    {
                        tsmiCopy.Enabled = false;//禁用复制单元格
                    }
                    else
                    {
                        tsmiCopy.Enabled = true;//启用复制单元格
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string copyText = this.GetCellValueStr();
                if (string.IsNullOrEmpty(copyText))
                {
                    Clipboard.Clear();
                }
                else
                {
                    Clipboard.SetText(copyText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 复制可视列行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiVisibleCopy_Click(object sender, EventArgs e)
        {
            this.CopyRow(true);
        }

        /// <summary>
        /// 复制整行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCopyRow_Click(object sender, EventArgs e)
        {
            this.CopyRow(false);
        }

        /// <summary>
        /// 复制行
        /// </summary>
        /// <param name="isCopyVisible">是否只复制可视列[true:只复制可见列;false:全部复制]</param>
        private void CopyRow(bool isCopyVisible)
        {
            try
            {
                if (this.dataGridView.CurrentRow == null)
                {
                    return;
                }

                int rowHandle = this.dataGridView.CurrentRow.Index;
                StringBuilder sb = new StringBuilder();
                string cellValueStr = null;
                foreach (DataGridViewColumn col in this.dataGridView.Columns)
                {
                    if (!col.Visible && isCopyVisible)
                    {
                        continue;
                    }

                    cellValueStr = this.GetCellValueStr(col.Name);
                    if (!string.IsNullOrEmpty(cellValueStr))
                    {
                        sb.Append(cellValueStr);
                        sb.Append(" ");
                    }
                }

                if (sb.Length > 0)
                {
                    Clipboard.SetText(sb.ToString());
                }
                else
                {
                    Clipboard.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
