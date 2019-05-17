using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Commands.AppControl;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class UCHostManagerControl
    {
        #region 主机监视应用右键菜单
        private string GetOperateCaption(AppControlType controlType, int stepIndex)
        {
            string caption;
            switch (controlType)
            {
                case AppControlType.Start:
                    caption = "启动";
                    break;
                case AppControlType.Stop:
                    caption = "停止";
                    break;
                case AppControlType.Restart:
                    caption = "重启";
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (stepIndex)
            {
                case 1:
                    caption = caption + "应用";
                    break;
                case 2:
                    caption = caption + "应用成功";
                    break;
                case 3:
                    caption = caption + "应用异常";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return caption;
        }

        private void ControlMonitorApp(AppControlType controlType)
        {
            try
            {
                DataGridViewRow[] selectedRows = dgvHostServiceList.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var hostInfo = this._currentSelectedhostInfo;
                if (hostInfo == null)
                {
                    return;
                }

                var appMonitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                var para = new PartAsynWaitPara<Tuple<HostManager, AppControlType, AppMonitorItem, HostInfo>, object>();
                para.Caption = this.GetOperateCaption(controlType, 1);
                para.IsShowCancel = false;
                para.Para = new Tuple<HostManager, AppControlType, AppMonitorItem, HostInfo>(this._hostManager, controlType, appMonitorItem, hostInfo);
                para.Function = (p) =>
                {
                    p.Para.Item1.ControlApp(p.Para.Item2, p.Para.Item3, p.Para.Item4);
                    Loger.Info(this.GetOperateCaption(controlType, 2));
                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, this.GetOperateCaption(controlType, 3));
                    }
                };

                PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiStartApp_Click(object sender, EventArgs e)
        {
            this.ControlMonitorApp(AppControlType.Start);
        }

        private void tsmiStopApp_Click(object sender, EventArgs e)
        {
            this.ControlMonitorApp(AppControlType.Stop);
        }

        private void tsmiRestartApp_Click(object sender, EventArgs e)
        {
            this.ControlMonitorApp(AppControlType.Restart);
        }

        private void tsmiRomoveMonitorApp_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow[] selectedRows = dgvHostServiceList.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var appMonitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                if (appMonitorItem == null)
                {
                    return;
                }

                var hostInfo = this._currentSelectedhostInfo;
                if (hostInfo == null)
                {
                    return;
                }

                if (this._routeManager.MonitorAppIsSerivice(hostInfo, appMonitorItem))
                {
                    Loger.Warn($"监视项[{appMonitorItem.AppName}]为服务,不允许从监视中移除操作");
                    return;
                }

                var para = new PartAsynWaitPara<Tuple<HostManager, HostInfo, AppMonitorItem>, object>();
                para.Caption = "正在从监视列表移除";
                para.IsShowCancel = false;
                para.Para = new Tuple<HostManager, HostInfo, AppMonitorItem>(this._hostManager, hostInfo, appMonitorItem);
                para.Function = (p) =>
                {
                    p.Para.Item1.RemoveMonitor(p.Para.Item2, p.Para.Item3);
                    Loger.Info("从监视列表移除成功");
                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, "从监视列表移除异常");
                    }
                };

                PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
