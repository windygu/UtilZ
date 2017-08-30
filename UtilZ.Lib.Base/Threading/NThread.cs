using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.Extend;

namespace UtilZ.Lib.Base.Threading
{
    /// <summary>
    /// 线程辅助类
    /// </summary>
    public abstract class NThread : IDisposable
    {
        #region 线程数限制
        /// <summary>
        /// 当前已开启线程数
        /// </summary>
        private static int _currentThreadCount = 0;

        /// <summary>
        /// 获取当前已开启线程数
        /// </summary>
        public static int CurrentThreadCount
        {
            get
            {
                lock (NThread._currentThreadCountMonitor)
                {
                    return NThread._currentThreadCount;
                }
            }
        }

        /// <summary>
        /// 当前已开启线程数线程监视器
        /// </summary>
        private static readonly object _currentThreadCountMonitor = new object();

        /// <summary>
        /// 不限制线程数的值,设置为-1,如果以后此值有其它用处,可更改为其它值
        /// </summary>
        private readonly static int _noLimitValue = -1;

        /// <summary>
        /// 允许的最大线程数,-1为无限制,默认-1
        /// </summary>
        private static int _maxThreadCount = NThread._noLimitValue;

        /// <summary>
        /// 获取或设置允许的最大线程数,-1为无限制,默认-1
        /// </summary>
        public static int MaxThreadCount
        {
            get
            {
                lock (NThread._currentThreadCountMonitor)
                {
                    return NThread._maxThreadCount;
                }
            }
            set
            {
                lock (NThread._currentThreadCountMonitor)
                {
                    NThread._maxThreadCount = value;
                }
            }
        }

        /// <summary>
        /// 验证当前是否允许再开启新的线程,如果允许就增加一个线程数量
        /// </summary>
        private static void ValidateIncreaseExcuteThreadCount()
        {
            lock (NThread._currentThreadCountMonitor)
            {
                //如果不限制则不验证
                if (NThread._maxThreadCount == NThread._noLimitValue)
                {
                    return;
                }

                //验证线程数是否在限制范围内
                if (NThread._currentThreadCount >= NThread._maxThreadCount)
                {
                    throw new Exception(string.Format("允许开启的线程数已达到最大值:{0}", NThread._maxThreadCount));
                }

                Interlocked.Increment(ref NThread._currentThreadCount);
            }
        }

        /// <summary>
        /// 减少一个线程执行的数量increase
        /// </summary>
        private static void DecreaseExcuteThreadCount()
        {
            lock (NThread._currentThreadCountMonitor)
            {
                //如果不限制则不减
                if (NThread._currentThreadCount == NThread._noLimitValue)
                {
                    return;
                }

                Interlocked.Decrement(ref NThread._currentThreadCount);
            }
        }
        #endregion

        #region 私有字段,属性,事件
        /// <summary>
        /// 线程执行结束事件
        /// </summary>
        private event EventHandler _exited;

        /// <summary>
        /// 线程执行结束事件,当线程处于运行时,不可更改注册此事件的委托
        /// </summary>
        public event EventHandler Exited
        {
            add
            {
                if (this._isRuning)
                {
                    //throw new ApplicationException("当线程处于运行时,不可更改此事件委托");
                    return;
                }

                this._exited += value;
            }
            remove
            {
                if (this._isRuning)
                {
                    //throw new ApplicationException("当线程处于运行时,不可更改此事件委托");
                    return;
                }

                this._exited -= value;
            }
        }

        /// <summary>
        /// 当线程执行结束事件
        /// </summary>
        private void OnRaiseExited()
        {
            if (this._exited != null)
            {
                this._exited(this, new EventArgs());
            }
        }

        /// <summary>
        /// 线程对象
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
        /// 当前线程是否正在运行
        /// </summary>
        private bool _isRuning = false;

        /// <summary>
        /// 获取线程当前的状态
        /// </summary>
        public ThreadState ThreadState
        {
            get
            {
                lock (this._threadMonitor)
                {
                    if (this._thread == null)
                    {
                        return System.Threading.ThreadState.Unstarted;
                    }
                    else
                    {
                        return this._thread.ThreadState;
                    }
                }
            }
        }

        /// <summary>
        /// 当前线程的执行状态
        /// </summary>
        public bool IsAlive
        {
            get
            {
                lock (this._threadMonitor)
                {
                    if (this._thread == null)
                    {
                        return false;
                    }
                    else
                    {
                        return this._thread.IsAlive;
                    }
                }
            }
        }

        /// <summary>
        /// 线程调试优先级
        /// </summary>
        private ThreadPriority _priority = ThreadPriority.Normal;

        /// <summary>
        /// 获取或设置线程调试优先级
        /// </summary>
        public ThreadPriority Priority
        {
            get
            {
                lock (this._threadMonitor)
                {
                    return this._priority;
                }
            }
            set
            {
                lock (this._threadMonitor)
                {
                    this._priority = value;
                    if (this._thread != null)
                    {
                        this._thread.Priority = this._priority;
                    }
                }
            }
        }

        /// <summary>
        /// 是否后台运行
        /// </summary>
        private bool _isBackground = false;

        /// <summary>
        /// 获取或设置是否后台运行
        /// </summary>
        public bool IsBackground
        {
            get
            {
                lock (this._threadMonitor)
                {
                    return this._isBackground;
                }
            }
            set
            {
                lock (this._threadMonitor)
                {
                    this._isBackground = value;
                    if (this._thread != null)
                    {
                        this._thread.IsBackground = this._isBackground;
                    }
                }
            }
        }

        /// <summary>
        /// 线程的名称
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// 获取或设置线程的名称
        /// </summary>
        public string Name
        {
            get
            {
                string name = string.Empty;
                lock (this._threadMonitor)
                {
                    if (this._thread != null)
                    {
                        name = this._thread.Name;
                    }
                    else
                    {
                        name = _name;
                    }
                }

                return name;
            }
            set
            {
                lock (this._threadMonitor)
                {
                    if (this._thread != null)
                    {
                        this._thread.Name = value;
                        this._name = this._thread.Name;
                    }
                    else
                    {
                        this._name = value;
                    }
                }
            }
        }

        /// <summary>
        /// 线程执行等待参数[仅当需要线程等待方法中初始化]
        /// </summary>
        private NThreadWaitPara _breezeThreadWaitPara = null;
        #endregion

        #region 启动子类无参数工作线程
        /// <summary>
        /// 启动子类无参数工作线程
        /// </summary>
        public void Start()
        {
            lock (this._threadMonitor)
            {
                if (this._isRuning)
                {
                    return;
                }

                NThread.ValidateIncreaseExcuteThreadCount();
                this._thread = new Thread(this.ThreadMethod);
                if (string.IsNullOrEmpty(this._name))
                {
                    this._thread.Name = "WorkThread:" + this._thread.ManagedThreadId.ToString();
                }
                else
                {
                    this._thread.Name = this._name;
                }
                this._thread.IsBackground = this._isBackground;
                this._thread.Priority = this._priority;
                this._thread.Start();
                this._isRuning = true;
            }
        }

        /// <summary>
        /// 执行无参数的线程方法
        /// </summary>
        private void ThreadMethod()
        {
            try
            {
                this._cts = new CancellationTokenSource();
                CancellationToken token;
                if (this._breezeThreadWaitPara == null)
                {
                    token = this._cts.Token;
                }
                else
                {
                    token = this._breezeThreadWaitPara.Token;
                }

                this.ExcuteThreadMethod(token);
            }
            catch (System.Threading.ThreadAbortException)
            {
                //线程手动停止异常,无需处理
            }
            catch (OperationCanceledException)
            {
                //操作取消异常,无需处理
            }
            finally
            {
                NThread.DecreaseExcuteThreadCount();
                this._isRuning = false;
                this._cts = null;
                this.OnRaiseExited();
            }
        }

        /// <summary>
        /// 无参数的线程方法,CancellationToken对象每次执行时都是不同的变量
        /// </summary>
        /// <param name="token">线程取消通知参数</param>
        protected virtual void ExcuteThreadMethod(CancellationToken token)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 启动子类带参数工作线程
        /// <summary>
        /// 启动子类带参数工作线程
        /// </summary>
        /// <param name="parameter">参数</param>
        public void Start(object parameter)
        {
            lock (this._threadMonitor)
            {
                if (this._isRuning)
                {
                    return;
                }
                NThread.ValidateIncreaseExcuteThreadCount();
                this._thread = new Thread(this.ThreadMethodByPara);
                if (string.IsNullOrEmpty(this._name))
                {
                    this._thread.Name = "WorkThread:" + this._thread.ManagedThreadId.ToString();
                }
                else
                {
                    this._thread.Name = this._name;
                }
                this._thread.IsBackground = this._isBackground;
                this._thread.Priority = this._priority;
                this._thread.Start(parameter);
                this._isRuning = true;
            }
        }

        /// <summary>
        /// 执行带参数的线程方法
        /// </summary>
        /// <param name="parameter">参数</param>
        private void ThreadMethodByPara(object parameter)
        {
            try
            {
                this._cts = new CancellationTokenSource();
                this.ExcuteThreadMethodByPara(this._cts.Token, parameter);
            }
            catch (System.Threading.ThreadAbortException)
            {
                //线程手动停止异常,无需处理
            }
            finally
            {
                NThread.DecreaseExcuteThreadCount();
                this._isRuning = false;
                this._cts = null;
                this.OnRaiseExited();
            }
        }

        /// <summary>
        /// 带参数的线程方法,CancellationToken对象每次执行时都是不同的变量
        /// </summary>
        /// <param name="token">线程取消通知参数</param>
        /// <param name="parameter">参数</param>
        protected virtual void ExcuteThreadMethodByPara(CancellationToken token, object parameter)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 停止工作线程
        /// </summary>
        /// <param name="forcesCancellFlag">是否强制结束标示,true:将调用线程的Abort方法强制终止线程,false:通过线程取消通知来终止,但需要线程方法中对此作判断[默认为false]</param>
        /// <param name="throwOnfirtException">指示是否立即传播异常[默认为false]</param>
        public virtual void Stop(bool forcesCancellFlag = false, bool throwOnfirtException = false)
        {
            lock (this._threadMonitor)
            {
                if (!this._isRuning)
                {
                    return;
                }

                if (this._breezeThreadWaitPara == null)
                {
                    //线程取消执行通知
                    if (this._cts != null && !this._cts.IsCancellationRequested)
                    {
                        //取消线程执行
                        this._cts.Cancel(throwOnfirtException);
                        //this._cts.Token.ThrowIfCancellationRequested();
                    }

                    //是否强制结束线程
                    if (forcesCancellFlag)
                    {
                        if (this._thread != null && (this._thread.ThreadState != ThreadState.Aborted || this._thread.ThreadState != ThreadState.Stopped))
                        {
                            this._thread.Abort();
                        }
                    }

                    this._isRuning = false;
                }
                else
                {
                    throw new Exception("在调用了WaitAll或WaitAny方法后,不可再调用取消执行方法");
                }
            }
        }

        #region 静态启动线程
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="action">要执行的委托</param>
        /// <param name="isBackground">是否是后台运行,true:后台运行,false:前台运行,默认值:false</param>
        /// <param name="priority">线程调试优先级,默认值:ThreadPriority.Normal</param>
        /// <returns>执行的线程</returns>
        public static NThread Start(Action<CancellationToken> action, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            NThread breezeNoParaThread = NThread.Create(action, isBackground, priority);
            breezeNoParaThread.Start();
            return breezeNoParaThread;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="action">要执行的带参数委托</param>
        /// <param name="parameter">参数</param>
        /// <param name="isBackground">是否是后台运行,true:后台运行,false:前台运行,默认值:false</param>
        /// <param name="priority">线程调试优先级,默认值:ThreadPriority.Normal</param>
        /// <returns>执行的线程</returns>
        public static NThread Start<T>(Action<CancellationToken, T> action, T parameter, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            NThread breezeParaThread = NThread.Create<T>(action, isBackground, priority);
            breezeParaThread.Start(parameter);
            return breezeParaThread;
        }

        /// <summary>
        /// 执行平行线程
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="items">参数集合</param>
        /// <param name="action">执行委托</param>
        /// <param name="count">线程限制数,-1为不限[默认为-1]</param>
        /// <param name="isBackground">是否是后台运行,true:后台运行,false:前台运行,默认值:false</param>
        /// <param name="priority">线程调试优先级,默认值:ThreadPriority.Normal</param>
        public static NThread StartParallelThread<T>(IEnumerable<T> items, Action<CancellationToken, T> action, int count = -1, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            NThread parallelThread = NThread.CreateParallelThread<T>(items, action, count);
            parallelThread.Start();
            return parallelThread;
        }

        /// <summary>
        /// 执行平行任务
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="items">参数集合</param>
        /// <param name="action">执行委托</param>
        /// <param name="count">线程限制数,-1为不限[默认为-1]</param>
        public static NThread StartParallelTask<T>(IEnumerable<T> items, Action<CancellationToken, T> action, int count = -1)
        {
            NThread parallelThread = NThread.CreateParallelTask<T>(items, action, count);
            parallelThread.Start();
            return parallelThread;
        }
        #endregion

        #region 静态创建线程
        /// <summary>
        /// 创建一个线程
        /// </summary>
        /// <param name="action">要执行的委托</param>
        /// <param name="isBackground">是否是后台运行,true:后台运行,false:前台运行,默认值:false</param>
        /// <param name="priority">线程调试优先级,默认值:ThreadPriority.Normal</param>
        /// <returns>创建好的线程</returns>
        public static NThread Create(Action<CancellationToken> action, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            NActionThread subThread = new NActionThread(action);
            subThread.IsBackground = isBackground;
            subThread.Priority = priority;
            return subThread;
        }

        /// <summary>
        /// 创建一个线程
        /// </summary>
        /// <param name="action">要执行的带参数委托</param>
        /// <param name="isBackground">是否是后台运行,true:后台运行,false:前台运行,默认值:false</param>
        /// <param name="priority">线程调试优先级,默认值:ThreadPriority.Normal</param>
        /// <returns>创建好的线程</returns>
        public static NThread Create<T>(Action<CancellationToken, T> action, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            BreezeActionParaThread<T> subThread = new BreezeActionParaThread<T>(action);
            subThread.IsBackground = isBackground;
            subThread.Priority = priority;
            return subThread;
        }

        /// <summary>
        /// 创建平行线程
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="items">参数集合</param>
        /// <param name="action">执行委托</param>
        /// <param name="count">线程限制数,-1为不限[默认为-1]</param>
        /// <param name="isBackground">是否是后台运行,true:后台运行,false:前台运行,默认值:false</param>
        /// <param name="priority">线程调试优先级,默认值:ThreadPriority.Normal</param>
        public static NThread CreateParallelThread<T>(IEnumerable<T> items, Action<CancellationToken, T> action, int count = -1, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            if (items == null || items.Count() == 0)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items));
            }

            if (action == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => action));
            }

            if (count != -1 && count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            BreezeParallelThread<T> bpThread = new BreezeParallelThread<T>(items, action, count);
            bpThread.IsBackground = isBackground;
            bpThread.Priority = priority;
            return bpThread;
        }

        /// <summary>
        /// 创建平行伤
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="items">参数集合</param>
        /// <param name="action">执行委托</param>
        /// <param name="count">线程限制数,-1为不限[默认为-1]</param>
        public static NThread CreateParallelTask<T>(IEnumerable<T> items, Action<CancellationToken, T> action, int count = -1)
        {
            if (items == null || items.Count() == 0)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items));
            }

            if (action == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => action));
            }

            if (count != -1 && count < 1)
            {
                throw new ArgumentOutOfRangeException(NExtendObject.GetVarName(p => count));
            }

            BreezeParallelTask<T> bpThread = new BreezeParallelTask<T>(items, action, count);
            return bpThread;
        }
        #endregion

        /// <summary>
        /// 将当前线程挂起指定的时间
        /// </summary>
        /// <param name="millisecondsTimeout">将当前线程挂起指定的时间</param>
        public static void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        /// <summary>
        /// 将当前线程阻塞指定的时间
        /// </summary>
        /// <param name="timeout">设置为线程被阻塞的时间量的 System.TimeSpan</param>
        public static void Sleep(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        #region 等待线程执行完成
        /// <summary>
        /// 等待提供的所有线程对象完成执行过程
        /// </summary>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>
        public static void WaitAll(params NThread[] breezeThreads)
        {
            NThread.Wait(true, NBaeHepler.GetForeverTimeSpan(), new CancellationToken(false), breezeThreads);
        }

        /// <summary>
        /// 等待提供的所有线程对象完成执行过程
        /// </summary>
        /// <param name="ts">超时时间片</param>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>
        public static void WaitAll(TimeSpan ts, params NThread[] breezeThreads)
        {
            NThread.Wait(true, ts, new CancellationToken(false), breezeThreads);
        }

        /// <summary>
        /// 等待提供的所有线程对象完成执行过程
        /// </summary>
        /// <param name="token">线程取消通知对象</param>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>
        public static void WaitAll(CancellationToken token, params NThread[] breezeThreads)
        {
            NThread.Wait(true, NBaeHepler.GetForeverTimeSpan(), token, breezeThreads);
        }

        /// <summary>
        /// 等待提供的任何线程对象完成执行过程
        /// </summary>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>
        public static void WaitAny(params NThread[] breezeThreads)
        {
            NThread.Wait(false, NBaeHepler.GetForeverTimeSpan(), new CancellationToken(false), breezeThreads);
        }

        /// <summary>
        /// 等待提供的任何线程对象完成执行过程
        /// </summary>
        /// <param name="ts">超时时间片</param>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>
        public static void WaitAny(TimeSpan ts, params NThread[] breezeThreads)
        {
            NThread.Wait(false, ts, new CancellationToken(false), breezeThreads);
        }

        /// <summary>
        /// 等待提供的任何线程对象完成执行过程
        /// </summary>
        /// <param name="token">线程取消通知对象</param>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>
        public static void WaitAny(CancellationToken token, params NThread[] breezeThreads)
        {
            NThread.Wait(false, NBaeHepler.GetForeverTimeSpan(), token, breezeThreads);
        }

        /// <summary>
        /// 等待提供的所有线程对象完成执行过程
        /// </summary>
        /// <param name="isWaitAll">是否等待全部[true:等待全部执行完成,false:任意完成]</param>
        /// <param name="ts">超时时间片</param>
        /// <param name="token">线程取消通知对象</param>
        /// <param name="breezeThreads">要等待的线程实例的数组</param>        
        public static void Wait(bool isWaitAll, TimeSpan ts, CancellationToken token, params NThread[] breezeThreads)
        {
            //空值验证
            if (breezeThreads == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(xx => breezeThreads));
            }

            //线程数组长度验证
            if (breezeThreads.Length == 0)
            {
                return;
            }

            //线程是否没启动验证
            foreach (var breezeThread in breezeThreads)
            {
                if (breezeThread._isRuning)
                {
                    throw new ArgumentException("有线程已经启动");
                }
            }

            //初始化线程执行结束委托
            object monitor = new object();
            int excuteExitThreadCount = 0;
            EventHandler exitedDelegate = (s, e) =>
            {
                lock (monitor)
                {
                    excuteExitThreadCount++;
                }
            };

            //遍历线程注册结束通知事件和启动线程
            foreach (var breezeThread in breezeThreads)
            {
                breezeThread._breezeThreadWaitPara = new NThreadWaitPara(token);
                breezeThread.Exited += exitedDelegate;
                breezeThread.Start();
            }

            DateTime startTime = DateTime.Now;
            //等待执行完成
            while (true)
            {
                //正常执行结束
                lock (monitor)
                {
                    if (isWaitAll && excuteExitThreadCount == breezeThreads.Length)
                    {
                        //等待全部线程执行完成,全部线程执行完成后结束
                        break;
                    }
                    else if (!isWaitAll && excuteExitThreadCount > 0)
                    {
                        //等待任意线程执行完成,任意线程执行完成后结束
                        break;
                    }
                    else
                    {
                        //每隔10毫秒检查一次
                        NThread.Sleep(10);
                    }
                }

                //超时结束或取消执行
                if (DateTime.Now - startTime >= ts)
                {
                    foreach (var breezeThread in breezeThreads)
                    {
                        breezeThread.Stop();
                    }

                    break;
                }
            }

            //清理线程资源
            foreach (var breezeThread in breezeThreads)
            {
                breezeThread._breezeThreadWaitPara = null;
                breezeThread.Exited -= exitedDelegate;
            }
        }
        #endregion

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
        }
    }
}
