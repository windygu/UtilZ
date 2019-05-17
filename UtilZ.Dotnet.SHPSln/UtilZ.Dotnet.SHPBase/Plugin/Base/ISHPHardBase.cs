using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    public interface ISHPHardBase : ISHPPluginBase
    {
        /// <summary>
        /// 插件是否可用
        /// </summary>
        bool Enable { get; }
    }
}
