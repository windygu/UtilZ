using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log.Config;
using UtilZ.Dotnet.Ex.Log.Config.Interface;
using UtilZ.Dotnet.Ex.Log.Config.Core;

namespace UtilZ.Dotnet.Ex.Log.Config.Framework
{
    /// <summary>
    /// 数据库日志记录器配置
    /// </summary>
    public class DatabaseLogElement : BaseLogConfig, IDatabaseLogConfig
    {
        /// <summary>
        /// DBID属性名称
        /// </summary>
        private const string DBIDPropertyName = "DBID";

        /// <summary>
        /// 是否使用数据库编号属性名称
        /// </summary>
        private const string IsUseDBIDPropertyName = "IsUseDBID";

        /// <summary>
        /// 日志表名属性名称
        /// </summary>
        private const string TableNamePropertyName = "TableName";

        /// <summary>
        /// 日志表名属性名称
        /// </summary>
        private const string ConnectionTypePropertyName = "ConnectionType";

        /// <summary>
        /// 数据库连接字符串属性名称
        /// </summary>
        private const string ConnectionStringPropertyName = "ConnectionString";

        /*
        /// <summary>
        /// 数据库参数符号属性名称
        /// </summary>
        private const string DbParaSignPropertyName = "DbParaSign";

        /// <summary>
        /// 是否插入主键属性名称
        /// </summary>
        private const string IsInsertIDPropertyName = "IsInsertID";

        /// <summary>
        /// 时间值是否要转换为整形值属性名称
        /// </summary>
        private const string IsConvertTimeValuePropertyName = "IsConvertTimeValue";
         * */

        /// <summary>
        /// 构造函数
        /// </summary>
        public DatabaseLogElement()
            : base()
        { }

        /// <summary>
        /// 数据库编号ID
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.DBIDPropertyName, DefaultValue = -1, IsRequired = false)]
        public int DBID
        {
            get
            {
                return Convert.ToInt32(this[DatabaseLogElement.DBIDPropertyName]);
            }
            set
            {
                this[DatabaseLogElement.DBIDPropertyName] = value;
            }
        }

        /// <summary>
        /// 是否使用数据库编号[true:使用数据编号;false:使用数据库配置]
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.IsUseDBIDPropertyName, DefaultValue = false, IsRequired = false)]
        public bool IsUseDBID
        {
            get
            {
                return Convert.ToBoolean(this[DatabaseLogElement.IsUseDBIDPropertyName]);
            }
            set
            {
                this[DatabaseLogElement.IsUseDBIDPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志表名
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.TableNamePropertyName, DefaultValue = "NLOG", IsRequired = false)]
        public string TableName
        {
            get
            {
                return Convert.ToString(this[DatabaseLogElement.TableNamePropertyName]);
            }
            set
            {
                this[DatabaseLogElement.TableNamePropertyName] = value;
            }
        }

        /// <summary>
        /// 数据连接对象类型字符串
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.ConnectionTypePropertyName, DefaultValue = null, IsRequired = false)]
        public string ConnectionType
        {
            get
            {
                return Convert.ToString(this[DatabaseLogElement.ConnectionTypePropertyName]);
            }
            set
            {
                this[DatabaseLogElement.ConnectionTypePropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.ConnectionStringPropertyName, DefaultValue = null, IsRequired = false)]
        public string ConnectionString
        {
            get
            {
                return Convert.ToString(this[DatabaseLogElement.ConnectionStringPropertyName]);
            }
            set
            {
                this[DatabaseLogElement.ConnectionStringPropertyName] = value;
            }
        }

        /*
        /// <summary>
        /// 数据库参数符号
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.DbParaSignPropertyName, DefaultValue = null, IsRequired = true)]
        public string DbParaSign
        {
            get
            {
                return Convert.ToString(this[DatabaseLogElement.DbParaSignPropertyName]);
            }
            set
            {
                this[DatabaseLogElement.DbParaSignPropertyName] = value;
            }
        }

        /// <summary>
        /// 是否插入主键[true:插入;false:不插入]
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.IsInsertIDPropertyName, DefaultValue = false, IsRequired = false)]
        public bool IsInsertID
        {
            get
            {
                return Convert.ToBoolean(this[DatabaseLogElement.IsInsertIDPropertyName]);
            }
            set
            {
                this[DatabaseLogElement.IsInsertIDPropertyName] = value;
            }
        }

        /// <summary>
        /// 时间值是否要转换为整形值[true:转换;false:不转换]
        /// </summary>
        [ConfigurationProperty(DatabaseLogElement.IsConvertTimeValuePropertyName, DefaultValue = false, IsRequired = false)]
        public bool IsConvertTimeValue
        {
            get
            {
                return Convert.ToBoolean(this[DatabaseLogElement.IsConvertTimeValuePropertyName]);
            }
            set
            {
                this[DatabaseLogElement.IsConvertTimeValuePropertyName] = value;
            }
        }
         * */
    }
}
