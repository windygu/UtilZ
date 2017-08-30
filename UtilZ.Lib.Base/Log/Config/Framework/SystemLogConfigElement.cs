using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Core;
using UtilZ.Lib.Base.NLog.Config.Interface;

namespace UtilZ.Lib.Base.NLog.Config.Framework
{
    /// <summary>
    /// 系统日志记录器配置
    /// </summary>
    public class SystemLogConfigElement : BaseLogConfig, ISystemLogConfig
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
        /// 构造函数
        /// </summary>
        public SystemLogConfigElement()
            : base()
        { }

        /// <summary>
        /// 日志保留天数
        /// </summary>
        [ConfigurationProperty(SystemLogConfigElement.DaysPropertyName, DefaultValue = 7, IsRequired = false)]
        public int Days
        {
            get
            {
                return Convert.ToInt32(this[SystemLogConfigElement.DaysPropertyName]);
            }
            set
            {
                this[SystemLogConfigElement.DaysPropertyName] = value;
            }
        }

        /// <summary>
        /// 日志文件上限大小,当文件超过此值则分隔成多个日志文件,单位/MB
        /// </summary>
        [ConfigurationProperty(SystemLogConfigElement.LogFileSizePropertyName, DefaultValue = 10, IsRequired = false)]
        public int LogFileSize
        {
            get
            {
                return Convert.ToInt32(this[SystemLogConfigElement.LogFileSizePropertyName]);
            }
            set
            {
                this[SystemLogConfigElement.LogFileSizePropertyName] = value;
            }
        }

        ///// <summary>
        ///// 判断两个配置项值是否相同[相同返回true;否则返回false]
        ///// </summary>
        ///// <param name="config">目标项</param>
        ///// <returns>相同返回true;否则返回false</returns>
        //public override bool Equals(IConfig config)
        //{
        //    if (!base.Equals(config))
        //    {
        //        return false;
        //    }

        //    SystemLogConfigElement exobj = config as SystemLogConfigElement;
        //    if (exobj == null)
        //    {
        //        return false;
        //    }

        //    if (this.Days != exobj.Days)
        //    {
        //        return false;
        //    }

        //    if (this.LogFileSize != exobj.LogFileSize)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
