using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// 进程扩展方法
    /// </summary>
    public class ProcessEx
    {
        /// <summary>
        /// 进程互斥对象
        /// </summary>
        private static Mutex _processMutex = null;

        /// <summary>
        /// 多线程锁
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 单进程检测[创建进程互斥对象成功返回true;否则返回false]
        /// </summary>
        /// <param name="rangeFlag">单进程检测范围[true:所有用户;false:仅当前用户]</param>
        /// <param name="mutexName">互斥变量名称</param>
        /// <returns>创建进程互斥对象成功返回true;否则返回false</returns>
        public static bool SingleProcessCheck(bool rangeFlag, string mutexName = null)
        {
            lock (_lock)
            {
                if (_processMutex != null)
                {
                    return true;
                }

                if (string.IsNullOrWhiteSpace(mutexName))
                {
                    mutexName = Assembly.GetEntryAssembly().GetCustomAttributes(true).Where(t => t is GuidAttribute).Cast<GuidAttribute>().Select(t => t.Value).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(mutexName))
                    {
                        mutexName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
                    }
                }

                if (rangeFlag)
                {
                    mutexName = @"Global\" + mutexName;
                }

                bool createNew;
                try
                {
                    _processMutex = new Mutex(true, mutexName, out createNew);
                }
                catch
                {
                    createNew = false;
                }

                return createNew;
            }
        }

        /// <summary>
        /// 同步执行程序并返回该程序的执行输出结果
        /// </summary>
        /// <param name="appPath">应用程序路径</param>
        /// <param name="args">启动参数</param>
        /// <param name="millisecondsTimeout">执行超时时长-1无限时长</param>
        /// <returns>执行输出结果</returns>
        public static string SynExcuteCmd(string appPath, string args, int millisecondsTimeout = Timeout.Infinite)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            try
            {
                Process pro = new Process();
                pro.StartInfo = new ProcessStartInfo();
                pro.StartInfo.FileName = appPath;
                pro.StartInfo.Arguments = args;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.UseShellExecute = false;
                pro.EnableRaisingEvents = true;
                pro.Exited += (s, e) =>
                {
                    autoResetEvent.Set();
                };

                pro.Start();
                if (autoResetEvent.WaitOne(millisecondsTimeout))
                {
                    return pro.StandardOutput.ReadToEnd();
                }
                else
                {
                    throw new TimeoutException();
                }
            }
            finally
            {
                autoResetEvent.Dispose();
            }
        }

        /// <summary>
        /// 计算CPU使用率
        /// </summary>
        /// <param name="curCpuTime"></param>
        /// <param name="interval"></param>
        /// <param name="lastCalTime"></param>
        /// <returns></returns>
        public static float CaculateCpuUsing(TimeSpan curCpuTime, int interval, ref DateTime lastCalTime)
        {
            //double innerInterval;
            //var time = DateTime.Now;
            //var ts = time - lastCalTime;
            //if (ts.TotalMilliseconds > interval)
            //{
            //    innerInterval = ts.TotalMilliseconds;
            //}
            //else
            //{
            //    innerInterval = interval;
            //}

            //var value=(curCpuTime-lastCalTime).
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证进程ID是否有效[有效返回true;无效返回false]
        /// </summary>
        /// <param name="id">进程ID</param>
        /// <returns>有效返回true;无效返回false</returns>
        public static bool ValidaProcessId(int id)
        {
            return id > 0;
        }

        /// <summary>
        /// 根据程序路径查找进程
        /// </summary>
        /// <param name="appExeFilePath">根据程序路</param>
        /// <returns>进程列表</returns>
        public static List<Process> FindProcessByFilePath(string appExeFilePath)
        {
            var resultPros = new List<Process>();
            Process[] pros = Process.GetProcesses();
            foreach (var pro in pros)
            {
                try
                {
                    if (string.Equals(appExeFilePath, pro.MainModule.FileName, StringComparison.OrdinalIgnoreCase))
                    {
                        resultPros.Add(pro);
                    }
                }
                catch
                { }
            }

            return resultPros;
        }

        /// <summary>
        /// 根据进程ID获取该进程的子进程列表
        /// </summary>
        /// <param name="id">进程ID</param>
        /// <returns>子进程列表</returns>
        public static List<Process> GetChildProcessListById(int id)
        {
            if (!ValidaProcessId(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var childProcessList = new List<Process>();
            int repeatCount = 0, maxRepeatCount = 3;
            Exception innerException = null;

            while (repeatCount < maxRepeatCount)
            {
                try
                {
                    repeatCount++;
                    childProcessList.Clear();
                    using (ManagementObjectSearcher search = new ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessId={id}"))
                    {
                        using (ManagementObjectCollection moc = search.Get())
                        {
                            foreach (ManagementObject mo in moc)
                            {
                                childProcessList.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
                            }
                        }
                    }

                    return childProcessList;
                }
                catch (ManagementException me)
                {
                    innerException = me;
                }
                catch (System.Runtime.InteropServices.COMException come)
                {
                    innerException = come;
                }
            }

            throw new Exception("获取该进程的子进程列表异常", innerException);
        }

        /// <summary>
        /// 根据进程ID获取父进程
        /// </summary>
        /// <param name="id">进程ID</param>
        /// <returns>父进程</returns>
        public static Process GetParentProcessById(int id)
        {
            if (!ValidaProcessId(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            //try
            //{
            var processName = Process.GetProcessById(id).ProcessName;
            var processByName = Process.GetProcessesByName(processName);
            string processIndexName = null;
            for (var i = 0; i < processByName.Length; i++)
            {
                processIndexName = i == 0 ? processName : processName + "#" + i;
                var processIdPc = new PerformanceCounter("Process", "ID Process", processIndexName);
                if ((int)processIdPc.NextValue() == id)
                {
                    break;
                }
            }

            var parentIdPc = new PerformanceCounter("Process", "Creating Process ID", processIndexName);
            var parentId = (int)parentIdPc.NextValue();
            var pro = Process.GetProcessById(parentId);
            return pro;
            //}
            //catch (ArgumentException ae)
            //{
            //    if (ae.HResult == -2147024809)
            //    {
            //        return null;
            //    }

            //    throw ae;
            //}
        }

        /// <summary>
        /// 获取指定进程ID的顶级进程(explorer的下一级)
        /// </summary>
        /// <param name="id">进程ID</param>
        /// <returns>顶级进程</returns>
        public static Process GetTopProcessById(int id)
        {
            if (!ValidaProcessId(id))
            {
                return null;
            }

            string explorerFileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");
            Process topProcess = null;
            try
            {
                Process lastProcess = Process.GetProcessById(id);
                while (!string.Equals(lastProcess.MainModule.FileName, explorerFileFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    topProcess = lastProcess;
                    lastProcess = GetParentProcessById(lastProcess.Id);
                }
            }
            catch
            { }

            return topProcess;
        }

        /// <summary>
        /// 根据进程ID杀死该进程以及所有子孙进程和父进程
        /// </summary>
        /// <param name="id">进程ID</param>
        public static void KillProcessTreeById(int id)
        {
            try
            {
                if (!ValidaProcessId(id))
                {
                    return;
                }

                Process process = GetTopProcessById(id);
                KillProcessTreeById(process);
            }
            catch
            { }
        }

        /// <summary>
        /// 根据进程ID杀死该进程以及所有子孙进程
        /// </summary>
        /// <param name="process">进程</param>
        public static void KillProcessTreeById(Process process)
        {
            Process hostProcess = Process.GetCurrentProcess();
            PrimitiveKillProcessTreeById(process, hostProcess.Id);
        }

        /// <summary>
        /// 根据进程ID杀死该进程以及所有子孙进程
        /// </summary>
        /// <param name="process">进程</param>
        /// <param name="hostProcessId">宿主进程ID(当前主进程ID)</param>
        private static void PrimitiveKillProcessTreeById(Process process, int hostProcessId)
        {
            try
            {
                if (process == null)
                {
                    return;
                }

                PrimitiveKill(process, hostProcessId);

                List<Process> childProcessList = GetChildProcessListById(process.Id);
                foreach (var childProcess in childProcessList)
                {
                    PrimitiveKill(childProcess, hostProcessId);
                    KillProcessTreeById(childProcess);
                }
            }
            catch
            { }
        }

        private static void PrimitiveKill(Process process, int hostProcessId)
        {
            try
            {
                process.Refresh();
                if (process == null || process.HasExited)
                {
                    return;
                }

                if (process.Id == hostProcessId)
                {
                    return;
                }

                process.Kill();
            }
            catch
            { }
        }

        /// <summary>
        /// 通过指定应用程序的名称和一组命令行参数来启动一个进程资源，并将该资源与新的 System.Diagnostics.Process 组件相关联
        /// </summary>
        /// <param name="fileName">要在该进程中运行的应用程序文件的名称</param>
        /// <param name="arguments">启动该进程时传递的命令行实参</param>
        /// <returns>与该进程关联的新的 System.Diagnostics.Process 组件；如果没有启动进程资源（例如，如果重用了现有进程），则为 null</returns>
        public static Process Start(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;
            return Process.Start(startInfo);
        }
    }
}
