using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Dotnet.SHPAgentProtect
{
    public partial class FProtect : Form
    {
        private readonly ProtectBLL _bll;
        public FProtect()
        {
            InitializeComponent();
        }

        internal FProtect(Process agentProcess)
            : this()
        {
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.Hide();

            this._bll = new ProtectBLL(agentProcess);
        }

        private void FProtect_Load(object sender, EventArgs e)
        {

        }
    }
}
