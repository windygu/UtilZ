using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    public partial class FServiceInsCountEdit : Form
    {
        public FServiceInsCountEdit()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public int ServiceInsCount
        {
            get
            {
                return (int)numServiceInsCount.Value;
            }
        }
    }
}
