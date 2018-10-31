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
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public sealed class Loger : LogerBase
    {
        #region 静态成员
        private static readonly LogerBase _emptyLoger = new EmptyLoger();
        private static ILoger _defaultLoger;

        /// <summary>
        /// [key:LogerName;Value:Loger]
        /// </summary>
        private static readonly Hashtable _htLoger = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 静态构造函数(初始化默认日志追加器)
        /// </summary>
        static Loger()
        {
            const string logConfigFileName = LogConstant.DefaultConfigFileName;
            if (File.Exists(logConfigFileName))
            {
                LoadConfig(logConfigFileName);
            }
            else
            {
                var defaultLoger = new Loger();
                defaultLoger._appenders.Add(new FileAppender(new FileAppenderConfig(null)));
                _defaultLoger = defaultLoger;
            }
        }

        /// <summary>
        /// 清空所有配置,包括默认
        /// </summary>
        public static void Clear()
        {
            lock (_htLoger.SyncRoot)
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

            try
            {
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
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("加载配置文件异常", ex);
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
                    lock (_htLoger.SyncRoot)
                    {
                        if (_htLoger.ContainsKey(name))
                        {
                            ((ILoger)_htLoger[name]).Dispose();
                        }

                        _htLoger[name] = loger;
                    }
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

                appenderTypeName = appenderTypeName.Trim();
                AppenderBase appender;
                if (appenderTypeName.Length == 1)
                {
                    appender = CreateAppenderByAppenderPattern(appenderTypeName, appenderEle);
                }
                else
                {
                    if (!appenderTypeName.Contains('.') && !appenderTypeName.Contains(','))
                    {
                        Type appenderBaseType = typeof(AppenderBase);
                        appenderTypeName = string.Format("{0}.{1},{2}", appenderBaseType.Namespace, appenderTypeName, Path.GetFileName(appenderBaseType.Assembly.Location));
                    }

                    Type appenderType = LogUtil.GetType(appenderTypeName);
                    if (appenderType == null)
                    {
                        return;
                    }

                    appender = Activator.CreateInstance(appenderType, new object[] { (object)appenderEle }) as AppenderBase;
                }

                if (appender == null)
                {
                    return;
                }

                appender.Name = appenderName;
                loger._appenders.Add(appender);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(string.Format("解析:{0}日志追加器异常", appenderName), ex);
            }
        }

        private static AppenderBase CreateAppenderByAppenderPattern(string appenderTypeName, XElement appenderEle)
        {
            AppenderBase appender;
            switch (appenderTypeName[0])
            {
                case LogConstant.FileAppenderPattern:
                    appender = new FileAppender(appenderEle);
                    break;
                case LogConstant.RedirectAppenderPattern:
                    appender = new RedirectAppender(appenderEle);
                    break;
                case LogConstant.ConsoleAppenderPattern:
                    appender = new ConsoleAppender(appenderEle);
                    break;
                case LogConstant.DatabaseAppenderPattern:
                    appender = new DatabaseAppender(appenderEle);
                    break;
                case LogConstant.MailAppenderPattern:
                    appender = new MailAppender(appenderEle);
                    break;
                case LogConstant.SystemAppenderPattern:
                    appender = new SystemLogAppender(appenderEle);
                    break;
                default:
                    appender = null;
                    break;
            }

            return appender;
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
                lock (_htLoger.SyncRoot)
                {
                    loger = _htLoger[logerName] as ILoger;
                }
            }

            if (loger == null)
            {
                loger = _emptyLoger;
            }

            return loger;
        }

        /// <summary>
        /// 添加日志记录器
        /// </summary>
        /// <param name="loger">日志记录器</param>
        public static void AddLoger(ILoger loger)
        {
            if (loger == null)
            {
                return;
            }

            string logerName = loger.Name;
            if (string.IsNullOrEmpty(logerName))
            {
                if (_defaultLoger != null)
                {
                    _defaultLoger.Dispose();
                }

                _defaultLoger = loger;
            }
            else
            {
                lock (_htLoger.SyncRoot)
                {
                    if (_htLoger.ContainsKey(logerName))
                    {
                        ((ILoger)_htLoger[logerName]).Dispose();
                    }

                    _htLoger[logerName] = loger;
                }
            }
        }
        #endregion

        #region 日志记录器实例成员
        /// <summary>
        /// 日志分发线程队列
        /// </summary>
        private readonly LogAsynQueue<LogItem> _logDispatcherQueue;

        private Loger() : base()
        {
            this._logDispatcherQueue = new LogAsynQueue<LogItem>(this.RecordLogCallback, "日志分发线程");
        }

        private void RecordLogCallback(LogItem item)
        {
            AppenderBase[] appenders;
            lock (base._appendersLock)
            {
                appenders = base._appenders.ToArray();
            }

            item.LogProcess();
            foreach (var appender in appenders)
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

        #region ILoger接口
        /// <summary>
        /// 静态方法添加日志的方法
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        internal override void ObjectAddLog(LogLevel level, int eventId, object tag, Exception ex, string format, params object[] args)
        {
            this.PrimitiveAddLog(5, level, eventId, tag, ex, format, args);
        }

        /// <summary>
        /// 实例添加日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        protected override void PrimitiveAddLog(int skipFrames, LogLevel level, int eventId, object tag, Exception ex, string format, params object[] args)
        {
            try
            {
                if (!this.Enable || level < this.Level)
                {
                    return;
                }

                var item = new LogItem(DateTime.Now, Thread.CurrentThread, skipFrames, true, this.Name, level, eventId, tag, ex, format, args);
                this._logDispatcherQueue.Enqueue(item);
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
            this._logDispatcherQueue.Dispose();
            lock (this._appendersLock)
            {
                foreach (var appender in this._appenders)
                {
                    try
                    {
                        appender.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogSysInnerLog.OnRaiseLog(this, ex);
                    }
                }
            }
        }
        #endregion

        #region 静态记录日志方法,默认日志快捷方法
        private static void SAddLog(LogLevel level, int eventId, object tag, Exception ex, string format, params object[] args)
        {
            try
            {
                var loger = _defaultLoger as LogerBase;
                if (loger == null)
                {
                    return;
                }

                loger.ObjectAddLog(level, eventId, tag, ex, format, args);
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
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Trace(string format, params object[] args)
        {
            SAddLog(LogLevel.Trace, LogConstant.DefaultEventId, null, null, format, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Trace(int eventId, object tag, string format, params object[] args)
        {
            SAddLog(LogLevel.Trace, eventId, tag, null, format, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        public static void Trace(Exception ex, int eventId = LogConstant.DefaultEventId, object tag = null)
        {
            SAddLog(LogLevel.Trace, eventId, tag, ex, null);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Trace(Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Trace, LogConstant.DefaultEventId, null, ex, format, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常信息</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Trace(int eventId, object tag, Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Trace, eventId, tag, ex, format, args);
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Debug(string format, params object[] args)
        {
            SAddLog(LogLevel.Debug, LogConstant.DefaultEventId, null, null, format, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Debug(int eventId, object tag, string format, params object[] args)
        {
            SAddLog(LogLevel.Debug, eventId, tag, null, format, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        public static void Debug(Exception ex, int eventId = LogConstant.DefaultEventId, object tag = null)
        {
            SAddLog(LogLevel.Debug, eventId, tag, ex, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Debug(Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Debug, LogConstant.DefaultEventId, null, ex, format, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常信息</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Debug(int eventId, object tag, Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Debug, eventId, tag, ex, format, args);
        }
        #endregion

        #region Info
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Info(string format, params object[] args)
        {
            SAddLog(LogLevel.Info, LogConstant.DefaultEventId, null, null, format, args);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Info(int eventId, object tag, string format, params object[] args)
        {
            SAddLog(LogLevel.Info, eventId, tag, null, format, args);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        public static void Info(Exception ex, int eventId = LogConstant.DefaultEventId, object tag = null)
        {
            SAddLog(LogLevel.Info, eventId, tag, ex, null);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Info(Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Info, LogConstant.DefaultEventId, null, ex, format, args);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常信息</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Info(int eventId, object tag, Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Info, eventId, tag, ex, format, args);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Warn(string format, params object[] args)
        {
            SAddLog(LogLevel.Warn, LogConstant.DefaultEventId, null, null, format, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Warn(int eventId, object tag, string format, params object[] args)
        {
            SAddLog(LogLevel.Warn, eventId, tag, null, format, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常警告</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        public static void Warn(Exception ex, int eventId = LogConstant.DefaultEventId, object tag = null)
        {
            SAddLog(LogLevel.Warn, eventId, tag, ex, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常警告</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Warn(Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Warn, LogConstant.DefaultEventId, null, ex, format, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常警告</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Warn(int eventId, object tag, Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Warn, eventId, tag, ex, format, args);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Error(string format, params object[] args)
        {
            SAddLog(LogLevel.Error, LogConstant.DefaultEventId, null, null, format, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Error(int eventId, object tag, string format, params object[] args)
        {
            SAddLog(LogLevel.Error, eventId, tag, null, format, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常错误</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        public static void Error(Exception ex, int eventId = LogConstant.DefaultEventId, object tag = null)
        {
            SAddLog(LogLevel.Error, eventId, tag, ex, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常错误</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Error(Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Error, LogConstant.DefaultEventId, null, ex, format, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常错误</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Error(int eventId, object tag, Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Error, eventId, tag, ex, format, args);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Fatal(string format, params object[] args)
        {
            SAddLog(LogLevel.Fatal, LogConstant.DefaultEventId, null, null, format, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Fatal(int eventId, object tag, string format, params object[] args)
        {
            SAddLog(LogLevel.Fatal, eventId, tag, null, format, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常致命</param>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        public static void Fatal(Exception ex, int eventId = LogConstant.DefaultEventId, object tag = null)
        {
            SAddLog(LogLevel.Fatal, eventId, tag, ex, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常致命</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Fatal(Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Fatal, LogConstant.DefaultEventId, null, ex, format, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="tag">与对象关联的用户定义数据</param>
        /// <param name="ex">异常致命</param>
        /// <param name="format">复合格式字符串,参数为空或null表示无格式化</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象</param>
        public static void Fatal(int eventId, object tag, Exception ex, string format, params object[] args)
        {
            SAddLog(LogLevel.Fatal, eventId, tag, ex, format, args);
        }
        #endregion
        #endregion
    }
}
