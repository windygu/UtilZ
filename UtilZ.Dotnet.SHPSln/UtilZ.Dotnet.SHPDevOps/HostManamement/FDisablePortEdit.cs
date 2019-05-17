using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class FDisablePortEdit : Form
    {
        private readonly BindingCollection<HostDisablePortInfo> _hostDisablePortInfoList;
        private readonly HostDisablePortInfo _modifyItem = null;
        public FDisablePortEdit()
        {
            InitializeComponent();
        }

        public FDisablePortEdit(BindingCollection<HostDisablePortInfo> hostDisablePortInfoList, HostDisablePortInfo modifyItem = null)
            : this()
        {
            this._hostDisablePortInfoList = hostDisablePortInfoList;
            this._modifyItem = modifyItem;

            if (modifyItem != null)
            {
                txtDes.Text = modifyItem.Des;
                numPort.Value = modifyItem.Port;
            }
        }

        private void FDisablePortEdit_Load(object sender, EventArgs e)
        {

        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._hostDisablePortInfoList.Where(t => { return t.Port == numPort.Value; }).Count() > 0)
                {
                    MessageBox.Show($"端口[{numPort.Value}]已存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (this._modifyItem == null)
                {
                    var modifyItem = new HostDisablePortInfo();
                    this.GetValue(modifyItem);
                    this._hostDisablePortInfoList.Add(modifyItem);
                }
                else
                {
                    this.GetValue(this._modifyItem);
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void GetValue(HostDisablePortInfo modifyItem)
        {
            modifyItem.Des = txtDes.Text;
            modifyItem.Port = (int)numPort.Value;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
