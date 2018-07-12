using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.LogOutput
{
    /// <summary>
    /// 日志输出中心
    /// </summary>
    public class LogOutputCenter : IDisposable
    {
        #region 单实例
        static LogOutputCenter()
        {
            _instance = new LogOutputCenter();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private LogOutputCenter()
        {
        }

        /// <summary>
        /// 日志订阅中心实例
        /// </summary>
        private static readonly LogOutputCenter _instance = null;

        /// <summary>
        /// 获取日志订阅中心实例
        /// </summary>
        public static LogOutputCenter Instance
        {
            get { return _instance; }
        }
        #endregion

        /// <summary>
        /// 日志输出对象
        /// </summary>
        private LogOutputObject _logOutputObject = null;

        /// <summary>
        /// 是否启用日志输出线程锁
        /// </summary>
        private readonly object _enableMonitor = new object();

        /// <summary>
        /// 是否启用日志输出[true:启用输出;false:禁用输出]
        /// </summary>
        private bool _enable = false;

        /// <summary>
        /// 获取或设置是否启用日志输出[true:启用输出;false:禁用输出]
        /// </summary>
        public bool Enable
        {
            get { return this._enable; }
            set
            {
                lock (this._enableMonitor)
                {
                    if (this._enable == value)
                    {
                        return;
                    }

                    this._enable = value;
                    if (this._enable)
                    {
                        this._logOutputObject = new LogOutputObject(this.LogOutput);
                    }
                    else
                    {
                        if (this._logOutputObject != null)
                        {
                            this._logOutputObject.Dispose();
                            this._logOutputObject = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem"></param>
        private void LogOutput(LogOutputArgs logItem)
        {
            List<LogOutputSubscribeItem> logOutputSubscribeItems;
            lock (this._logOutputSubscribeItemsMonitor)
            {
                logOutputSubscribeItems = this._logOutputSubscribeItems.ToList();
            }

            foreach (var logOutputSubscribeItem in logOutputSubscribeItems)
            {
                logOutputSubscribeItem.Logoutput(logItem);
            }
        }

        /// <summary>
        /// 日志输出订阅项集合
        /// </summary>
        private readonly List<LogOutputSubscribeItem> _logOutputSubscribeItems = new List<LogOutputSubscribeItem>();

        /// <summary>
        /// 获取日志输出订阅项集合
        /// </summary>
        public List<LogOutputSubscribeItem> LogOutputSubscribeItems
        {
            get
            {
                lock (this._logOutputSubscribeItemsMonitor)
                {
                    return _logOutputSubscribeItems.ToList();
                }
            }
        }

        /// <summary>
        /// 日志输出订阅项集合线程锁
        /// </summary>
        private readonly object _logOutputSubscribeItemsMonitor = new object();

        /// <summary>
        /// 添加日志输出订阅项
        /// </summary>
        /// <param name="item">日志输出订阅项</param>
        public void AddLogOutput(LogOutputSubscribeItem item)
        {
            if (item == null)
            {
                return;
            }

            lock (this._logOutputSubscribeItemsMonitor)
            {
                if (!this._logOutputSubscribeItems.Contains(item))
                {
                    this._logOutputSubscribeItems.Add(item);
                }
            }
        }

        /// <summary>
        /// 移除日志输出订阅项
        /// </summary>
        /// <param name="item">日志输出订阅项</param>
        public void RemoveLogOutput(LogOutputSubscribeItem item)
        {
            if (item == null)
            {
                return;
            }

            lock (this._logOutputSubscribeItemsMonitor)
            {
                if (this._logOutputSubscribeItems.Contains(item))
                {
                    this._logOutputSubscribeItems.Remove(item);
                }
            }
        }

        /// <summary>
        /// 清空日志输出
        /// </summary>
        public void ClearOutputLog()
        {
            lock (this._logOutputSubscribeItemsMonitor)
            {
                this._logOutputSubscribeItems.Clear();
            }
        }

        /// <summary>
        /// 添加输出日志
        /// </summary>
        /// <param name="logRecorderName">日志记录器名称</param>
        /// <param name="logItem">日志项</param>
        public void AddOutputLog(string logRecorderName, LogItem logItem)
        {
            lock (this._enableMonitor)
            {
                if (this._enable)
                {
                    this._logOutputObject.AddOutputLog(logRecorderName, logItem);
                }
            }
        }

        #region IDisposable
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
            if (this._logOutputObject != null)
            {
                this._logOutputObject.Dispose();
                this._logOutputObject = null;
            }
        }
        #endregion
    }
}
