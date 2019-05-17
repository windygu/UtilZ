using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class FScriptEdit : Form
    {
        public FScriptEdit()
        {
            InitializeComponent();
        }

        private void FScriptEdit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            DropdownBoxHelper.BindingEnumToComboBox<ScriptType>(comboBoxScriptType, ScriptType.Bat);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtxtScriptContent.Text))
            {
                Loger.Warn("脚本内容不能为空或全空格");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public ScriptType ScriptType
        {
            get
            {
                return DropdownBoxHelper.GetEnumFromComboBox<ScriptType>(comboBoxScriptType);
            }
        }

        public string ScriptContent
        {
            get
            {
                return rtxtScriptContent.Text;
            }
        }

        public int MillisecondsTimeout
        {
            get
            {
                return (int)numSecondsTimeout.Value * 1000;
            }
        }
    }
}
