using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestUtilZ
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UtilZ.Lib.Winform.PageGrid.FPageGridColumnsSetting(new Control() { Size = new System.Drawing.Size(200, 200) }, new Control(), new UtilZ.Lib.Base.DataStruct.UIBinding.UIBindingList<DataGridViewColumn>()) { ColumnSettingStatus = UtilZ.Lib.Winform.PageGrid.PageGridColumnSettingStatus.Float });
            Application.Run(new FTestNoneBorderForm());
        }
    }
}
