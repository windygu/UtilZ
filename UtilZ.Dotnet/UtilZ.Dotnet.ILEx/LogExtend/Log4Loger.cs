using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Config.Framework;
using UtilZ.Dotnet.Ex.Log.Config.Interface;
using UtilZ.Dotnet.Ex.Log.Layout;
using UtilZ.Dotnet.Ex.Log.LogRecorder;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.ILEx.LogExtend
{
    /// <summary>
    /// Log4日志记录器
    /// </summary>
    public class Log4Loger : BaseLoger
    {
        /// <summary>
        /// log4net日志记录器名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">log4net日志记录器名称</param>
        public Log4Loger(string name)
        {
            this._name = name;
        }

        /// <summary>
        /// log4net配置
        /// </summary>
        private FileLogConfigElement _log4netConfig = new FileLogConfigElement();

        /// <summary>
        /// 基础配置
        /// </summary>
        public override IConfig BaseConfig
        {
            get
            {
                return _log4netConfig;
            }
            set
            {
                _log4netConfig = value as FileLogConfigElement;
                if (_log4netConfig == null)
                {
                    _log4netConfig = new FileLogConfigElement();
                }
            }
        }

        /// <summary>
        /// 内部日志添加到ILoger记录事件
        /// </summary>
        public event EventHandler<Log4InnerLogAddedILogerArgs> AddIlogerLog;

        /// <summary>
        /// ILoger日志记录事件记录日志信息
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        protected override void OnRaiseAddIlogerLog(LogLevel level, string msg, Exception ex, int eventID, object extendInfo)
        {
            int skipFrames;
            Log4ExtendInfo log4ExtendInfo = extendInfo as Log4ExtendInfo;
            if (log4ExtendInfo != null)
            {
                skipFrames = 7;
                extendInfo = log4ExtendInfo.ExtendInfo;
            }
            else
            {
                skipFrames = 3;
            }

            //创建日志项
            var item = new LogItem(DateTime.Now, System.Threading.Thread.CurrentThread, skipFrames, level, msg, ex, this._name, eventID, extendInfo);

            //log4net记录日志
            this.RecordLog(this.BaseConfig, item);

            //日志框架输出记录的日志
            this.LogFrameworkOutput(item);
        }

        /// <summary>
        /// 日志框架输出记录的日志
        /// </summary>
        /// <param name="item">要输出的日志项</param>
        private void LogFrameworkOutput(LogItem item)
        {
            try
            {
                var handler = this.AddIlogerLog;
                if (handler != null)
                {
                    handler(this, new Log4InnerLogAddedILogerArgs(item));
                }
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this, exi);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="config">配置项</param>
        /// <param name="item">日志项</param>
        private void RecordLog(IConfig config, LogItem item)
        {
            try
            {
                // 获取log4net日志记录器
                log4net.ILog log;
                string name = item.Name;//日志记录器名称
                if (string.IsNullOrEmpty(name))
                {
                    name = LogConstant.DefaultLogRecorderName;
                }

                //输出日志
                this.OutputLog(name, item);

                log = log4net.LogManager.GetLogger(name);
                if (log == null)
                {
                    return;
                }

                item.LogProcess();
                string content = LayoutManager.LayoutLog(item, config);
                switch (item.Level)
                {
                    case LogLevel.Debug:
                        log.Debug(content);
                        break;
                    case LogLevel.Info:
                        log.Info(content);
                        break;
                    case LogLevel.Warn:
                        log.Warn(content);
                        break;
                    case LogLevel.Error:
                        log.Error(content);
                        break;
                    case LogLevel.Faltal:
                        log.Fatal(content);
                        break;
                }
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this.GetType().FullName, exi);
            }
        }
    }
}
