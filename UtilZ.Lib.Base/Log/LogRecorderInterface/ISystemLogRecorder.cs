using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Log.Config;
using UtilZ.Lib.Base.Log.Config.Interface;

namespace UtilZ.Lib.Base.Log.LogRecorderInterface
{
    /// <summary>
    /// 系统日志记录接口
    /// </summary>
    public interface ISystemLogRecorder : ILogRecorder
    {
        /// <summary>
        /// 获取或设置日志记录器名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        ISystemLogConfig Config { get; set; }
    }
}
