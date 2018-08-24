using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.RedirectOuput
{
    /// <summary>
    /// 外部日志输出事件参数
    /// </summary>
    public class RedirectOuputArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appenderName">日志追加器名称</param>
        /// <param name="item">日志信息项</param>
        public RedirectOuputArgs(string appenderName, LogItem item)
            : base()
        {
            this.AppenderName = appenderName;
            this.Item = item;
        }

        /// <summary>
        /// 日志追加器名称
        /// </summary>
        public string AppenderName { get; private set; }

        /// <summary>
        /// 日志信息项
        /// </summary>
        public LogItem Item { get; private set; }
    }
}
