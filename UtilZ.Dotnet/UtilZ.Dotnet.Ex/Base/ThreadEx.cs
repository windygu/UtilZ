using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Model;
using UtilZ.Dotnet.Ex.NativeMethod;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// 线程扩展类
    /// </summary>
    public class ThreadEx : IThreadEx
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
                lock (_currentThreadCountMonitor)
                {
                    return _currentThreadCount;
                }
            }
        }

        /// <summary>
        /// 当前已开启线程数线程监视器
        /// </summary>
        private static readonly object _currentThreadCountMonitor = new object();

        /// <summary>
        /// 不限制线程数的值,设置为0,如果以后此值有其它用处,可更改为其它值
        /// </summary>
        private readonly static int _noLimitValue = 0;

        /// <summary>
        /// 允许的最大线程数,0为无限制,默认0
        /// </summary>
        private static int _maxThreadCount = _noLimitValue;

        /// <summary>
        /// 获取或设置允许的最大线程数,0为无限制,默认0
        /// </summary>
        public static int MaxThreadCount
        {
            get
            {
                lock (_currentThreadCountMonitor)
                {
                    return _maxThreadCount;
                }
            }
            set
            {
                lock (_currentThreadCountMonitor)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("值无效,不能小于0");
                    }

                    if (_currentThreadCount > value)
                    {
                        throw new ArgumentException("已开启线程数大于目标值,设置失败");
                    }

                    _maxThreadCount = value;
                }
            }
        }

        /// <summary>
        /// 增加一个线程数量,如果超出上限,则抛出异常
        /// </summary>
        private static void IncreaseExcuteThreadCount()
        {
            lock (_currentThreadCountMonitor)
            {
                //如果不限制则不验证,验证线程数是否在限制范围内
                if (_maxThreadCount != _noLimitValue && _currentThreadCount >= _maxThreadCount)
                {
                    throw new Exception(string.Format("允许开启的线程数已达到最大值:{0}", _maxThreadCount));
                }

                Interlocked.Increment(ref _currentThreadCount);
            }
        }

        /// <summary>
        /// 减少一个线程执行的数量increase
        /// </summary>
        private static void DecreaseExcuteThreadCount()
        {
            lock (_currentThreadCountMonitor)
            {
                Interlocked.Decrement(ref _currentThreadCount);
            }
        }
        #endregion

        /// <summary>
        /// 线程要执行的委托,无参数
        /// </summary>
        private readonly Action<CancellationToken> _action;

        /// <summary>
        /// 线程要执行的委托,带参数
        /// </summary>
        private readonly Action<CancellationToken, object> _actionObj;

        /// <summary>
        /// true:无参数;false:带参数
        /// </summary>
        private bool _flag;

        /// <summary>
        /// 线程名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 是否后台运行[true:后台线程;false:前台线程]
        /// </summary>
        private bool _isBackground;

        /// <summary>
        /// 执行线程
        /// </summary>
        private Thread _thread = null;

        /// <summary>
        /// 向应该被取消的 System.Threading.CancellationToken 发送信号对象
        /// </summary>
        private CancellationTokenSource _cts = null;

        /// <summary>
        /// 同步停止通知AutoResetEvent
        /// </summary>
        private readonly AutoResetEvent _syncStopAutoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 外部调用线程锁
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// 是否已请求终止线程(包括取消和终止两种)
        /// </summary>
        private bool _isReqAbort = false;

        /// <summary>
        /// 当前线程是否正在运行
        /// </summary>
        private bool _isRuning = false;

        /// <summary>
        /// 对象是否已释放[true:已释放;false:未释放]
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">线程要执行的委托</param>
        /// <param name="name">线程名称</param>
        /// <param name="isBackground">是否后台运行[true:后台线程;false:前台线程]</param>
        public ThreadEx(Action<CancellationToken> action, string name = null, bool isBackground = true)
        {
            this._action = action;
            this._flag = true;
            this._name = name;
            this._isBackground = isBackground;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">线程要执行的委托</param>
        /// <param name="name">线程名称</param>
        /// <param name="isBackground">是否后台运行[true:后台线程;false:前台线程]</param>
        public ThreadEx(Action<CancellationToken, object> action, string name = null, bool isBackground = true)
        {
            this._actionObj = action;
            this._flag = false;
            this._name = name;
            this._isBackground = isBackground;
        }

        #region 静态方法
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <param name="action">线程要执行的委托</param>
        /// <param name="name">线程名称</param>
        /// <param name="isBackground">是否后台运行[true:后台线程;false:前台线程]</param>
        /// <returns>返回线程对象</returns>
        public static IThreadEx Start(Action<CancellationToken> action, string name = null, bool isBackground = true)
        {
            var ext = new ThreadEx(action, name, isBackground);
            ext.Start();
            return ext;
        }

        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <param name="action">线程要执行的委托</param>
        /// <param name="obj">线程启动参数</param>
        /// <param name="name">线程名称</param>
        /// <param name="isBackground">是否后台运行[true:后台线程;false:前台线程]</param>
        /// <returns>返回线程对象</returns>
        public static IThreadEx Start(Action<CancellationToken, object> action, object obj, string name = null, bool isBackground = true)
        {
            var ext = new ThreadEx(action, name, isBackground);
            ext.Start(obj);
            return ext;
        }

        /// <summary>
        /// win32方式指定当前线程运行在指定CPU核心上
        /// </summary>
        /// <param name="coreID">指定CPU核心ID</param>
        /// <returns>设置结果</returns>
        public static UIntPtr AssignCoreRun(uint coreID)
        {
            //return NativeMethods.SetThreadAffinityMask(NativeMethods.GetCurrentThread(), new UIntPtr(SetCpuID(coreNum)));
            return NativeMethods.SetThreadAffinityMask(NativeMethods.GetCurrentThread(), new UIntPtr(coreID));
        }

        /// <summary>
        /// .net方式指定当前线程运行在指定CPU核心上[多个核心间切换运行,不像win32方式是在一个核心上运行]
        /// </summary>
        /// <param name="threadID">线程ID</param>
        /// <param name="idealProcessor">首选处理器</param>
        /// <param name="coreID">目标处理器(Power(2,0-4]之间的单值或或位运算值)</param>
        public static void AssignCoreRun(int threadID, int idealProcessor, int coreID)
        {
            foreach (ProcessThread proThreadItem in System.Diagnostics.Process.GetCurrentProcess().Threads)
            {
                if (threadID == proThreadItem.Id)
                {
                    proThreadItem.IdealProcessor = idealProcessor;
                    proThreadItem.ProcessorAffinity = (IntPtr)coreID;
                }
            }
        }

        //static ulong SetCpuID(int id)
        //{
        //    ulong cpuid = 0;
        //    if (id < 0 || id >= System.Environment.ProcessorCount)
        //    {
        //        id = 0;
        //    }

        //    cpuid |= 1UL << id;

        //    return cpuid;
        //}

        /// <summary>
        /// 设置线程是否为后台线程
        /// </summary>
        /// <param name="thread">要设置的线程</param>
        /// <param name="isBackground">true:后台线程;false:前台线程</param>
        public static void SetThreadIsBackground(Thread thread, bool isBackground)
        {
            try
            {
                if (thread == null)
                {
                    return;
                }

                thread.IsBackground = isBackground;
            }
            catch
            { }
        }
        #endregion

        #region IThreadEx
        /// <summary>
        /// 线程执行完成事件
        /// </summary>
        public event EventHandler<ThreadExCompletedArgs> Completed;

        /// <summary>
        /// 触发线程执行完成事件
        /// </summary>
        /// <param name="type">线程执行完成类型</param>
        /// <param name="ex">当执行异常可取消时可能的异常信息</param>
        private void OnRaiseCompleted(ThreadExCompletedType type, Exception ex = null)
        {
            var handler = this.Completed;
            if (handler != null)
            {
                handler(this, new ThreadExCompletedArgs(type, ex));
            }
        }

        /// <summary>
        /// 获取线程当前的状态
        /// </summary>
        public System.Threading.ThreadState ThreadState
        {
            get
            {
                lock (this._lock)
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
        /// 获取当前托管线程的唯一标识符
        /// </summary>
        public int ManagedThreadId
        {
            get
            {
                lock (this._lock)
                {
                    if (this._thread == null)
                    {
                        throw new Exception("线程未启动");
                    }
                    else
                    {
                        return this._thread.ManagedThreadId;
                    }
                }
            }
        }

        /// <summary>
        /// 当前线程是否正在运行
        /// </summary>
        public bool IsRuning
        {
            get
            {
                lock (this._lock)
                {
                    return this._isRuning;
                }
            }
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="obj">线程启动参数</param>
        /// <param name="apartmentState">指定的单元状态 System.Threading.Thread</param>
        public void Start(object obj = null, ApartmentState apartmentState = ApartmentState.Unknown)
        {
            lock (this._lock)
            {
                if (this._isDisposed)
                {
                    throw new ObjectDisposedException(string.Empty, "对象已释放");
                }

                if (this._isRuning)
                {
                    return;
                }

                IncreaseExcuteThreadCount();
                this._cts = new CancellationTokenSource();

                this._thread = new Thread(new ParameterizedThreadStart(this.ThreadExcuteMethod));
                this._thread.SetApartmentState(apartmentState);
                if (string.IsNullOrWhiteSpace(this._name))
                {
                    var st = new System.Diagnostics.StackTrace(1, true);
                    var sf = st.GetFrame(0);
                    var method = sf.GetMethod();
                    this._thread.Name = string.Format("{0}.{1}.{2}.{3}", sf.GetFileName(), sf.GetFileLineNumber(), method.DeclaringType.FullName, method.Name);
                }
                else
                {
                    this._thread.Name = this._name;
                }

                this._thread.IsBackground = this._isBackground;
                this._isRuning = true;
                this._isReqAbort = false;
                this._thread.Start(new Tuple<object, CancellationToken>(obj, this._cts.Token));
            }
        }

        /// <summary>
        /// 线程执行方法
        /// </summary>
        /// <param name="obj">线程参数</param>
        private void ThreadExcuteMethod(object obj)
        {
            try
            {
                var tuple = (Tuple<object, CancellationToken>)obj;
                var token = tuple.Item2;
                if (this._flag)
                {
                    this._action(token);
                }
                else
                {
                    this._actionObj(token, tuple.Item1);
                }

                if (this._isReqAbort || token.IsCancellationRequested)
                {
                    this.OnRaiseCompleted(ThreadExCompletedType.Cancel);
                }
                else
                {
                    this.OnRaiseCompleted(ThreadExCompletedType.Completed);
                }
            }
            catch (System.Threading.ThreadAbortException aex)
            {
                this.OnRaiseCompleted(ThreadExCompletedType.Cancel, aex);
                throw;
            }
            catch (ObjectDisposedException oex)
            {
                this.OnRaiseCompleted(ThreadExCompletedType.Exception, oex);
                throw;
            }
            catch (Exception ex)
            {
                this.OnRaiseCompleted(ThreadExCompletedType.Exception, ex);
                throw;
            }
            finally
            {
                lock (this._lock)
                {
                    try
                    {
                        if (!this._isDisposed)
                        {
                            this._syncStopAutoResetEvent.Set();
                        }
                    }
                    catch (ObjectDisposedException)
                    { }

                    this._isRuning = false;
                    DecreaseExcuteThreadCount();
                }
            }
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <param name="isSycn">是否同步调用停止方法,同步调用会等线程结束后才退出本方法[true:同步;false:异步]</param>
        /// <param name="synMillisecondsTimeout">同步超时时间,-1表示无限期等待,单位/毫秒[isSycn为true时有效]</param>
        public void Stop(bool isSycn = false, int synMillisecondsTimeout = -1)
        {
            lock (this._lock)
            {
                if (this._isDisposed)
                {
                    return;
                }

                if (this._isReqAbort ||
                this._cts == null ||
                this._cts.Token.IsCancellationRequested ||
                this._thread == null ||
                this._thread.ThreadState == System.Threading.ThreadState.Aborted ||
                this._thread.ThreadState == System.Threading.ThreadState.AbortRequested ||
                this._thread.ThreadState == System.Threading.ThreadState.StopRequested ||
                this._thread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    return;
                }

                if (this._cts != null && !this._cts.IsCancellationRequested)
                {
                    this._cts.Cancel();
                    this._cts.Dispose();
                    this._cts = null;
                }

                this._isReqAbort = true;
                this._thread = null;
            }

            if (isSycn)
            {
                try
                {
                    this._syncStopAutoResetEvent.WaitOne(synMillisecondsTimeout);
                }
                catch (ObjectDisposedException)
                { }
            }
        }

        /// <summary>
        /// 终止线程
        /// </summary>
        public void Abort()
        {
            lock (this._lock)
            {
                if (this._isReqAbort || this._thread == null)
                {
                    return;
                }

                this._thread.Abort();
                this._isReqAbort = true;
                this._thread = null;
            }
        }
        #endregion

        #region IDisposable
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
            lock (this._lock)
            {
                if (this._isDisposed)
                {
                    return;
                }

                this.Stop(true);
                this._isDisposed = true;
                try
                {
                    if (this._syncStopAutoResetEvent != null)
                    {
                        this._syncStopAutoResetEvent.Set();
                        this._syncStopAutoResetEvent.Dispose();
                    }

                    var cts = this._cts;
                    if (cts != null)
                    {
                        if (!cts.IsCancellationRequested)
                        {
                            cts.Cancel();
                            cts.Dispose();
                        }
                    }
                }
                catch { }
                this._cts = null;
            }
        }
        #endregion
    }

    /// <summary>
    /// 扩展线程接口
    /// </summary>
    public interface IThreadEx : IDisposable
    {
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="obj">线程启动参数</param>
        /// <param name="apartmentState">指定的单元状态 System.Threading.Thread</param>
        void Start(object obj = null, ApartmentState apartmentState = ApartmentState.Unknown);

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <param name="isSycn">是否同步调用停止方法,同步调用会等线程结束后才退出本方法[true:同步;false:异步]</param>
        /// <param name="synMillisecondsTimeout">同步超时时间,-1表示无限期等待,单位/毫秒</param>
        void Stop(bool isSycn = false, int synMillisecondsTimeout = -1);

        /// <summary>
        /// 终止线程
        /// </summary>
        void Abort();

        /// <summary>
        /// 线程执行完成事件
        /// </summary>
        event EventHandler<ThreadExCompletedArgs> Completed;

        /// <summary>
        /// 获取线程当前的状态
        /// </summary>
        System.Threading.ThreadState ThreadState { get; }

        /// <summary>
        /// 获取当前托管线程的唯一标识符
        /// </summary>
        int ManagedThreadId { get; }

        /// <summary>
        /// 当前线程是否正在运行
        /// </summary>
        bool IsRuning { get; }
    }

    /// <summary>
    /// 线程执行完成事件参数
    /// </summary>
    public class ThreadExCompletedArgs : EventArgs
    {
        /// <summary>
        /// 线程执行完成类型
        /// </summary>
        public ThreadExCompletedType Type { get; private set; }

        /// <summary>
        /// 当执行异常可取消时可能的异常信息
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">线程执行完成类型</param>
        /// <param name="ex">当执行异常可取消时可能的异常信息</param>
        public ThreadExCompletedArgs(ThreadExCompletedType type, Exception ex)
        {
            this.Type = type;
            this.Exception = ex;
        }
    }

    /// <summary>
    /// 线程执行完成类型
    /// </summary>
    public enum ThreadExCompletedType
    {
        /// <summary>
        /// 完成
        /// </summary>
        Completed,

        /// <summary>
        /// 异常
        /// </summary>
        Exception,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }
}
