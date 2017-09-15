using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
        private readonly Control _titleControl;
        /// <summary>
        /// 隐藏列绑定列表
        /// </summary>
        private readonly UIBindingList<DataGridViewColumn> _hidenCols;
        private readonly List<DataGridViewColumn> _srcHidenCols;
        private PageGridColumnSettingStatus _columnSettingStatus = PageGridColumnSettingStatus.Dock;

        private bool _isLoaded = false;
        private Size _lastParentControlSize;

        /// <summary>
        /// 获取或设置当前列设置状态
        /// </summary>
        public PageGridColumnSettingStatus ColumnSettingStatus
        {
            get { return _columnSettingStatus; }
            set
            {
                _columnSettingStatus = value;
                this.SwitchColumnSettingStatus(this._columnSettingStatus);
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
        /// <param name="titleControl">隐藏时的标题控件</param>
        /// <param name="hidenCols">隐藏列集合</param>
        public FPageGridColumnsSetting(Control parentControl, Control titleControl, UIBindingList<DataGridViewColumn> hidenCols) : this()
        {
            this.Dock = DockStyle.Fill;
            this.IsAllowMinimize = false;
            this.IsUseInOutEffect = false;
            this._parentControl = parentControl;
            this._titleControl = titleControl;
            this._hidenCols = hidenCols;
            this._lastParentControlSize = parentControl.Size;

            this._srcHidenCols = this._hidenCols.ToList();
            this.listBoxCol.DataSource = this._hidenCols;
            this.listBoxCol.DisplayMember = "HeaderText";
            this.listBoxCol.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxCol_MouseDoubleClick);

            this.SwitchColumnSettingStatus(this._columnSettingStatus);
        }

        private void FPageGridColumnsSetting_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this._isLoaded = true;
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
        public void SwitchColumnSettingStatus(PageGridColumnSettingStatus status)
        {
            this._columnSettingStatus = status;
            switch (status)
            {
                case PageGridColumnSettingStatus.Dock:
                    this.StatusDock();
                    break;
                case PageGridColumnSettingStatus.Float:
                    this.StatusFloat();
                    break;
                case PageGridColumnSettingStatus.Hiden:
                    this.StatusHiden();
                    break;
                default:
                    MessageBox.Show(string.Format("不识别的状态:{0}", status.ToString()));
                    return;
            }

            if (!this._isLoaded)
            {
                this.Show();
            }
        }

        private void StatusHiden()
        {
            this._parentControl.Visible = true;
            this._parentControl.Controls.Remove(this);
            this.Visible = false;
            this._titleControl.Visible = true;
            this._parentControl.Width = this._parentControl.MinimumSize.Width;
        }

        private void StatusFloat()
        {
            this._parentControl.Controls.Remove(this);
            this._parentControl.Visible = false;
            this.TopLevel = true;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.ShowInTaskbar = false;
            this._titleControl.Visible = false;
            this.IsDisableDragMoveForm = false;
            this.Location = Cursor.Position;
        }

        private void StatusDock()
        {
            this.TopLevel = false;
            this.Visible = true;
            this.IsDisableDragMoveForm = true;
            this._titleControl.Visible = false;
            this._parentControl.Size = this._lastParentControlSize;
            this._parentControl.Controls.Add(this);
            this._parentControl.Visible = true;
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
    }
}
