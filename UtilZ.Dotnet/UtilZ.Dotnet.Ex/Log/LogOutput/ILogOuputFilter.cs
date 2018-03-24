using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.LogOutput
{
    /// <summary>
    /// 日志输出过滤接口
    /// </summary>
    public interface ILogOuputFilter
    {
        /// <summary>
        /// 日志输出过滤[返回值:true:过滤通过输出;false:过滤不通过放弃输出]
        /// </summary>
        /// <param name="subItem">日志输出订阅项</param>
        /// <param name="logItem">待输出日志项</param>
        /// <returns>过滤结果</returns>
        bool Filter(LogOutputSubscribeItem subItem, LogOutputArgs logItem);
    }
}
