using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    public partial class UCServiceManagerControl : UserControl, ICollectionOwner
    {
        private ServiceManager _serviceManager;
        private HostManager _hostManager;
        private RouteManager _routeManager;
        private readonly BindingCollection<ServiceMirrorInfo> _serviceMirrosInfoList;

        public UCServiceManagerControl()
        {
            InitializeComponent();

            this._serviceMirrosInfoList = new BindingCollection<ServiceMirrorInfo>(this);
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
            this._serviceManager = bll.ServiceManager;
            this._hostManager = bll.HostManager;
            this._routeManager = bll.RouteManager;
            dgvServiceInfo.ShowData(this._serviceManager.ServiceInfoList.DataSource, null);
            dgvServiceInfo.GridControl.ContextMenuStrip = cmsServiceInfo;

            dgvServiceMirror.ShowData(this._serviceMirrosInfoList.DataSource, "UtilZ.Dotnet.SHPDevOps.ServiceManagement.UCServiceManagerControl.dgvServiceMirror");
            dgvServiceMirror.GridControl.ContextMenuStrip = cmsServiceMirror;
        }

        #region 服务
        private void tsmiServiceTypeAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FServiceEdit(this._serviceManager, this._hostManager, null);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiServiceTypeDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceInfo.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceInfoItem = (ServiceInfo)selectedRows[0].DataBoundItem;
                this._serviceManager.RemoveServiceInfo(serviceInfoItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiServiceTypeModify_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceInfo.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceInfoItem = (ServiceInfo)selectedRows[0].DataBoundItem;
                var frm = new FServiceEdit(this._serviceManager, this._hostManager, serviceInfoItem);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiServiceTypeClear_Click(object sender, EventArgs e)
        {
            try
            {
                this._serviceManager.ClearServiceInfo();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiServiceInfoDeploy_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceInfo.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceInfo = (ServiceInfo)selectedRows[0].DataBoundItem;

                var frm = new FServiceInsCountEdit();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }


                var para = new PartAsynWaitPara<Tuple<RouteManager, ServiceInfo, int>, object>();
                para.Caption = $"服务[{serviceInfo.Name}]部署中...";
                para.IsShowCancel = false;
                para.Para = new Tuple<RouteManager, ServiceInfo, int>(this._routeManager, serviceInfo, frm.ServiceInsCount);
                para.Function = (p) =>
                {
                    p.Para.Item1.DeployServiceIns(p.Para.Item2, p.Para.Item3, p.AsynWait);
                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Completed)
                    {
                        Loger.Info($"服务[{ret.Para.Item2.Name}]部署[{ret.Para.Item3}]个服务实例完成");
                    }
                    else if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, $"部署服务[{ret.Para.Item2.Name}]发生异常");
                    }
                    else
                    {
                        Loger.Warn($"部署服务[{ret.Para.Item2.Name}]操作取消");
                    }
                };

                PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiServiceMirrorUpload_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var selectedRows = this.dgvServiceInfo.SelectedRows;
                    if (selectedRows == null || selectedRows.Length == 0)
                    {
                        return;
                    }

                    var serviceInfoItem = (ServiceInfo)selectedRows[0].DataBoundItem;
                    var frm = new FAddServiceMirror(this._serviceManager, serviceInfoItem);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        var serviceMirrorInfo = frm.ServiceMirrorInfo;
                        this._serviceMirrosInfoList.Add(serviceMirrorInfo);

                        //如果上传的镜像是第一个,则默认使用该镜像
                        if (this._serviceMirrosInfoList.Count == 1)
                        {
                            this._serviceManager.UsageServiceMirror(serviceInfoItem, serviceMirrorInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion

        private void dgvServiceInfo_DataRowSelectionChanged(object sender, DataRowSelectionChangedArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceInfo.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceInfoItem = (ServiceInfo)selectedRows[0].DataBoundItem;
                this._serviceMirrosInfoList.Clear();
                rtxtVersionDes.Text = string.Empty;
                var serviceMirrorInfoArr = this._serviceManager.GetServiceInfoMirror(serviceInfoItem.Id);
                this._serviceMirrosInfoList.AddRange(serviceMirrorInfoArr);
                this.dgvServiceMirror.Tag = serviceInfoItem;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void dgvServiceVersion_DataRowSelectionChanged(object sender, DataRowSelectionChangedArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceMirror.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceMirrorInfo = (ServiceMirrorInfo)selectedRows[0].DataBoundItem;
                rtxtVersionDes.Text = serviceMirrorInfo.Des;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        #region 服务镜像
        private void tsmiServiceMirrorUsage_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceMirror.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceMirrorInfo = (ServiceMirrorInfo)selectedRows[0].DataBoundItem;
                var serviceInfoItem = (ServiceInfo)this.dgvServiceMirror.Tag;
                this._serviceManager.UsageServiceMirror(serviceInfoItem, serviceMirrorInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiServiceMirrorDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvServiceMirror.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var serviceMirrorInfo = (ServiceMirrorInfo)selectedRows[0].DataBoundItem;
                var serviceInfoItem = (ServiceInfo)this.dgvServiceMirror.Tag;
                if (serviceInfoItem.ServiceMirrorId == serviceMirrorInfo.Id)
                {
                    Loger.Warn($"要删除版本的镜像正在使用中,拒绝删除");
                    return;
                }

                this._serviceManager.DeleteServiceMirrorById(serviceMirrorInfo.Id);
                this._serviceMirrosInfoList.Remove(serviceMirrorInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
