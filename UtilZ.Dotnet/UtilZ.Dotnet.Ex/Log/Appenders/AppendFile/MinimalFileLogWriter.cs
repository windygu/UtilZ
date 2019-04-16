using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log.Appenders.AppendFile
{
    internal class MinimalFileLogWriter : FileLogWriterBase
    {
        public MinimalFileLogWriter(FileAppenderConfig fileAppenderConfig, FileAppenderPathManager pathManager) :
            base(fileAppenderConfig, pathManager)
        {


        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="fileAppenderConfig">配置</param>
        /// <param name="pathManager">路由管理器</param>
        /// <param name="createFilePathTime">创建时间</param>
        /// <param name="item">日志项</param>
        protected override void WriteLog(FileAppenderConfig fileAppenderConfig, FileAppenderPathManager pathManager, DateTime createFilePathTime, LogItem item)
        {
            string logFilePath = base.GetLogFilePath();
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                return;
            }

            using (var sw = File.AppendText(logFilePath))
            {
                base.WriteLogToFile(item, sw);
            }
        }
    }
}
