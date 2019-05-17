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

namespace UtilZ.Dotnet.SHPDevOps.Base
{
    public partial class FHostChioce : Form
    {
        private readonly List<HostGroup> _hostGroupList;
        private readonly List<HostInfo> _allHostInfoList;
        private readonly bool _muiltSelect;
        public FHostChioce()
        {
            InitializeComponent();
        }

        public FHostChioce(List<HostGroup> hostGroupList, List<HostInfo> allHostInfoList, bool muiltSelect)
            : this()
        {
            if (allHostInfoList == null || allHostInfoList.Count == 0)
            {
                throw new ArgumentException("主机列表不能为空", nameof(allHostInfoList));
            }

            this._hostGroupList = hostGroupList;
            this._allHostInfoList = allHostInfoList;
            this._muiltSelect = muiltSelect;
        }

        private List<HostInfo> _hostInfoList = null;
        public List<HostInfo> HostInfoList
        {
            get { return this._hostInfoList; }
        }

        private void FHostChioce_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                ucHostTreeControl.RefreshHostTree(this._hostGroupList, this._allHostInfoList);
                ucHostTreeControl.tvHost.CheckBoxes = true;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this._hostInfoList = ucHostTreeControl.GetCheckHostInfoList();
                if (this._hostInfoList.Count == 0)
                {
                    Loger.Warn("未选择任何主机");
                    return;
                }

                if (!this._muiltSelect && this._hostInfoList.Count > 1)
                {
                    Loger.Warn($"选择主机数{this._hostInfoList.Count}过多,只允许选择一个主机");
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
