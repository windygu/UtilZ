using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using UtilZ.Lib.Base.NLog.Config.Framework;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.Config.Core
{
    /// <summary>
    /// 日志配置节类
    /// </summary>
    public class LogConfigSection : ConfigurationSection
    {
        /// <summary>
        /// 日志级别属性名称
        /// </summary>
        private const string LevelPropertyName = "Level";

        /// <summary>
        /// 日志记录器类型属性名称
        /// </summary>
        private const string LogRecorderTypePropertyName = "LogRecorderType";

        /// <summary>
        /// 配置文件路径属性名称
        /// </summary>
        private const string ConfigFilePathPropertyName = "ConfigFilePath";

        /// <summary>
        /// 日志跳过堆栈调用层数属性名称
        /// </summary>
        private const string SkipFramesPropertyName = "SkipFrames";

        /// <summary>
        /// 系统日志属性名称
        /// </summary>
        private const string SystemLogConfigPropertyName = "SystemLogConfig";

        /// <summary>
        /// 数据库配置属性名称
        /// </summary>
        private const string DatabaseLogConfigPropertyName = "DatabaseLogConfig";

        /// <summary>
        /// 文件日志属性名称
        /// </summary>
        private const string FileLogConfigPropertyName = "FileLogConfig";

        /// <summary>
        /// 邮件日志属性名称
        /// </summary>
        private const string MailLogConfigPropertyName = "MailLogConfig";

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LogConfigSection()
        {

        }

        /// <summary>
        /// 日志级别
        /// </summary>
        [ConfigurationProperty(LogConfigSection.LevelPropertyName, DefaultValue = LogLevel.Debug, IsRequired = false)]
        public LogLevel Level
        {
            get
            {
                LogLevel level = LogLevel.Debug;
                try
                {
                    level = (LogLevel)this[LogConfigSection.LevelPropertyName];
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }

                return level;
            }
            set
            {
                this[LogConfigSection.LevelPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        [ConfigurationProperty(LogConfigSection.LogRecorderTypePropertyName, DefaultValue = null, IsRequired = false)]
        public string LogRecorderType
        {
            get
            {
                return Convert.ToString(this[LogConfigSection.LogRecorderTypePropertyName]);
            }
            set
            {
                this[LogConfigSection.LogRecorderTypePropertyName] = value;
            }
        }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        [ConfigurationProperty(LogConfigSection.ConfigFilePathPropertyName, DefaultValue = null, IsRequired = false)]
        public string ConfigFilePath
        {
            get
            {
                return Convert.ToString(this[LogConfigSection.ConfigFilePathPropertyName]);
            }
            set
            {
                this[LogConfigSection.ConfigFilePathPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志跳过堆栈调用层数属性名称
        /// </summary>
        [ConfigurationProperty(LogConfigSection.SkipFramesPropertyName, DefaultValue = 0, IsRequired = false)]
        public int SkipFrames
        {
            get
            {
                return Convert.ToInt32(this[LogConfigSection.SkipFramesPropertyName]);
            }
            set
            {
                this[LogConfigSection.SkipFramesPropertyName] = value;
            }
        }

        /// <summary>
        /// 系统日志配置节点
        /// </summary>
        [ConfigurationProperty(LogConfigSection.SystemLogConfigPropertyName, DefaultValue = null, IsRequired = false)]
        public virtual SystemLogConfigCollection SystemLogConfig
        {
            get
            {
                return base[LogConfigSection.SystemLogConfigPropertyName] as SystemLogConfigCollection;
            }
            private set
            {
                base[LogConfigSection.SystemLogConfigPropertyName] = value as SystemLogConfigCollection;
            }
        }

        /// <summary>
        /// 文件日志配置集合
        /// </summary>
        [ConfigurationProperty(LogConfigSection.FileLogConfigPropertyName, IsDefaultCollection = false, DefaultValue = null, IsRequired = false)]
        public virtual FileLogConfigCollection FileLogConfig
        {
            get
            {
                return base[LogConfigSection.FileLogConfigPropertyName] as FileLogConfigCollection;
            }
            private set
            {
                base[LogConfigSection.FileLogConfigPropertyName] = value as FileLogConfigCollection;
            }
        }

        /// <summary>
        /// 数据库日志配置集合
        /// </summary>
        [ConfigurationProperty(LogConfigSection.DatabaseLogConfigPropertyName, IsDefaultCollection = false, DefaultValue = null, IsRequired = false)]
        public virtual DatabaseLogConfigCollection DatabaseLogConfig
        {
            get
            {
                return base[LogConfigSection.DatabaseLogConfigPropertyName] as DatabaseLogConfigCollection;
            }
            private set
            {
                base[LogConfigSection.DatabaseLogConfigPropertyName] = value as DatabaseLogConfigCollection;
            }
        }

        /// <summary>
        /// 邮件日志配置集合
        /// </summary>
        [ConfigurationProperty(LogConfigSection.MailLogConfigPropertyName, IsDefaultCollection = false, DefaultValue = null, IsRequired = false)]
        public virtual MailLogConfigCollection MailLogConfig
        {
            get
            {
                return base[LogConfigSection.MailLogConfigPropertyName] as MailLogConfigCollection;
            }
            private set
            {
                base[LogConfigSection.MailLogConfigPropertyName] = value as MailLogConfigCollection;
            }
        }

        /// <summary>
        /// 获取日志记录器个数
        /// </summary>
        public int Count
        {
            get
            {
                return this.SystemLogConfig.Count + this.FileLogConfig.Count + this.DatabaseLogConfig.Count + this.MailLogConfig.Count;
            }
        }
    }
}
