using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base.Log.Model;

namespace UtilZ.Lib.Base.Log.LogOutput
{
    /// <summary>
    /// 日志输出中心
    /// </summary>
    public class LogOutputCenter : IDisposable
    {
        #region 单实例
        /// <summary>
        /// 构造函数
        /// </summary>
        private LogOutputCenter()
        {
        }

        /// <summary>
        /// 日志订阅中心实例
        /// </summary>
        private static readonly LogOutputCenter _instance = new LogOutputCenter();

        /// <summary>
        /// 获取日志订阅中心实例
        /// </summary>
        public static LogOutputCenter Instance
        {
            get { return LogOutputCenter._instance; }
        }
        #endregion

        /// <summary>
        /// 日志输出线程
        /// </summary>
        private Thread _logOutputThread = null;

        /// <summary>
        /// 线程取消通知对象
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// 日志输出线程同步对象
        /// </summary>
        private AutoResetEvent _logOutputAutoResetEvent = null;

        /// <summary>
        /// 日志输出队列
        /// </summary>
        private ConcurrentQueue<LogOutputArgs> _logOutputQueue = null;

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
            get { return _enable; }
            set
            {
                if (_enable == value)
                {
                    return;
                }

                lock (this._enableMonitor)
                {
                    if (_enable == value)
                    {
                        return;
                    }

                    _enable = value;
                    if (this._enable)
                    {
                        this.StartOutputThread();
                    }
                    else
                    {
                        this.StopOutputThread();
                    }
                }
            }
        }

        /// <summary>
        /// 启动日志输出线程
        /// </summary>
        private void StartOutputThread()
        {
            this._logOutputAutoResetEvent = new AutoResetEvent(false);
            this._logOutputQueue = new ConcurrentQueue<LogOutputArgs>();
            this._cts = new CancellationTokenSource();
            object obj = new Tuple<AutoResetEvent, ConcurrentQueue<LogOutputArgs>, CancellationTokenSource>(this._logOutputAutoResetEvent, this._logOutputQueue, this._cts);

            this._logOutputThread = new Thread(this.LogOutputThreadMethod);
            this._logOutputThread.IsBackground = true;
            this._logOutputThread.Name = "日志输出中心.日志输出线程";
            this._logOutputThread.Start(obj);
        }

        /// <summary>
        /// 停止日志输出线程
        /// </summary>
        private void StopOutputThread()
        {
            if (this._cts != null)
            {
                this._cts.Cancel();
                this._logOutputAutoResetEvent.Set();

                this._cts = null;
                this._logOutputAutoResetEvent = null;
                this._logOutputThread = null;
                this._logOutputQueue = null;
            }
        }

        /// <summary>
        /// 日志输出线程方法
        /// </summary>
        /// <param name="obj">参数</param>
        private void LogOutputThreadMethod(object obj)
        {
            var para = (Tuple<AutoResetEvent, ConcurrentQueue<LogOutputArgs>, CancellationTokenSource>)obj;
            var token = para.Item3.Token;
            var logOutputQueue = para.Item2;
            var logOutputAutoResetEvent = para.Item1;
            LogOutputArgs item;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (logOutputQueue.Count == 0)
                    {
                        logOutputAutoResetEvent.WaitOne();
                    }

                    if (logOutputQueue.Count == 0)
                    {
                        continue;
                    }

                    if (logOutputQueue.TryDequeue(out item))
                    {
                        this.LogOutput(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }

            para.Item3.Dispose();
            logOutputAutoResetEvent.Dispose();
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
                    this._logOutputQueue.Enqueue(new LogOutputArgs(logRecorderName, logItem));
                    this._logOutputAutoResetEvent.Set();
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
            this.StopOutputThread();

            if (this._logOutputAutoResetEvent != null)
            {
                this._logOutputAutoResetEvent.Dispose();
            }
        }
        #endregion
    }
}
