using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log.Config;
using UtilZ.Dotnet.Ex.Log.Config.Core;
using UtilZ.Dotnet.Ex.Log.Config.Interface;

namespace UtilZ.Dotnet.Ex.Log.Config.Framework
{
    /// <summary>
    /// 文件日志记录器配置类
    /// </summary>
    public class FileLogConfigElement : BaseLogConfig, IFileLogConfig
    {
        /// <summary>
        /// 日志保留天数属性名称
        /// </summary>
        private const string DaysPropertyName = "Days";

        /// <summary>
        /// 日志文件上限大小属性名称
        /// </summary>
        private const string LogFileSizePropertyName = "LogFileSize";

        /// <summary>
        /// 置日志存放目录属性名称
        /// </summary>
        private const string LogDirectoryPropertyName = "LogDirectory";

        /// <summary>
        /// 日志级别分类策略属性名称
        /// </summary>
        private const string LevelCategoryPolicyPropertyName = "LevelCategoryPolicy";

        /// <summary>
        /// 日志安全策略属性名称
        /// </summary>
        private const string SecurityPolicyPropertyName = "SecurityPolicy";

        /// <summary>
        /// 进程同步锁名称属性名称
        /// </summary>
        private const string MutexNamePropertyName = "MutexName";

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileLogConfigElement()
            : base()
        {

        }

        /// <summary>
        /// 日志保留天数
        /// </summary>
        [ConfigurationProperty(FileLogConfigElement.DaysPropertyName, DefaultValue = 7, IsRequired = false)]
        public int Days
        {
            get
            {
                return Convert.ToInt32(this[FileLogConfigElement.DaysPropertyName]);
            }
            set
            {
                this[FileLogConfigElement.DaysPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志文件上限大小,当文件超过此值则分隔成多个日志文件,单位/MB
        /// </summary>
        [ConfigurationProperty(FileLogConfigElement.LogFileSizePropertyName, DefaultValue = 10, IsRequired = false)]
        public int LogFileSize
        {
            get
            {
                return Convert.ToInt32(this[FileLogConfigElement.LogFileSizePropertyName]);
            }
            set
            {
                this[FileLogConfigElement.LogFileSizePropertyName] = value;
            }
        }

        /// <summary>
        /// 置日志存放目录
        /// </summary>
        [ConfigurationProperty(FileLogConfigElement.LogDirectoryPropertyName, DefaultValue = "Log", IsRequired = false)]
        public string LogDirectory
        {
            get
            {
                return Convert.ToString(this[FileLogConfigElement.LogDirectoryPropertyName]);
            }
            set
            {
                this[FileLogConfigElement.LogDirectoryPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志级别分类策略,多项分组之间分号间隔，组内逗号分隔
        /// </summary>
        [ConfigurationProperty(FileLogConfigElement.LevelCategoryPolicyPropertyName, DefaultValue = null, IsRequired = false)]
        public string LevelCategoryPolicy
        {
            get
            {
                return Convert.ToString(this[FileLogConfigElement.LevelCategoryPolicyPropertyName]);
            }
            set
            {
                this[FileLogConfigElement.LevelCategoryPolicyPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志安全策略,该类型为实现接口ILogSecurityPolicy的子类,必须实现Encryption方法
        /// </summary>
        [ConfigurationProperty(FileLogConfigElement.SecurityPolicyPropertyName, DefaultValue = null, IsRequired = false)]
        public string SecurityPolicy
        {
            get
            {
                return Convert.ToString(this[FileLogConfigElement.SecurityPolicyPropertyName]);
            }
            set
            {
                this[FileLogConfigElement.SecurityPolicyPropertyName] = value;
            }
        }

        /// <summary>
        /// 进程同步锁名称
        /// </summary>
        [ConfigurationProperty(FileLogConfigElement.MutexNamePropertyName, DefaultValue = null, IsRequired = false)]
        public string MutexName
        {
            get
            {
                return Convert.ToString(this[FileLogConfigElement.MutexNamePropertyName]);
            }
            set
            {
                this[FileLogConfigElement.MutexNamePropertyName] = value;
            }
        }
    }
}
