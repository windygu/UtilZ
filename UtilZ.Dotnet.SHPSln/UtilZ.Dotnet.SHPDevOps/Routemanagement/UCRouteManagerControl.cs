using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPDevOps.Base;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.Routemanagement
{
    public partial class UCRouteManagerControl : UserControl
    {
        private DevOpsBLL _bll;

        public UCRouteManagerControl()
        {
            InitializeComponent();
        }

        private void UCServiceManagerControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

        }

        public void Init(DevOpsBLL bll)
        {
            this._bll = bll;
            dgvDataRoute.ShowData(this._bll.RouteManager.DataRouteInfoList.DataSource, null);
            dgvDataRoute.GridControl.ContextMenuStrip = cmsDataRoute;

            dgvServiceIns.ShowData(this._bll.RouteManager.ServiceInsList.DataSource, "UCRouteManagerControl.ServiceRouteList");
            dgvServiceIns.GridControl.ContextMenuStrip = cmsServiceIns;
        }

        #region 数据路由
        private void tsmiDataRouteAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FDataRouteEdit(this._bll.RouteManager, this._bll.ServiceManager.ServiceInfoList.DataSource, null);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDataRouteDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvDataRoute.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var dataRouteItem = (DataRouteInfo)selectedRows[0].DataBoundItem;
                this._bll.RouteManager.RemoveDataRoute(dataRouteItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDataRouteModify_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvDataRoute.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var dataRouteItem = (DataRouteInfo)selectedRows[0].DataBoundItem;
                var frm = new FDataRouteEdit(this._bll.RouteManager, this._bll.ServiceManager.ServiceInfoList.DataSource, dataRouteItem);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDataRouteClear_Click(object sender, EventArgs e)
        {
            try
            {
                this._bll.RouteManager.ClearDataRoute();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion

        #region 服务实例
        private void tsmiServiceInsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceIns.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceInsInfo = (SHPServiceInsInfo)selectedRows[0].DataBoundItem;
                this._bll.RouteManager.DeleteServiceIns(serviceInsInfo);

                //var para = new PartAsynWaitPara<Tuple<RouteManager, SHPServiceInsInfo>, object>();
                //para.Caption = "正在删除服务实例";
                //para.IsShowCancel = false;
                //para.Para = new Tuple<RouteManager, SHPServiceInsInfo>(this._bll.RouteManager, serviceInsInfo);
                //para.Function = (p) =>
                //{
                //    p.Para.Item1.DeleteServiceIns(p.Para.Item2);
                //    return null;
                //};

                //para.Completed = (ret) =>
                //{
                //    if (ret.Status == PartAsynExcuteStatus.Completed)
                //    {
                //        //this.Invoke(new Action(() =>
                //        //{
                //        //    this.DialogResult = DialogResult.OK;
                //        //}));
                //    }
                //    else if (ret.Status == PartAsynExcuteStatus.Exception)
                //    {
                //        Loger.Error(ret.Exception, "提交升级包异常");
                //    }
                //};

                //PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
