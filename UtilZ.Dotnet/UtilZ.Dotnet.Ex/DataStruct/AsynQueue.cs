using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.Ex.DataStruct
{
    /// <summary>
    /// 异步队列
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class AsynQueue<T> : IDisposable
    {
        /// <summary>
        /// 异步队列线程
        /// </summary>
        private Thread _thread = null;

        /// <summary>
        /// 线程操作监视锁
        /// </summary>
        private readonly object _threadMonitor = new object();

        /// <summary>
        /// 线程取消通知对象
        /// </summary>
        private CancellationTokenSource _cts = null;

        /// <summary>
        /// 是否是后台线程[true:后台线程，false:前台线程]
        /// </summary>
        private readonly bool _isBackground = true;

        /// <summary>
        /// BlockingCollection
        /// </summary>
        private readonly BlockingCollection<T> _blockingCollection;

        /// <summary>
        /// 停止线程消息通知
        /// </summary>
        private readonly AutoResetEvent _stopAutoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 操作外部线程锁
        /// </summary>
        private readonly object _blockingCollectionMonitor = new object();

        /// <summary>
        /// 异步队列线程名称
        /// </summary>
        private string _threadName = "异步队列线程";
        /// <summary>
        /// 异步队列线程名称
        /// </summary>
        public string ThreadName
        {
            get { return _threadName; }
        }

        /// <summary>
        /// 队列线程状态[true:线程正在运行;false:线程未运行]
        /// </summary>
        private bool _status = false;
        /// <summary>
        /// 获取队列线程状态[true:线程正在运行;false:线程未运行]
        /// </summary>
        public bool Status
        {
            get { return _status; }
        }

        /// <summary>
        /// 获取队列容量[如果设置的容量小于当前已有队列长度,则丢弃掉队列头的项.直到队列长度与目标容量一致]
        /// </summary>
        public int Capity
        {
            get { return this._blockingCollection.BoundedCapacity; }
        }

        /// <summary>
        /// 数据处理委托
        /// </summary>
        public Action<T> ProcessAction;

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="item">待处理数据项</param>
        private void OnRaiseProcess(T item)
        {
            var handler = this.ProcessAction;
            if (handler != null)
            {
                handler(item);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processAction">数据处理委托</param>
        /// <param name="threadName">异步队列线程名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        /// <param name="isAutoStart">是否自动启动线程</param>
        /// <param name="capcity">队列容量,-1为不限容量,小于</param>
        public AsynQueue(Action<T> processAction, string threadName = null, bool isBackground = true, bool isAutoStart = false, int capcity = -1)
        {
            this.ProcessAction = processAction;
            this._isBackground = isBackground;
            this._threadName = threadName;
            if (capcity == 0 || capcity < -1)
            {
                throw new ArgumentException("capcity");
            }

            if (capcity == -1)
            {
                capcity = int.MaxValue;
            }

            this._blockingCollection = new BlockingCollection<T>(new ConcurrentQueue<T>(), capcity);
            if (isAutoStart)
            {
                this.Start();
            }
        }

        /// <summary>
        /// 启动子类无参数工作线程
        /// </summary>
        public void Start()
        {
            lock (this._threadMonitor)
            {
                if (this._status)
                {
                    return;
                }

                this._cts = new CancellationTokenSource();
                this._thread = new Thread(this.RunThreadQueueProcessMethod);
                this._thread.IsBackground = this._isBackground;
                this._thread.Name = this._threadName;
                this._thread.Start();
                this._status = true;
            }
        }

        /// <summary>
        /// 停止工作线程
        /// </summary>       
        /// <param name="isAbort">是否立即终止处理方法[true:立即终止;false:等待方法执行完成;默认false]</param>
        /// <param name="isSync">是否同步停止[true:同步停止;false:异常停止];注:注意线程死锁,典型场景:刷新UI,在UI上执行同步停止</param>
        /// <param name="synMillisecondsTimeout">同步超时时间,-1表示无限期等待,单位/毫秒[isSycn为true时有效]</param>
        public void Stop(bool isAbort = false, bool isSync = false, int synMillisecondsTimeout = -1)
        {
            lock (this._threadMonitor)
            {
                if (this._status)
                {
                    this._cts.Cancel();
                    if (isAbort)
                    {
                        this._thread.Abort();
                    }

                    if (isSync)
                    {
                        if (!this._stopAutoResetEvent.WaitOne(synMillisecondsTimeout))
                        {
                            if (!isAbort)
                            {
                                this._thread.Abort();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 线程队列处理方法
        /// </summary>
        private void RunThreadQueueProcessMethod()
        {
            CancellationToken token = this._cts.Token;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        foreach (T item in this._blockingCollection.GetConsumingEnumerable(token))
                        {
                            try
                            {
                                this.OnRaiseProcess(item);//数据处理
                            }
                            catch (ThreadAbortException)
                            {
                                return;
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //Loger.Info("OperationCanceledException");
                        break;
                    }
                    catch (ThreadAbortException)
                    {
                        //Loger.Info("ThreadAbortException");
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        break;//已释放
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //millisecondsTimeout 是一个非 -1 的负数，而 - 1 表示无限期超时
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        //基础集合已在此 System.Collections.Concurrent.BlockingCollection`1 实例外部进行了修改
                        break;
                    }
                    catch (ArgumentNullException)
                    {
                        //Loger.Warn("this._blockingCollection.GetConsumingEnumerable(token) ArgumentNullException");
                        continue;
                    }
                }
            }
            catch (ThreadAbortException)
            { }

            lock (this._threadMonitor)
            {
                this._thread = null;
                this._status = false;
            }

            try
            {
                this._stopAutoResetEvent.Set();
            }
            catch (ObjectDisposedException)
            { }
        }

        /// <summary>
        /// 线程队列处理方法
        /// </summary>
        private void RunThreadQueueProcessMethod_bk()
        {
            try
            {
                CancellationToken token = this._cts.Token;
                T item;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (this._blockingCollection.TryTake(out item, 5000, token))
                        {
                            this.OnRaiseProcess(item);//数据处理
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        //已释放
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //millisecondsTimeout 是一个非 -1 的负数，而 - 1 表示无限期超时
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        //基础集合已在此 System.Collections.Concurrent.BlockingCollection`1 实例外部进行了修改
                        break;
                    }
                    catch (ArgumentNullException)
                    {
                        continue;
                    }
                }
            }
            catch (ThreadAbortException)
            { }

            try
            {
                this._stopAutoResetEvent.Set();
            }
            catch (ObjectDisposedException)
            { }
        }

        /// <summary>
        /// 将对象添加到队列的结尾处[如果在指定的时间内可以将 item 添加到集合中，则为 true；否则为 false]
        /// </summary>
        /// <param name="item">待添加的对象</param>
        /// <param name="millisecondsTimeout">等待的毫秒数，或为 System.Threading.Timeout.Infinite (-1)，表示无限期等待</param>
        /// <param name="overflowItems">当队列超出策略为Dequeue时,先进入队列中移除项输出集合,如果为null则不输出</param>
        /// <returns>如果在指定的时间内可以将 item 添加到集合中，则为 true；否则为 false</returns>
        public bool Enqueue(T item, int millisecondsTimeout = System.Threading.Timeout.Infinite, List<T> overflowItems = null)
        {
            return this._blockingCollection.TryAdd(item, millisecondsTimeout);
        }

        /// <summary>
        /// 移除位于开始处的指定个数对象
        /// </summary>
        /// <param name="count">要移除的项数</param>
        public void Remove(int count = 1)
        {
            T result;
            int removeCount = 0;
            lock (this._blockingCollectionMonitor)
            {
                while (removeCount < count && this._blockingCollection.Count > 0)
                {
                    //移除一项
                    if (this._blockingCollection.TryTake(out result))
                    {
                        removeCount++;
                    }
                }
            }
        }

        /// <summary>
        /// 获取队列中包含的元素数
        /// </summary>
        public int Count
        {
            get
            {
                return this._blockingCollection.Count;
            }
        }

        /// <summary>
        /// 将队列中存储的元素复制到新数组中
        /// </summary>
        /// <returns>新数组</returns>
        public T[] ToArray()
        {
            return this._blockingCollection.ToArray();
        }

        /// <summary>
        /// 从指定数组索引开始将 System.Collections.Concurrent.ConcurrentQueue`1 元素复制到现有一维 System.Array中
        /// 异常:
        /// T:System.ArgumentNullException:array 为 null 引用（在 Visual Basic 中为 Nothing）。
        /// T:System.ArgumentOutOfRangeException:index 小于零。
        /// T:System.ArgumentException:index 等于或大于该长度的 array -源中的元素数目 System.Collections.Concurrent.ConcurrentQueue`1大于从的可用空间 index 目标从头到尾 array。
        /// </summary>
        /// <param name="array">一维 System.Array，用作从 System.Collections.Concurrent.ConcurrentQueue`1 所复制的元素的目标数组。System.Array 必须具有从零开始的索引。</param>
        /// <param name="index">array 中从零开始的索引，从此处开始复制</param>
        public void CopyTo(T[] array, int index)
        {
            this._blockingCollection.CopyTo(array, index);
        }

        /// <summary>
        /// 清空队列,必须在停止时执行,否则后果未知
        /// </summary>
        public void Clear()
        {
            T result;
            lock (this._blockingCollectionMonitor)
            {
                while (this._blockingCollection.Count > 0)
                {
                    this._blockingCollection.TryTake(out result);
                }
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
        /// 释放资源方法
        /// </summary>
        /// <param name="isDispose">是否释放标识</param>
        protected virtual void Dispose(bool isDispose)
        {
            this.Stop(false, false, 5000);
            if (this._cts != null)
            {
                this._cts.Dispose();
            }

            this._blockingCollection.Dispose();
            this._stopAutoResetEvent.Dispose();
        }
    }
}
