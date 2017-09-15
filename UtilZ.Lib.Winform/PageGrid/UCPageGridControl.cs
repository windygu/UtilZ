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

        /// <summary>
        /// 目标列
        /// </summary>
        private DataGridViewColumn _targetCol = null;

        public UCPageGridControl()
        {
            InitializeComponent();
        }

        public bool IsColSettingVisible
        {
            get { return panelColVisibleSetting.Visible; }
            set { panelColVisibleSetting.Visible = value; }
        }

        public object DataSource
        {
            get { return dataGridView.DataSource; }
            set { dataGridView.DataSource = value; }
        }

        private void UCPageGridControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this.listBoxColVisibleSetting.DataSource = this._hidenCols;
            this.listBoxColVisibleSetting.DisplayMember = "HeaderText";
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

        private void listBoxColVisibleSetting_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridViewColumn targetCol = this.listBoxColVisibleSetting.SelectedItem as DataGridViewColumn;
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
    }
}
