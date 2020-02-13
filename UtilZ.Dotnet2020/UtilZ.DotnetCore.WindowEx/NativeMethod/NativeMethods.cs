using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.NativeMethod
{
    /// <summary>
    /// 系统win32方法
    /// </summary>
    public class NativeMethods
    {
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

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms633499(v=vs.85).aspx
        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings. This function does not search child windows. This function does not perform a case-sensitive search.
        /// To search child windows, beginning with a specified child window, use the FindWindowEx function.
        /// If the lpWindowName parameter is not NULL, FindWindow calls the GetWindowText function to retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks for GetWindowText.
        /// </summary>
        /// <param name="lpClassName">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero.
        /// If lpClassName points to a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names.
        /// If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.</param>
        /// <param name="lpWindowName">The window name (the window's title). If this parameter is NULL, all window names match</param>
        /// <returns>Type:
        /// Type: HWND
        /// If the function succeeds, the return value is a handle to the window that has the specified class name and window name.
        /// If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Retrieves a handle to a window whose class name and window name match the specified strings. The function searches child windows, beginning with the one following the specified child window. This function does not perform a case-sensitive search.
        /// If the lpszWindow parameter is not NULL, FindWindowEx calls the GetWindowText function to retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks section of GetWindowText.
        /// An application can call this function in the following way.
        /// FindWindowEx(NULL, NULL, MAKEINTATOM(0x8000), NULL );
        /// Note that 0x8000 is the atom for a menu class. When an application calls this function, the function checks whether a context menu is being displayed that the application created.
        /// </summary>
        /// <param name="hwndParent">A handle to the parent window whose child windows are to be searched.
        /// If hwndParent is NULL, the function uses the desktop window as the parent window.The function searches among windows that are child windows of the desktop.
        /// If hwndParent is HWND_MESSAGE, the function searches all message-only windows.</param>
        /// <param name="hwndChildAfter">A handle to a child window. The search begins with the next child window in the Z order. The child window must be a direct child window of hwndParent, not just a descendant window.
        /// If hwndChildAfter is NULL, the search begins with the first child window of hwndParent.
        /// Note that if both hwndParent and hwndChildAfter are NULL, the function searches all top-level and message-only windows</param>
        /// <param name="lpszClass">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be placed in the low-order word of lpszClass; the high-order word must be zero.
        /// If lpszClass is a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names, or it can be MAKEINTATOM(0x8000). In this latter case, 0x8000 is the atom for a menu class. For more information, see the Remarks section of this topic</param>
        /// <param name="lpszWindow">The window name (the window's title). If this parameter is NULL, all window names match.</param>
        /// <returns>ype:
        /// Type: HWND
        ///  If the function succeeds, the return value is a handle to the window that has the specified class and window names.
        ///  If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 刷新系统缓存图标
        /// Notifies the system of an event that an application has performed. An application should use this function if it performs an action that may affect the Shell. 
        /// </summary>
        /// <param name="wEventId">Describes the event that has occurred. Typically, only one event is specified at a time. If more than one event is specified, the values contained in the dwItem1 and dwItem2 parameters must be the same, respectively, for all specified events. This parameter can be one or more of the following values. </param>
        /// <param name="uFlags">Flags that indicate the meaning of the dwItem1 and dwItem2 parameters. The uFlags parameter must be one of the following values.</param>
        /// <param name="dwItem1">First event-dependent value. </param>
        /// <param name="dwItem2">Second event-dependent value.</param>
        [DllImport("shell32.dll", EntryPoint = "SHChangeNotify", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void SHChangeNotify(Int32 wEventId, UInt32 uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// 根据句柄查询该句柄对应的进程ID
        /// </summary>
        /// <param name="hwnd">应用程序句柄</param>
        /// <param name="processId">进程ID</param>
        /// <returns>创建窗口的线程ID</returns>
        [DllImport(@"user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processId);

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
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        #region 最小化窗口在任务栏闪烁
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
                return FlashWindowEx(ref fi);
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
                return FlashWindowEx(ref fi);
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
                return FlashWindowEx(ref fi);
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
                return FlashWindowEx(ref fi);
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
            return AnimateWindow(hwnd, dwTime, dwFlags);
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
            return AnimateWindow(hwnd, dwTime, dwFlags);
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
            return AnimateWindow(hwnd, dwTime, dwFlags);
        }
        #endregion

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

        /// <summary>
        /// 指定hThread运行在核心dwThreadAffinityMask
        /// </summary>
        /// <param name="hThread">线程句柄</param>
        /// <param name="dwThreadAffinityMask">CPU核心编号</param>
        /// <returns>设置结果</returns>
        [DllImport("kernel32.dll")]
        public static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);

        /// <summary>
        /// 得到当前线程的句柄
        /// </summary>
        /// <returns>当前线程的句柄</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        #region C#动态调用C++编写的DLL函数
        /// <summary>
        /// 添加目录到DLL搜索路径
        /// </summary>
        /// <param name="dir">The directory to be added to the search path. 
        /// If this parameter is an empty string (""), 
        /// the call removes the current directory from the default DLL search order. 
        /// If this parameter is NULL, the function restores the default search order</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern bool SetDllDirectory(string dir);

        /// <summary>
        /// 加载C++ dll
        /// </summary>
        /// <param name="dllPath">dll路径</param>
        /// <returns>库句柄</returns>
        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string dllPath);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms684179(v=vs.85).aspx
        /// <summary>
        /// 加载C++ dll
        /// </summary>
        /// <param name="dllPath">dll路径</param>
        /// <param name="hFile">This parameter is reserved for future use. It must be NULL.</param>
        /// <param name="dwFlags">The action to be taken when loading the module. If no flags are specified, the behavior of this function is identical to that of the LoadLibrary function. This parameter can be one of the following values.</param>
        /// <returns>库句柄</returns>
        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryEx(string dllPath, int hFile = 0, int dwFlags = 0x00000008);

        /// <summary>
        /// 获取方法句柄
        /// </summary>
        /// <param name="libHandle">库句柄</param>
        /// <param name="funcName">方法名称</param>
        /// <returns>方法句柄</returns>
        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetProcAddress(IntPtr libHandle, string funcName);

        /// <summary>
        /// 释放库
        /// </summary>
        /// <param name="libHandle">库句柄</param>
        /// <returns>释放结果</returns>
        [DllImport("Kernel32")]
        public static extern int FreeLibrary(IntPtr libHandle);
        #endregion

        /// <summary>
        /// 窗口淡入淡出
        /// </summary>
        /// <param name="hwnd">handle to window</param>
        /// <param name="dwTime">duration of animation</param>
        /// <param name="dwFlags">animation type</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        /// <summary>
        /// 
        /// </summary>
        [DllImport("icmp.dll", SetLastError = true)]
        public static extern IntPtr IcmpCreateFile();

        /// <summary>
        /// 
        /// </summary>
        [DllImport("icmp.dll", SetLastError = true)]
        public static extern bool IcmpCloseHandle(IntPtr handle);

        /// <summary>
        /// 
        /// </summary>
        [DllImport("icmp.dll", SetLastError = true)]
        public static extern Int32 IcmpSendEcho(IntPtr icmpHandle, Int32 destinationAddress, IntPtr requestData, Int16 requestSize, IntPtr requestOptions, IntPtr replyBuffer, Int32 replySize, Int32 timeout);

        /// <summary>
        /// 
        /// </summary>
        [DllImport("icmp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int IcmpSendEcho(IntPtr icmpHandle, int destinationAddress, string requestData, short requestSize, ref ICMP_OPTIONS requestOptions, ref ICMP_ECHO_REPLY replyBuffer, int replySize, int timeout);
    }
}
