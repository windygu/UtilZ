using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.ILEx.LogExtend
{
    /// <summary>
    /// 内部日志添加到ILoger记录事件参数
    /// </summary>
    public class Log4InnerLogAddedILogerArgs : EventArgs
    {
        /// <summary>
        /// 日志项
        /// </summary>
        public LogItem Item { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="item">日志项</param>
        public Log4InnerLogAddedILogerArgs(LogItem item)
        {
            this.Item = item;
        }
    }
}
