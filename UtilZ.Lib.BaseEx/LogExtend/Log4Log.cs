using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Base.Log.LogRecorderInterface;
using UtilZ.Lib.Base.Log.Model;

namespace UtilZ.Lib.BaseEx.LogExtend
{
    /// <summary>
    /// Log4日志记录类
    /// </summary>
    public class Log4Log : Base.Log.Core.BaseLog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Log4Log()
        {

        }

        /// <summary>
        /// 开启日志记录
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// 停止日志记录
        /// </summary>
        public override void Stop()
        {
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public override void LoadConfig(string configFilePath)
        {
            var fileInfo = new System.IO.FileInfo(configFilePath);
            if (!fileInfo.Exists)
            {
                return;
            }

            log4net.Config.XmlConfigurator.Configure(fileInfo);
        }

        /// <summary>
        /// 已加载的Log4net日志记录器集合[key:类型,value:log4net日志记录对象(log4net.ILog)]
        /// </summary>
        private readonly Hashtable _htLogers = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 已加载的Log4net日志记录器集合线程锁
        /// </summary>
        private readonly object _htLogersMonitor = new object();

        /// <summary>
        /// 根据日志记录器名称获取日志记录器
        /// </summary>
        /// <param name="name">日志记录器名称</param>
        /// <returns>日志记录器</returns>
        public override ILoger GetLoger(string name)
        {
            Log4Loger loger = _htLogers[name] as Log4Loger;
            if (loger == null)
            {
                lock (this._htLogersMonitor)
                {
                    loger = _htLogers[name] as Log4Loger;
                    if (loger == null)
                    {
                        loger = new Log4Loger(name);
                        //ILoger日志记录事件记录日志信息
                        //loger.AddIlogerLog += (s, e) => { this.OnRaiseLogOutput(e.Item); };
                        _htLogers[name] = loger;
                    }
                }
            }

            return loger;
        }

        #region 记录日志方法
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        private void RecordLog(LogLevel level, string msg, Exception ex, string name, int eventID, object extendInfo)
        {
            ILoger log = this.GetLoger(name);
            extendInfo = new Log4ExtendInfo(extendInfo);
            switch (level)
            {
                case LogLevel.Debug:
                    log.Debug(msg, ex, eventID, extendInfo);
                    break;
                case LogLevel.Info:
                    log.Info(msg, ex, eventID, extendInfo);
                    break;
                case LogLevel.Warn:
                    log.Warn(msg, ex, eventID, extendInfo);
                    break;
                case LogLevel.Error:
                    log.Error(msg, ex, eventID, extendInfo);
                    break;
                case LogLevel.Faltal:
                    log.Faltal(msg, ex, eventID, extendInfo);
                    break;
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
            this.RecordLog(LogLevel.Debug, msg, ex, name, eventID, extendInfo);
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
            this.RecordLog(LogLevel.Info, msg, ex, name, eventID, extendInfo);
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
            this.RecordLog(LogLevel.Warn, msg, ex, name, eventID, extendInfo);
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
            this.RecordLog(LogLevel.Error, msg, ex, name, eventID, extendInfo);
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
            this.RecordLog(LogLevel.Faltal, msg, ex, name, eventID, extendInfo);
        }
        #endregion
    }
}
