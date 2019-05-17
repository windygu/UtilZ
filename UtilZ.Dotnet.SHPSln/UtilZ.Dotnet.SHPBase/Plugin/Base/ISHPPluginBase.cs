using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    /// <summary>
    /// 插件超级接口
    /// </summary>
    public interface ISHPPluginBase : IDisposable
    {
        /// <summary>
        /// 加载插件
        /// </summary>
        void Load();

        /// <summary>
        /// 加载插件完成
        /// </summary>
        void Loaded();

        /// <summary>
        /// 卸载插件
        /// </summary>
        void Unload();
    }
}
