using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBIBase.DBModel.Config
{
    /// <summary>
    /// 数据库配置节类
    /// </summary>
    public class DBConfigSection : ConfigurationSection
    {
        /// <summary>
        /// 数据库配置集合属性名称
        /// </summary>
        private const string DBItemsPropertyName = "DBItems";

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DBConfigSection()
        {

        }

        /// <summary>
        /// 数据库配置集合
        /// </summary>
        [ConfigurationProperty(DBItemsPropertyName, DefaultValue = null, IsRequired = false)]
        public virtual DBConfigCollection DBItems
        {
            get
            {
                return base[DBItemsPropertyName] as DBConfigCollection;
            }
            private set
            {
                base[DBItemsPropertyName] = value as DBConfigCollection;
            }
        }
    }
}
