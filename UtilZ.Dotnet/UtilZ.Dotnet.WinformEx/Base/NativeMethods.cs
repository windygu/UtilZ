using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.Dotnet.WinformEx.Base
{
    public class NativeMethods
    {
        /// <summary>
        /// 窗口淡入淡出
        /// </summary>
        /// <param name="hwnd">handle to window</param>
        /// <param name="dwTime">duration of animation</param>
        /// <param name="dwFlags">animation type</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        /// <summary>
        /// 最小化窗口在任务栏闪烁一次
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="bInvert">true:表示窗口从一个状态闪烁到另一个状态;false:表示窗口恢复到初始状态（可能是激活的也可能是非激活的）</param>
        /// <returns>表示调用FlashWindow函数之前窗口的活动状态，若指定窗口在调用函数之前是激活的，那么返回非零值，否则返回零值</returns>
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        /// <summary>
        /// 进程无焦点时，任务栏最小化窗口闪烁
        /// </summary>
        /// <param name="pwfi">窗口闪烁信息</param>
        /// <returns>返回调用 FlashWindowEx 函数之前指定窗口状态。如果调用之前窗口标题是活动的，返回值为非零值</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

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
        public static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);

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
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
    }
}
