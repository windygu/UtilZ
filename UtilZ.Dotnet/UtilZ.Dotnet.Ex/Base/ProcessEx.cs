using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        /// <returns>执行输出结果</returns>
        public static string SynExcuteCmd(string appPath, string args)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
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
            autoResetEvent.WaitOne();
            return pro.StandardOutput.ReadToEnd();
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
    }
}
