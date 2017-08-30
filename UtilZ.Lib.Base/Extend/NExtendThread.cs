using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// 线程扩展方法
    /// </summary>
    public static class NExtendThread
    {
        /// <summary>
        /// win32方式指定当前线程运行在指定CPU核心上
        /// </summary>
        /// <param name="coreID">指定CPU核心ID</param>
        /// <returns>设置结果</returns>
        public static UIntPtr AssignCoreRun(uint coreID)
        {
            //return NativeMethods.SetThreadAffinityMask(NativeMethods.GetCurrentThread(), new UIntPtr(SetCpuID(coreNum)));
            return NativeMethods.SetThreadAffinityMask(NativeMethods.GetCurrentThread(), new UIntPtr(coreID));
        }

        /// <summary>
        /// .net方式指定当前线程运行在指定CPU核心上[多个核心间切换运行,不像win32方式是在一个核心上运行]
        /// </summary>
        /// <param name="threadID">线程ID</param>
        /// <param name="idealProcessor">首选处理器</param>
        /// <param name="coreID">目标处理器(Power(2,0-4]之间的单值或或位运算值)</param>
        public static void AssignCoreRun(int threadID, int idealProcessor, int coreID)
        {
            foreach (ProcessThread proThreadItem in System.Diagnostics.Process.GetCurrentProcess().Threads)
            {
                if (threadID == proThreadItem.Id)
                {
                    proThreadItem.IdealProcessor = idealProcessor;
                    proThreadItem.ProcessorAffinity = (IntPtr)coreID;
                }
            }
        }

        //static ulong SetCpuID(int id)
        //{
        //    ulong cpuid = 0;
        //    if (id < 0 || id >= System.Environment.ProcessorCount)
        //    {
        //        id = 0;
        //    }

        //    cpuid |= 1UL << id;

        //    return cpuid;
        //}
    }
}
