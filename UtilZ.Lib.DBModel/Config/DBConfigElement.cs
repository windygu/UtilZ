using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Constant;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.Foundation;

namespace UtilZ.Lib.DBModel.Config
{
    /// <summary>
    /// 日志基础配置类
    /// </summary>
    public class DBConfigElement : BaseConfig
    {
        /// <summary>
        /// 数据库编号属性名称
        /// </summary>
        private const string DBIDPropertyName = "DBID";

        /// <summary>
        /// 数据库连接名称属性名称
        /// </summary>
        private const string ConNamePropertyName = "ConName";

        /// <summary>
        /// 数据库连接信息类型属性名称
        /// </summary>
        private const string DBConInfoTypePropertyName = "DBConInfoType";

        /// <summary>
        /// 数据库连接字符串属性名称
        /// </summary>
        private const string ConStrPropertyName = "ConStr";

        /// <summary>
        /// 数据库服务器主机名或IP属性名称
        /// </summary>
        private const string HostPropertyName = "Host";

        /// <summary>
        /// 数据库服务器端口号属性名称
        /// </summary>
        private const string PortPropertyName = "Port";

        /// <summary>
        /// SQL语句执行超时时间属性名称
        /// </summary>
        private const string CommandTimeoutPropertyName = "CommandTimeout";

        /// <summary>
        /// 数据库名称属性名称
        /// </summary>
        private const string DatabaseNamePropertyName = "DatabaseName";

        /// <summary>
        /// 帐号属性名称
        /// </summary>
        private const string AccountPropertyName = "Account";

        /// <summary>
        /// 密码属性名称
        /// </summary>
        private const string PasswordPropertyName = "Password";

        /// <summary>
        /// 数据库连接信息解密接口程序集名属性名称
        /// </summary>
        private const string DecryptionPropertyName = "Decryption";

        /// <summary>
        /// 数据库访问工厂类型名称属性名称
        /// </summary>
        private const string DBFactoryPropertyName = "DBFactory";

        /// <summary>
        /// sql语句最大长度属性名称
        /// </summary>
        private const string SqlMaxLengthPropertyName = "SqlMaxLength";

        /// <summary>
        /// 扩展参数
        /// </summary>
        private const string ExtendPropertyName = "Extend";

        /// <summary>
        /// 数据库写连接数属性名称
        /// </summary>
        private const string WriteConCountPropertyName = "WriteConCount";

        /// <summary>
        /// 数据库读连接数属性名称
        /// </summary>
        private const string ReadConCountPropertyName = "ReadConCount";

        /// <summary>
        /// 获取连接超时时长属性名称
        /// </summary>
        private const string GetConTimeoutPropertyName = "GetConTimeout";


        /// <summary>
        /// 数据库编号,int.MinValue无效编号
        /// </summary>
        [ConfigurationProperty(DBIDPropertyName, DefaultValue = int.MinValue, IsRequired = true)]
        public int DBID
        {
            get
            {
                return Convert.ToInt32(this[DBIDPropertyName]);
            }
            set
            {
                this[DBIDPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库连接名称
        /// </summary>
        [ConfigurationProperty(ConNamePropertyName, DefaultValue = null, IsRequired = false)]
        public string ConName
        {
            get
            {
                return Convert.ToString(this[ConNamePropertyName]);
            }
            set
            {
                this[ConNamePropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库连接信息类型[0:字符串;1:ip端口号等分散信息]
        /// </summary>
        [ConfigurationProperty(DBConInfoTypePropertyName, DefaultValue = 0, IsRequired = false)]
        public int DBConInfoType
        {
            get
            {
                return Convert.ToInt32(this[DBConInfoTypePropertyName]);
            }
            set
            {
                this[DBConInfoTypePropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [ConfigurationProperty(ConStrPropertyName, DefaultValue = null, IsRequired = false)]
        public string ConStr
        {
            get
            {
                return Convert.ToString(this[ConStrPropertyName]);
            }
            set
            {
                this[ConStrPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [ConfigurationProperty(HostPropertyName, DefaultValue = null, IsRequired = false)]
        public string Host
        {
            get
            {
                return Convert.ToString(this[HostPropertyName]);
            }
            set
            {
                this[HostPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库服务器端口号
        /// </summary>
        [ConfigurationProperty(PortPropertyName, DefaultValue = 0, IsRequired = false)]
        public int Port
        {
            get
            {
                return Convert.ToInt32(this[PortPropertyName]);
            }
            set
            {
                this[PortPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        [ConfigurationProperty(DatabaseNamePropertyName, DefaultValue = null, IsRequired = false)]
        public string DatabaseName
        {
            get
            {
                return Convert.ToString(this[DatabaseNamePropertyName]);
            }
            set
            {
                this[DatabaseNamePropertyName] = value;
            }
        }

        /// <summary>
        /// 帐号
        /// </summary>
        [ConfigurationProperty(AccountPropertyName, DefaultValue = null, IsRequired = false)]
        public string Account
        {
            get
            {
                return Convert.ToString(this[AccountPropertyName]);
            }
            set
            {
                this[AccountPropertyName] = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [ConfigurationProperty(PasswordPropertyName, DefaultValue = null, IsRequired = false)]
        public string Password
        {
            get
            {
                return Convert.ToString(this[PasswordPropertyName]);
            }
            set
            {
                this[PasswordPropertyName] = value;
            }
        }

        /// <summary>
        /// SQL语句执行超时时间,DBConstant.CommandTimeout为默认值
        /// </summary>
        [ConfigurationProperty(CommandTimeoutPropertyName, DefaultValue = DBConstant.CommandTimeout, IsRequired = false)]
        public int CommandTimeout
        {
            get
            {
                return Convert.ToInt32(this[CommandTimeoutPropertyName]);
            }
            set
            {
                this[CommandTimeoutPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库连接信息解密接口程序集名
        /// </summary>
        [ConfigurationProperty(DecryptionPropertyName, DefaultValue = null, IsRequired = false)]
        public string Decryption
        {
            get
            {
                return Convert.ToString(this[DecryptionPropertyName]);
            }
            set
            {
                this[DecryptionPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库访问工厂类型名称
        /// </summary>
        [ConfigurationProperty(DBFactoryPropertyName, DefaultValue = null, IsRequired = false)]
        public string DBFactory
        {
            get
            {
                return Convert.ToString(this[DBFactoryPropertyName]);
            }
            set
            {
                this[DBFactoryPropertyName] = value;
            }
        }

        /// <summary>
        /// sql语句最大长度,DBConstant.SqlMaxLength为数制库默认值
        /// </summary>
        [ConfigurationProperty(SqlMaxLengthPropertyName, DefaultValue = DBConstant.SqlMaxLength, IsRequired = false)]
        public long SqlMaxLength
        {
            get
            {
                return Convert.ToInt32(this[SqlMaxLengthPropertyName]);
            }
            set
            {
                this[SqlMaxLengthPropertyName] = value;
            }
        }

        /// <summary>
        /// 扩展参数
        /// </summary>
        [ConfigurationProperty(ExtendPropertyName, DefaultValue = null, IsRequired = false)]
        public string Extend
        {
            get
            {
                return Convert.ToString(this[ExtendPropertyName]);
            }
            set
            {
                this[ExtendPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库写连接数,小于1为不限制
        /// </summary>
        [ConfigurationProperty(WriteConCountPropertyName, DefaultValue = DBConstant.WriteConCount, IsRequired = false)]
        public int WriteConCount
        {
            get
            {
                return Convert.ToInt32(this[WriteConCountPropertyName]);
            }
            set
            {
                this[WriteConCountPropertyName] = value;
            }
        }

        /// <summary>
        /// 数据库读连接数,小于1为不限制
        /// </summary>
        [ConfigurationProperty(ReadConCountPropertyName, DefaultValue = DBConstant.ReadConCount, IsRequired = false)]
        public int ReadConCount
        {
            get
            {
                return Convert.ToInt32(this[ReadConCountPropertyName]);
            }
            set
            {
                this[ReadConCountPropertyName] = value;
            }
        }

        /// <summary>
        /// 获取连接超时时长,单位/毫秒
        /// </summary>
        [ConfigurationProperty(GetConTimeoutPropertyName, DefaultValue = DBConstant.GetConTimeout, IsRequired = false)]
        public int GetConTimeout
        {
            get
            {
                return Convert.ToInt32(this[GetConTimeoutPropertyName]);
            }
            set
            {
                this[GetConTimeoutPropertyName] = value;
            }
        }

        /// <summary>
        /// 返回表示当前对象的String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = this.ConName;
            if (string.IsNullOrWhiteSpace(str))
            {
                str = this.DatabaseName;
            }

            return str;
        }
    }
}
