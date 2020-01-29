using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.DotnetStd.Ex.Log;

namespace UtilZ.DotnetStd.Ex.Base
{
    /// <summary>
    /// 异步队列
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class AsynQueue<T> : IDisposable
    {
        private readonly object _lock = new object();
        private Thread _thread = null;
        private CancellationTokenSource _cts = null;
        private BlockingCollection<T> _queue = null;
        private readonly object _queueLock = new object();
        private bool _running = false;
        public bool Running
        {
            get
            {
                lock (this._lock)
                {
                    return _running;
                }
            }
        }


        /// <summary>
        /// 线程名称
        /// </summary>
        private string _threadName;

        /// <summary>
        /// 是否后台运行[true:后台线程;false:前台线程]
        /// </summary>
        private bool _isBackground;

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

        private readonly int _capity;
        /// <summary>
        /// 获取队列容量[如果设置的容量小于当前已有队列长度,则丢弃掉队列头的项.直到队列长度与目标容量一致]
        /// </summary>
        public int Capity
        {
            get { return this._capity; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processAction">数据处理委托</param>
        /// <param name="threadName">异步队列线程名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        /// <param name="isAutoStart">是否自动启动线程</param>
        /// <param name="capcity">队列容量</param>
        public AsynQueue(Action<T> processAction, string threadName = null, bool isBackground = true, bool isAutoStart = false, int capcity = int.MaxValue)
        {
            this._isBackground = isBackground;
            this._threadName = threadName;
            if (capcity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capcity));
            }

            this._capity = capcity;
            this.ProcessAction = processAction;

            if (isAutoStart)
            {
                this.Start();
            }
        }



        public void Start()
        {
            lock (this._lock)
            {
                if (this._running)
                {
                    return;
                }

                this._queue = this.CreateQueue();
                this.StartProcessThread();
                this._running = true;
            }
        }

        private BlockingCollection<T> CreateQueue()
        {
            return new BlockingCollection<T>(new ConcurrentQueue<T>(), this._capity);
        }

        private void StartProcessThread()
        {
            this._cts = new CancellationTokenSource();
            this._thread = new Thread(new ParameterizedThreadStart(this.RunThreadQueueProcessMethod));
            this._thread.Name = this._threadName;
            this._thread.IsBackground = this._isBackground;
            this._thread.Start(this._cts.Token);
        }

        private void StopProcessThread()
        {
            this._cts.Cancel();
            this._cts.Dispose();
            this._cts = null;
            this._thread = null;
        }

        /// <summary>
        /// 线程执行方法
        /// </summary>
        /// <param name="obj">线程参数</param>
        private void RunThreadQueueProcessMethod(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            T item;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        item = this._queue.Take(token);
                        //数据处理
                        this.OnRaiseProcess(item);
                    }
                    catch (ArgumentNullException)
                    {
                        continue;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch (Exception exi)
                    {
                        Loger.Error(exi);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        public void Stop()
        {
            lock (this._lock)
            {
                if (!this._running)
                {
                    return;
                }

                this.StopProcessThread();
                this._queue.Dispose();
                this._queue = null;
                this._running = false;
            }
        }

        /// <summary>
        /// 将对象添加到队列的结尾处,如果队列已达到上限，则一直阻塞到有项被消费
        /// </summary>
        /// <param name="item">待添加的对象</param>
        public void Enqueue(T item)
        {
            try
            {
                if (!this._running)
                {
                    return;
                }

                lock (this._queueLock)
                {
                    this._queue.Add(item, this._cts.Token);
                }
            }
            catch (ArgumentNullException)
            { }
            catch (OperationCanceledException)
            { }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        /// <summary>
        /// 将对象添加到队列的结尾处,如果队列已达到上限添加超时，则会移除起始项，直到添加成功
        /// </summary>
        /// <param name="item">待添加的对象</param>
        /// <param name="millisecondsTimeout">添加毫秒超时，默认为100毫秒</param>
        public void Enqueue(T item, int millisecondsTimeout = 100)
        {
            if (!this._running)
            {
                return;
            }

            lock (this._queueLock)
            {
                try
                {
                    while (!this._queue.TryAdd(item, millisecondsTimeout, this._cts.Token))
                    {
                        try
                        {
                            this._queue.Take();
                        }
                        catch (ArgumentNullException)
                        {
                            continue;
                        }
                    }
                }
                catch (ArgumentNullException)
                { }
                catch (OperationCanceledException)
                { }
                catch (ObjectDisposedException)
                { }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        /// <summary>
        /// 移除位于开始处的指定个数对象,未运行返回null
        /// </summary>
        /// <param name="count">要移除的项数</param>
        /// <param name="millisecondsTimeout">添加毫秒超时，默认为100毫秒</param>
        /// <returns>移除项集合</returns>
        public List<T> Remove(int count, int millisecondsTimeout = 100)
        {
            if (!_running)
            {
                return null;
            }

            lock (this._queueLock)
            {
                var items = new List<T>();
                T item;
                while (items.Count < count)
                {
                    try
                    {
                        if (this._queue == null)
                        {
                            break;
                        }

                        if (this._queue.TryTake(out item, millisecondsTimeout, this._cts.Token))
                        {
                            items.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        //不晓得新平台这个bug修复了没有,TryTake调用TryTake方法偶尔会抛出ArgumentNullException异常
                        continue;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// 移除满足条件的元素,未运行返回null
        /// </summary>
        /// <param name="predicate">用于定义要移除的元素应满足的条件</param>
        /// <returns>移除项集合</returns>
        public T[] Remove(Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            lock (this._lock)
            {
                if (!_running)
                {
                    return null;
                }

                lock (this._queueLock)
                {
                    this.StopProcessThread();
                    T[] array = this._queue.ToArray();
                    var removeItems = array.Where(predicate).ToArray();
                    if (removeItems.Count() > 0)
                    {
                        this._queue.Dispose();
                        this._queue = this.CreateQueue();
                        foreach (var item in array)
                        {
                            if (removeItems.Contains(item))
                            {
                                continue;
                            }
                            else
                            {
                                this._queue.Add(item);
                            }
                        }
                    }

                    this.StartProcessThread();
                    return removeItems;
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
                lock (this._lock)
                {
                    if (!_running)
                    {
                        return 0;
                    }

                    return this._queue.Count;
                }
            }
        }

        /// <summary>
        /// 将队列中存储的元素复制到新数组中,未运行返回null
        /// </summary>
        /// <returns>新数组</returns>
        public T[] ToArray()
        {
            lock (this._lock)
            {
                if (!_running)
                {
                    return null;
                }

                return this._queue.ToArray();
            }
        }

        /// <summary>
        /// 清空队列,必须在停止时执行,否则后果未知
        /// </summary>
        public void Clear()
        {
            lock (this._lock)
            {
                if (!_running || this._queue.Count == 0)
                {
                    return;
                }

                //方法一,停止处理线程，重新创建队列，重启处理线程
                this.StopProcessThread();
                this._queue.Dispose();
                this._queue = this.CreateQueue();
                this.StartProcessThread();

                //方法二,将队列中的项全部移除
                //const int millisecondsTimeout = 1;
                //T item;
                //while (this._queue.TryTake(out item, millisecondsTimeout, this._cts.Token))
                //{

                //}
            }
        }


        private bool _disposed = false;
        public void Dispose()
        {
            try
            {
                lock (this)
                {
                    if (this._disposed)
                    {
                        return;
                    }
                    this._disposed = true;

                    this.Stop();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
