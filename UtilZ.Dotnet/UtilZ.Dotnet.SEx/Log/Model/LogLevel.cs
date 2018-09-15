using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SEx.Log
{
    /// <summary>
    /// 日志级别[1-5,值越大;优先级:Faltal(5)>Error(4)>Warn(3)>Info(2)>Debug(1)]
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 追踪
        /// </summary>
        Trace = 11,

        /// <summary>
        /// 调试
        /// </summary>
        Debug = 12,

        /// <summary>
        /// 提示
        /// </summary>
        Info = 13,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 14,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 15,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal = 16,

        /// <summary>
        /// 关闭
        /// </summary>
        Off = 100
    }
}
