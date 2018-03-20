using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Log.Config;
using UtilZ.Lib.Base.Log.Config.Interface;

namespace UtilZ.Lib.Base.Log.LogRecorderInterface
{
    /// <summary>
    ///邮件日志记录器接口
    /// </summary>
    public interface IEmailLogRecorder : ILogRecorder
    {
        /// <summary>
        /// 配置
        /// </summary>
        IEmailLogConfig Config { get; set; }
    }
}
