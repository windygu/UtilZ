using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Core;
using UtilZ.Lib.Base.NLog.Config.Framework;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.LogRecorder;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

////////////////////////////////////////////////////////////////////
//                          _ooOoo_                               //
//                         o8888888o                              //
//                         88" . "88                              //
//                         (| ^_^ |)                              //
//                         O\  =  /O                              //
//                      ____/`---'\____                           //
//                    .'  \\|     |//  `.                         //
//                   /  \\|||  :  |||//  \                        //
//                  /  _||||| -:- |||||-  \                       //
//                  |   | \\\  -  /// |   |                       //
//                  | \_|  ''\---/''  |   |                       //
//                  \  .-\__  `-`  ___/-. /                       //
//                ___`. .'  /--.--\  `. . ___                     //
//              ."" '<  `.___\_<|>_/___.'  >'"".                  //
//            | | :  `- \`.;`\ _ /`;.`/ - ` : | |                 //
//            \  \ `-.   \_ __\ /__ _/   .-` /  /                 //
//      ========`-.____`-.___\_____/___.-`____.-'========         //
//                           `=---='                              //
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //
//         佛祖保佑       永无BUG     永不修改                    //
////////////////////////////////////////////////////////////////////

namespace UtilZ.Lib.Base.NLog.Core
{
    /// <summary>
    /// 内部Log
    /// </summary>
    public class XLog : BaseLog
    {
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
        /// 日志记录器字典集合[key:日志名称;value:日志记录器]
        /// </summary>
        private readonly ConcurrentDictionary<string, ILogRecorder> _dicLogRecorders = new ConcurrentDictionary<string, ILogRecorder>();

        /// <summary>
        /// 日志记录器字典集合线程锁
        /// </summary>
        private readonly object _dicLogRecordersMonitor = new object();

        /// <summary>
        /// 空日志对象
        /// </summary>
        private NullLoger _nullLoger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public XLog()
            : base()
        {

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
            this._logQueueThread.Name = "日志线程";
            this._logQueueThread.Start();
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
            try
            {
                if (!File.Exists(configFilePath))
                {
                    return;
                }

                var exeConfigurationFileMap = new ExeConfigurationFileMap();
                exeConfigurationFileMap.ExeConfigFilename = configFilePath;
                System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
                LogConfigSection logConfig = config.GetSection(LogConstant.ConfigSectionName) as LogConfigSection;
                this.ParseConfig(logConfig);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="logConfig">配置参数</param>
        internal void ParseConfig(LogConfigSection logConfig)
        {
            lock (this._dicLogRecordersMonitor)
            {
                this.InnerClearLogRecorder();
                if (logConfig == null)
                {
                    return;
                }

                this.Level = logConfig.Level;
                this.SkipFrames = logConfig.SkipFrames;

                //系统日志
                this.CreateLogRecorder<SystemLogRecorder, ISystemLogConfig>(logConfig.SystemLogConfig);

                //文件日志
                this.CreateLogRecorder<FileLogRecorder, IFileLogConfig>(logConfig.FileLogConfig);

                //数据库日志
                this.CreateLogRecorder<DatabaseLogRecorder, IDatabaseLogConfig>(logConfig.DatabaseLogConfig);

                //邮件日志
                this.CreateLogRecorder<EmailLogRecorder, IEmailLogConfig>(logConfig.MailLogConfig);
            }
        }

        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <typeparam name="T">日志记录器类型</typeparam>
        /// <typeparam name="W">配置集合中的日志配置类型</typeparam>
        /// <param name="configCollection">配置集合</param>
        private void CreateLogRecorder<T, W>(ConfigurationElementCollection configCollection)
            where T : class, ILogRecorder, new()
            where W : IConfig
        {
            try
            {
                if (configCollection == null || configCollection.Count == 0)
                {
                    return;
                }

                ILogRecorder logRecorder = null;
                foreach (W config in configCollection)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(config.ExtendLogRecorderType))
                        {
                            logRecorder = new T();
                        }
                        else
                        {
                            logRecorder = LogUtil.CreateInstance(config.ExtendLogRecorderType) as ILogRecorder;
                        }

                        if (logRecorder != null)
                        {
                            try
                            {
                                //创建日志追加记录器
                                if (!string.IsNullOrWhiteSpace(config.LogAppenderType))
                                {
                                    logRecorder.LogAppender = LogUtil.CreateInstance(config.LogAppenderType) as ILogRecorder;
                                    if (logRecorder.LogAppender != null)
                                    {
                                        logRecorder.LogAppender.BaseConfig = config;
                                    }
                                }
                            }
                            catch (Exception exii)
                            {
                                LogSysInnerLog.OnRaiseLog(this, exii);
                            }

                            logRecorder.BaseConfig = config;
                            this.AddLogRecorderInner(logRecorder);
                        }
                    }
                    catch (Exception exi)
                    {
                        LogSysInnerLog.OnRaiseLog(this, exi);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }

        /// <summary>
        /// 根据日志记录器名称获取日志记录器
        /// </summary>
        /// <param name="name">日志记录器名称</param>
        /// <returns>日志记录器</returns>
        public override ILoger GetLoger(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = LogConstant.DefaultLogRecorderName;
            }

            ILoger loger;
            lock (this._dicLogRecordersMonitor)
            {
                if (this._dicLogRecorders.ContainsKey(name))
                {
                    loger = this._dicLogRecorders[name];
                }
                else
                {
                    if (this._nullLoger == null)
                    {
                        this._nullLoger = new NullLoger();
                    }

                    loger = this._nullLoger;
                }
            }

            return loger;
        }

        /// <summary>
        /// 添加日志记录器
        /// </summary>
        /// <param name="config">配置[继承自接口IFileLogConfig,ISystemLogConfig,IDatabaseLogConfig之一]</param>
        public void AddLogRecorder(IConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config", "配置不能为null");
            }

            Type configType = config.GetType();
            ILogRecorder logRecorder = null;
            if (configType.GetInterface(typeof(IFileLogConfig).FullName) != null)
            {
                logRecorder = new FileLogRecorder();
            }
            else if (configType.GetInterface(typeof(ISystemLogConfig).FullName) != null)
            {
                logRecorder = new SystemLogRecorder();
            }
            else if (configType.GetInterface(typeof(IDatabaseLogConfig).FullName) != null)
            {
                logRecorder = new DatabaseLogRecorder();
            }
            else if (configType.GetInterface(typeof(IEmailLogConfig).FullName) != null)
            {
                logRecorder = new EmailLogRecorder();
            }
            else
            {
                throw new ArgumentException(string.Format("配置必须继承接口IFileLogConfig,ISystemLogConfig,IDatabaseLogConfig之一配置类型{0}无效", configType.FullName));
            }

            logRecorder.BaseConfig = config;
            this.AddLogRecorder(logRecorder);
        }

        /// <summary>
        /// 添加日志记录器
        /// </summary>
        /// <param name="logRecorder">日志记录器</param>
        public void AddLogRecorder(ILogRecorder logRecorder)
        {
            lock (this._dicLogRecordersMonitor)
            {
                this.AddLogRecorderInner(logRecorder);
            }
        }

        /// <summary>
        /// 内部添加日志记录器
        /// </summary>
        /// <param name="logRecorder">日志记录器</param>
        private void AddLogRecorderInner(ILogRecorder logRecorder)
        {
            if (logRecorder == null)
            {
                return;
            }

            string key = logRecorder.BaseConfig.Name;
            if (string.IsNullOrEmpty(key))
            {
                key = LogConstant.DefaultLogRecorderName;
            }

            if (this._dicLogRecorders.ContainsKey(key))
            {
                ILogRecorder oldLoger = this._dicLogRecorders[key];
                oldLoger.InnerLogAddedIloger -= AddIlogerLog;
            }

            logRecorder.InnerLogAddedIloger += AddIlogerLog;
            this._dicLogRecorders[key] = logRecorder;
        }

        /// <summary>
        /// 清空日志记录器
        /// </summary>
        public void ClearLogRecorder()
        {
            lock (this._dicLogRecordersMonitor)
            {
                this.InnerClearLogRecorder();
            }
        }

        /// <summary>
        /// 清空日志记录器
        /// </summary>
        private void InnerClearLogRecorder()
        {
            foreach (var loger in this._dicLogRecorders.Values)
            {
                loger.InnerLogAddedIloger -= AddIlogerLog;
            }

            this._dicLogRecorders.Clear();
        }

        /// <summary>
        /// 移除日志记录器
        /// </summary>
        /// <param name="name">日志记录器名称</param>
        public void RemoveLogRecorder(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            lock (this._dicLogRecordersMonitor)
            {
                if (this._dicLogRecorders.ContainsKey(name))
                {
                    ILogRecorder logRecorder;
                    if (this._dicLogRecorders.TryRemove(name, out logRecorder))
                    {
                        if (logRecorder != null)
                        {
                            logRecorder.InnerLogAddedIloger -= AddIlogerLog;
                        }
                    }
                }
            }
        }

        #region 日志记录线程
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

                    if (this._logQueue.TryDequeue(out item))
                    {
                        item.LogProcess();
                        this.RecordLog(item);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="item">日志项</param>
        private void RecordLog(LogItem item)
        {
            try
            {
                ILogRecorder logRecorder;
                if (this._dicLogRecorders.TryGetValue(item.Name, out logRecorder))
                {
                    if (logRecorder != null)
                    {
                        logRecorder.WriteLog(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }
        #endregion

        #region 记录日志方法
        /// <summary>
        /// 内部日志添加到ILoger记录
        /// </summary>
        /// <param name="sender">日志产生者</param>
        /// <param name="e">内部日志添加到ILoger记录事件参数</param>
        private void AddIlogerLog(object sender, InnerLogAddedIlogerArgs e)
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
                    logInfo = new LogItem(DateTime.Now, Thread.CurrentThread, 4 + this.SkipFrames, e.Level, e.Message, e.Exception, e.Name, e.EventID, e.ExtendInfo);
                }
                else
                {
                    logInfo = new LogItem(DateTime.Now, Thread.CurrentThread, ex.StackTrace, e.Level, e.Message, e.Exception, e.Name, e.EventID, e.ExtendInfo);
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
                    logInfo = new LogItem(DateTime.Now, Thread.CurrentThread, ex.StackTrace, level, msg, ex, name, eventID, extendInfo);
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

        /// <summary>
        /// 释放资源方法
        /// </summary>
        /// <param name="isDispose">是否释放标识</param>
        protected override void Dispose(bool isDispose)
        {
            base.Dispose(isDispose);
            this._cts.Dispose();
            this._autoResetEvent.Dispose();
        }
    }
}
