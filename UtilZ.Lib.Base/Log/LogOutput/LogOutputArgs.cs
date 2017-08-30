using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.NLog.Model
{
    /// <summary>
    /// 外部日志输出事件参数
    /// </summary>
    public class LogOutputArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logRecorderName">日志记录器名称</param>
        /// <param name="item">日志信息项</param>
        public LogOutputArgs(string logRecorderName, LogItem item)
            : base()
        {
            this.LogRecorderName = logRecorderName;
            this.Item = item;
        }

        /// <summary>
        /// 日志记录器名称
        /// </summary>
        public string LogRecorderName { get; private set; }

        /// <summary>
        /// 日志信息项
        /// </summary>
        public LogItem Item { get; private set; }
    }
}
