using UtilZ.Lib.Base.Ex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace UtilZ.Lib.Winform.Extend
{
    /// <summary>
    /// Process扩展方法类
    /// </summary>
    public static class WindowNestProcess
    {
        /// <summary>
        /// 在窗口控件中启动应用程序
        /// </summary>
        /// <param name="process">进程实例</param>
        /// <param name="containerControl">容器控件</param>
        public static NestProcess Start(this Process process, System.Windows.Forms.Control containerControl)
        {
            if (process == null)
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => process));
            }

            NestProcess nprocess = new NestProcess(process);
            nprocess.StartNormalApp(containerControl);
            return nprocess;
        }
    }

    /// <summary>
    /// 应用程序嵌套启动类
    /// </summary>
    public class NestProcess
    {
        /// <summary>
        /// 私有构造函数不允许实例化
        /// </summary>
        internal NestProcess(Process process)
        {
            this._process = process;
        }

        /// <summary>
        /// 设置应用程序的父窗口
        /// </summary>
        /// <param name="hWndChild">子窗口句柄</param>
        /// <param name="hWndNewParent">父窗口句柄</param>
        /// <returns>long</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        /// 移除应用启动的程序窗口边框
        /// </summary>
        /// <param name="hwnd">要移除边框的应用程序句柄</param>
        /// <param name="nIndex">索引</param>
        /// <param name="dwNewLong">边框值</param>
        /// <returns>long</returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        private static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);

        /// <summary>
        /// 移动应用程序窗口位置
        /// </summary>
        /// <param name="hwnd">程序窗口句柄</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="cx">宽度</param>
        /// <param name="cy">高度</param>
        /// <param name="repaint">是否修正</param>
        /// <returns>bool</returns>
        [DllImport("user32.dll", EntryPoint = "MoveWindow", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        /// <summary>
        /// 移除窗口边框消息索引
        /// </summary>
        private const int GWL_STYLE = (-16);

        /// <summary>
        /// 移除窗口边框消息编号
        /// </summary>
        private const int WS_VISIBLE = 0x10000000;

        /// <summary>
        /// 窗口控件中的应用程序进程
        /// </summary>
        private readonly Process _process = null;

        /// <summary>
        /// 获取窗口控件中的应用程序进程
        /// </summary>
        public Process Process
        {
            get { return _process; }
        }

        /// <summary>
        /// 在窗口控件中启动应用程序
        /// </summary>
        /// <param name="containerControl">容器控件</param>
        internal void StartNormalApp(System.Windows.Forms.Control containerControl)
        {
            //启动进程
            this._process.Start();

            // Wait for process to be created and enter idle condition 
            this._process.WaitForInputIdle();

            //启用的应用程序要等待这么久,否则不会被嵌套到指定的容器控件中,具体原因为毛以后再研究
            System.Threading.Thread.Sleep(190);

            // Remove border and whatnot
            NestProcess.SetWindowLong(this._process.MainWindowHandle, NestProcess.GWL_STYLE, NestProcess.WS_VISIBLE);

            // Put it into this form
            NestProcess.SetParent(this._process.MainWindowHandle, containerControl.Handle);

            // Move the window to overlay it on this window
            NestProcess.MoveWindow(this._process.MainWindowHandle, 0, 0, containerControl.Width, containerControl.Height, true);

            //注册窗口控件大小改变及窗口控件所在窗口关闭事件
            containerControl.Resize += containerControl_Resize;
            containerControl.FindForm().FormClosing += form_FormClosing;
        }

        /// <summary>
        /// 窗口控件所在窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this._process.CloseMainWindow();
            this._process.WaitForExit();

            // Post a colse message
            //Qianru.PostMessage(this._process.MainWindowHandle, 0x10, 0, 0);
            //this._process.WaitForExit();

            //process.Kill();
        }

        /// <summary>
        /// 窗口控件大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void containerControl_Resize(object sender, EventArgs e)
        {
            System.Windows.Forms.Control containerControl = sender as System.Windows.Forms.Control;
            NestProcess.MoveWindow(this._process.MainWindowHandle, 0, 0, containerControl.Width, containerControl.Height, true);
        }
    }
}
