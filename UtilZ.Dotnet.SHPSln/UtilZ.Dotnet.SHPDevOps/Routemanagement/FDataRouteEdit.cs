using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace UtilZ.Dotnet.SHPDevOps.Routemanagement
{
    public partial class FDataRouteEdit : Form
    {
        private RouteManager _routeManager;
        private DataRouteInfo _dataRouteItem;

        public FDataRouteEdit()
        {
            InitializeComponent();
        }

        public FDataRouteEdit(RouteManager routeManager, IEnumerable<ServiceInfo> serviceInfoItemList, DataRouteInfo dataRouteItem)
            : this()
        {
            if (serviceInfoItemList == null || serviceInfoItemList.Count() < 1)
            {
                throw new InvalidOperationException("服务列表为空,请先添加服务...");
            }

            this._routeManager = routeManager;
            this._dataRouteItem = dataRouteItem;
            ServiceInfo defaultServiceInfoItem = null;
            if (dataRouteItem != null)
            {
                numDtaCode.Value = dataRouteItem.DataCode;
                txtName.Text = dataRouteItem.Name;
                txtDes.Text = dataRouteItem.Des;
                defaultServiceInfoItem = serviceInfoItemList.Where(t => { return t.Id == dataRouteItem.ServiceInfoId; }).FirstOrDefault();
            }
            else
            {
                defaultServiceInfoItem = serviceInfoItemList.ElementAt(0);
            }

            DropdownBoxHelper.BindingIEnumerableGenericToComboBox<ServiceInfo>(comboBoxDstService, serviceInfoItemList, nameof(ServiceInfo.Name), defaultServiceInfoItem);
        }

        private void FDataRouteEdit_Load(object sender, EventArgs e)
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
                if (this._dataRouteItem == null)
                {
                    var dataRouteItem = new DataRouteInfo();
                    this.GetValue(dataRouteItem);
                    this._routeManager.AddDataRoute(dataRouteItem);
                }
                else
                {
                    this.GetValue(this._dataRouteItem);
                    this._routeManager.ModifyDataRoute(this._dataRouteItem);
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void GetValue(DataRouteInfo dataRouteItem)
        {
            dataRouteItem.DataCode = (int)numDtaCode.Value;
            dataRouteItem.Name = txtName.Text;
            dataRouteItem.Des = txtDes.Text;
            dataRouteItem.ServiceInfoId = DropdownBoxHelper.GetGenericFromComboBox<ServiceInfo>(comboBoxDstService).Id;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
