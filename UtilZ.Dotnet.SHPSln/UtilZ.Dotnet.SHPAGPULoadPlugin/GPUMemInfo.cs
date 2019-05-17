using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.SHPAGPULoadPlugin
{
    /// <summary>
    /// GPU卡内存信息
    /// </summary>
    internal class GPUMemInfo
    {
        /// <summary>
        /// GPU索引
        /// </summary>
        public uint DeviceIndex { get; private set; }

        /// <summary>
        /// 总内存
        /// </summary>
        public ulong TotalMem { get; private set; }

        /// <summary>
        /// 可用内存
        /// </summary>
        public ulong FreeMem { get; private set; }

        /// <summary>
        /// 已用内存
        /// </summary>
        public ulong UsedMem { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceIndex">GPU索引</param>
        /// <param name="totalMem">总内存</param>
        /// <param name="freeMem">可用内存</param>
        /// <param name="usedMem">已用内存</param>
        public GPUMemInfo(uint deviceIndex, ulong totalMem, ulong freeMem, ulong usedMem)
        {
            this.DeviceIndex = deviceIndex;
            this.TotalMem = totalMem;
            this.FreeMem = freeMem;
            this.UsedMem = usedMem;
        }
    }
}
