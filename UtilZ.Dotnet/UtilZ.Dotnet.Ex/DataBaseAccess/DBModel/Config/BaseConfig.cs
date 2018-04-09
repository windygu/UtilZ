using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Config
{
    /// <summary>
    /// 基础配置类
    /// </summary>
    public abstract class BaseConfig : System.Configuration.ConfigurationElement
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseConfig()
        {
            this.Key = Guid.NewGuid();
        }

        /// <summary>
        /// Key
        /// </summary>
        public Guid Key { get; private set; }
    }
}
