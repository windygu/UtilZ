using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Plugin.PluginABase
{
    public abstract class SHPAHardCollectBase : SHPHardBase, ISHPAHardCollect
    {
        public SHPAHardCollectBase()
            : base()
        {

        }

        /// <summary>
        /// 获取硬件信息
        /// </summary>
        /// <returns></returns>
        public abstract string GetHardInfo();

        /// <summary>
        /// 获取负载信息
        /// </summary>
        /// <returns>负载</returns>
        public abstract string GetLoad();
    }
}
