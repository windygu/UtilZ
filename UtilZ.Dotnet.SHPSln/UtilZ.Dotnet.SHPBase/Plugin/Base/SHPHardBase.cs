using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    public abstract class SHPHardBase : SHPPluginBase, ISHPHardBase
    {
        public SHPHardBase()
            : base()
        {

        }

        /// <summary>
        /// 插件是否可用
        /// </summary>
        public abstract bool Enable { get; }
    }
}
