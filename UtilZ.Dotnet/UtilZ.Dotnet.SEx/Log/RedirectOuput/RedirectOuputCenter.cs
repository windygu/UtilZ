using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.RedirectOuput
{
    /// <summary>
    /// 重定向输出中心
    /// </summary>
    public sealed class RedirectOuputCenter : IDisposable
    {
        #region 单实例
        static RedirectOuputCenter()
        {
            _instance = new RedirectOuputCenter();
        }

        /// <summary>
        /// 日志订阅中心实例
        /// </summary>
        private static readonly RedirectOuputCenter _instance = null;

        /// <summary>
        /// 获取日志订阅中心实例
        /// </summary>
        public static RedirectOuputCenter Instance
        {
            get { return _instance; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        private RedirectOuputCenter()
        {
            this._logOutputThread = new Thread(this.LogOutputThreadMethod);
            this._logOutputThread.IsBackground = true;
            this._logOutputThread.Name = "日志输出中心.日志输出线程";
            this._logOutputThread.Start();
        }

        #region 日志输出线程
        /// <summary>
        /// 日志输出线程
        /// </summary>
        private readonly Thread _logOutputThread;

        /// <summary>
        /// 线程取消通知对象
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 日志输出线程同步对象
        /// </summary>
        private readonly AutoResetEvent _logOutputAutoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 日志输出队列
        /// </summary>
        private readonly ConcurrentQueue<RedirectOuputArgs> _logOutputQueue = new ConcurrentQueue<RedirectOuputArgs>();

        /// <summary>
        /// 日志输出线程方法
        /// </summary>
        /// <param name="obj">参数</param>
        private void LogOutputThreadMethod(object obj)
        {
            var token = this._cts.Token;
            RedirectOuputArgs item;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (this._logOutputQueue.Count == 0)
                    {
                        this._logOutputAutoResetEvent.WaitOne();
                    }

                    if (this._logOutputQueue.Count == 0)
                    {
                        continue;
                    }

                    if (this._logOutputQueue.TryDequeue(out item))
                    {
                        this.LogOutput(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem"></param>
        private void LogOutput(RedirectOuputArgs logItem)
        {
            List<RedirectOutputSubscribeItem> logOutputSubscribeItems;
            lock (this._logOutputSubscribeItemsMonitor)
            {
                logOutputSubscribeItems = this._logOutputSubscribeItems.ToList();
            }

            foreach (var logOutputSubscribeItem in logOutputSubscribeItems)
            {
                logOutputSubscribeItem.OnRaiseLogOutput(logItem);
            }
        }
        #endregion

        #region 日志输出订阅
        /// <summary>
        /// 日志输出订阅项集合
        /// </summary>
        private readonly List<RedirectOutputSubscribeItem> _logOutputSubscribeItems = new List<RedirectOutputSubscribeItem>();

        /// <summary>
        /// 获取日志输出订阅项集合
        /// </summary>
        public List<RedirectOutputSubscribeItem> LogOutputSubscribeItems
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
        public void AddLogOutput(RedirectOutputSubscribeItem item)
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
        public void RemoveLogOutput(RedirectOutputSubscribeItem item)
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
        /// 清空日志输出订阅项
        /// </summary>
        public void ClearOutputLog()
        {
            lock (this._logOutputSubscribeItemsMonitor)
            {
                this._logOutputSubscribeItems.Clear();
            }
        }
        #endregion

        /// <summary>
        /// 添加输出日志
        /// </summary>
        /// <param name="appenderName">日志追加器名称</param>
        /// <param name="logItem">日志项</param>
        internal void AddOutputLog(string appenderName, LogItem logItem)
        {
            this._logOutputQueue.Enqueue(new RedirectOuputArgs(appenderName, logItem));
            this._logOutputAutoResetEvent.Set();
        }

        #region IDisposable
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this._cts.Cancel();
            this._logOutputAutoResetEvent.Set();

            this._cts.Dispose();
            this._logOutputAutoResetEvent.Dispose();
        }
        #endregion
    }
}
