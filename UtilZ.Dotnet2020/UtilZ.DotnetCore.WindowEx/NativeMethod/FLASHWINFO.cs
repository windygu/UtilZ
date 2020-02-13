using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.NativeMethod
{
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
