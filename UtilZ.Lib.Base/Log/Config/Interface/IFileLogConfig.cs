using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.NLog.Config.Interface
{
    /// <summary>
    /// 文件日志配置接口
    /// </summary>
    public interface IFileLogConfig : IConfig
    {
        /// <summary>
        /// 日志保留天数
        /// </summary>
        int Days { get; set; }

        /// <summary>
        /// 日志文件上限大小,当文件超过此值则分隔成多个日志文件,单位/MB
        /// </summary>
        int LogFileSize { get; set; }

        /// <summary>
        /// 置日志存放目录
        /// </summary>
        string LogDirectory { get; set; }

        /// <summary>
        /// 日志级别分类策略,多项分组之间分号间隔，组内逗号分隔
        /// </summary>
        string LevelCategoryPolicy { get; set; }

        /// <summary>
        /// 日志安全策略
        /// </summary>
        string SecurityPolicy { get; set; }

        /// <summary>
        /// 进程同步锁名称
        /// </summary>
        string MutexName { get; set; }
    }
}
