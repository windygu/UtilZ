using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Lib.Winform
{
    /// <summary>
    /// 扩展DataGridView
    /// </summary>
    public class ZDataGridView : DataGridView
    {
        /// <summary>
        /// 鼠标右键是否选中行
        /// </summary>
        private bool _mouseRightButtonChangeSelectedRow = false;

        /// <summary>
        /// 鼠标右键是否选中行[true:选中;false:不选中]
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("鼠标右键是否选中行[true:选中;false:不选中]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("扩展")]
        public bool MouseRightButtonChangeSelectedRow
        {
            get { return _mouseRightButtonChangeSelectedRow; }
            set
            {
                if (_mouseRightButtonChangeSelectedRow == value)
                {
                    return;
                }

                _mouseRightButtonChangeSelectedRow = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ZDataGridView() : base()
        {

        }

        /// <summary>
        /// 重写OnCellMouseDown
        /// </summary>
        /// <param name="e">e</param>
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);

            if (this._mouseRightButtonChangeSelectedRow)
            {
                this.UpdateSelectedRow(e);
            }
        }

        /// <summary>
        /// 更新选中行
        /// </summary>
        /// <param name="e">e</param>
        private void UpdateSelectedRow(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            //清空之前的选中项
            this.ClearSelection();
            if (!this.Rows[e.RowIndex].Selected)
            {
                switch (this.SelectionMode)
                {
                    case DataGridViewSelectionMode.RowHeaderSelect:
                        this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        break;
                    case DataGridViewSelectionMode.FullRowSelect:
                        this.Rows[e.RowIndex].Selected = true;
                        break;
                    case DataGridViewSelectionMode.CellSelect:
                    case DataGridViewSelectionMode.ColumnHeaderSelect:
                    case DataGridViewSelectionMode.FullColumnSelect:
                        this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        break;
                }
            }
        }
    }
}
