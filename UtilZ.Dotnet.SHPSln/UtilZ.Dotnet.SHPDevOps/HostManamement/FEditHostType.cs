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
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsBLL;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class FEditHostType : Form
    {
        public FEditHostType()
        {
            InitializeComponent();
        }

        private readonly HostManager _bll;
        private readonly HostTypeItem _hostTypeItem = null;
        public FEditHostType(HostManager bll, HostTypeItem hostTypeItem)
            : this()
        {
            this._bll = bll;
            this._hostTypeItem = hostTypeItem;

            if (hostTypeItem != null)
            {
                txtHostTypeName.Text = hostTypeItem.Name;
                txtHostTypeDes.Text = hostTypeItem.Des;
            }
        }

        internal HostTypeItem GetHostTypeItem()
        {
            throw new NotImplementedException();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtHostTypeName.Text))
                {
                    Loger.Info("类型名称不为空");
                    return;
                }

                if (this._hostTypeItem == null)
                {
                    var hostTypeItem = new HostTypeItem();
                    hostTypeItem.Name = txtHostTypeName.Text;
                    hostTypeItem.Des = txtHostTypeDes.Text;
                    this._bll.AddHostType(hostTypeItem);
                    Loger.Info($"添加主机类型[{txtHostTypeName.Text}]成功");
                }
                else
                {
                    this._hostTypeItem.Name = txtHostTypeName.Text;
                    this._hostTypeItem.Des = txtHostTypeDes.Text;
                    this._bll.ModifyHostType(this._hostTypeItem);
                    Loger.Info($"修改主机类型[{txtHostTypeName.Text}]成功");
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "提交分组信息异常");
            }
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
