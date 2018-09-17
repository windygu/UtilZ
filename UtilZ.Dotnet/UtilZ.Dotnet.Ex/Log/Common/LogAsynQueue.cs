using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 日志志属异步队列
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class LogAsynQueue<T> : IDisposable
    {
        /// <summary>
        /// 异步队列线程
        /// </summary>
        private Thread _thread = null;

        /// <summary>
        /// 线程取消通知对象
        /// </summary>
        private CancellationTokenSource _cts = null;

        /// <summary>
        /// Queue
        /// </summary>
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        /// <summary>
        /// 空队列等待线程消息通知
        /// </summary>
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 数据处理委托
        /// </summary>
        private readonly Action<T> _processAction;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processAction">数据处理委托</param>
        /// <param name="threadName">异步队列线程名称</param>
        public LogAsynQueue(Action<T> processAction, string threadName)
        {
            if (processAction == null)
            {
                throw new ArgumentNullException(nameof(processAction), "数据处理回调不能为null");
            }

            this._processAction = processAction;
            this._cts = new CancellationTokenSource();
            this._thread = new Thread(this.LogThreadMethod);
            this._thread.IsBackground = true;
            this._thread.Name = threadName;
            this._thread.Start();
        }

        /// <summary>
        /// 线程方法
        /// </summary>
        private void LogThreadMethod()
        {
            T item;
            var token = this._cts.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (this._queue.Count == 0)
                    {
                        this._thread.IsBackground = true;
                        this._autoResetEvent.WaitOne();
                    }

                    if (this._queue.TryDequeue(out item))
                    {
                        this._processAction(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        /// <summary>
        /// 将对象添加到队列的结尾处
        /// </summary>
        /// <param name="item">待添加的对象</param>
        public void Enqueue(T item)
        {
            this._queue.Enqueue(item);
            if (this._thread.IsBackground)
            {
                this._thread.IsBackground = false;
            }

            this._autoResetEvent.Set();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._cts == null)
            {
                return;
            }

            this._cts.Dispose();
            this._autoResetEvent.Dispose();
            this._cts = null;
        }
    }
}
