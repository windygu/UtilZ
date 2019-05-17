using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Plugin.PluginABase
{
    /// <summary>
    /// Agent端硬件数据收集接口
    /// </summary>
    public interface ISHPAHardCollect : ISHPHardBase
    {
        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <returns></returns>
        string GetHardInfo();

        /// <summary>
        /// 获取负载信息
        /// </summary>
        /// <returns>负载</returns>
        string GetLoad();
    }
}
