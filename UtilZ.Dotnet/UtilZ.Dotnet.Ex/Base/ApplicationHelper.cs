using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// 应用程序辅助类,控制台程序正常结束需要手动调用ApplicationHelper.OnRaiseApplicationExitNotify方法
    /// </summary>
    public class ApplicationHelper
    {
        private static readonly List<ApplicationExitNotify> _list = new List<ApplicationExitNotify>();
        private readonly static object _listLock = new object();


        private delegate bool ControlCtrlDelegate(int ctrltype);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate handler, bool add);
        private static ControlCtrlDelegate _consoleAppCloseCtrlHandler = null;

        static ApplicationHelper()
        {
            //var exeAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //Type appType = exeAssembly.EntryPoint.DeclaringType;

            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Exit += Current_Exit;
            }
            else
            {
                System.Windows.Forms.Application.ApplicationExit += Application_ApplicationExit;
                _consoleAppCloseCtrlHandler = new ControlCtrlDelegate(HandlerCallback);
                SetConsoleCtrlHandler(_consoleAppCloseCtrlHandler, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctrlType">[0:Ctrl+C关闭;1:点击控制台状态按钮关闭]</param>
        /// <returns></returns>
        private static bool HandlerCallback(int ctrlType)
        {
            SetConsoleCtrlHandler(_consoleAppCloseCtrlHandler, false);
            OnRaiseApplicationExitNotify();
            return false;
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ApplicationExit -= Application_ApplicationExit;
            OnRaiseApplicationExitNotify();
        }

        private static void Current_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            System.Windows.Application.Current.Exit -= Current_Exit;
            OnRaiseApplicationExitNotify();
        }




        /// <summary>
        /// 触发应用程序结束通知
        /// </summary>
        public static void OnRaiseApplicationExitNotify()
        {
            lock (_listLock)
            {
                foreach (var applicationExitNotify in _list)
                {
                    applicationExitNotify.OnRaiseApplicationExitNotify();
                }
            }
        }

        /// <summary>
        /// 注册应用程序退出通知
        /// </summary>
        /// <param name="applicationExitNotify"></param>
        public static void RegesterExitNotify(ApplicationExitNotify applicationExitNotify)
        {
            if (applicationExitNotify == null)
            {
                return;
            }

            lock (_listLock)
            {
                if (_list.Contains(applicationExitNotify))
                {
                    return;
                }

                _list.Add(applicationExitNotify);
            }
        }

        /// <summary>
        /// 移除应用程序退出通知
        /// </summary>
        /// <param name="applicationExitNotify"></param>
        public static void RemoveExitNotify(ApplicationExitNotify applicationExitNotify)
        {
            if (applicationExitNotify == null)
            {
                return;
            }

            lock (_listLock)
            {
                _list.Remove(applicationExitNotify);
            }
        }

        /// <summary>
        /// 清空应用程序退出通知
        /// </summary>
        public static void ClearExitNotify()
        {
            lock (_listLock)
            {
                _list.Clear();
            }
        }
    }





    /// <summary>
    /// 应用程序退出通知
    /// </summary>
    public class ApplicationExitNotify
    {
        /// <summary>
        /// 结束通知回调
        /// </summary>
        private readonly Action _exitNotifyCallback;

        /// <summary>
        /// 触发应用程序结束通知
        /// </summary>
        public void OnRaiseApplicationExitNotify()
        {
            this._exitNotifyCallback();
        }

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="exitNotifyCallback">结束通知回调</param>
        public ApplicationExitNotify(Action exitNotifyCallback)
        {
            if (exitNotifyCallback == null)
            {
                throw new ArgumentNullException(nameof(exitNotifyCallback));
            }

            this._exitNotifyCallback = exitNotifyCallback;
        }
    }
}
