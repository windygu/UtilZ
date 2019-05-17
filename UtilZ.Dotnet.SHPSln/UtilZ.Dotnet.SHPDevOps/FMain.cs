using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps
{
    public partial class FMain : Form, ICollectionOwner
    {
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Process _pro;
        private TimeSpan _lastCpuTime = TimeSpan.FromMilliseconds(0);
        private DateTime _lastCalTime;
        private readonly long _totalMemorySize;
        private readonly DateTime _defaulttime;
        private readonly DevOpsBLL _bll;

        public FMain()
        {
            InitializeComponent();

            this._pro = Process.GetCurrentProcess();
            tsslStartTime.Text = this._pro.StartTime.ToString();
            this._defaulttime = this._pro.StartTime;
            this._totalMemorySize = (long)(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory);
            this._timer = new System.Windows.Forms.Timer();
            this._timer.Interval = 1000;
            this._timer.Tick += _timer_Tick;

            this._bll = new DevOpsBLL(this);
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
                RedirectOuputCenter.Add(new RedirectOutputChannel(this.LogOutputToUI, null));
                checkBoxLockLog.Checked = logControl.IsLock;
                this._timer.Start();
                this._bll.Start();
                this.InitDevOpsControl();
                this.ucHostManager.Init(this._bll);
                this.ucHostManager.RefreshRootGroupAndNoneHroupHost();
                this.ucServiceManagerControl.Init(this._bll);
                this.ucRouteManagerControl.Init(this._bll);
                this.ucSettingControl.Init(this._bll);
                Loger.Info("启动完成");

                //var para = new PartAsynWaitPara<DevOpsBLL, object>();
                //para.Caption = "正在启动";
                //para.IsShowCancel = false;
                //para.Para = this._bll;
                //para.Function = (p) =>
                //{
                //    //p.Para.Start();
                //    this.ucHostManager.Init(this._bll, p.AsynWait);
                //    this.ucHostManager.RefreshRootGroupAndNoneHroupHost(p.AsynWait);
                //    Loger.Info("启动完成");
                //    return null;
                //};

                //para.Completed = (ret) =>
                //{
                //    if (ret.Status == PartAsynExcuteStatus.Exception)
                //    {
                //        Loger.Error(ret.Exception, "启动异常");
                //    }
                //};

                //PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Fatal(ex, "启动异常");
            }
        }

        private void InitDevOpsControl()
        {
            Dictionary<int, PluginInfo<ISHPDDevOps>>.ValueCollection dDevOpsPlugins = this._bll.GetDDevOpsPlugins();
            foreach (var dDevOpsPlugin in dDevOpsPlugins)
            {
                try
                {
                    var control = dDevOpsPlugin.Plugin.GetDevOpsControl();
                    if (control == null)
                    {
                        continue;
                    }

                    control.Dock = DockStyle.Fill;
                    var tp = new TabPage();
                    tp.Controls.Add(control);
                    tp.Text = dDevOpsPlugin.PluginAttribute.Name;
                    tp.Name = $"{nameof(tabControlExtend)}_tp_{tp.Text}";
                    tabControlExtend.TabPages.Add(tp);
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, $"加载{dDevOpsPlugin.GetType().FullName}控件发生异常");
                }
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

        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._bll.Dispose();
        }

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

        #region 显示-隐藏-退出
        private bool _isHiden = true;
        private void ShowDevOps()
        {
            this.Opacity = 100;
            this.ShowInTaskbar = true;
        }

        private void HidenDevOps()
        {
            this.Opacity = 0;
            this.ShowInTaskbar = false;
        }

        private void tsmiNotifyIconOpen_Click(object sender, EventArgs e)
        {
            this.ShowDevOps();
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
                this.HidenDevOps();
                e.Cancel = true;
            }
            else
            {
                //关闭
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowDevOps();
            this.Activate();
        }
        #endregion
    }
}
