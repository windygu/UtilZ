using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UtilZ.Lib.Base.Log.Model;

namespace UtilZ.Lib.Base.Log.LogOutput
{
    /// <summary>
    /// 输出日志订阅项
    /// </summary>
    [Serializable]
    public class LogOutputSubscribeItem
    {
        /// <summary>
        /// 日志记录器名称过滤表达式
        /// </summary>
        public string LogRecorderNameFilter { get; private set; }

        /// <summary>
        /// 日志内容过滤表达式
        /// </summary>
        public string LogContentFilter { get; private set; }

        /// <summary>
        /// 是否是自定义过滤[true:自定义过滤;false:内部正则表达式过滤]
        /// </summary>
        private bool _isUseCustomFilter;

        /// <summary>
        /// 自定义日志输出过滤对象
        /// </summary>
        private ILogOuputFilter _customOutputFilter;

        /// <summary>
        /// 日志输出事件
        /// </summary>
        public event EventHandler<LogOutputArgs> LogOutput;

        /// <summary>
        /// 触发日志输出事件
        /// </summary>
        /// <param name="para">日志输出项</param>
        private void OnRaiseLogOutput(LogOutputArgs para)
        {
            var handler = this.LogOutput;
            if (handler != null)
            {
                handler(this, para);
            }
        }

        /// <summary>
        /// 日志系统日志输出过滤
        /// </summary>
        /// <param name="logRecorderNameFilter">日志记录器名称过滤表达式[当值为空或null时,不区分日志记录器,只验证过滤表达式是否满足,满足就输出,否则先验证日志记录器是否满足]</param>
        /// <param name="logContentFilter">日志内容过滤表达式</param>
        public LogOutputSubscribeItem(string logRecorderNameFilter, string logContentFilter)
        {
            this._isUseCustomFilter = false;
            this.LogRecorderNameFilter = logRecorderNameFilter;
            this.LogContentFilter = logContentFilter;
        }

        /// <summary>
        /// 自定义日志输出过滤
        /// </summary>
        /// <param name="logRecorderNameFilter">日志记录器名称过滤表达式</param>
        /// <param name="logContentFilter">日志内容过滤表达式</param>
        /// <param name="filter">日志输出过滤器</param>
        public LogOutputSubscribeItem(string logRecorderNameFilter, string logContentFilter, ILogOuputFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this._isUseCustomFilter = true;
            this.LogRecorderNameFilter = logRecorderNameFilter;
            this.LogContentFilter = logContentFilter;
            this._customOutputFilter = filter;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem">要输出的日志项</param>
        public virtual void Logoutput(LogOutputArgs logItem)
        {
            if (logItem == null)
            {
                this.OnRaiseLogOutput(logItem);
                return;
            }

            bool isOutput;
            if (this._isUseCustomFilter)
            {
                isOutput = this._customOutputFilter.Filter(this, logItem);
            }
            else
            {
                var logContent = logItem.Item == null ? string.Empty : logItem.Item.Content;
                var expression = this.LogContentFilter;
                if (string.IsNullOrEmpty(this.LogRecorderNameFilter))
                {
                    if (string.IsNullOrEmpty(expression))
                    {
                        isOutput = true;
                    }
                    else
                    {
                        isOutput = Regex.IsMatch(logContent, expression);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(expression))
                    {
                        isOutput = Regex.IsMatch(logItem.LogRecorderName, this.LogRecorderNameFilter);
                    }
                    else
                    {
                        isOutput = Regex.IsMatch(logItem.LogRecorderName, this.LogRecorderNameFilter) && Regex.IsMatch(logContent, expression);
                    }
                }
            }

            if (isOutput)
            {
                this.OnRaiseLogOutput(logItem);
            }
        }
    }
}
