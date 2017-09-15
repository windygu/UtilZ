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

namespace UtilZ.Lib.Winform.PageGrid
{
    public partial class UCPageGridControl : UserControl
    {
        /// <summary>
        /// 隐藏列绑定列表
        /// </summary>
        private readonly UIBindingList<DataGridViewColumn> _hidenCols = new UIBindingList<DataGridViewColumn>();

        private readonly FPageGridColumnsSetting _fPageGridColumnsSetting;

        /// <summary>
        /// 目标列
        /// </summary>
        private DataGridViewColumn _targetCol = null;

        public UCPageGridControl()
        {
            InitializeComponent();

            this._fPageGridColumnsSetting = new FPageGridColumnsSetting(this.panelColVisibleSetting, this.labelTitle, this._hidenCols);
            this._fPageGridColumnsSetting.SaveColumnDisplaySetting += _fPageGridColumnsSetting_SaveColumnDisplaySetting;
        }

        private void _fPageGridColumnsSetting_SaveColumnDisplaySetting(object sender, EventArgs e)
        {
            try
            {
                //..
                this._fPageGridColumnsSetting.ColumnDisplaySettingChanged = false;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsColSettingVisible
        {
            get { return panelColVisibleSetting.Visible; }
            set { panelColVisibleSetting.Visible = value; }
        }

        public bool IsPageVissible
        {
            get { return panelPage.Visible; }
            set { panelPage.Visible = value; }
        }

        public object DataSource
        {
            get { return dataGridView.DataSource; }
            set
            {
                dataGridView.DataSource = value;

            }
        }


        public PageGridColumnSettingStatus ColumnSettingStatus
        {
            get { return _fPageGridColumnsSetting.ColumnSettingStatus; }
            set { _fPageGridColumnsSetting.ColumnSettingStatus = value; }
        }

        private void UCPageGridControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this._fPageGridColumnsSetting.Show();
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && e.Clicks == 1)
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

        private void tsmiShowHidenColList_Click(object sender, EventArgs e)
        {
            panelColVisibleSetting.Visible = true;
        }

        private void tsmiHidenHidenColList_Click(object sender, EventArgs e)
        {
            panelColVisibleSetting.Visible = false;
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {
            this._fPageGridColumnsSetting.SwitchColumnSettingStatus(PageGridColumnSettingStatus.Dock);
        }

        private void dataGridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this._fPageGridColumnsSetting.ColumnDisplaySettingChanged = true;
        }
    }
}
