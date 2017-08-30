using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// 非托管程序集扩展类
    /// </summary>
    public class NExtendUnmanagedDll
    {
        //帮助文章:http://www.cnblogs.com/fdyang/p/4015396.html

        /// <summary>
        /// 加载C++ dll
        /// </summary>
        /// <param name="dllPath">dll路径</param>
        /// <returns>库句柄</returns>
        public static int LoadLibrary(string dllPath)
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
        public static int LoadLibraryEx(string dllPath, int hFile = 0, int dwFlags = 0x00000008)
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
        public static Delegate GetProcDelegate(int libHandle, string funcName, Type delegateType)
        {
            //通过非托管函数名转换为对应的委托
            int address = NativeMethods.GetProcAddress(libHandle, funcName);
            if (address == 0)
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(new IntPtr(address), delegateType);
            }
        }

        /// <summary>
        /// 释放库
        /// </summary>
        /// <param name="libHandle">库句柄</param>
        /// <returns>释放结果</returns>
        public static int FreeLibrary(int libHandle)
        {
            return NativeMethods.FreeLibrary(libHandle);
        }
    }
}
