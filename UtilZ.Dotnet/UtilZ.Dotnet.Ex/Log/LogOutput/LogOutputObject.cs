using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.LogOutput
{
    /// <summary>
    /// 日志输出对象
    /// </summary>
    internal class LogOutputObject : IDisposable
    {
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
        private readonly ConcurrentQueue<LogOutputArgs> _logOutputQueue = new ConcurrentQueue<LogOutputArgs>();

        /// <summary>
        /// 日志输出委托
        /// </summary>
        private readonly Action<LogOutputArgs> _logOutput;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logOutput">日志输出委托</param>
        public LogOutputObject(Action<LogOutputArgs> logOutput)
        {
            this._logOutput = logOutput;
            this._logOutputThread = new Thread(this.LogOutputThreadMethod);
            this._logOutputThread.IsBackground = true;
            this._logOutputThread.Name = "日志输出中心.日志输出线程";
            this._logOutputThread.Start();
        }

        /// <summary>
        /// 日志输出线程方法
        /// </summary>
        /// <param name="obj">参数</param>
        private void LogOutputThreadMethod(object obj)
        {
            var token = this._cts.Token;
            LogOutputArgs item;
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
                        this._logOutput(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        /// <summary>
        /// 添加输出日志
        /// </summary>
        /// <param name="logRecorderName">日志记录器名称</param>
        /// <param name="logItem">日志项</param>
        public void AddOutputLog(string logRecorderName, LogItem logItem)
        {
            this._logOutputQueue.Enqueue(new LogOutputArgs(logRecorderName, logItem));
            this._logOutputAutoResetEvent.Set();
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
            this._cts.Cancel();
            this._logOutputAutoResetEvent.Set();

            this._cts.Dispose();
            this._logOutputAutoResetEvent.Dispose();
        }
        #endregion
    }
}
