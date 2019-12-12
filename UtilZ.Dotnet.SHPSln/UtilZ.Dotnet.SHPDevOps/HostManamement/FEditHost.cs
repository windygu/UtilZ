using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class FEditHost : Form, ICollectionOwner
    {
        public FEditHost()
        {
            InitializeComponent();
        }

        private readonly HostManager _bll;
        private BindingCollection<HostDisablePortInfo> _hostDisablePortInfoList;
        private HostInfo _hostInfo = null;
        private readonly long _hostGroupId;

        public FEditHost(HostManager bll, long hostGroupId, HostInfo hostInfo)
            : this()
        {
            this._bll = bll;
            this._hostGroupId = hostGroupId;
            this._hostInfo = hostInfo;

            this._hostDisablePortInfoList = new BindingCollection<HostDisablePortInfo>(this);

            var hostTypeList = bll.HostTypeList;
            if (hostTypeList == null || hostTypeList.Count == 0)
            {
                throw new ArgumentException("主机类型为空,请先添加主机类型");
            }

            HostTypeItem defaultSelecteItem = null;
            if (hostInfo != null)
            {
                txtHostName.Text = hostInfo.Name;
                txtHostIp.Text = hostInfo.Ip;
                this._hostDisablePortInfoList.AddRange(hostInfo.HostDisablePortInfoList);
                defaultSelecteItem = hostTypeList.Where(t => { return t.Id == hostInfo.HostTypeId; }).FirstOrDefault();
            }

            DropdownBoxHelper.BindingIEnumerableGenericToComboBox<HostTypeItem>(comboBoxHostType, hostTypeList, nameof(HostTypeItem.Name), defaultSelecteItem);
        }

        public HostInfo GetHostInfo()
        {
            return this._hostInfo;
        }

        private void FEditHost_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            dgvDisablePort.GridControl.ContextMenuStrip = cmsDisablePort;
            dgvDisablePort.GridControl.MultiSelect = true;
            dgvDisablePort.IsLastColumnAutoSizeModeFill = true;
            dgvDisablePort.ShowData(this._hostDisablePortInfoList.DataSource);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtHostName.Text))
                {
                    Loger.Info("名称不为空");
                    return;
                }

                var ip = txtHostIp.Text.Trim();
                IPAddress ipaddr;
                if (!IPAddress.TryParse(ip, out ipaddr))
                {
                    Loger.Info("主机ip地址无效");
                    return;
                }


                if (this._hostInfo == null)
                {
                    var hostInfo = new HostInfo();
                    this.GetValue(hostInfo);
                    this.Commit(hostInfo, true, false, this._bll);
                    this._hostInfo = hostInfo;
                }
                else
                {
                    bool ipChanged = !string.Equals(this._hostInfo.Ip, ip);
                    this.GetValue(this._hostInfo);
                    this.Commit(this._hostInfo, false, ipChanged, this._bll);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "提交主机信息异常");
            }
        }

        private void Commit(HostInfo hostInfo, bool editType, bool ipChanged, HostManager bll)
        {
            var para = new PartAsynWaitPara<Tuple<bool, bool, HostInfo, HostManager>, object>();
            para.Caption = "提交主机";
            para.IsShowCancel = false;
            para.Para = new Tuple<bool, bool, HostInfo, HostManager>(editType, ipChanged, hostInfo, bll);
            para.Function = (p) =>
            {
                if (p.Para.Item1)
                {
                    p.Para.Item4.AddHost(p.Para.Item3);
                    Loger.Info($"添加主机[{txtHostName.Text}]成功");
                }
                else
                {
                    p.Para.Item4.ModifyHost(p.Para.Item3, p.Para.Item2);
                    Loger.Info($"修改主机[{txtHostName.Text}]成功");
                }

                return null;
            };

            para.Completed = (ret) =>
            {
                if (ret.Status == PartAsynExcuteStatus.Completed)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else if (ret.Status == PartAsynExcuteStatus.Exception)
                {
                    Loger.Error(ret.Exception, "提交主机异常");
                }
            };

            WinformPartAsynWaitHelper.Wait(para, this);
        }

        private void GetValue(HostInfo hostInfo)
        {
            hostInfo.Name = txtHostName.Text;
            hostInfo.Ip = txtHostIp.Text;
            hostInfo.HostGoupId = this._hostGroupId;
            var hostType = DropdownBoxHelper.GetGenericFromComboBox<HostTypeItem>(comboBoxHostType);
            hostInfo.HostTypeId = hostType.Id;
            hostInfo.HostDisablePortInfoList.Clear();
            hostInfo.HostDisablePortInfoList.AddRange(this._hostDisablePortInfoList);
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #region 不可用端口右键菜单
        private void tsmiDisablePortAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FDisablePortEdit(this._hostDisablePortInfoList, null);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDisablePortModify_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = dgvDisablePort.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var oldItem = (HostDisablePortInfo)selectedRows[0].DataBoundItem;
                var frm = new FDisablePortEdit(this._hostDisablePortInfoList, oldItem);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDisablePortDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = dgvDisablePort.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                foreach (var selectedRow in selectedRows)
                {
                    this._hostDisablePortInfoList.Remove((HostDisablePortInfo)selectedRow.DataBoundItem);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDisablePortClear_Click(object sender, EventArgs e)
        {
            try
            {
                this._hostDisablePortInfoList.Clear();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void cmsDisablePort_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                var selectedRows = dgvDisablePort.SelectedRows;
                if (selectedRows != null)
                {
                    tsmiDisablePortModify.Enabled = selectedRows.Length > 1;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
