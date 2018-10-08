using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log.Appenders.AppendFile
{
    internal class ExclusiveFileLogWriter : FileLogWriterBase
    {
        private StreamWriter _sw = null;

        public ExclusiveFileLogWriter(FileAppenderConfig fileAppenderConfig, FileAppenderPathManager pathManager) :
            base(fileAppenderConfig, pathManager)
        {


        }

        public override void WriteLog(LogItem item)
        {
            DateTime currentTime = DateTime.Now;
            if (this._sw != null &&
                !base._pathManager.IsFixPath &&
                (base._fileAppenderConfig.MaxFileLength > 0 &&
                this._sw.BaseStream.Length >= base._fileAppenderConfig.MaxFileLength ||
                currentTime.Year != base._createFilePathTime.Year ||
                currentTime.Month != base._createFilePathTime.Month ||
                currentTime.Day != base._createFilePathTime.Day))
            {
                this._sw.Close();
                this._sw = null;
            }

            if (this._sw == null)
            {
                string logFilePath = base.GetLogFilePath();
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    return;
                }

                this._sw = File.AppendText(logFilePath);
            }

            base.WriteLogToFile(item, this._sw);
        }
    }
}
