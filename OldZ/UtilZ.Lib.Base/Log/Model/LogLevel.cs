using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Log.Model
{
    /// <summary>
    /// 日志级别[1-5,值越大;优先级:Faltal(5)>Error(4)>Warn(3)>Info(2)>Debug(1)]
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 提示
        /// </summary>
        Info = 2,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,

        /// <summary>
        /// 致命
        /// </summary>
        Faltal = 5,

        /// <summary>
        /// 关闭
        /// </summary>
        Off
    }
}
