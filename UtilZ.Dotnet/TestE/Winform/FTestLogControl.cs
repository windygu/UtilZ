using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestE.Winform
{
    public partial class FTestLogControl : Form
    {
        public FTestLogControl()
        {
            InitializeComponent();
        }

        private void FTestLogControl_Load(object sender, EventArgs e)
        {
            logControl1.MaxItemCount = 10;
            checkBox1.Checked = logControl1.IsLock;
        }

        private int _index = 0;
        private void btnTest_Click(object sender, EventArgs e)
        {
            logControl1.AddLogStyleForColor(string.Format("{0}_{1}_safsdf", DateTime.Now, _index++), Color.Red);
            logControl1.AddLogStyleForClass(string.Format("{0}_{1} Error", DateTime.Now, _index++), "error");
            logControl1.AddLogStyleForClass(string.Format("{0}_{1} Warn", DateTime.Now, _index++), "warn");
            logControl1.AddLogStyleForClass(string.Format("{0}_{1} Info", DateTime.Now, _index++), "info");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            logControl1.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            logControl1.IsLock = checkBox1.Checked;
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            //logControl1.SetTemplate(@"LogControlTemplate1.html", "bid", "p");
            logControl1.SetTemplate(@"LogControlTemplate2.html", "uid", "li");
        }
    }
}
