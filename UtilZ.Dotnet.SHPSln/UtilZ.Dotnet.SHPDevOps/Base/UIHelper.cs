using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid;

namespace UtilZ.Dotnet.SHPDevOps.Base
{
    internal class UIHelper
    {
        public static void BasicSetUCPageGridControl(UCPageGridControl control)
        {
            var dgv = control.GridControl;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.MultiSelect = false;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
