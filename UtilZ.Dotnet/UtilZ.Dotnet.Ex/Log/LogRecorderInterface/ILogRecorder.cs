using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Config;
using UtilZ.Dotnet.Ex.Log.Config.Interface;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.LogRecorderInterface
{
    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILogRecorder : IRecorder
    {
        /// <summary>
        /// 日志追加器
        /// </summary>
        ILogRecorder LogAppender { get; set; }

        /// <summary>
        /// 过滤器验证是否通过[true:通过;false:不通过]
        /// </summary>
        /// <param name="level">待验证级别</param>
        /// <returns>true:通过;false:不通过</returns>
        bool FilterValidate(LogLevel level);
    }
}
