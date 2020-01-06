using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// BlockingCollectionEx线程
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlockingCollectionExThread<T> : IDisposable
    {
        private readonly BlockingCollectionEx<T> _blockingCollection = new BlockingCollectionEx<T>();
        private readonly int _maxThreadCount;
        private readonly string _threadNamePre;
        private readonly Action<T> _process;
        private readonly int _addProcessThreadThreshold;
        private readonly List<ThreadEx> _threadList = new List<ThreadEx>();
        private readonly object _threadLock = new object();
        private bool _allowAddThread = true;
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxThreadCount">最大并发处理线程数,小于1无限制</param>
        /// <param name="threadNamePre">线程名前续</param>
        /// <param name="process">处理回调</param>
        /// <param name="addProcessThreadThreshold">新添加处理线程阈值条件,当未处理集合中的项数超过此值时会新添加一个处理线程</param>
        public BlockingCollectionExThread(int maxThreadCount, string threadNamePre, Action<T> process, int addProcessThreadThreshold = 100)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (addProcessThreadThreshold < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(addProcessThreadThreshold), "新添加处理线程阈值条件不能小于1");
            }

            this._maxThreadCount = maxThreadCount;
            this._threadNamePre = threadNamePre;
            this._process = process;
            this._addProcessThreadThreshold = addProcessThreadThreshold;
            this.CreateProcessThread();
        }

        private void CreateProcessThread()
        {
            if (!this._allowAddThread)
            {
                return;
            }

            lock (this._threadLock)
            {
                if (!this._allowAddThread)
                {
                    return;
                }

                if (this._isDisposed)
                {
                    this._allowAddThread = false;
                    return;
                }

                if (this._threadList.Count >= Environment.ProcessorCount ||
                    this._maxThreadCount > 0 && this._threadList.Count >= this._maxThreadCount)
                {
                    this._allowAddThread = false;
                    return;
                }

                string threadName = $"{this._threadNamePre}{this._threadList.Count}";
                ThreadEx thread = new ThreadEx(this.ProcessThreadMethod, threadName, true);
                this._threadList.Add(thread);
                thread.Start();
            }
        }


        private void ProcessThreadMethod(CancellationToken token)
        {
            try
            {
                const int millisecondsTimeout = 1000;
                T item;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (!this._blockingCollection.TryTake(out item, millisecondsTimeout, token))
                        {
                            continue;
                        }

                        this.AddNewProcessThread();
                        this._process(item);
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

        private void AddNewProcessThread()
        {
            if (this._blockingCollection.Count <= this._addProcessThreadThreshold)
            {
                return;
            }

            this.CreateProcessThread();
        }


        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            this._blockingCollection.Add(item);
        }


        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {
            try
            {
                lock (this._threadLock)
                {
                    if (this._isDisposed)
                    {
                        return;
                    }
                    this._isDisposed = true;

                    foreach (var thread in this._threadList)
                    {
                        thread.Stop();
                        thread.Dispose();
                    }

                    this._threadList.Clear();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
