using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Model;
using UtilZ.Lib.Base.NLog.Config.Interface;

namespace UtilZ.Lib.Base.NLog.Config.Core
{
    /// <summary>
    /// 日志基础配置类
    /// </summary>
    public abstract class BaseLogConfig : System.Configuration.ConfigurationElement, IConfig
    {
        /// <summary>
        /// 日志记录器名称属性名称
        /// </summary>
        private const string NamePropertyName = "Name";

        /// <summary>
        /// 日志记录器是否启用属性名称
        /// </summary>
        private const string EnablePropertyName = "Enable";

        /// <summary>
        /// 日志布局属性名称
        /// </summary>
        private const string LayoutPropertyName = "Layout";

        /// <summary>
        /// 时间格式属性名称
        /// </summary>
        private const string DateFormatPropertyName = "DateFormat";

        /// <summary>
        /// 是否记录异常堆栈信息属性名称
        /// </summary>
        private const string IsRecordExceptionStackInfoPropertyName = "IsRecordExceptionStackInfo";

        /// <summary>
        /// 是否记录线程信息属性名称
        /// </summary>
        private const string IsRecordThreadInfoPropertyName = "IsRecordThreadInfo";

        /// <summary>
        /// 过滤日志级别起始值属性名称
        /// </summary>
        private const string FilterFromPropertyName = "FilterFrom";

        /// <summary>
        /// 过滤日志级别结束值属性名称
        /// </summary>
        private const string FilterToPropertyName = "FilterTo";

        /// <summary>
        /// 过滤日志名称属性名称
        /// </summary>
        private const string FilterNamePropertyName = "FilterName";

        /// <summary>
        /// 扩展日志记录器类型属性名称
        /// </summary>
        private const string ExtendLogRecorderTypePropertyName = "ExtendLogRecorderType";

        /// <summary>
        /// 日志追加器器类型属性名称
        /// </summary>
        private const string LogAppenderTypePropertyName = "LogAppenderType";

        /// <summary>
        /// 分隔线长度属性名称
        /// </summary>
        private const string SeparatorCountPropertyName = "SeparatorCount";

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseLogConfig()
        {
            this.Key = Guid.NewGuid();
        }

        /// <summary>
        /// Key
        /// </summary>
        public Guid Key { get; private set; }

        /// <summary>
        /// 日志记录器名称
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.NamePropertyName, DefaultValue = null, IsRequired = false)]
        public string Name
        {
            get
            {
                return Convert.ToString(this[BaseLogConfig.NamePropertyName]);
            }
            set
            {
                this[BaseLogConfig.NamePropertyName] = value;
            }
        }

        /// <summary>
        /// 日志记录器是否启用
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.EnablePropertyName, DefaultValue = true, IsRequired = false)]
        public bool Enable
        {
            get
            {
                return Convert.ToBoolean(this[BaseLogConfig.EnablePropertyName]);
            }
            set
            {
                this[BaseLogConfig.EnablePropertyName] = value;
            }
        }

        /// <summary>
        /// 日志布局
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.LayoutPropertyName, DefaultValue = null, IsRequired = false)]
        public string Layout
        {
            get
            {
                return Convert.ToString(this[BaseLogConfig.LayoutPropertyName]);
            }
            set
            {
                this[BaseLogConfig.LayoutPropertyName] = value;
            }
        }

        /// <summary>
        /// 时间格式
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.DateFormatPropertyName, DefaultValue = null, IsRequired = false)]
        public string DateFormat
        {
            get
            {
                return Convert.ToString(this[BaseLogConfig.DateFormatPropertyName]);
            }
            set
            {
                this[BaseLogConfig.DateFormatPropertyName] = value;
            }
        }

        /// <summary>
        /// 是否记录异常堆栈信息,true:记录,false不记录[默认为true]
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.IsRecordExceptionStackInfoPropertyName, DefaultValue = true, IsRequired = false)]
        public bool IsRecordExceptionStackInfo
        {
            get
            {
                return Convert.ToBoolean(this[BaseLogConfig.IsRecordExceptionStackInfoPropertyName]);
            }
            set
            {
                this[BaseLogConfig.IsRecordExceptionStackInfoPropertyName] = value;
            }
        }

        /// <summary>
        /// 是否记录线程信息,true:记录,false不记录[默认为true]
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.IsRecordThreadInfoPropertyName, DefaultValue = true, IsRequired = false)]
        public bool IsRecordThreadInfo
        {
            get
            {
                return Convert.ToBoolean(this[BaseLogConfig.IsRecordThreadInfoPropertyName]);
            }
            set
            {
                this[BaseLogConfig.IsRecordThreadInfoPropertyName] = value;
            }
        }

        /// <summary>
        /// 过滤日志级别起始值
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.FilterFromPropertyName, DefaultValue = LogLevel.Debug, IsRequired = false)]
        public LogLevel FilterFrom
        {
            get
            {
                LogLevel level = LogLevel.Debug;
                try
                {
                    level = (LogLevel)this[BaseLogConfig.FilterFromPropertyName];
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }

                return level;
            }
            set
            {
                this[BaseLogConfig.FilterFromPropertyName] = value;
            }
        }

        /// <summary>
        /// 过滤日志级别结束值
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.FilterToPropertyName, DefaultValue = LogLevel.Faltal, IsRequired = false)]
        public LogLevel FilterTo
        {
            get
            {
                LogLevel level = LogLevel.Debug;
                try
                {
                    level = (LogLevel)this[BaseLogConfig.FilterToPropertyName];
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }

                return level;
            }
            set
            {
                this[BaseLogConfig.FilterToPropertyName] = value;
            }
        }

        /// <summary>
        /// 过滤日志名称
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.FilterNamePropertyName, DefaultValue = null, IsRequired = false)]
        public string FilterName
        {
            get
            {
                return Convert.ToString(this[BaseLogConfig.FilterNamePropertyName]);
            }
            set
            {
                this[BaseLogConfig.FilterNamePropertyName] = value;
            }
        }

        /// <summary>
        /// 扩展日志记录器类型
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.ExtendLogRecorderTypePropertyName, DefaultValue = null, IsRequired = false)]
        public string ExtendLogRecorderType
        {
            get
            {
                return Convert.ToString(this[BaseLogConfig.ExtendLogRecorderTypePropertyName]);
            }
            set
            {
                this[BaseLogConfig.ExtendLogRecorderTypePropertyName] = value;
            }
        }

        /// <summary>
        /// 日志追加器器类型
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.LogAppenderTypePropertyName, DefaultValue = null, IsRequired = false)]
        public string LogAppenderType
        {
            get
            {
                return Convert.ToString(this[BaseLogConfig.LogAppenderTypePropertyName]);
            }
            set
            {
                this[BaseLogConfig.LogAppenderTypePropertyName] = value;
            }
        }

        /// <summary>
        /// 分隔线长度
        /// </summary>
        [ConfigurationProperty(BaseLogConfig.SeparatorCountPropertyName, DefaultValue = 140, IsRequired = false)]
        public int SeparatorCount
        {
            get
            {
                return Convert.ToInt32(this[BaseLogConfig.SeparatorCountPropertyName]);
            }
            set
            {
                this[BaseLogConfig.SeparatorCountPropertyName] = value;
            }
        }

        /// <summary>
        /// 分隔线
        /// </summary>
        private string _separatorLine = null;

        /// <summary>
        /// 获取分隔线
        /// </summary>
        public string SeparatorLine
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_separatorLine))
                {
                    _separatorLine = new string('-', this.SeparatorCount);
                }

                return _separatorLine;
            }
        }

        /// <summary>
        /// 判断两个配置项值是否相同[相同返回true;否则返回false]
        /// </summary>
        /// <param name="config">目标项</param>
        /// <returns>相同返回true;否则返回false</returns>
        public virtual bool Equals(IConfig config)
        {
            if (config == null)
            {
                return false;
            }
            else if (this == config)
            {
                return true;
            }

            if (this.GetType() != config.GetType())
            {
                return false;
            }

            if (!(this.IsRecordExceptionStackInfo && config.IsRecordExceptionStackInfo))
            {
                return false;
            }

            if (this.FilterFrom != config.FilterFrom)
            {
                return false;
            }

            if (this.FilterTo != config.FilterTo)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 字符串比较[相同返回true;不同返回false]
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        /// <returns>相同返回true;不同返回false</returns>
        protected bool StringEquals(string str1, string str2)
        {
            if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2) && !str1.Equals(str2))
            {
                return false;
            }
            else if (!string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2) ||
                string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
            {
                return false;
            }

            return true;
        }
    }
}
