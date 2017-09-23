using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.DataStruct.UIBinding;
using UtilZ.Lib.Base.Log;

namespace UtilZ.Lib.Winform.PageGrid
{
    /// <summary>
    /// 分页数据显示控件列设置窗口
    /// </summary>
    public partial class FPageGridColumnsSetting : NoneBorderForm
    {
        private readonly Control _parentControl;

        /// <summary>
        /// 当前绑定的全部列集合
        /// </summary>
        private readonly UIBindingList<ColumnSettingInfo> _columnSettingInfos = new UIBindingList<ColumnSettingInfo>();

        /// <summary>
        /// 隐藏列绑定列表
        /// </summary>
        private readonly UIBindingList<DataGridViewColumn> _hidenCols;
        private readonly List<DataGridViewColumn> _srcHidenCols;
        private PageGridColumnSettingStatus _columnSettingStatus = PageGridColumnSettingStatus.Hiden;

        private Size _lastParentControlSize;

        /// <summary>
        /// 获取或设置当前列设置状态
        /// </summary>
        public PageGridColumnSettingStatus ColumnSettingStatus
        {
            get { return _columnSettingStatus; }
            set
            {
                if (_columnSettingStatus == value)
                {
                    return;
                }

                PageGridColumnSettingStatus oldStatus = _columnSettingStatus;
                _columnSettingStatus = value;
                this.SwitchColumnSettingStatus(this._columnSettingStatus, oldStatus);
            }
        }

        private bool _isColumnDisplaySettingChange = false;

        /// <summary>
        /// 列显示是否改变
        /// </summary>
        public bool ColumnDisplaySettingChanged
        {
            get { return _isColumnDisplaySettingChange; }
            set
            {
                if (_isColumnDisplaySettingChange == value)
                {
                    return;
                }

                _isColumnDisplaySettingChange = value;
            }
        }

        /// <summary>
        /// 保存列显示设置事件
        /// </summary>
        public event EventHandler SaveColumnDisplaySetting;

        /// <summary>
        /// 列显示高级设置是否可见
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AdvanceSettingVisible
        {
            get { return this.tabControl.TabPages.Contains(this.tpColSetting); }
            set
            {
                if (value)
                {
                    if (this.tabControl.TabPages.Contains(this.tpColSetting))
                    {
                        return;
                    }
                    else
                    {
                        this.tabControl.TabPages.Add(this.tpColSetting);
                    }
                }
                else
                {
                    if (this.tabControl.TabPages.Contains(this.tpColSetting))
                    {
                        this.tabControl.TabPages.Remove(this.tpColSetting);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FPageGridColumnsSetting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="hidenCols">隐藏列集合</param>
        public FPageGridColumnsSetting(Control parentControl, UIBindingList<DataGridViewColumn> hidenCols) : this()
        {
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            this.IsAllowMinimize = false;
            this.IsUseInOutEffect = false;
            this._parentControl = parentControl;
            this._hidenCols = hidenCols;
            this._lastParentControlSize = new Size(100, 100);

            this._srcHidenCols = this._hidenCols.ToList();
            this.listBoxCol.DataSource = this._hidenCols;
            this.listBoxCol.DisplayMember = "HeaderText";
            this.listBoxCol.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxCol_MouseDoubleClick);

            this.dgvColumnSetting.DataSource = this._columnSettingInfos;
            this.dgvColumnSetting.Columns[this.dgvColumnSetting.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.Visible = true;
            this.SwitchColumnSettingStatus(this._columnSettingStatus, PageGridColumnSettingStatus.Dock);
        }

        private void FPageGridColumnsSetting_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

        }

        private void listBoxCol_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridViewColumn targetCol = this.listBoxCol.SelectedItem as DataGridViewColumn;
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

        private void pictureBoxMenu_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip.Show(Cursor.Position);
        }

        private void tsmiHiden_Click(object sender, EventArgs e)
        {
            this.ColumnSettingStatus = PageGridColumnSettingStatus.Hiden;
        }

        private void tsmiFloat_Click(object sender, EventArgs e)
        {
            this.ColumnSettingStatus = PageGridColumnSettingStatus.Float;
        }

        private void tsmiDock_Click(object sender, EventArgs e)
        {
            this.ColumnSettingStatus = PageGridColumnSettingStatus.Dock;
        }

        /// <summary>
        /// 切换列设置状态
        /// </summary>
        /// <param name="status">列设置状态</param>
        /// <param name="oldStatus">旧状态</param>
        private void SwitchColumnSettingStatus(PageGridColumnSettingStatus status, PageGridColumnSettingStatus oldStatus)
        {
            if (!this.Visible)
            {
                return;
            }

            this._columnSettingStatus = status;
            switch (status)
            {
                case PageGridColumnSettingStatus.Dock:
                    this.StatusDock(oldStatus, status);
                    break;
                case PageGridColumnSettingStatus.Float:
                    this.StatusFloat(oldStatus, status);
                    break;
                case PageGridColumnSettingStatus.Hiden:
                    this.StatusHiden(oldStatus, status);
                    break;
                default:
                    MessageBox.Show(string.Format("不识别的状态:{0}", status.ToString()));
                    return;
            }
        }

        private readonly Size _labelTitleSizeH = new Size(53, 12);
        private readonly Size _labelTitleSizeV = new Size(15, 50);
        private void StatusHiden(PageGridColumnSettingStatus oldStatus, PageGridColumnSettingStatus newStatus)
        {
            this._lastParentControlSize = this.Size;
            this.labelTitle.Size = this._labelTitleSizeV;
            this.Width = this._parentControl.MinimumSize.Width;
            this.tabControl.Visible = false;
            this.IsDisableDragMoveForm = true;
            this.FormResizeStyle = ResizeStyle.None;

            if (oldStatus == PageGridColumnSettingStatus.Float)
            {
                this.TopLevel = false;
                this._parentControl.Controls.Add(this);
            }
        }

        private void StatusFloat(PageGridColumnSettingStatus oldStatus, PageGridColumnSettingStatus newStatus)
        {
            this._parentControl.Controls.Remove(this);
            this.TopLevel = true;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.ShowInTaskbar = false;
            this.IsDisableDragMoveForm = false;
            this.Location = Cursor.Position;
            this.FormResizeStyle = ResizeStyle.All;
            if (oldStatus == PageGridColumnSettingStatus.Hiden)
            {
                this.Size = this._lastParentControlSize;
                this.labelTitle.Size = this._labelTitleSizeH;
                this.tabControl.Visible = true;
            }
        }

        private void StatusDock(PageGridColumnSettingStatus oldStatus, PageGridColumnSettingStatus newStatus)
        {
            this.TopLevel = false;
            this.IsDisableDragMoveForm = false;
            this._parentControl.Controls.Add(this);
            this.FormResizeStyle = ResizeStyle.Left;
            if (oldStatus == PageGridColumnSettingStatus.Hiden)
            {
                this.Size = this._lastParentControlSize;
                this.labelTitle.Size = this._labelTitleSizeH;
                this.tabControl.Visible = true;
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            var handler = this.SaveColumnDisplaySetting;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool tsmiSaveEnabled;
            if (this._isColumnDisplaySettingChange)
            {
                tsmiSaveEnabled = true;
            }
            else
            {
                if (this._hidenCols.Count != this._srcHidenCols.Count)
                {
                    tsmiSaveEnabled = true;
                }
                else
                {
                    tsmiSaveEnabled = false;
                    foreach (var hidenCol in this._hidenCols)
                    {
                        if (!this._srcHidenCols.Contains(hidenCol))
                        {
                            tsmiSaveEnabled = true;
                            break;
                        }
                    }
                }
            }

            this.tsmiSave.Enabled = tsmiSaveEnabled;

            switch (this._columnSettingStatus)
            {
                case PageGridColumnSettingStatus.Dock:
                    this.tsmiFloat.Enabled = true;
                    this.tsmiHiden.Enabled = true;
                    this.tsmiDock.Enabled = false;
                    break;
                case PageGridColumnSettingStatus.Float:
                    this.tsmiFloat.Enabled = false;
                    this.tsmiHiden.Enabled = true;
                    this.tsmiDock.Enabled = true;
                    break;
                case PageGridColumnSettingStatus.Hiden:
                    this.tsmiFloat.Enabled = false;
                    this.tsmiHiden.Enabled = false;
                    this.tsmiDock.Enabled = false;
                    break;
                default:
                    MessageBox.Show(string.Format("不识别的状态:{0}", this._columnSettingStatus.ToString()));
                    return;
            }
        }

        /// <summary>
        /// 更新高级设置
        /// </summary>
        /// <param name="cols">显示列集合</param>
        public void UpdateAdvanceSetting(DataGridViewColumnCollection cols)
        {
            this._columnSettingInfos.Clear();
            if (cols == null || cols.Count == 0)
            {
                return;
            }

            foreach (DataGridViewColumn col in cols)
            {
                this._columnSettingInfos.Add(new ColumnSettingInfo(col, this.ColumnVisibleChangedNotify));
            }
        }

        /// <summary>
        /// 列可见性改变通知处理方法
        /// </summary>
        /// <param name="col">DataGridViewColumn</param>
        private void ColumnVisibleChangedNotify(DataGridViewColumn col)
        {
            if (col.Visible)
            {
                if (this._hidenCols.Contains(col))
                {
                    this._hidenCols.Remove(col);
                }
            }
            else
            {
                if (!this._hidenCols.Contains(col))
                {
                    this._hidenCols.Add(col);
                }
            }
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {
            PageGridColumnSettingStatus newStatus;
            if (this._columnSettingStatus == PageGridColumnSettingStatus.Hiden)
            {
                newStatus = PageGridColumnSettingStatus.Dock;
            }
            else if (this._columnSettingStatus == PageGridColumnSettingStatus.Dock)
            {
                newStatus = PageGridColumnSettingStatus.Hiden;
            }
            else
            {
                return;
            }

            this.SwitchColumnSettingStatus(newStatus, this._columnSettingStatus);
        }
    }

    /// <summary>
    /// 列设置类
    /// </summary>
    public class ColumnSettingInfo : NBaseModel
    {
        /// <summary>
        /// 列可见性改变通知委托
        /// </summary>
        private Action<DataGridViewColumn> _columnVisibleChangedNotify;

        /// <summary>
        /// 目标列
        /// </summary>
        [Browsable(false)]
        public DataGridViewColumn Column { get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [DisplayName("是否显示")]
        public bool Visible
        {
            get { return this.Column.Visible; }
            set
            {
                this.Column.Visible = value;
                this.OnRaisePropertyChanged("Visible");
                var handler = _columnVisibleChangedNotify;
                if (handler != null)
                {
                    handler(this.Column);
                }
            }
        }

        /// <summary>
        /// 列标题
        /// </summary>
        [DisplayName("列")]
        public string Header
        {
            get { return this.Column.HeaderText; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="column">列</param>
        /// <param name="columnVisibleChangedNotify">列可见性改变通知委托</param>
        public ColumnSettingInfo(DataGridViewColumn column, Action<DataGridViewColumn> columnVisibleChangedNotify)
        {
            this.Column = column;
            this._columnVisibleChangedNotify = columnVisibleChangedNotify;
        }
    }
}
