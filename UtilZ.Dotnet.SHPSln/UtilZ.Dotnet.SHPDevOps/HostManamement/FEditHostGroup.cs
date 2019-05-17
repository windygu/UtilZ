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
    public partial class FEditHostGroup : Form
    {
        public FEditHostGroup()
        {
            InitializeComponent();
        }

        private readonly HostManager _bll;
        private readonly long _parentId;
        private HostGroup _group = null;

        public FEditHostGroup(HostManager bll, long parentId, HostGroup hostGroup)
            : this()
        {
            this._bll = bll;
            this._parentId = parentId;
            this._group = hostGroup;

            if (hostGroup != null)
            {
                txtHostGroupName.Text = hostGroup.Name;
                txtHostGroupDes.Text = hostGroup.Des;
            }
        }

        public HostGroup GetHostGroup()
        {
            return this._group;
        }

        private void FEditHostGroup_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtHostGroupName.Text))
                {
                    Loger.Info("分组名称不为空");
                    return;
                }

                if (this._group == null)
                {
                    var group = new HostGroup();
                    group.ParentId = this._parentId;
                    group.Name = txtHostGroupName.Text;
                    group.Des = txtHostGroupDes.Text;
                    this._bll.AddHostGroup(group);
                    this._group = group;
                    Loger.Info($"添加分组[{txtHostGroupName.Text}]成功");
                }
                else
                {
                    this._group.ParentId = this._parentId;
                    this._group.Name = txtHostGroupName.Text;
                    this._group.Des = txtHostGroupDes.Text;
                    this._bll.ModifyHostGroup(this._group);
                    Loger.Info($"修改分组[{txtHostGroupName.Text}]成功");
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
