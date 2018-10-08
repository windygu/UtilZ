using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log.Appenders.AppendFile
{
    internal class InterProcessFileLogWriter : FileLogWriterBase
    {
        public InterProcessFileLogWriter(FileAppenderConfig fileAppenderConfig, FileAppenderPathManager pathManager) :
            base(fileAppenderConfig, pathManager)
        {


        }

        public override void WriteLog(LogItem item)
        {
            Mutex mutex = null;
            try
            {
                //获得日志文件路径
                string logFilePath = base.GetLogFilePath();
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    return;
                }

                mutex = this.GetMutex(logFilePath);
                using (var sw = File.AppendText(logFilePath))
                {
                    //日志处理
                    base.WriteLogToFile(item, sw);
                }
            }
            finally
            {
                this.ReleaseMutex(mutex);
            }
        }

        /// <summary>
        /// 获取进程锁
        /// </summary>
        /// <returns>进程锁</returns>
        private Mutex GetMutex(string logFilePath)
        {
            string mutexName = logFilePath.Replace("\\", "_").Replace(":", "_").Replace("/", "_");
            Mutex mutex = null;
            while (mutex == null)
            {
                try
                {
                    //如果此命名互斥对象已存在则请求打开
                    if (!Mutex.TryOpenExisting(mutexName, out mutex))
                    {
                        //打开失败则创建一个
                        mutex = new Mutex(false, mutexName);
                    }

                    mutex.WaitOne();
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                    Thread.Sleep(10);
                }
            }

            return mutex;
        }

        /// <summary>
        /// 释放进程锁
        /// </summary>
        /// <param name="mutex">进程锁</param>
        private void ReleaseMutex(Mutex mutex)
        {
            try
            {
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }
    }
}
