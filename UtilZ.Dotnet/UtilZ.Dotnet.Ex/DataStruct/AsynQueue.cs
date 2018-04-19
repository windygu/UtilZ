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
    public class AsynQueue<T> : IEnumerable<T>, IDisposable
    {
        /// <summary>
        /// 异步队列线程名称
        /// </summary>
        private string _threadName = "异步队列线程";

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
        /// 需要处理的数据队列
        /// </summary>
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        /// <summary>
        /// 输出消息到UI的线程消息通知
        /// </summary>
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 停止线程消息通知
        /// </summary>
        private readonly AutoResetEvent _stopAutoResetEvent = new AutoResetEvent(false);

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

        private string _name = string.Empty;
        /// <summary>
        /// 获取或设置队列名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.Equals(_name, value))
                {
                    return;
                }

                _name = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    this._threadName = string.Empty;
                }
                else
                {
                    this._threadName = value + "线程";
                }
            }
        }

        /// <summary>
        /// 数据处理委托
        /// </summary>
        public Action<T> ProcessAction;

        /// <summary>
        /// 同步操作对象
        /// </summary>
        public readonly object SyncRoot = new object();

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
        /// <param name="name">异步队列名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        /// <param name="isAutoStart">是否自动启动线程</param>
        public AsynQueue(Action<T> processAction, string name = null, bool isBackground = true, bool isAutoStart = false)
        {
            this.ProcessAction = processAction;
            this._isBackground = isBackground;
            this.Name = name;

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
        /// <param name="isSync">是否同步停止,isAbort为false时有效[true:同步停止;false:异常停止]</param>
        /// <param name="synMillisecondsTimeout">同步超时时间,-1表示无限期等待,单位/毫秒[isSycn为true时有效]</param>
        public void Stop(bool isAbort = false, bool isSync = true, int synMillisecondsTimeout = -1)
        {
            lock (this._threadMonitor)
            {
                if (this._status)
                {
                    this._cts.Cancel();
                    this._autoResetEvent.Set();
                    if (isAbort)
                    {
                        this._thread.Abort();
                    }
                    else
                    {
                        if (isSync)
                        {
                            this._stopAutoResetEvent.WaitOne(synMillisecondsTimeout);
                        }
                    }

                    this._thread = null;
                    this._status = false;
                }
            }
        }

        /// <summary>
        /// 线程队列处理方法
        /// </summary>
        private void RunThreadQueueProcessMethod()
        {
            try
            {
                CancellationToken token = this._cts.Token;
                T item;
                while (!token.IsCancellationRequested)
                {
                    if (this._queue.Count == 0)
                    {
                        this._autoResetEvent.WaitOne(5000);
                    }
                    else
                    {
                        lock (this.SyncRoot)
                        {
                            if (this._queue.TryDequeue(out item))
                            {
                                //数据处理
                                this.OnRaiseProcess(item);
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            { }

            this._stopAutoResetEvent.Set();
        }

        /// <summary>
        /// 将对象添加到队列的结尾处
        /// </summary>
        /// <param name="item">待添加的对象</param>
        public void Enqueue(T item)
        {
            this._queue.Enqueue(item);
            if (this._queue.Count > this._maxcount)
            {
                //超过线程队列最大项数,则移除超出队列开始的项
                T value = default(T);
                while (this._queue.Count > this._maxcount)
                {
                    this._queue.TryDequeue(out value);
                }
            }

            this._autoResetEvent.Set();
        }

        /// <summary>
        /// 移除位于开始处的指定个数对象
        /// </summary>
        /// <param name="count">要移除的项数</param>
        public void Remove(int count)
        {
            T result;
            int removeCount = 0;
            while (removeCount < count)
            {
                //如果为空了就跳出
                if (this._queue.Count == 0)
                {
                    break;
                }

                //移除一项
                if (this._queue.TryDequeue(out result))
                {
                    removeCount++;
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
                return this._queue.Count;
            }
        }

        /// <summary>
        /// 线程队列最大项数,当超过这个值时,则移除超出的值
        /// </summary>
        private int _maxcount = int.MaxValue;

        /// <summary>
        /// 获取或设置线程队列最大项数,当超过这个值时,则移除超出队列开始的项
        /// </summary>
        public int Maxcount
        {
            get { return _maxcount; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(string.Format("线程队列最大项数不能小于1,值{0}无效", value), "value");
                }

                _maxcount = value;
            }
        }

        /// <summary>
        /// 异步队列线程名称
        /// </summary>
        public string ThreadName
        {
            get { return _threadName; }
        }

        /// <summary>
        /// 将队列中存储的元素复制到新数组中
        /// </summary>
        /// <returns>新数组</returns>
        public T[] ToArray()
        {
            return this._queue.ToArray();
        }

        /// <summary>
        /// 清空队列,必须在停止时执行,否则后果未知
        /// </summary>
        public void Clear()
        {
            this.Remove(this._queue.Count);
        }

        /// <summary>
        /// 返回循环的遍历枚举
        /// </summary>
        /// <returns>循环的遍历枚举</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._queue.GetEnumerator();
        }

        /// <summary>
        /// 返回循环的遍历枚举
        /// </summary>
        /// <returns>循环的遍历枚举</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
            this.Stop(true, true, 5000);
            if (this._cts != null)
            {
                this._cts.Dispose();
            }

            this._autoResetEvent.Dispose();
            this._stopAutoResetEvent.Dispose();
        }
    }
}
