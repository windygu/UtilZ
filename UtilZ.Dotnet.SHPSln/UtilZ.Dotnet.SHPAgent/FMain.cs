using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPAgentBLL;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.SHPAgent
{
    public partial class FMain : Form, ICollectionOwner
    {
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Process _pro;
        private TimeSpan _lastCpuTime = TimeSpan.FromMilliseconds(0);
        private DateTime _lastCalTime;
        private readonly long _totalMemorySize;
        private readonly DateTime _defaulttime;

        private readonly AgentBLL _bll;

        public FMain()
        {
            InitializeComponent();
        }

        internal FMain(string protectAppPath, int agentProtectProcessId)
            : this()
        {
            this.dgvAppMonitor.GridControl.ContextMenuStrip = this.cmsAppMonitor;

            this._pro = Process.GetCurrentProcess();
            tsslStartTime.Text = this._pro.StartTime.ToString();
            this._defaulttime = this._pro.StartTime;
            this._totalMemorySize = (long)(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory);
            this._timer = new System.Windows.Forms.Timer();
            this._timer.Interval = 1000;
            this._timer.Tick += _timer_Tick;

            this._bll = new AgentBLL(protectAppPath, agentProtectProcessId, this);

            bool startShowStatus;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings[AppConfigKeys.START_SHOW_STATUS], out startShowStatus);
            if (!startShowStatus)
            {
                this.HidenAgent();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                this._pro.Refresh();
                TimeSpan tsRun = DateTime.Now - this._pro.StartTime;
                tsslRuntime.Text = string.Format("{0}天{1}小时{2}分{3}秒", tsRun.Days, tsRun.Hours, tsRun.Minutes, tsRun.Seconds);
                long workingSet64 = this._pro.WorkingSet64;
                tsslMemory.Text = string.Format("{0}MB({1}%)", workingSet64 / 1024 / 1024, ((float)workingSet64 * 100 / this._totalMemorySize).ToString("F2"));
                tsslCPUUse.Text = string.Format("{0}%", this.CaculateCpuUsing(this._pro.TotalProcessorTime, this._timer.Interval));
                tsslThreadCount.Text = this._pro.Threads.Count.ToString();
                tsslHandleCount.Text = this._pro.HandleCount.ToString();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private string CaculateCpuUsing(TimeSpan curCpuTime, int reportInterval)
        {
            double interval;
            var time = DateTime.Now;
            if (this._lastCalTime == this._defaulttime)
            {
                interval = reportInterval;
            }
            else
            {
                interval = (time - this._lastCalTime).TotalMilliseconds;
            }

            var value = (curCpuTime - this._lastCpuTime).TotalMilliseconds * 100 / (interval * Environment.ProcessorCount);
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 100)
            {
                value = 100;
            }

            this._lastCpuTime = curCpuTime;
            this._lastCalTime = time;
            return value.ToString("F0");
        }



        private void FMain_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                RedirectOuputCenter.Add(new RedirectOutputChannel(this.LogOutputToUI, System.Configuration.ConfigurationManager.AppSettings[AppConfigKeys.REDIRECT_TO_UI_APPENDER_NAME]));
                checkBoxLockLog.Checked = logControl.IsLock;
                this._timer.Start();
                dgvAppMonitor.ShowData(this._bll.AppMonitorItemManager.AppMonitorList.DataSource, null);
                this._bll.Start();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "启动Agent异常");
                this.tsmiNotifyIconExit_Click(sender, e);
            }
        }

        /// <summary>
        /// 日志输出到UI
        /// </summary>
        /// <param name="e"></param>
        private void LogOutputToUI(RedirectOuputItem e)
        {
            try
            {
                if (e == null || e.Item == null)
                {
                    return;
                }

                string logInfo = string.Format("{0} {1} {2}", e.Item.Time.ToString("yyyy-MM-dd HH:mm:ss"), LogConstant.GetLogLevelName(e.Item.Level), e.Item.Content);
                logControl.AddLog(logInfo, e.Item.Level);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        #region 显示-隐藏-退出
        private bool _isHiden = true;
        private void ShowAgent()
        {
            this.Opacity = 100;
            this.ShowInTaskbar = true;
            this._timer.Start();
        }

        private void HidenAgent()
        {
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this._timer.Stop();
        }
        private void tsmiNotifyIconOpen_Click(object sender, EventArgs e)
        {
            this.ShowAgent();
        }

        private void tsmiNotifyIconExit_Click(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = false;
            this._isHiden = false;
            this.Close();
        }

        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._isHiden)
            {
                this.HidenAgent();
                e.Cancel = true;
            }
            else
            {
                //关闭
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowAgent();
            this.Activate();
        }
        #endregion

        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._bll.Dispose();
        }

        #region 监视项菜单
        private void tsmiAppMonitorAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FAppMonitorItemEdit();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this._bll.AppMonitorItemManager.AddMonitorItem(frm.GetAppMonitorItem());
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAppMonitorDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvAppMonitor.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var monitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                this._bll.AssertRemoveMonitor(monitorItem);
                this._bll.AppMonitorItemManager.RemoveMonitorItem(monitorItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAppMonitorModify_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvAppMonitor.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var monitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                var frm = new FAppMonitorItemEdit(monitorItem);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this._bll.AppMonitorItemManager.ModifyMonitorItem(monitorItem, frm.GetAppMonitorItem());
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAppMonitorStart_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvAppMonitor.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var monitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                this._bll.AppMonitorItemManager.StartMonitorItem(monitorItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAppMonitorStop_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvAppMonitor.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var monitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                this._bll.AppMonitorItemManager.StopMonitorItem(monitorItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAppMonitorRestart_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvAppMonitor.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var monitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                this._bll.AppMonitorItemManager.RestartMonitorItem(monitorItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAppMonitorFileLocation_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvAppMonitor.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var monitorItem = (AppMonitorItem)selectedRows[0].DataBoundItem;
                Ex.Base.DirectoryInfoEx.OpenFileDirectory(monitorItem.AppExeFilePath);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion

        private void checkBoxLockLog_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                logControl.IsLock = checkBoxLockLog.Checked;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                logControl.Clear();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
