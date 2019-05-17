using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    public abstract class SHPPluginBase : ISHPPluginBase
    {
        public SHPPluginBase()
        {

        }

        /// <summary>
        /// 加载插件
        /// </summary>
        public virtual void Load()
        {

        }

        /// <summary>
        /// 加载插件完成
        /// </summary>
        public virtual void Loaded()
        {

        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        public virtual void Unload()
        {

        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {

        }
    }
}
