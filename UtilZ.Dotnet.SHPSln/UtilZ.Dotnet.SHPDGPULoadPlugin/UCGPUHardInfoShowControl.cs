using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Dotnet.SHPDGPULoadPlugin
{
    public partial class UCGPUHardInfoShowControl : UserControl
    {
        public UCGPUHardInfoShowControl()
        {
            InitializeComponent();
        }

        private void UCGPUHardInfoShowControl_Load(object sender, EventArgs e)
        {

        }

        internal void RefreshHardInfo(string hardInfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    this.RefreshHardInfo(hardInfo);
                }));
            }
            else
            {
                rtxt.Text = hardInfo;
            }
        }
    }
}
