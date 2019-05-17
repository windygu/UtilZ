using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.SHPAGPULoadPlugin
{
    internal class NativeMethods
    {
        private const string GPU_InfoDllPath = @"x64\GPU_Info.dll";

        /// <summary>
        /// 获取GPU卡数量
        /// </summary>
        /// <param name="gpuCount"></param>
        /// <returns></returns>
        [DllImport(GPU_InfoDllPath)]
        private static extern bool GPUGetCount(ref uint gpuCount);

        /// <summary>
        /// 获取GPU卡数量
        /// </summary>
        /// <returns></returns>
        public static uint GetGPUCoreCount()
        {
            uint gpuCount = 0;
            if (GPUGetCount(ref gpuCount))
            {
                return gpuCount;
            }
            else
            {
                throw new Exception("获取GPU核心数失败");
            }
        }
        //__declspec(dllimport) bool GPUGetCount(unsigned int *n);

        /// <summary>
        /// 获取GPU卡内存使用信息,单位均为byte
        /// </summary>
        /// <param name="devID">GPU索引</param>
        /// <param name="totalMem">总内存</param>
        /// <param name="freeMem">可用内存</param>
        /// <param name="usedMem">已用内存</param>
        /// <returns></returns>
        [DllImport(GPU_InfoDllPath)]
        private static extern bool GPUGetMemInfo(uint devID, ref ulong totalMem, ref ulong freeMem, ref ulong usedMem);
        //__declspec(dllimport) bool GPUGetMemInfo(unsigned int devID, unsigned long long *totalMem, unsigned long long *freeMem, unsigned long long *usedMem);

        /// <summary>
        /// 获取GPU卡内存使用信息
        /// </summary>
        /// <param name="deviceIndex">GPU索引</param>
        /// <returns>GPU卡内存使用信息</returns>
        public static GPUMemInfo GPUGetMemInfo(uint deviceIndex)
        {
            ulong totalMem = 0, freeMem = 0, usedMem = 0;
            if (GPUGetMemInfo(deviceIndex, ref totalMem, ref freeMem, ref usedMem))
            {
                return new GPUMemInfo(deviceIndex, totalMem, freeMem, usedMem);
            }
            else
            {
                throw new Exception("获取GPU卡内存信息失败");
            }
        }

        /// <summary>
        /// 获取GPU使用率
        /// </summary>
        /// <param name="devID"></param>
        /// <param name="utilizationRate"></param>
        /// <returns></returns>
        [DllImport(GPU_InfoDllPath)]
        private static extern bool GPUGetUtilizationRate(uint devID, ref uint utilizationRate);
        //__declspec(dllimport) bool GPUGetUtilizationRate(unsigned int devID, unsigned int *utilizationRate);

        /// <summary>
        /// 获取GPU使用率
        /// </summary>
        /// <param name="deviceIndex">GPU索引</param>
        /// <returns>GPU使用率</returns>
        public static uint GPUGetUtilizationRate(uint deviceIndex)
        {
            uint utilizationRate = 0;
            if (GPUGetUtilizationRate(deviceIndex, ref utilizationRate))
            {
                return utilizationRate;
            }
            else
            {
                throw new Exception($"GPU设备[{deviceIndex}]获取GPU使用率失败");
            }
        }
    }
}
