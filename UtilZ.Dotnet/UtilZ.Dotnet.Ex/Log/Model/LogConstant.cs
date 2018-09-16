using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 日志常量
    /// </summary>
    public class LogConstant
    {
        /// <summary>
        /// 默认事件ID
        /// </summary>
        public const int DefaultEventId = -1;

        #region 日志级别中文字符串常量
        /// <summary>
        /// 追踪
        /// </summary>
        public const string TRACESTR = "追踪";

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
                case LogLevel.Trace:
                    title = TRACESTR;
                    break;
                case LogLevel.Debug:
                    title = DEBUGSTR;
                    break;
                case LogLevel.Info:
                    title = INFOSTR;
                    break;
                case LogLevel.Warn:
                    title = WARNSTR;
                    break;
                case LogLevel.Error:
                    title = ERRORSTR;
                    break;
                case LogLevel.Fatal:
                    title = FATALSTR;
                    break;
                default:
                    throw new Exception(string.Format("未知的日志级别:{0}", level));
            }

            return title;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public const char DatePatternFlagChar = '*';
    }
}
