using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.NLog.Model
{
    /// <summary>
    /// 日志常量类
    /// </summary>
    public class LogConstant
    {
        #region 日志级别中文字符串常量
        /// <summary>
        /// 调试
        /// </summary>
        public const string DEBUGSTR = "调试";

        /// <summary>
        /// 一般
        /// </summary>
        public const string INFOSTR = "提示";

        /// <summary>
        /// 警告
        /// </summary>
        public const string WARNSTR = "警告";

        /// <summary>
        /// 错误
        /// </summary>
        public const string ERRORSTR = "错误";

        /// <summary>
        /// 致命
        /// </summary>
        public const string FATALSTR = "致命";

        /// <summary>
        /// 日志文件扩展名
        /// </summary>
        public const string LOGEXTENSION = @".log";

        /// <summary>
        /// 日志日期格式
        /// </summary>
        public const string LOGDATAFORMAT = @"yyyy-MM-dd";

        /// <summary>
        /// 日期格式字符串
        /// </summary>
        public const string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 等待重试时间,毫秒
        /// </summary>
        public const int WAITREPEATTIME = 200;
        #endregion

        #region 日志表字段名称
        /// <summary>
        /// 日志表名
        /// </summary>
        public const string TableName = "NLOG";

        /// <summary>
        /// 主键列名
        /// </summary>
        public const string IDColName = "ID";

        /// <summary>
        /// 时间列名
        /// </summary>
        public const string TimeColName = "TIME";

        /// <summary>
        /// 线程ID列名
        /// </summary>
        public const string ThreadIDColName = "THREADID";

        /// <summary>
        /// 线程名称列名
        /// </summary>
        public const string ThreadNameColName = "THREADNAME";

        /// <summary>
        /// EventID列名
        /// </summary>
        public const string EVENTIDColName = "EVENTID";

        /// <summary>
        /// 日志级别文本列名
        /// </summary>
        public const string LevelColName = "LOGLEVEL";

        /// <summary>
        /// 日志产生类名称列名
        /// </summary>
        public const string LogerColName = "LOGGER";

        /// <summary>
        /// 日志信息对象列名
        /// </summary>
        public const string MessageColName = "MESSAGE";

        /// <summary>
        /// 异常信息列名
        /// </summary>
        public const string ExceptionColName = "EXCEPTION";
        #endregion

        /// <summary>
        /// 默认日志记录器名称
        /// </summary>
        //public static string DefaultLogRecorderName = @"445B5705-A7BB-490D-8C44-8540DA15AFF2";
        public static string DefaultLogRecorderName = @"DefaultLog";

        /// <summary>
        /// 默认空日志记录器名称
        /// </summary>
        public static string DefaultNullLogRecorderName = @"DefaultNullLog";

        /// <summary>
        /// 默认日志事件ID
        /// </summary>
        public static int DefaultEventID = 0;

        /// <summary>
        /// 日志配置Section节点名称
        /// </summary>
        public const string ConfigSectionName = @"logConfigSection";

        /// <summary>
        /// 获取日志等级名称
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <returns>日志标题</returns>
        public static string GetLogLevelName(LogLevel level)
        {
            string title = null;
            switch (level)
            {
                case LogLevel.Debug:
                    title = LogConstant.DEBUGSTR;
                    break;
                case LogLevel.Info:
                    title = LogConstant.INFOSTR;
                    break;
                case LogLevel.Warn:
                    title = LogConstant.WARNSTR;
                    break;
                case LogLevel.Error:
                    title = LogConstant.ERRORSTR;
                    break;
                case LogLevel.Faltal:
                    title = LogConstant.FATALSTR;
                    break;
                default:
                    throw new Exception(string.Format("未知的日志级别:{0}", level));
            }

            return title;
        }
    }
}
