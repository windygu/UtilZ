using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPAgent
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

                int agentProtectProcessId = ParseArgs(args);
                var protectAppPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, System.Configuration.ConfigurationManager.AppSettings[AppConfigKeys.PROTECT_APP_PATH]);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FMain(protectAppPath, agentProtectProcessId));
            }
            catch (Exception ex)
            {
                Loger.Fatal(ex, "启动异常");
            }
        }

        private static int ParseArgs(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                return -1;
            }

            int agentProtectProcessId = -1;
            if (!int.TryParse(args[0], out agentProtectProcessId))
            {
                //参数值无效
                Loger.Fatal($"进程ID参数值[{args[0]}]不是有效进程ID");
                return -1;
            }

            return agentProtectProcessId;
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
