using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class UCHostManagerControl
    {
        #region 进程右键菜单
        private void tsmiProcessListKill_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow[] selectedRows = dgvProcessList.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var hostProcessInfoItem = selectedRows[0].DataBoundItem as HostProcessInfoItem;
                if (hostProcessInfoItem == null)
                {
                    return;
                }

                var hostInfo = this._currentSelectedhostInfo;
                if (hostInfo == null)
                {
                    return;
                }

                var para = new PartAsynWaitPara<Tuple<HostManager, HostInfo, int>, object>();
                para.Caption = "正在结束进程";
                para.IsShowCancel = false;
                para.Para = new Tuple<HostManager, HostInfo, int>(this._hostManager, hostInfo, hostProcessInfoItem.Id);
                para.Function = (p) =>
                {
                    p.Para.Item1.KillProcess(p.Para.Item2, p.Para.Item3);
                    Loger.Info("结束进程成功");
                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, "结束进程异常");
                    }
                };

                PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiProcessListKillTree_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow[] selectedRows = dgvProcessList.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var hostProcessInfoItem = selectedRows[0].DataBoundItem as HostProcessInfoItem;
                if (hostProcessInfoItem == null)
                {
                    return;
                }

                var hostInfo = this._currentSelectedhostInfo;
                if (hostInfo == null)
                {
                    return;
                }

                var para = new PartAsynWaitPara<Tuple<HostManager, HostInfo, int>, object>();
                para.Caption = "正在结束进程树";
                para.IsShowCancel = false;
                para.Para = new Tuple<HostManager, HostInfo, int>(this._hostManager, hostInfo, hostProcessInfoItem.Id);
                para.Function = (p) =>
                {
                    p.Para.Item1.KillTreeProcess(p.Para.Item2, p.Para.Item3);
                    Loger.Info("结束进程树成功");
                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, "结束进程树异常");
                    }
                };

                PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiProcessListAddMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow[] selectedRows = dgvProcessList.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var hostProcessInfoItem = selectedRows[0].DataBoundItem as HostProcessInfoItem;
                if (hostProcessInfoItem == null)
                {
                    return;
                }

                var hostInfo = this._currentSelectedhostInfo;
                if (hostInfo == null)
                {
                    return;
                }

                var para = new PartAsynWaitPara<Tuple<HostManager, HostInfo, int>, object>();
                para.Caption = "正在添加到监视";
                para.IsShowCancel = false;
                para.Para = new Tuple<HostManager, HostInfo, int>(this._hostManager, hostInfo, hostProcessInfoItem.Id);
                para.Function = (p) =>
                {
                    p.Para.Item1.AddMonitor(p.Para.Item2, p.Para.Item3);
                    Loger.Info("添加到监视成功");
                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, "添加到监视异常");
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
