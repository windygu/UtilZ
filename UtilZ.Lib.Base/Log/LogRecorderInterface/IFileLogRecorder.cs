using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Interface;

namespace UtilZ.Lib.Base.NLog.LogRecorderInterface
{
    /// <summary>
    /// 文件日志记录接口
    /// </summary>
    public interface IFileLogRecorder : ILogRecorder
    {
        /// <summary>
        /// 配置
        /// </summary>
        IFileLogConfig Config { get; set; }
    }
}
