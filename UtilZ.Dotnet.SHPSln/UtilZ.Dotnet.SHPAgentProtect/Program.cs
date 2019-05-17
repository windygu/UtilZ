using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPAgentProtect
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                if (!ProcessEx.SingleProcessCheck(true))
                {
                    Loger.Fatal("创建单进程互斥对象失败");
                    return;
                }

                Process agentProcess = ParseArgs(args);
                if (agentProcess == null)
                {
                    Loger.Fatal("无效的启动参数");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FProtect(agentProcess));
            }
            catch (Exception ex)
            {
                Loger.Fatal(ex, "启动异常");
            }
        }

        private static Process ParseArgs(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Loger.Fatal("参数为空或无效");
                return null;
            }

            int processId;
            if (!int.TryParse(args[0], out processId))
            {
                //参数值无效
                Loger.Fatal($"进程ID参数值[{args[0]}]不是进程ID");
                return null;
            }

            Process agentProcess = Process.GetProcessById(processId);
            if (agentProcess == null)
            {
                //参数值对应的进程不存在
                Loger.Fatal($"进程ID[{processId}]对应的进程不存在");
                return null;
            }

            return agentProcess;
        }

        /// <summary>
        /// 应用程序域未捕获异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Loger.Error(ex);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                Loger.Error(ex);
            }
        }
    }
}
