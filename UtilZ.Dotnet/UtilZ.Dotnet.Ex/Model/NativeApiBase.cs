using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.Ex.Model
{
    /// <summary>
    /// Windows系统API类
    /// </summary>
    public class NativeApiBase
    {
        /// <summary>
        /// 刷新系统缓存图标
        /// </summary>
        public static void RefreshSystemdCatchIcon()
        {
            NativeMethods.SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// 根据句柄查询该句柄对应的进程ID
        /// </summary>
        /// <param name="hwnd">应用程序句柄</param>
        /// <param name="processId">进程ID</param>
        /// <returns>创建窗口的线程ID</returns>
        public static int GetWindowThreadProcessId(IntPtr hwnd, out int processId)
        {
            return NativeMethods.GetWindowThreadProcessId(hwnd, out processId);
        }

        //private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    //使本程序与文件类型关联
        //    ShortcutHelper.AssociateWithFile("ATCSTestCaseFile", ".tc", @"C:\Program Files\ATCS\ATCS.exe");
        //    // 刷新系统缓存图标
        //    RefreshSystemdCatchIcon();

        //    //分离本程序与文件类型的关联
        //    ShortcutHelper.DisassocateWithFile("ATCSTestCaseFile", ".tc");
        //    // 刷新系统缓存图标
        //    RefreshSystemdCatchIcon();
        //}

        #region 最小化窗口在任务栏闪烁
        /// <summary>
        /// 最小化窗口在任务栏闪烁一次
        /// </summary>
        /// <param name="hwnd">窗口够本</param>
        /// <param name="bInvert">true:表示窗口从一个状态闪烁到另一个状态;false:表示窗口恢复到初始状态（可能是激活的也可能是非激活的）</param>
        /// <returns>表示调用FlashWindow函数之前窗口的活动状态，若指定窗口在调用函数之前是激活的，那么返回非零值，否则返回零值</returns>
        public static bool FlashWindow(IntPtr hwnd, bool bInvert)
        {
            return NativeMethods.FlashWindow(hwnd, bInvert);
        }

        /// <summary>
        /// Stop flashing. The system restores the window to its original stae.
        /// </summary>
        private const uint FLASHW_STOP = 0;

        /// <summary>
        /// Flash the window caption.
        /// </summary>
        private const uint FLASHW_CAPTION = 1;

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        private const uint FLASHW_TRAY = 2;

        /// <summary>
        /// Flash both the window caption and taskbar button.
        /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        private const uint FLASHW_ALL = 3;

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        private const uint FLASHW_TIMER = 4;

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        private const uint FLASHW_TIMERNOFG = 12;

        /// <summary>
        /// A boolean value indicating whether the application is running on Windows 2000 or later.
        /// </summary>
        private static bool Win2000OrLater
        {
            get { return System.Environment.OSVersion.Version.Major >= 5; }
        }

        /// <summary>
        /// 创建窗口闪烁对象信息
        /// </summary>
        /// <param name="handle">窗口够本</param>
        /// <param name="flags">The Flash Status.</param>
        /// <param name="count">次数</param>
        /// <param name="timeout">The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.</param>
        /// <returns></returns>
        private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
        {
            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = Convert.ToUInt32(System.Runtime.InteropServices.Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }

        /// <summary>
        /// 任务栏窗口闪烁直到该窗口接收到焦点为止
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns></returns>
        public static bool Flash(IntPtr hwnd)
        {
            // Make sure we're running under Windows 2000 or later
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);
                return NativeMethods.FlashWindowEx(ref fi);
            }
            return false;
        }

        /// <summary>
        /// Flash the specified Window (form) for the specified number of times
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="count">闪烁次数</param>
        /// <returns></returns>
        public static bool Flash(IntPtr hwnd, uint count)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_ALL, count, 0);
                return NativeMethods.FlashWindowEx(ref fi);
            }
            return false;
        }

        /// <summary>
        /// Start Flashing the specified Window (form)
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns></returns>
        public static bool Start(IntPtr hwnd)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_ALL, uint.MaxValue, 0);
                return NativeMethods.FlashWindowEx(ref fi);
            }
            return false;
        }

        /// <summary>
        /// Stop Flashing the specified Window (form)
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns></returns>
        public static bool Stop(IntPtr hwnd)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_STOP, uint.MaxValue, 0);
                return NativeMethods.FlashWindowEx(ref fi);
            }
            return false;
        }
        #endregion

        #region 窗口淡入淡出
        /// <summary>
        /// 窗口淡入淡出
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="dwTime">动画持续时间</param>
        /// <param name="dwFlags">动画类型(WindowAnimateType中的值按位枚举运算)</param>
        /// <returns>结果</returns>
        public static bool WindowFadeInOut(IntPtr hwnd, int dwTime, int dwFlags)
        {
            return NativeMethods.AnimateWindow(hwnd, dwTime, dwFlags);
        }

        /// <summary>
        /// 窗口淡入
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="dwTime">动画持续时间</param>
        /// <param name="dwFlags">动画类型(WindowAnimateType中的值按位枚举运算)</param>
        /// <returns>结果</returns>
        public static bool WindowFadeIn(IntPtr hwnd, int dwTime = 300, int dwFlags = WindowAnimateType.AW_BLEND)
        {
            return NativeMethods.AnimateWindow(hwnd, dwTime, dwFlags);
        }

        /// <summary>
        /// 窗口淡出
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="dwTime">动画持续时间</param>
        /// <param name="dwFlags">动画类型(WindowAnimateType中的值按位枚举运算)</param>
        /// <returns>结果</returns>
        public static bool WindowFadeOut(IntPtr hwnd, int dwTime = 300, int dwFlags = WindowAnimateType.AW_SLIDE | WindowAnimateType.AW_HIDE | WindowAnimateType.AW_BLEND)
        {
            return NativeMethods.AnimateWindow(hwnd, dwTime, dwFlags);
        }
        #endregion
    }

    /// <summary>
    /// 动画类型定义类
    /// </summary>
    public class WindowAnimateType
    {
        /// <summary>
        /// 从左到右打开窗口
        /// </summary>
        public const int AW_HOR_POSITIVE = 0x00000001;

        /// <summary>
        /// 从右到左打开窗口
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0x00000002;

        /// <summary>
        /// 从上到下打开窗口
        /// </summary>
        public const int AW_VER_POSITIVE = 0x00000004;

        /// <summary>
        /// 从下到上打开窗口
        /// </summary>
        public const int AW_VER_NEGATIVE = 0x00000008;

        /// <summary>
        /// 若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
        /// </summary>
        public const int AW_CENTER = 0x00000010;

        /// <summary>
        /// 隐藏窗口，缺省则显示窗口
        /// </summary>
        public const int AW_HIDE = 0x00010000;

        /// <summary>
        /// 激活窗口。在使用了AW_HIDE标志后不要使用这个标志
        /// </summary>
        public const int AW_ACTIVATE = 0x00020000;

        /// <summary>
        /// 使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        /// </summary>
        public const int AW_SLIDE = 0x00040000;

        /// <summary>
        /// 使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志
        /// </summary>
        public const int AW_BLEND = 0x00080000;
    }
}
