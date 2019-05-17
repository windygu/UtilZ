using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Model;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    internal class AppMonitorManagement : IAppMonitorManagement, IDisposable
    {
        private readonly object _appMonitorListLock = new object();
        private readonly BindingCollection<AppMonitorItem> _appMonitorList;
        private readonly string _appMonitorListFilePath = "AppMonitorList.db";

        private readonly ThreadEx _checkAppNoRespondingThread;
        private readonly AutoResetEvent _checkAppNoRespondingEventHandle = new AutoResetEvent(false);

        /// <summary>
        /// 监视应用列表
        /// </summary>
        public BindingCollection<AppMonitorItem> AppMonitorList
        {
            get { return _appMonitorList; }
        }

        /// <summary>
        /// 监视应用列表线程锁
        /// </summary>
        public object AppMonitorListLock
        {
            get { return _appMonitorListLock; }
        }

        public AppMonitorManagement(ICollectionOwner owner)
        {
            this._appMonitorList = new BindingCollection<AppMonitorItem>(owner);
            this._checkAppNoRespondingThread = new ThreadEx(this.CheckAppNoRespondingThreadMethod, "检测监视APP未响应线程", true);
        }

        #region werfault错误处理
        private void CheckAppNoRespondingThreadMethod(CancellationToken token)
        {
            //Werfault.exe进程名称
            const string WERFAULT_PROCESS_NAME = "werfault";
            const int interval = 500;
            AppMonitorItem[] appMonitorItems;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    this._checkAppNoRespondingEventHandle.WaitOne(interval);
                    lock (this._appMonitorListLock)
                    {
                        appMonitorItems = this._appMonitorList.ToArray();
                    }

                    if (appMonitorItems.Length == 0)
                    {
                        continue;
                    }

                    this.CheckWerfault(WERFAULT_PROCESS_NAME, appMonitorItems);
                    this.CheckProcessStatus(appMonitorItems);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        private void CheckProcessStatus(AppMonitorItem[] appMonitorItems)
        {
            try
            {
                foreach (var appMonitorItem in appMonitorItems)
                {
                    appMonitorItem.CheckProcessStatus();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void CheckWerfault(string WERFAULT_PROCESS_NAME, AppMonitorItem[] appMonitorItems)
        {
            try
            {
                Process[] werfaultProcess = Process.GetProcessesByName(WERFAULT_PROCESS_NAME);
                foreach (var werfaultProces in werfaultProcess)
                {
                    string commandLine = this.GetProcessCommandLine(werfaultProces.Id);
                    if (string.IsNullOrWhiteSpace(commandLine))
                    {
                        continue;
                    }

                    foreach (var appMonitorItem in appMonitorItems)
                    {
                        if (commandLine.Contains(string.Format(" {0} ", appMonitorItem.ProcessId)))
                        {
                            appMonitorItem.StopApp();
                            appMonitorItem.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 获取进程命令行信息
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        private string GetProcessCommandLine(int pId)
        {
            try
            {
                using (ManagementObjectSearcher search = new ManagementObjectSearcher($"select CommandLine from Win32_Process where ProcessId={pId}"))
                {
                    using (ManagementObjectCollection moc = search.Get())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            return mo["CommandLine"].ToString();
                        }
                    }
                }
            }
            catch (ManagementException)
            { }
            catch (System.Runtime.InteropServices.COMException)
            { }

            return string.Empty;
        }
        #endregion

        #region 监视项管理
        /// <summary>
        /// 添加监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        public void AddMonitorItem(AppMonitorItem appMonitorItem)
        {
            lock (this._appMonitorListLock)
            {
                if (!this.ContainsMonitorItem(appMonitorItem))
                {
                    this._appMonitorList.Add(appMonitorItem);
                }
            }

            appMonitorItem.Start();
            this.StoreAppMonitorItemsData();
        }

        /// <summary>
        /// 修改监视项
        /// </summary>
        /// <param name="oldAppMonitorItem">旧监视项</param>
        /// <param name="newAppMonitorItem">新监视项</param>
        public void ModifyMonitorItem(AppMonitorItem oldAppMonitorItem, AppMonitorItem newAppMonitorItem)
        {
            oldAppMonitorItem.StopApp();
            oldAppMonitorItem.Update(newAppMonitorItem);
            oldAppMonitorItem.Start();
            this.StoreAppMonitorItemsData();
        }

        /// <summary>
        /// 是否包含监视项[包含返回true;否则返回false]
        /// </summary>
        /// <param name="appMonitorItem">目标监视项</param>
        /// <returns>包含返回true;否则返回false</returns>
        public bool ContainsMonitorItem(IAppMonitor appMonitorItem)
        {
            lock (this._appMonitorListLock)
            {
                return this._appMonitorList.Where(t => { return AppMonitorHelper.Equals(t, appMonitorItem); }).Count() > 0;
            }
        }

        /// <summary>
        /// 移除监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        public void RemoveMonitorItem(AppMonitorItem appMonitorItem)
        {
            appMonitorItem.UnMonitorApp();
            lock (this._appMonitorListLock)
            {
                this._appMonitorList.Remove(appMonitorItem);
            }

            this.StoreAppMonitorItemsData();
        }

        private readonly object _storeAppMonitorItemsDataLock = new object();
        private void StoreAppMonitorItemsData()
        {
            try
            {
                lock (this._storeAppMonitorItemsDataLock)
                {
                    SerializeEx.XmlSerializer(this._appMonitorList, this._appMonitorListFilePath);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "存储监视应用列表数据异常");
            }
        }

        private void ReadAppMonitorItemsData()
        {
            if (File.Exists(this._appMonitorListFilePath))
            {
                try
                {
                    var appMonitorList = SerializeEx.XmlDeserializerFromFile<List<AppMonitorItem>>(this._appMonitorListFilePath);
                    this._appMonitorList.AddRange(appMonitorList);

                    var pros = Process.GetProcesses();

                    //[key:进程文件路径;value:进程列表]
                    Dictionary<string, List<Process>> processFilePathProcessDic = this.BuildProcessFilePathProcessDic(pros);

                    Process monitorProcess = null;
                    string processFilePath = null;
                    foreach (var appMonitorItem in appMonitorList)
                    {
                        if (!appMonitorItem.IsMonitor)
                        {
                            continue;
                        }

                        monitorProcess = null;
                        processFilePath = appMonitorItem.AppProcessFilePath;
                        foreach (var kv in processFilePathProcessDic)
                        {
                            if (string.Equals(processFilePath, kv.Key, StringComparison.OrdinalIgnoreCase))
                            {
                                var mainProcess = kv.Value.FirstOrDefault();
                                monitorProcess = ProcessEx.GetTopProcessById(mainProcess.Id);
                                break;
                            }
                        }

                        appMonitorItem.Start(monitorProcess);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        private Dictionary<string, List<Process>> BuildProcessFilePathProcessDic(Process[] pros)
        {
            //[key:进程文件路径;value:进程]
            var processFilePathProcessDic = new Dictionary<string, List<Process>>();
            List<Process> processFilePathProcessList;
            foreach (var pro in pros)
            {
                try
                {
                    if (processFilePathProcessDic.ContainsKey(pro.MainModule.FileName))
                    {
                        processFilePathProcessList = processFilePathProcessDic[pro.MainModule.FileName];
                    }
                    else
                    {
                        processFilePathProcessList = new List<Process>();
                        processFilePathProcessDic.Add(pro.MainModule.FileName, processFilePathProcessList);
                    }

                    processFilePathProcessList.Add(pro);
                }
                catch
                { }
            }

            return processFilePathProcessDic;
        }

        /// <summary>
        /// 启动监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        public void StartMonitorItem(AppMonitorItem monitorItem)
        {
            monitorItem.Start();
            this.StoreAppMonitorItemsData();
        }

        /// <summary>
        /// 停止监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        public void StopMonitorItem(AppMonitorItem monitorItem)
        {
            monitorItem.StopApp();
            this.StoreAppMonitorItemsData();
        }

        /// <summary>
        /// 重启监视项
        /// </summary>
        /// <param name="monitorItem">监视项</param>
        public void RestartMonitorItem(AppMonitorItem monitorItem)
        {
            monitorItem.Restart();
        }
        #endregion

        internal void Start()
        {
            this.ReadAppMonitorItemsData();
            this._checkAppNoRespondingThread.Start();
        }

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.StoreAppMonitorItemsData();
                this._checkAppNoRespondingThread.Stop();
                this._checkAppNoRespondingEventHandle.Set();
                this._checkAppNoRespondingThread.Dispose();
                this._checkAppNoRespondingEventHandle.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
