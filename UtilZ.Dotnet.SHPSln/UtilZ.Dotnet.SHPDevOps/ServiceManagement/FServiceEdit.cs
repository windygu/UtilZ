using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOps.Base;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    public partial class FServiceEdit : Form
    {
        public FServiceEdit()
        {
            InitializeComponent();
        }

        private ServiceManager _serviceManager;
        private HostManager _hostManager;
        private ServiceInfo _serviceInfo;

        public FServiceEdit(ServiceManager serviceManager, HostManager hostManager, ServiceInfo serviceInfo)
            : this()
        {
            this._serviceManager = serviceManager;
            this._hostManager = hostManager;
            this._serviceInfo = serviceInfo;

            var hostTypeList = hostManager.HostTypeList;
            if (hostTypeList == null || hostTypeList.Count == 0)
            {
                throw new ArgumentException("主机类型为空,请先添加主机类型");
            }

            HostTypeItem defaultHostTypeItem = null;
            TransferProtocal defaultTransferProtocal = TransferProtocal.Udp;

            if (serviceInfo != null)
            {
                txtName.Text = serviceInfo.Name;
                txtDes.Text = serviceInfo.Des;
                defaultTransferProtocal = serviceInfo.TransferProtocal;
                defaultHostTypeItem = hostTypeList.Where(t => { return t.Id == serviceInfo.HostTypeId; }).FirstOrDefault();
                numServiceInsMaxCount.Value = serviceInfo.ServiceInsMaxCount;
            }

            DropdownBoxHelper.BindingIEnumerableGenericToComboBox<HostTypeItem>(comboBoxHostType, hostTypeList, nameof(HostTypeItem.Name), defaultHostTypeItem);
            DropdownBoxHelper.BindingEnumToComboBox<TransferProtocal>(comboBoxTransferProtocal, defaultTransferProtocal);
        }

        private void FServiceTypeEdit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    Loger.Warn("服务名称不为空");
                    return;
                }

                if (this._serviceInfo == null)
                {
                    //添加
                    var serviceInfoItem = new ServiceInfo();
                    this.GetValue(serviceInfoItem);
                    this._serviceManager.AddServiceInfo(serviceInfoItem);
                }
                else
                {
                    //修改
                    this.GetValue(this._serviceInfo);
                    this._serviceManager.ModifyServiceInfo(this._serviceInfo);
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void GetValue(ServiceInfo serviceInfo)
        {
            serviceInfo.Name = txtName.Text;
            serviceInfo.Des = txtDes.Text;
            serviceInfo.TransferProtocal = DropdownBoxHelper.GetEnumFromComboBox<TransferProtocal>(comboBoxTransferProtocal);
            serviceInfo.HostTypeId = DropdownBoxHelper.GetGenericFromComboBox<HostTypeItem>(comboBoxHostType).Id;
            serviceInfo.ServiceInsMaxCount = (int)numServiceInsMaxCount.Value;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
