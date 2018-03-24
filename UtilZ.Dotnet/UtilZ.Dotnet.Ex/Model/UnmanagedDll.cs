using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UtilZ.Dotnet.Ex;

namespace UtilZ.Dotnet.Ex.Model
{
    /// <summary>
    /// 非托管程序集扩展类
    /// </summary>
    public class UnManagedDll
    {
        //帮助文章:http://www.cnblogs.com/fdyang/p/4015396.html

        /// <summary>
        /// 添加目录到DLL搜索路径[代码中第一行一般都是SetDllDirectory(""),目的是为了防DLL挟持]
        /// The SetDllDirectory function affects all subsequent calls to the LoadLibrary and LoadLibraryEx functions.It also effectively disables safe DLL search mode while the specified directory is in the search path.
        /// After calling SetDllDirectory, the standard DLL search path is:
        /// The directory from which the application loaded.
        /// The directory specified by the lpPathName parameter.
        /// The system directory.Use the GetSystemDirectory function to get the path of this directory.The name of this directory is System32.
        /// The 16-bit system directory.There is no function that obtains the path of this directory, but it is searched.The name of this directory is System.
        /// The Windows directory.Use the GetWindowsDirectory function to get the path of this directory.
        /// The directories that are listed in the PATH environment variable.
        /// </summary>
        /// <param name="dir">The directory to be added to the search path. 
        /// If this parameter is an empty string (""), 
        /// the call removes the current directory from the default DLL search order. 
        /// If this parameter is NULL, the function restores the default search order</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        public static bool SetDllDirectory(string dir)
        {
            //https://msdn.microsoft.com/en-us/library/windows/desktop/ms686203(v=vs.85).aspx
            //http://blog.csdn.net/zvall/article/details/51770853
            return NativeMethods.SetDllDirectory(dir);
        }

        /// <summary>
        /// 加载C++ dll
        /// </summary>
        /// <param name="dllPath">dll路径</param>
        /// <returns>库句柄</returns>
        public static IntPtr LoadLibrary(string dllPath)
        {
            return NativeMethods.LoadLibrary(dllPath);
        }

        /// <summary>
        /// 加载C++ dll
        /// </summary>
        /// <param name="dllPath">dll路径</param>
        /// <param name="hFile">This parameter is reserved for future use. It must be NULL.</param>
        /// <param name="dwFlags">The action to be taken when loading the module. If no flags are specified, the behavior of this function is identical to that of the LoadLibrary function. This parameter can be one of the following values.</param>
        /// <returns>库句柄</returns>
        public static IntPtr LoadLibraryEx(string dllPath, int hFile = 0, int dwFlags = 0x00000008)
        {
            return NativeMethods.LoadLibraryEx(dllPath, hFile, dwFlags);
        }

        /// <summary>
        /// 获取方法委托
        /// </summary>
        /// <param name="libHandle">库句柄</param>
        /// <param name="funcName">方法名称</param>
        /// <param name="delegateType">与非托管dll中方法定义对应的委托类型</param>
        /// <returns>方法句柄</returns>
        public static Delegate GetProcDelegate(IntPtr libHandle, string funcName, Type delegateType)
        {
            //通过非托管函数名转换为对应的委托
            IntPtr address = NativeMethods.GetProcAddress(libHandle, funcName);
            if (address == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(address, delegateType);
            }
        }

        /// <summary>
        /// 释放库
        /// </summary>
        /// <param name="libHandle">库句柄</param>
        /// <returns>释放结果</returns>
        public static int FreeLibrary(IntPtr libHandle)
        {
            return NativeMethods.FreeLibrary(libHandle);
        }
    }
}
