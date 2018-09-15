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
    public class RedirectOutputSubscribeItem
    {
        /// <summary>
        /// 日志输出事件
        /// </summary>
        public event EventHandler<RedirectOuputArgs> LogOutput;

        /// <summary>
        /// 过滤日志追加器名称[空或null不作验证,其它值需要有匹配的日志追加器验证]
        /// </summary>
        private readonly string _appenderName = null;

        /// <summary>
        /// 日志系统日志输出过滤
        /// </summary>
        /// <param name="appenderName">过滤日志追加器名称[空或null不作验证,其它值需要有匹配的日志追加器验证]</param>
        public RedirectOutputSubscribeItem(string appenderName = null)
        {
            this._appenderName = appenderName;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem">要输出的日志项</param>
        internal void OnRaiseLogOutput(RedirectOuputArgs logItem)
        {
            try
            {
                var handler = this.LogOutput;
                if (handler == null || logItem == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(this._appenderName) ||
                    string.Equals(this._appenderName, logItem.AppenderName))
                {
                    handler(this, logItem);
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }
    }
}
