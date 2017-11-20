using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.Lib.Base
{
    /// <summary>
    /// 系统win32方法
    /// </summary>
    public class NativeMethods
    {
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
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

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
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
    }

    /// <summary>
    /// 窗口闪烁结构信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        /// <summary>
        /// The size of the structure in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// A Handle to the Window to be Flashed. The window can be either opened or minimized.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// The Flash Status.
        /// </summary>
        public uint dwFlags;

        /// <summary>
        /// The number of times to Flash the window.
        /// </summary>
        public uint uCount;

        /// <summary>
        /// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
        /// </summary>
        public uint dwTimeout;
    }
}
