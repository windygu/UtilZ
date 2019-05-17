using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.Monitor
{
    [Serializable]
    public class AppMonitorItem : SHPBaseModel, IAppMonitor
    {
        [Browsable(false)]
        public long ProcessId
        {
            get
            {
                var monitorProcess = this._monitorProcess;
                return monitorProcess != null ? monitorProcess.Id : 0;
            }
        }

        #region IAppMonitor
        private bool _isMonitor = true;
        [Browsable(false)]
        public bool IsMonitor
        {
            get { return _isMonitor; }
            set { _isMonitor = value; }
        }

        private string _appName = string.Empty;
        [DisplayName("应用名称")]
        public string AppName
        {
            get { return _appName; }
            set
            {
                _appName = value;
                base.OnRaisePropertyChanged(nameof(AppName));
            }
        }

        private string _arguments = string.Empty;
        [DisplayName("启动参数")]
        public string Arguments
        {
            get { return _arguments; }
            set
            {
                _arguments = value;
                base.OnRaisePropertyChanged(nameof(Arguments));
            }
        }

        protected string _appProcessFilePath = string.Empty;
        [DisplayName("进程文件路径")]
        public string AppProcessFilePath
        {
            get { return _appProcessFilePath; }
            set
            {
                _appProcessFilePath = value;
                base.OnRaisePropertyChanged(nameof(AppProcessFilePath));
            }
        }

        protected string _appExeFilePath = string.Empty;
        [DisplayName("启动文件路径")]
        public string AppExeFilePath
        {
            get { return _appExeFilePath; }
            set
            {
                _appExeFilePath = value;
                base.OnRaisePropertyChanged(nameof(AppExeFilePath));
            }
        }
        #endregion

        private bool _runStatus = false;
        [Browsable(false)]
        public bool RunStatus
        {
            get { return _runStatus; }
            set
            {
                _runStatus = value;
                base.OnRaisePropertyChanged(nameof(RunStatusText));
            }
        }

        private void UpdateRunStatus()
        {
            this.RunStatus = this._monitorProcess != null;
        }

        [DisplayName("运行状态")]
        public string RunStatusText
        {
            get
            {
                return this._runStatus ? "运行" : "停止";
            }
        }

        private DateTime _startTime = DateTime.Now;
        [Browsable(false)]
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                base.OnRaisePropertyChanged(nameof(StartTimeText));
            }
        }

        [DisplayName("启动时间")]
        public string StartTimeText
        {
            get { return this._startTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public AppMonitorItem()
            : base()
        {

        }

        public AppMonitorItem(string appName, string appExeFilePath, string appProcessFilePath, string arguments)
            : base()
        {
            this._appName = appName;
            this._appExeFilePath = appExeFilePath;
            this._appProcessFilePath = appProcessFilePath;
            this._arguments = arguments;
        }

        [NonSerialized]
        private Process _monitorProcess = null;
        public void Start(Process monitorProcess)
        {
            if (monitorProcess == null)
            {
                this.Start();
            }
            else
            {
                this._monitorProcess = monitorProcess;
                this.UpdateRunStatus();
                this.MonitorApp();
                var proTreeList = new List<Process>();
            }
        }

        private void GetProcessTree(List<Process> proTreeList, Process process)
        {
            try
            {
                var childProcessList = ProcessEx.GetChildProcessListById(process.Id);
                proTreeList.AddRange(childProcessList);

                foreach (var childProcess in childProcessList)
                {
                    this.GetProcessTree(proTreeList, childProcess);
                }
            }
            catch (Exception)
            { }
        }

        public void Start()
        {
            try
            {
                this._isMonitor = true;
                if (this._monitorProcess != null)
                {
                    Loger.Info($"程序[{this._appExeFilePath}]已启动,忽略");
                    return;
                }

                this._monitorProcess = ProcessEx.FindProcessByFilePath(this._appProcessFilePath).FirstOrDefault();
                if (this._monitorProcess == null || this._monitorProcess.HasExited)
                {
                    this.StartApp();
                }
                else
                {
                    this._monitorProcess = ProcessEx.GetTopProcessById(this._monitorProcess.Id);
                    if (this._monitorProcess == null)
                    {
                        ProcessEx.KillProcessTreeById(this._monitorProcess.Id);
                        this.StartApp();
                    }
                    else
                    {
                        this.MonitorApp();
                    }
                }

                Loger.Info($"启动程序[{this._appExeFilePath}]成功");
            }
            catch (Exception ex)
            {
                Loger.Error(ex, $"启动程序[{this._appExeFilePath}]异常");
            }
            finally
            {
                this.UpdateRunStatus();
            }
        }

        private void StartApp()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = this._appExeFilePath;
            startInfo.Arguments = this._arguments;
            startInfo.WorkingDirectory = Path.GetDirectoryName(this._appExeFilePath);
            startInfo.Verb = "runas";
            this._monitorProcess = Process.Start(startInfo);
            this.MonitorApp();
        }

        private void MonitorProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                Loger.Info($"程序[{this._appExeFilePath}]停止,正在启动...");
                this.UpdateRunStatus();
                this._monitorProcess = null;
                this.StartApp();
                Loger.Info($"程序[{this._appExeFilePath}]启动完成");
            }
            catch (Exception ex)
            {
                Loger.Error(ex, $"启动程序[{this._appExeFilePath}]异常");
            }

            this.UpdateRunStatus();
        }

        public void MonitorApp()
        {
            if (this._monitorProcess == null)
            {
                return;
            }

            this.StartTime = this._monitorProcess.StartTime;
            this._monitorProcess.EnableRaisingEvents = true;
            this._monitorProcess.Exited += MonitorProcess_Exited;
        }

        public void UnMonitorApp()
        {
            try
            {
                this._isMonitor = false;
                this.PrimitveUnMonitorApp();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "取消进程监视异常");
            }
        }

        private void PrimitveUnMonitorApp()
        {
            if (this._monitorProcess == null)
            {
                return;
            }

            this._monitorProcess.EnableRaisingEvents = false;
            this._monitorProcess.Exited -= MonitorProcess_Exited;
        }

        public void StopApp()
        {
            try
            {
                this._isMonitor = false;
                Loger.Info($"程序[{this._appExeFilePath}]正在停止...");
                this.PrimitveUnMonitorApp();
                if (this._monitorProcess != null && !this._monitorProcess.HasExited)
                {
                    ProcessEx.KillProcessTreeById(this._monitorProcess);
                }

                var monitorProcessList = ProcessEx.FindProcessByFilePath(this._appExeFilePath);
                foreach (var process in monitorProcessList)
                {
                    ProcessEx.KillProcessTreeById(process);
                }

                this._monitorProcess = null;
            }
            catch (Exception ex)
            {
                Loger.Error(ex, $"停止应用[{this._appExeFilePath}]发生异常");
            }
            finally
            {
                this.UpdateRunStatus();
                Loger.Info($"程序[{this._appExeFilePath}]停止完成");
            }
        }

        public void Restart()
        {
            try
            {
                this._isMonitor = true;
                this.StopApp();
                this.StartApp();
            }
            finally
            {
                this.UpdateRunStatus();
                Loger.Info($"程序[{this._appExeFilePath}]停止完成");
            }
        }

        public void Update(AppMonitorItem appMonitorItem)
        {
            AppMonitorHelper.SetValueTo(appMonitorItem, this);
            this.StartTime = appMonitorItem._startTime;
            this.RunStatus = appMonitorItem._runStatus;
        }

        public bool ServiceInsInfoIsMonitorItem(SHPAgentServiceInsInfo serviceInsInfo)
        {
            return string.Equals(this._appExeFilePath, serviceInsInfo.FilePath, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(this._appProcessFilePath, serviceInsInfo.FilePath, StringComparison.OrdinalIgnoreCase);
        }

        public void CheckProcessStatus()
        {
            try
            {
                if (!this._isMonitor)
                {
                    return;
                }

                var monitorProcess = ProcessEx.FindProcessByFilePath(this._appProcessFilePath).FirstOrDefault();
                if (monitorProcess == null || monitorProcess.HasExited)
                {
                    this.StartApp();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        public override string ToString()
        {
            return this._appName;
        }
    }
}
