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
        /// 批量出队列线程消息通知
        /// </summary>
        private readonly AutoResetEvent _batchAutoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 操作外部线程锁
        /// </summary>
        private readonly object _blockingCollectionMonitor = new object();

        /// <summary>
        /// 对象是否已释放[true:已释放;false:未释放]
        /// </summary>
        private bool _isDisposed = false;

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
        /// 是否每次抛出多项
        /// </summary>
        private readonly bool _isDequeueMuiltItem;

        /// <summary>
        /// 批量处理最大项数
        /// </summary>
        private readonly int _batchCount = 0;

        /// <summary>
        /// 数据处理委托
        /// </summary>
        public Action<T> ProcessAction;

        /// <summary>
        /// 数据处理委托
        /// </summary>
        public Action<List<T>> ProcessAction2;

        private void OnRaiseProcessAction2(List<T> items)
        {
            var handler = this.ProcessAction2;
            if (handler != null)
            {
                handler(items);//数据处理
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isDequeueMuiltItem">是否每次抛出多项</param>
        /// <param name="threadName">异步队列线程名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        /// <param name="isAutoStart">是否自动启动线程</param>
        /// <param name="capcity">队列容量,-1为不限容量,小于</param>
        private AsynQueue(bool isDequeueMuiltItem, string threadName = null, bool isBackground = true, bool isAutoStart = false, int capcity = -1)
        {
            this._isDequeueMuiltItem = isDequeueMuiltItem;
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
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processAction">数据处理委托</param>
        /// <param name="threadName">异步队列线程名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        /// <param name="isAutoStart">是否自动启动线程</param>
        /// <param name="capcity">队列容量,-1为不限容量,小于</param>
        public AsynQueue(Action<T> processAction, string threadName = null, bool isBackground = true, bool isAutoStart = false, int capcity = -1) :
            this(false, threadName, isBackground, isAutoStart, capcity)
        {
            this.ProcessAction = processAction;
            if (isAutoStart)
            {
                this.Start();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processAction">数据处理委托</param>
        /// <param name="batchCount">批量处理最大项数</param>
        /// <param name="threadName">异步队列线程名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        /// <param name="isAutoStart">是否自动启动线程</param>
        /// <param name="capcity">队列容量,-1为不限容量,小于</param>
        public AsynQueue(Action<List<T>> processAction, int batchCount = 10, string threadName = null, bool isBackground = true, bool isAutoStart = false, int capcity = -1) :
            this(true, threadName, isBackground, isAutoStart, capcity)
        {
            if (batchCount < 1)
            {
                throw new ArgumentException(string.Format("批量处理最大项数不能小于1,值:{0}无效", batchCount));
            }

            this.ProcessAction2 = processAction;
            this._batchCount = batchCount;
            if (isAutoStart)
            {
                this.Start();
            }
        }

        /// <summary>
        /// 启动子类无参数工作线程
        /// </summary>
        /// <param name="apartmentState">指定的单元状态 System.Threading.Thread</param>
        public void Start(ApartmentState apartmentState = ApartmentState.Unknown)
        {
            lock (this._threadMonitor)
            {
                if (this._isDisposed)
                {
                    throw new ObjectDisposedException(string.Empty, "对象已释放");
                }

                if (this._status)
                {
                    return;
                }

                this._cts = new CancellationTokenSource();
                if (this._isDequeueMuiltItem)
                {
                    this._thread = new Thread(this.RunThreadQueueMuiltProcessMethod);
                }
                else
                {
                    this._thread = new Thread(this.RunThreadQueueSingleProcessMethod);
                }

                this._thread.IsBackground = this._isBackground;
                this._thread.SetApartmentState(apartmentState);
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
                this.PrimitiveStop(isAbort, isSync, synMillisecondsTimeout);
            }
        }

        /// <summary>
        /// 停止工作线程
        /// </summary>       
        /// <param name="isAbort">是否立即终止处理方法[true:立即终止;false:等待方法执行完成;默认false]</param>
        /// <param name="isSync">是否同步停止[true:同步停止;false:异常停止];注:注意线程死锁,典型场景:刷新UI,在UI上执行同步停止</param>
        /// <param name="synMillisecondsTimeout">同步超时时间,-1表示无限期等待,单位/毫秒[isSycn为true时有效]</param>
        private void PrimitiveStop(bool isAbort, bool isSync, int synMillisecondsTimeout)
        {
            if (this._isDisposed)
            {
                return;
            }

            if (!this._status)
            {
                return;
            }

            this._cts.Cancel();
            if (this._isDequeueMuiltItem)
            {
                this._batchAutoResetEvent.Set();
            }

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

        /// <summary>
        /// 线程队列处理方法
        /// </summary>
        private void RunThreadQueueSingleProcessMethod()
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
                                var handler = this.ProcessAction;
                                if (handler != null)
                                {
                                    handler(item);//数据处理
                                }
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

            this.ThreadRunFinish();
        }

        /// <summary>
        /// 线程队列处理方法
        /// </summary>
        private void RunThreadQueueMuiltProcessMethod()
        {
            try
            {
                CancellationToken token = this._cts.Token;
                T item;
                List<T> items = new List<T>();
                List<T> outItems;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (this._blockingCollection.TryTake(out item, 1, token))
                        {
                            items.Add(item);
                            if (items.Count >= this._batchCount)
                            {
                                outItems = items.ToList();
                                items.Clear();
                                this.OnRaiseProcessAction2(outItems);//数据处理                                
                            }
                        }
                        else
                        {
                            if (items.Count > 0)
                            {
                                outItems = items.ToList();
                                items.Clear();
                                this.OnRaiseProcessAction2(outItems);//数据处理  
                            }

                            //等待
                            this._batchAutoResetEvent.WaitOne();
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

            this.ThreadRunFinish();
        }

        private void ThreadRunFinish()
        {
            lock (this._threadMonitor)
            {
                this._thread = null;
                this._status = false;
                try
                {
                    if (!this._isDisposed)
                    {
                        this._stopAutoResetEvent.Set();
                    }
                }
                catch (ObjectDisposedException)
                { }
            }
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
            bool ret = this._blockingCollection.TryAdd(item, millisecondsTimeout);
            if (ret && this._isDequeueMuiltItem)
            {
                this._batchAutoResetEvent.Set();
            }

            return ret;
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
            lock (this._threadMonitor)
            {
                if (this._isDisposed)
                {
                    return;
                }


                this.PrimitiveStop(false, false, 5000);
                if (this._cts != null)
                {
                    this._cts.Dispose();
                }

                this._blockingCollection.Dispose();
                this._stopAutoResetEvent.Dispose();
                this._batchAutoResetEvent.Dispose();
                this._isDisposed = true;
            }
        }
    }
}
