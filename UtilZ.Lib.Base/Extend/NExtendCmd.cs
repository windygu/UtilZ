using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// doc命令调用辅助类
    /// </summary>
    public static class NExtendCmd
    {
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
    }
}
