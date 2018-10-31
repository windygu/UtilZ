using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log.Appender
{
    /// <summary>
    /// 日志追加器基类
    /// </summary>
    public abstract class AppenderBase : IDisposable
    {
        /// <summary>
        /// 当前日志追加器状态是否可用[true:可用;false:不可用]
        /// </summary>
        protected readonly bool _status = true;

        private string _name = null;
        /// <summary>
        /// 日志追加器名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        protected readonly BaseConfig _config;

        /// <summary>
        /// 日志写线程队列
        /// </summary>
        private LogAsynQueue<LogItem> _logWriteQueue = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public AppenderBase(XElement ele)
        {
            this._config = this.CreateConfig(ele);
            this._status = true;
            if (this._config != null && this._config.EnableOutputCache)
            {
                this._logWriteQueue = new LogAsynQueue<LogItem>(this.PrimitiveWriteLog, string.Format("{0}日志输出线程", this._config.Name));
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置对象</param>
        public AppenderBase(BaseConfig config)
        {
            if (config == null)
            {
                config = this.CreateConfig(null);
            }

            this._config = config;
            this._status = true;
            if (this._config != null && this._config.EnableOutputCache)
            {
                this._logWriteQueue = new LogAsynQueue<LogItem>(this.PrimitiveWriteLog, string.Format("{0}日志输出线程", this._config.Name));
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        protected abstract void PrimitiveWriteLog(LogItem item);

        /// <summary>
        /// 创建配置对象实例
        /// </summary>
        /// <param name="ele">配置元素</param>
        /// <returns>配置对象实例</returns>
        protected abstract BaseConfig CreateConfig(XElement ele);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public void WriteLog(LogItem item)
        {
            if (this._config.EnableOutputCache)
            {
                this._logWriteQueue.Enqueue(item);
            }
            else
            {
                this.PrimitiveWriteLog(item);
            }
        }

        /// <summary>
        /// 验证日志是否允许输出[返回值true:允许输出;false:丢弃]
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="item">要验证输出的日志项</param>
        /// <returns>true:允许输出;false:丢弃</returns>
        protected virtual bool Validate(BaseConfig config, LogItem item)
        {
            if (item == null)
            {
                return false;
            }

            if (!config.Enable)
            {
                return false;
            }

            if (config.Levels != null && !config.Levels.Contains(item.Level))
            {
                return false;
            }

            if (config.EventIdMin != LogConstant.DefaultEventId && item.EventID < config.EventIdMin)
            {
                return false;
            }

            if (config.EventIdMax != LogConstant.DefaultEventId && item.EventID > config.EventIdMax)
            {
                return false;
            }

            var matchString = config.MatchString;
            if (!string.IsNullOrEmpty(matchString) &&
                !string.IsNullOrEmpty(item.Format) &&
                !Regex.IsMatch(item.Format, matchString))
            {
                return false;
            }

            var matchExceptionType = config.MatchExceptionType;
            if (matchExceptionType != null && item.Exception != null)
            {
                Type exType = item.Exception.GetType();
                if (exType != matchExceptionType && !exType.IsSubclassOf(matchExceptionType))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._name == null ? base.ToString() : this._name;
        }

        /// <summary>
        /// 构造函数释放非托管资源
        /// </summary>
        ~AppenderBase()
        {
            this.Dispose(false);
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
        /// <param name="disposing">释放资源标识</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._logWriteQueue != null)
            {
                this._logWriteQueue.Dispose();
            }
        }
    }
}
