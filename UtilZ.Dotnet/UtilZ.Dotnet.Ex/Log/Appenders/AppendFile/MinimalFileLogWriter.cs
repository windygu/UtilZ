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

        public override void WriteLog(LogItem item)
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
