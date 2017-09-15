using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base.Extend;

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

            ucPageGridControl1.DataSource = new List<TestDGVModel>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucPageGridControl1.IsColSettingVisible = !ucPageGridControl1.IsColSettingVisible;
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
