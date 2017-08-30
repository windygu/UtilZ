using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.NLog.Config.Interface
{
    /// <summary>
    /// 系统日志配置接口
    /// </summary>
    public interface ISystemLogConfig : IConfig
    {
        /// <summary>
        /// 日志保留天数
        /// </summary>
        int Days { get; set; }

        /// <summary>
        /// 日志文件上限大小,当文件超过此值则分隔成多个日志文件,单位/MB
        /// </summary>
        int LogFileSize { get; set; }

        //       LogDisplayName 获取事件日志的友好名称。 
        //MachineName 获取或设置在其上读取或写入事件的计算机名称。 
        //MaximumKilobytes 获取或设置最大事件日志大小（以 KB 为单位）。 
        //MinimumRetentionDays 获取要在事件日志中保留项的天数。 
        //OverflowAction 获取在事件日志达到其最大日志文件大小时存储新项的已配置行为。 
        //Site 获取或设置 Component 的 ISite。 （继承自 Component。） 
        //Source 获取或设置在写入事件日志时要注册和使用的源名称 
    }
}
