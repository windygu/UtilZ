using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.LogRecorder
{
    /// <summary>
    /// 基础日志记录器
    /// </summary>
    public abstract class BaseLogRecorder : BaseLoger, ILogRecorder, IDisposable
    {
        /// <summary>
        /// 日志追加器
        /// </summary>
        public ILogRecorder LogAppender { get; set; }

        /// <summary>
        /// 过滤器验证是否通过[true:通过;false:不通过]
        /// </summary>
        /// <param name="level">待验证级别</param>
        /// <returns>true:通过;false:不通过</returns>
        public bool FilterValidate(Model.LogLevel level)
        {
            return level >= this.BaseConfig.FilterFrom && level <= this.BaseConfig.FilterTo;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public abstract void WriteLog(Model.LogItem item);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        public abstract void WriteLog(List<Model.LogItem> items);

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="item">日志项</param>
        protected void AppenderLog(LogItem item)
        {
            if (item != null)
            {
                this.AppenderLog(new List<LogItem> { item });
            }
        }

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        protected void AppenderLog(List<Model.LogItem> items)
        {
            try
            {
                if (this.LogAppender != null)
                {
                    this.LogAppender.WriteLog(items);
                }
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this, exi);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDispose">是否释放资源</param>
        protected virtual void Dispose(bool isDispose)
        {

        }
    }
}
