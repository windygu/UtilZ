using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using UtilZ.Dotnet.SEx.Log.Appender;
using UtilZ.Dotnet.SEx.Log.Core;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.Core
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public sealed class Loger : LogerBase
    {
        #region 静态成员
        private static readonly LogerBase _emptyLoger = new EmptyLoger();
        private static LogerBase _defaultLoger;

        /// <summary>
        /// [key:LogerName;Value:Loger]
        /// </summary>
        private static readonly Hashtable _htLoger = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 日志追加器集合
        /// </summary>
        private readonly List<AppenderBase> _appenders = new List<AppenderBase>();

        /// <summary>
        /// 静态构造函数(初始化默认日志追加器)
        /// </summary>
        static Loger()
        {
            var defaultLoger = new Loger();
            defaultLoger._logerName = string.Empty;
            defaultLoger._appenders.Add(new FileAppender());
            _defaultLoger = defaultLoger;
        }

        /// <summary>
        /// 清空所有配置,包括默认
        /// </summary>
        public static void Clear()
        {
            foreach (ILoger loger in _htLoger.Values)
            {
                loger.Dispose();
            }

            if (_defaultLoger != _emptyLoger && _defaultLoger != null)
            {
                _defaultLoger.Dispose();
            }

            _defaultLoger = _emptyLoger;
            _htLoger.Clear();
        }

        /// <summary>
        /// 加载配置,加载前清空旧的配置
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public static void LoadConfig(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                return;
            }

            var xdoc = XDocument.Load(configFilePath);
            IEnumerable<XElement> logerEles = xdoc.XPathSelectElements(@"logconfig/loger");
            if (logerEles.Count() == 0)
            {
                return;
            }

            foreach (var logerEle in logerEles)
            {
                ParseLogerConfig(logerEle);
            }
        }

        private static void ParseLogerConfig(XElement logerEle)
        {
            try
            {
                string name = LogUtil.GetAttributeValue(logerEle, "name");
                if (string.IsNullOrEmpty(name))
                {
                    if (_defaultLoger != _emptyLoger && _defaultLoger != null)
                    {
                        _defaultLoger.Dispose();
                    }

                    _defaultLoger = _emptyLoger;
                }

                var loger = new Loger();
                loger.Name = name;
                LogLevel level;
                if (Enum.TryParse<LogLevel>(LogUtil.GetAttributeValue(logerEle, "level"), true, out level))
                {
                    loger.Level = level;
                }

                bool enable;
                if (bool.TryParse(LogUtil.GetAttributeValue(logerEle, "enable"), out enable))
                {
                    loger.Enable = enable;
                }

                IEnumerable<XElement> appenderEles = logerEle.XPathSelectElements("appender");
                foreach (var appenderEle in appenderEles)
                {
                    try
                    {
                        CreateAppender(appenderEle, loger);
                    }
                    catch (Exception exi)
                    {
                        LogSysInnerLog.OnRaiseLog(null, exi);
                    }
                }

                if (string.IsNullOrEmpty(name))
                {
                    _defaultLoger = loger;
                }
                else
                {
                    _htLoger[name] = loger;
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("解析配置文件异常", ex);
            }
        }

        private static void CreateAppender(XElement appenderEle, Loger loger)
        {
            string appenderName = LogUtil.GetAttributeValue(appenderEle, "name");
            try
            {
                string appenderTypeName = LogUtil.GetAttributeValue(appenderEle, "type");
                if (string.IsNullOrWhiteSpace(appenderTypeName))
                {
                    return;
                }

                object obj = LogUtil.CreateInstance(appenderTypeName);
                AppenderBase appender = obj as AppenderBase;
                if (appender == null)
                {
                    return;
                }

                appender.Name = appenderName;
                appender.Init(appenderEle);
                loger._appenders.Add(appender);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(string.Format("解析:{0}日志追加器异常", appenderName), ex);
            }
        }

        /// <summary>
        /// 获取日志记录器,如果日志记录器成功返回配置的日志记录器,如果不存在返回空日志记录器
        /// </summary>
        /// <param name="logerName">日志记录器名称</param>
        /// <returns>日志记录器</returns>
        public static ILoger GetLoger(string logerName)
        {
            ILoger loger;
            if (string.IsNullOrEmpty(logerName))
            {
                loger = _defaultLoger;
            }
            else
            {
                loger = _htLoger[logerName] as ILoger;
            }

            if (loger == null)
            {
                loger = _emptyLoger;
            }

            return loger;
        }
        #endregion

        #region 日志记录器实例成员
        /// <summary>
        /// 日志记录器名称
        /// </summary>
        public string Name { get; private set; } = null;

        /// <summary>
        /// 是否启用日志追加器
        /// </summary>
        public bool Enable { get; private set; } = true;

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; private set; } = LogLevel.Trace;

        private readonly Thread _logThread;
        private BlockingCollection<LogItem> _logQueue;
        private readonly CancellationTokenSource _cts;
        private Loger() : base()
        {
            this._cts = new CancellationTokenSource();
            this._logQueue = new BlockingCollection<LogItem>(new ConcurrentQueue<LogItem>());
            this._logThread = new Thread(this.LogThreadMethod);
            this._logThread.IsBackground = true;
            this._logThread.Start();
        }

        private void LogThreadMethod()
        {
            CancellationToken token = this._cts.Token;
            LogItem item;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (this._logQueue.TryTake(out item, Timeout.Infinite, token))
                    {
                        item.LogProcess();
                        foreach (var appender in this._appenders)
                        {
                            try
                            {
                                appender.WriteLog(item);
                            }
                            catch (Exception exi)
                            {
                                LogSysInnerLog.OnRaiseLog(this, exi);
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;//线程停止
                }
                catch (ObjectDisposedException)
                {
                    break;//已释放
                }
                catch (ArgumentNullException ex)
                {
                    //微软BUG,不晓得.net core里面还有没此BUG
                    LogSysInnerLog.OnRaiseLog(this, new Exception("this._logQueue.TryTake.ArgumentNullException", ex));
                    continue;
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        #region ILoger接口
        /// <summary>
        /// 静态方法添加日志的方法
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        internal override void ObjectAddLog(LogLevel level, string msg, Exception ex, int eventID, params object[] args)
        {
            this.PrimitiveAddLog(5, level, msg, ex, eventID, args);
        }

        /// <summary>
        /// 实例添加日志
        /// </summary>
        /// <param name="skipFrames">跳过堆栈帧数</param>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        protected override void PrimitiveAddLog(int skipFrames, LogLevel level, string msg, Exception ex, int eventID, params object[] args)
        {
            try
            {
                if (!this.Enable || level < this.Level)
                {
                    return;
                }

                var item = new LogItem(DateTime.Now, Thread.CurrentThread, skipFrames, level, msg, ex, base._logerName, eventID, true, args);
                this._logQueue.Add(item);
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this, exi);
            }
        }
        #endregion

        /// <summary>
        /// 释放资源方法
        /// </summary>
        /// <param name="isDisposing">是否释放标识</param>
        protected override void Dispose(bool isDisposing)
        {
            this._cts.Cancel();
            this._logQueue.Dispose();
        }
        #endregion

        #region 静态记录日志方法,默认日志快捷方法
        private static void SAddLog(LogLevel level, string msg, Exception ex, int eventID, params object[] args)
        {
            try
            {
                _defaultLoger.ObjectAddLog(level, msg, ex, eventID, args);
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(null, exi);
            }
        }

        #region Trace
        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        public static void Trace(string formatMsg, params object[] args)
        {
            SAddLog(LogLevel.Trace, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Trace(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Trace, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        public static void Trace(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            SAddLog(LogLevel.Trace, null, ex, eventID, null);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Trace(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Trace, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        public static void Debug(string formatMsg, params object[] args)
        {
            SAddLog(LogLevel.Debug, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Debug(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Debug, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        public static void Debug(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            SAddLog(LogLevel.Debug, null, ex, eventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Debug(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Debug, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Info
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        public static void Info(string formatMsg, params object[] args)
        {
            SAddLog(LogLevel.Info, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Info(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Info, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        public static void Info(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            SAddLog(LogLevel.Info, null, ex, eventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Info(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Info, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        public static void Warn(string formatMsg, params object[] args)
        {
            SAddLog(LogLevel.Warn, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Warn(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Warn, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        public static void Warn(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            SAddLog(LogLevel.Warn, null, ex, eventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Warn(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Warn, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        public static void Error(string formatMsg, params object[] args)
        {
            SAddLog(LogLevel.Error, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Error(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Error, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        public static void Error(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            SAddLog(LogLevel.Error, null, ex, eventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Error(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Error, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Faltal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        public static void Faltal(string formatMsg, params object[] args)
        {
            SAddLog(LogLevel.Fatal, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Faltal(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Fatal, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        public static void Faltal(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            SAddLog(LogLevel.Fatal, null, ex, eventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        public static void Faltal(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            SAddLog(LogLevel.Fatal, formatMsg, ex, eventID, args);
        }
        #endregion
        #endregion
    }
}
