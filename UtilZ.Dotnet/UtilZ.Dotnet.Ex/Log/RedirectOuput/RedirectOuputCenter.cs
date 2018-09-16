using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 重定向输出中心
    /// </summary>
    public sealed class RedirectOuputCenter
    {
        static RedirectOuputCenter()
        {
            _logOutputThread = new Thread(LogOutputThreadMethod);
            _logOutputThread.IsBackground = true;
            _logOutputThread.Name = "日志输出中心.日志输出线程";
            _logOutputThread.Start();
        }

        #region 日志输出线程
        /// <summary>
        /// 日志输出线程
        /// </summary>
        private static readonly Thread _logOutputThread;

        /// <summary>
        /// 线程取消通知对象
        /// </summary>
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 日志输出线程同步对象
        /// </summary>
        private static readonly AutoResetEvent _logOutputAutoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 日志输出队列
        /// </summary>
        private static readonly ConcurrentQueue<RedirectOuputArgs> _logOutputQueue = new ConcurrentQueue<RedirectOuputArgs>();

        /// <summary>
        /// 日志输出线程方法
        /// </summary>
        /// <param name="obj">参数</param>
        private static void LogOutputThreadMethod(object obj)
        {
            var token = _cts.Token;
            RedirectOuputArgs item;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_logOutputQueue.Count == 0)
                    {
                        _logOutputAutoResetEvent.WaitOne();
                    }

                    if (_logOutputQueue.Count == 0)
                    {
                        continue;
                    }

                    if (_logOutputQueue.TryDequeue(out item))
                    {
                        LogOutput(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(null, ex);
                }
            }
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="logItem"></param>
        private static void LogOutput(RedirectOuputArgs logItem)
        {
            RedirectOutputSubscribeItem[] logOutputSubscribeItems;
            lock (_logOutputSubscribeItemsMonitor)
            {
                logOutputSubscribeItems = _logOutputSubscribeItems.ToArray();
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
        private static readonly List<RedirectOutputSubscribeItem> _logOutputSubscribeItems = new List<RedirectOutputSubscribeItem>();

        /// <summary>
        /// 获取日志输出订阅项集合
        /// </summary>
        public static List<RedirectOutputSubscribeItem> LogOutputSubscribeItems
        {
            get
            {
                lock (_logOutputSubscribeItemsMonitor)
                {
                    return _logOutputSubscribeItems.ToList();
                }
            }
        }

        /// <summary>
        /// 日志输出订阅项集合线程锁
        /// </summary>
        private static readonly object _logOutputSubscribeItemsMonitor = new object();

        /// <summary>
        /// 添加日志输出订阅项
        /// </summary>
        /// <param name="item">日志输出订阅项</param>
        public static void Add(RedirectOutputSubscribeItem item)
        {
            if (item == null)
            {
                return;
            }

            lock (_logOutputSubscribeItemsMonitor)
            {
                if (!_logOutputSubscribeItems.Contains(item))
                {
                    _logOutputSubscribeItems.Add(item);
                }
            }
        }

        /// <summary>
        /// 移除日志输出订阅项
        /// </summary>
        /// <param name="item">日志输出订阅项</param>
        public static void Remove(RedirectOutputSubscribeItem item)
        {
            if (item == null)
            {
                return;
            }

            lock (_logOutputSubscribeItemsMonitor)
            {
                if (_logOutputSubscribeItems.Contains(item))
                {
                    _logOutputSubscribeItems.Remove(item);
                }
            }
        }

        /// <summary>
        /// 清空日志输出订阅项
        /// </summary>
        public static void Clear()
        {
            lock (_logOutputSubscribeItemsMonitor)
            {
                _logOutputSubscribeItems.Clear();
            }
        }
        #endregion

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="appenderName">日志追加器名称</param>
        /// <param name="logItem">日志项</param>
        internal static void Output(string appenderName, LogItem logItem)
        {
            _logOutputQueue.Enqueue(new RedirectOuputArgs(appenderName, logItem));
            _logOutputAutoResetEvent.Set();
        }
    }
}
