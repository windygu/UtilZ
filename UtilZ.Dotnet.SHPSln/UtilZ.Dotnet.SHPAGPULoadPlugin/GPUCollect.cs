using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;
using UtilZ.Dotnet.SHPBase.Plugin.PluginABase;

namespace UtilZ.Dotnet.SHPAGPULoadPlugin
{
    /// <summary>
    /// GPU负载收集类
    /// </summary>
    [SHPPluginAttribute(10001, "GPU", "GPU硬件监视A端")]
    public class GPUCollect : SHPAHardCollectBase
    {
        private readonly uint _gpuCoreCiount = 0;
        public GPUCollect()
        {
            try
            {
                this._gpuCoreCiount = NativeMethods.GetGPUCoreCount();
                this._enable = true;
            }
            catch (Exception)
            {
                //Loger.Error(ex);
            }
        }

        private bool _enable = false;
        /// <summary>
        /// 插件是否可用
        /// </summary>
        public override bool Enable
        {
            get
            {
                return this._enable;
            }
        }

        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <returns></returns>
        public override string GetHardInfo()
        {
            return "GPU信息";
        }

        /// <summary>
        /// 获取负载信息
        /// </summary>
        /// <returns>负载</returns>
        public override string GetLoad()
        {
            var items = new List<GPUInfoItem>();
            for (uint i = 0; i < this._gpuCoreCiount; i++)
            {
                var item = new GPUInfoItem();
                try
                {
                    item.DeviceIndex = i;
                    item.Use = NativeMethods.GPUGetUtilizationRate(i);
                    var memInfo = NativeMethods.GPUGetMemInfo(i);
                    item.FreeMem = memInfo.FreeMem;
                    item.TotalMem = memInfo.TotalMem;
                    item.UsedMem = memInfo.UsedMem;
                    item.Status = true;
                }
                catch (Exception ex)
                {
                    item.Status = false;
                    Loger.Warn(ex);
                }

                items.Add(item);
            }

            return SerializeEx.WebScriptJsonSerializerObject(items);
        }
    }
}
