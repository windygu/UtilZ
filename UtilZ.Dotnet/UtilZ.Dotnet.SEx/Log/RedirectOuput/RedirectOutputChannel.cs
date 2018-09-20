using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilZ.Dotnet.SEx.Log
{
    /// <summary>
    /// 输出日志订阅项
    /// </summary>
    [Serializable]
    public class RedirectOutputChannel
    {
        /// <summary>
        /// 日志输出回调
        /// </summary>
        private readonly Action<RedirectOuputItem> _logOutput;

        /// <summary>
        /// 过滤日志追加器名称,忽略大小写[空或null不作验证,其它值需要有匹配的日志追加器验证]
        /// </summary>
        private readonly string _appenderName = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logOutput">日志输出回调</param>
        /// <param name="appenderName">过滤日志追加器名称,忽略大小写[空或null不作验证,其它值需要有匹配的日志追加器验证]</param>
        public RedirectOutputChannel(Action<RedirectOuputItem> logOutput, string appenderName = null)
        {
            this._logOutput = logOutput;
            this._appenderName = appenderName;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem">要输出的日志项</param>
        internal void OnRaiseLogOutput(RedirectOuputItem logItem)
        {
            try
            {
                var handler = this._logOutput;
                if (handler == null || logItem == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(this._appenderName) ||
                    string.Equals(this._appenderName, logItem.AppenderName, StringComparison.OrdinalIgnoreCase))
                {
                    handler(logItem);
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }
    }
}
