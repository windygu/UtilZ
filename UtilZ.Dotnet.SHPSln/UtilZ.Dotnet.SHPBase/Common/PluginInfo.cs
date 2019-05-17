using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Common
{
    public class PluginInfo<T>
    {
        public T Plugin { get; private set; }

        public SHPPluginAttribute PluginAttribute { get; private set; }

        public PluginInfo(T plugin, SHPPluginAttribute pluginAttribute)
        {
            this.Plugin = plugin;
            this.PluginAttribute = pluginAttribute;
        }
    }
}
