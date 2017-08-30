using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.Layout;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.LogRecorder
{
    /// <summary>
    /// 系统日志记录器
    /// </summary>
    public class SystemLogRecorder : BaseLogRecorder, ISystemLogRecorder
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SystemLogRecorder()
        {
            ////获取计算机名称,上面这个在加载自定义配置时会报错
            ////SystemLogRecorder._machineName = System.Net.Dns.GetHostName();
            //SystemLogRecorder._machineName = Environment.MachineName;
            SystemLogRecorder._applicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
            {
                SystemLogRecorder.MAX_EVENTLOG_MESSAGE_SIZE = MAX_EVENTLOG_MESSAGE_SIZE_VISTA_OR_NEWER;
            }
            else
            {
                SystemLogRecorder.MAX_EVENTLOG_MESSAGE_SIZE = MAX_EVENTLOG_MESSAGE_SIZE_DEFAULT;
            }
        }

        ///// <summary>
        ///// 日志所在的计算机
        ///// </summary>
        //private static string _machineName;

        /// <summary>
        /// 应用程序名称
        /// </summary>
        private static string _applicationName;

        /// <summary>
        /// The maximum size supported by default.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/xzwc042w(v=vs.100).aspx
        /// The 32766 documented max size is two bytes shy of 32K (I'm assuming 32766 
        /// may leave space for a two byte null terminator of #0#0). The 32766 max 
        /// length is what the .NET 4.0 source code checks for, but this is WRONG! 
        /// Strings with a length > 31839 on Windows Vista or higher can CORRUPT 
        /// the event log! See: System.Diagnostics.EventLogInternal.InternalWriteEvent() 
        /// for the use of the 32766 max size.
        /// </remarks>
        private readonly static int MAX_EVENTLOG_MESSAGE_SIZE_DEFAULT = 32766;

        /// <summary>
        /// The maximum size supported by a windows operating system that is vista
        /// or newer.
        /// </summary>
        /// <remarks>
        /// See ReportEvent API:
        ///		http://msdn.microsoft.com/en-us/library/aa363679(VS.85).aspx
        /// ReportEvent's lpStrings parameter:
        /// "A pointer to a buffer containing an array of 
        /// null-terminated strings that are merged into the message before Event Viewer 
        /// displays the string to the user. This parameter must be a valid pointer 
        /// (or NULL), even if wNumStrings is zero. Each string is limited to 31,839 characters."
        /// 
        /// Going beyond the size of 31839 will (at some point) corrupt the event log on Windows
        /// Vista or higher! It may succeed for a while...but you will eventually run into the
        /// error: "System.ComponentModel.Win32Exception : A device attached to the system is
        /// not functioning", and the event log will then be corrupt (I was able to corrupt 
        /// an event log using a length of 31877 on Windows 7).
        /// 
        /// The max size for Windows Vista or higher is documented here:
        ///		http://msdn.microsoft.com/en-us/library/xzwc042w(v=vs.100).aspx.
        /// Going over this size may succeed a few times but the buffer will overrun and 
        /// eventually corrupt the log (based on testing).
        /// 
        /// The maxEventMsgSize size is based on the max buffer size of the lpStrings parameter of the ReportEvent API.
        /// The documented max size for EventLog.WriteEntry for Windows Vista and higher is 31839, but I'm leaving room for a
        /// terminator of #0#0, as we cannot see the source of ReportEvent (though we could use an API monitor to examine the
        /// buffer, given enough time).
        /// </remarks>
        private readonly static int MAX_EVENTLOG_MESSAGE_SIZE_VISTA_OR_NEWER = 31839 - 2;

        /// <summary>
        /// The maximum size that the operating system supports for
        /// a event log message.
        /// </summary>
        /// <remarks>
        /// Used to determine the maximum string length that can be written
        /// to the operating system event log and eventually truncate a string
        /// that exceeds the limits.
        /// </remarks>
        private static int MAX_EVENTLOG_MESSAGE_SIZE;

        /// <summary>
        /// 配置
        /// </summary>
        public virtual ISystemLogConfig Config { get; set; }

        /// <summary>
        /// 基础配置
        /// </summary>
        public override IConfig BaseConfig
        {
            get { return this.Config; }
            set { this.Config = value as ISystemLogConfig; }
        }

        /// <summary>
        /// 获取或设置日志记录器名称[指定的计算机上日志的名称]
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemLogRecorder()
        {
            //日志名称默认为应用程序名称
            this.Name = SystemLogRecorder._applicationName;
        }

        /// <summary>
        /// 转换为系统日志级别
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <returns>系统日志级别</returns>
        private EventLogEntryType ConverToEntryType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Faltal:
                case LogLevel.Error:
                    return EventLogEntryType.Error;
                case LogLevel.Warn:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Error;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            if (this.Config == null || !this.Config.Enable)
            {
                return;
            }

            try
            {
                //过滤条件验证
                if (!base.FilterValidate(item.Level))
                {
                    return;
                }

                //输出日志
                this.OutputLog(this.Config.Name, item);

                //生成日志信息
                string msg = LayoutManager.LayoutLog(item, this.Config, false);
                if (msg.Length > SystemLogRecorder.MAX_EVENTLOG_MESSAGE_SIZE)
                {
                    msg = msg.Substring(0, SystemLogRecorder.MAX_EVENTLOG_MESSAGE_SIZE);
                }

                //转换日志级别为系统日志级别
                EventLogEntryType entryType = this.ConverToEntryType(item.Level);

                //日志源
                string source = item.Logger;
                // Create the source, if it does not already exist.
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, this.Name);
                }

                // Create an EventLog instance and assign its source.
                EventLog eventLog = new EventLog();
                eventLog.Source = source;
                eventLog.WriteEntry(msg, entryType, item.EventID, 1);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }

            //追加日志
            base.AppenderLog(item);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        public override void WriteLog(List<LogItem> items)
        {
            foreach (var item in items)
            {
                this.WriteLog(item);
            }
        }
    }
}
