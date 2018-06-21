using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log.Config;
using UtilZ.Dotnet.Ex.Log.Config.Core;
using UtilZ.Dotnet.Ex.Log.Core;
using UtilZ.Dotnet.Ex.Log.Layout;
using UtilZ.Dotnet.Ex.Log.LogOutput;
using UtilZ.Dotnet.Ex.Log.LogRecorderInterface;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public sealed class Loger
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Loger()
        {
            try
            {
                //加载默认配置
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ParseConfig(config);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(null, ex);
            }
        }

        /// <summary>
        /// 日志线程锁
        /// </summary>
        private static readonly object _logMonitor = new object();

        /// <summary>
        /// 日志实例,默认为XLog
        /// </summary>
        private static ILog _log;

        /// <summary>
        /// 获取或设置日志实例
        /// </summary>
        public static ILog Log
        {
            get { return _log; }
            set
            {
                lock (_logMonitor)
                {
                    if (_log != null)
                    {
                        _log.Stop();
                    }

                    _log = value;
                    if (_log != null)
                    {
                        try
                        {
                            _log.Start();
                        }
                        catch (Exception ex)
                        {
                            LogSysInnerLog.OnRaiseLog(null, ex);
                        }
                    }
                    else
                    {
                        _log = new NullLog();
                        _log.Start();
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置日志级别
        /// </summary>
        public static LogLevel Level
        {
            get
            {
                return _log.Level;
            }
            set
            {
                _log.Level = value;
            }
        }

        /// <summary>
        /// 生成日志扩展信息接口
        /// </summary>
        private static IGenerateExtendInfo _generateExtendInfo = null;

        /// <summary>
        /// 获取或设置是否自动生成日志扩展信息
        /// </summary>
        public static void SetAutoGenerateExtendInfo(IGenerateExtendInfo generateExtendInfo)
        {
            _generateExtendInfo = generateExtendInfo;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        public static LogOutputCenter LogOutput
        {
            get { return LogOutputCenter.Instance; }
        }

        #region 解析配置并创建日志记录器
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public static void LoadConfig(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                return;
            }

            var exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = configFilePath;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            ParseConfig(config);
        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="config">配置</param>
        private static void ParseConfig(System.Configuration.Configuration config)
        {
            if (config == null)
            {
                return;
            }

            try
            {
                //从配置中获取日志配置参数节点
                LogConfigSection logConfig = config.GetSection(LogConstant.ConfigSectionName) as LogConfigSection;
                //根据配置创建日志记录器
                CreateLogRecoder(logConfig);
            }
            catch (Exception ex)
            {
                //创建默认日志记录器
                Loger.CreateDefaultLogRecoder();
                LogSysInnerLog.OnRaiseLog(null, ex);
            }
        }

        /// <summary>
        /// 根据配置创建日志记录器
        /// </summary>
        /// <param name="logConfig">配置参数</param>
        public static void CreateLogRecoder(LogConfigSection logConfig)
        {
            if (logConfig == null || string.IsNullOrEmpty(logConfig.LogRecorderType) || logConfig.Count == 0)
            {
                logConfig = Loger.GetDefaultLogConfigSection();
            }

            try
            {
                object instance = LogUtil.CreateInstance(logConfig.LogRecorderType);
                ILog log = instance as ILog;
                if (log != null)
                {
                    if (log.GetType() == typeof(XLog))
                    {
                        ((XLog)log).ParseConfig(logConfig);
                    }

                    if (!string.IsNullOrEmpty(logConfig.ConfigFilePath))
                    {
                        string configFilePath;
                        if (string.IsNullOrEmpty(System.IO.Path.GetPathRoot(logConfig.ConfigFilePath)))
                        {
                            configFilePath = System.IO.Path.Combine(Path.GetDirectoryName(log.GetType().Assembly.Location), logConfig.ConfigFilePath);
                        }
                        else
                        {
                            configFilePath = logConfig.ConfigFilePath;
                        }

                        if (File.Exists(configFilePath))
                        {
                            log.LoadConfig(configFilePath);
                        }
                    }
                }

                Log = log;
            }
            catch (Exception ex)
            {
                //创建默认日志记录器
                Loger.CreateDefaultLogRecoder();
                LogSysInnerLog.OnRaiseLog(null, ex);
            }
        }

        /// <summary>
        /// 创建默认日志记录器
        /// </summary>
        private static void CreateDefaultLogRecoder()
        {
            try
            {
                //默认为空日志
                if (Loger.Log == null)
                {
                    Loger.Log = new NullLog();
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(typeof(Loger).Name, ex);
            }
        }

        /// <summary>
        /// 获取默认日志配置节
        /// </summary>
        /// <returns>默认日志配置节</returns>
        private static LogConfigSection GetDefaultLogConfigSection()
        {
            var logConfig = new LogConfigSection();
            logConfig.LogRecorderType = typeof(XLog).AssemblyQualifiedName;
            logConfig.Level = UtilZ.Dotnet.Ex.Log.Model.LogLevel.Debug;
            logConfig.ConfigFilePath = null;

            var defaultFileLog = new UtilZ.Dotnet.Ex.Log.Config.Framework.FileLogConfigElement();
            defaultFileLog.Name = LogConstant.DefaultLogRecorderName;
            defaultFileLog.Days = 7;
            //defaultFileLog.Layout = string.Format("%d {0} 线程:{1} 事件ID:{2} 信息:{3} 位置:{4}",
            //    LayoutManager.LEVEL, LayoutManager.THREAD, LayoutManager.EVENT, LayoutManager.CONTENT, LayoutManager.POSITION);
            defaultFileLog.LogFileSize = 10;
            defaultFileLog.LogDirectory = System.IO.Path.Combine(Path.GetDirectoryName(typeof(XLog).Assembly.Location), @"Log");
            defaultFileLog.SecurityPolicy = null;
            logConfig.FileLogConfig.Add(defaultFileLog);
            return logConfig;
        }
        #endregion

        #region 记录日志方法
        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        private static void AddLog(LogLevel level, string msg, Exception ex, string name, int eventID, object extendInfo)
        {
            try
            {
                ILog log = _log;
                if (log == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = LogConstant.DefaultLogRecorderName;
                }

                if (extendInfo == null && _generateExtendInfo != null)
                {
                    try
                    {
                        extendInfo = _generateExtendInfo.GetExtendInfo();
                    }
                    catch (Exception exi)
                    {
                        LogSysInnerLog.OnRaiseLog(null, exi);
                    }
                }

                switch (level)
                {
                    case LogLevel.Debug:
                        log.Debug(msg, ex, name, eventID, extendInfo);
                        break;
                    case LogLevel.Error:
                        log.Error(msg, ex, name, eventID, extendInfo);
                        break;
                    case LogLevel.Faltal:
                        log.Faltal(msg, ex, name, eventID, extendInfo);
                        break;
                    case LogLevel.Info:
                        log.Info(msg, ex, name, eventID, extendInfo);
                        break;
                    case LogLevel.Warn:
                        log.Warn(msg, ex, name, eventID, extendInfo);
                        break;
                    default:
                        throw new NotSupportedException(string.Format("不支持的类型{0}", level.ToString()));
                }
            }
            catch (Exception logEx)
            {
                LogSysInnerLog.OnRaiseLog(null, logEx);
            }
        }

        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Debug(string msg)
        {
            AddLog(LogLevel.Debug, msg, null, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Debug(string msg, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Debug, msg, null, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Debug(string msg, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Debug, msg, null, name, eventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Debug(Exception ex)
        {
            AddLog(LogLevel.Debug, null, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Debug(Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Debug, null, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Debug(Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Debug, null, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public static void Debug(string msg, Exception ex)
        {
            AddLog(LogLevel.Debug, msg, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Debug(string msg, Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Debug, msg, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Debug(string msg, Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Debug, msg, ex, name, eventID, extendInfo);
        }
        #endregion

        #region Info
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Info(string msg)
        {
            AddLog(LogLevel.Info, msg, null, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Info(string msg, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Info, msg, null, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Info(string msg, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Info, msg, null, name, eventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Info(Exception ex)
        {
            AddLog(LogLevel.Info, null, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Info(Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Info, null, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Info(Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Info, null, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public static void Info(string msg, Exception ex)
        {
            AddLog(LogLevel.Info, msg, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Info(string msg, Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Info, msg, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Info(string msg, Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Info, msg, ex, name, eventID, extendInfo);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Warn(string msg)
        {
            AddLog(LogLevel.Warn, msg, null, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Warn(string msg, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Warn, msg, null, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Warn(string msg, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Warn, msg, null, name, eventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Warn(Exception ex)
        {
            AddLog(LogLevel.Warn, null, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Warn(Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Warn, null, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Warn(Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Warn, null, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public static void Warn(string msg, Exception ex)
        {
            AddLog(LogLevel.Warn, msg, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Warn(string msg, Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Warn, msg, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Warn(string msg, Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Warn, msg, ex, name, eventID, extendInfo);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Error(string msg)
        {
            AddLog(LogLevel.Error, msg, null, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Error(string msg, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Error, msg, null, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Error(string msg, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Error, msg, null, name, eventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Error(Exception ex)
        {
            AddLog(LogLevel.Error, null, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Error(Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Error, null, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Error(Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Error, null, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public static void Error(string msg, Exception ex)
        {
            AddLog(LogLevel.Error, msg, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Error(string msg, Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Error, msg, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Error(string msg, Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Error, msg, ex, name, eventID, extendInfo);
        }
        #endregion

        #region Faltal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Faltal(string msg)
        {
            AddLog(LogLevel.Faltal, msg, null, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Faltal(string msg, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Faltal, msg, null, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Faltal(string msg, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Faltal, msg, null, name, eventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Faltal(Exception ex)
        {
            AddLog(LogLevel.Faltal, null, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Faltal(Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Faltal, null, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>        
        /// <param name="extendInfo">扩展信息</param>
        public static void Faltal(Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Faltal, null, ex, name, eventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public static void Faltal(string msg, Exception ex)
        {
            AddLog(LogLevel.Faltal, msg, ex, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Faltal(string msg, Exception ex, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Faltal, msg, ex, name, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="extendInfo">扩展信息</param>
        public static void Faltal(string msg, Exception ex, int eventID = 0, string name = null, object extendInfo = null)
        {
            AddLog(LogLevel.Faltal, msg, ex, name, eventID, extendInfo);
        }
        #endregion
        #endregion
    }
}
