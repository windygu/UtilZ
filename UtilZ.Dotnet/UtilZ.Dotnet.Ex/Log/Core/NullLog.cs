using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log.LogRecorder;
using UtilZ.Dotnet.Ex.Log.LogRecorderInterface;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.Core
{
    /// <summary>
    /// 空日志,记录日志
    /// </summary>
    public class NullLog : BaseLog
    {
        /// <summary>
        /// 空日志记录器
        /// </summary>
        private readonly NullLoger _loger;

        /// <summary>
        /// 日志外部队列
        /// </summary>
        private readonly ConcurrentQueue<LogItem> _logQueue = new ConcurrentQueue<LogItem>();

        /// <summary>
        /// 日志外部队列线程通知对象
        /// </summary>
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 日志外部队列线程
        /// </summary>
        private Thread _logQueueThread = null;

        /// <summary>
        /// 线程取消通知对象
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NullLog()
            : base()
        {
            this._loger = new NullLoger();
            this._loger.InnerLogAddedIloger += InnerLogAddedIloger;
        }

        /// <summary>
        /// 开启日志记录
        /// </summary>
        public override void Start()
        {
            //启动日志记录线程
            this._cts = new CancellationTokenSource();
            this._logQueueThread = new Thread(this.RecordLogThreadMethod);
            this._logQueueThread.IsBackground = true;
            this._logQueueThread.Name = "空日志线程";
            this._logQueueThread.Start();
        }

        /// <summary>
        /// 日志记录线程方法
        /// </summary>
        private void RecordLogThreadMethod()
        {
            LogItem item;
            var token = this._cts.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (this._logQueue.Count == 0)
                    {
                        this._autoResetEvent.WaitOne();
                    }

                    if (this._logQueue.Count > 0 && this._logQueue.TryDequeue(out item))
                    {
                        item.LogProcess();
                        this._loger.WriteLog(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        /// <summary>
        /// 停止日志记录
        /// </summary>
        public override void Stop()
        {
            this._cts.Cancel();
            this._autoResetEvent.Set();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public override void LoadConfig(string configFilePath)
        {

        }

        /// <summary>
        /// 根据日志记录器名称获取日志记录器
        /// </summary>
        /// <param name="name">日志记录器名称</param>
        /// <returns>日志记录器</returns>
        public override ILoger GetLoger(string name)
        {
            return this._loger;
        }

        #region 记录日志方法
        /// <summary>
        /// 内部日志添加到ILoger记录
        /// </summary>
        /// <param name="sender">日志产生者</param>
        /// <param name="e">内部日志添加到ILoger记录事件参数</param>
        private void InnerLogAddedIloger(object sender, InnerLogAddedIlogerArgs e)
        {
            try
            {
                if (e.Level < this.Level)
                {
                    return;
                }

                var ex = e.Exception;
                LogItem logInfo;
                if (ex == null || string.IsNullOrWhiteSpace(ex.StackTrace))
                {
                    logInfo = new LogItem(DateTime.Now, System.Threading.Thread.CurrentThread, 4 + this.SkipFrames, e.Level, e.Message, e.Exception, e.Name, e.EventID, e.ExtendInfo);
                }
                else
                {
                    logInfo = new LogItem(DateTime.Now, System.Threading.Thread.CurrentThread, e.Level, e.Message, e.Exception, e.Name, e.EventID, e.ExtendInfo);
                }

                this._logQueue.Enqueue(logInfo);
                this._autoResetEvent.Set();
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this, exi);
            }
        }

        /// <summary>
        /// Loger日志记录
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        private void AddLog(LogLevel level, string msg, Exception ex, string name, int eventID, object extendInfo)
        {
            try
            {
                if (level < this.Level)
                {
                    return;
                }

                LogItem logInfo;
                if (ex == null || string.IsNullOrWhiteSpace(ex.StackTrace))
                {
                    logInfo = new LogItem(DateTime.Now, Thread.CurrentThread, 5 + this.SkipFrames, level, msg, ex, name, eventID, extendInfo);
                }
                else
                {
                    logInfo = new LogItem(DateTime.Now, Thread.CurrentThread, level, msg, ex, name, eventID, extendInfo);
                }

                this._logQueue.Enqueue(logInfo);
                this._autoResetEvent.Set();
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this, exi);
            }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public override void Debug(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null)
        {
            this.AddLog(LogLevel.Debug, msg, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public override void Info(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null)
        {
            this.AddLog(LogLevel.Info, msg, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public override void Warn(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null)
        {
            this.AddLog(LogLevel.Warn, msg, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public override void Error(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null)
        {
            this.AddLog(LogLevel.Error, msg, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public override void Faltal(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null)
        {
            this.AddLog(LogLevel.Faltal, msg, ex, name, eventID, extendInfo);
        }
        #endregion
    }
}
