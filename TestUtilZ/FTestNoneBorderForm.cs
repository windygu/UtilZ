using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Winform;

namespace TestUtilZ
{
    public partial class FTestNoneBorderForm : NoneBorderForm
    {
        public FTestNoneBorderForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FTestNoneBorderForm_Load(object sender, EventArgs e)
        {

        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.SetWindowState(FormWindowState.Minimized);
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            this.SetWindowState(FormWindowState.Maximized);
        }

        private void btnNM_Click(object sender, EventArgs e)
        {
            this.SwitchWindowState(cbFullScreen.Checked);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FTestNoneBorderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = cbAllowClose.Checked;
        }
    }
}
