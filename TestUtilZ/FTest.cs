using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base.DataStruct.UIBinding;
using UtilZ.Lib.Base.Extend;
using UtilZ.Lib.Base.Log;

namespace TestUtilZ
{
    public partial class FTest : Form
    {
        public FTest()
        {
            InitializeComponent();
        }

        private void FTest_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            var dgv = this.ucPageGridControl1.GridControl;
            dgv.MouseRightButtonChangeSelectedRow = true;
            dgv.MultiSelect = false;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            var items = new UIBindingList<TestDGVModel>();
            items.Add(new TestDGVModel());
            items.Add(new TestDGVModel());
            items.Add(new TestDGVModel());
            try
            {
                ucPageGridControl1.ShowData(items);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucPageGridControl1.ColumnSettingVisible = !ucPageGridControl1.ColumnSettingVisible;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ucPageGridControl1.PagingVisible = !ucPageGridControl1.PagingVisible;
        }
    }

    public class TestDGVModel
    {
        public bool IsSelected { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Addr { get; set; }
    }
}
