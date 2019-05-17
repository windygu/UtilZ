using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPDevOpsBLL;

namespace UtilZ.Dotnet.SHPDevOps.Setting
{
    public partial class UCSettingControl : UserControl
    {
        private DevOpsBLL _bll;
        public UCSettingControl()
        {
            InitializeComponent();
        }

        public void Init(DevOpsBLL bll)
        {
            this._bll = bll;
        }

        private void UCSettingControl_Load(object sender, EventArgs e)
        {

        }

        private void btnDevOpsMigrate_Click(object sender, EventArgs e)
        {
            this._bll.HostManager.DevOpsMigrate();
        }
    }
}
