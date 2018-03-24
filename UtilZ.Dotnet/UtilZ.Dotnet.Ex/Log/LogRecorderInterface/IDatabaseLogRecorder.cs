using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Config;
using UtilZ.Dotnet.Ex.Log.Config.Interface;

namespace UtilZ.Dotnet.Ex.Log.LogRecorderInterface
{
    /// <summary>
    /// 数据库日志记录接口
    /// </summary>
    public interface IDatabaseLogRecorder : ILogRecorder
    {
        /// <summary>
        /// 配置
        /// </summary>
        IDatabaseLogConfig Config { get; set; }
    }
}
