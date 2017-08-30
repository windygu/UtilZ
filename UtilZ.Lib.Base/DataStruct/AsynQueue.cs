using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Base.Threading;

namespace UtilZ.Lib.Base.DataStruct
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
        private readonly string _threadName = "异步队列线程";

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
            if (this.ProcessAction != null)
            {
                this.ProcessAction(item);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AsynQueue()
            : this(string.Empty, true)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">异步队列名称</param>
        public AsynQueue(string name)
            : this(name, true)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        public AsynQueue(bool isBackground)
              : this(string.Empty, isBackground)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">异步队列名称</param>
        /// <param name="isBackground">是否是后台线程[true:后台线程，false:前台线程]</param>
        public AsynQueue(string name, bool isBackground)
        {
            this._isBackground = isBackground;
            this._threadName = name;
        }

        /// <summary>
        /// 启动子类无参数工作线程
        /// </summary>
        public void Start()
        {
            lock (this._threadMonitor)
            {
                if (this._thread != null)
                {
                    return;
                }

                this._cts = new CancellationTokenSource();
                this._thread = new Thread(this.RunThreadQueueProcessMethod);
                this._thread.IsBackground = this._isBackground;
                this._thread.Name = this.ThreadName;
                this._thread.Start();
            }
        }

        /// <summary>
        /// 停止工作线程
        /// </summary>
        public void Stop()
        {
            lock (this._threadMonitor)
            {
                if (this._thread != null)
                {
                    this._cts.Cancel();
                    this._autoResetEvent.Set();
                    this._thread = null;
                }
            }
        }

        /// <summary>
        /// 线程队列处理方法
        /// </summary>
        private void RunThreadQueueProcessMethod()
        {
            CancellationToken token = this._cts.Token;
            T item;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (this._queue.Count == 0)
                    {
                        this._autoResetEvent.WaitOne();
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
                catch (Exception ex)
                {
                    Loger.Error(ex.Message);
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
        private int _maxcount = 10240;

        /// <summary>
        /// 获取或设置线程队列最大项数,当超过这个值时,则移除超出队列开始的项[默认为10240项]
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
            if (this._cts != null)
            {
                this._cts.Dispose();
            }

            this._autoResetEvent.Dispose();
        }
    }
}
