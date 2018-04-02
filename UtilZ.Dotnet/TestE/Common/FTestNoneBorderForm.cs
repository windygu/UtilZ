using System;
using System.Windows.Forms;
using UtilZ.Dotnet.WindowEx.Winform.Controls;

namespace TestE.Common
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
            this.FormResizeStyle = ResizeStyle.Right | ResizeStyle.Left | ResizeStyle.Top;
            //this.FormResizeStyle = ResizeStyle.None;
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
