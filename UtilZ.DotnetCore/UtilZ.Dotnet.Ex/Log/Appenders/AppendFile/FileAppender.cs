using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log.Appender
{
    /// <summary>
    /// 文件日志追加器
    /// </summary>
    public class FileAppender : AppenderBase
    {
        private FileAppenderConfig _fileAppenderConfig;

        /// <summary>
        /// 日志安全策略
        /// </summary>
        private ILogSecurityPolicy _securityPolicy = null;
        private long _maxFileSize;
        private string _filePath;
        private long _fileSize = 0;
        private FileAppenderPathManager _pathManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public FileAppender(XElement ele) : base(ele)
        {
            this.Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置对象</param>
        public FileAppender(BaseConfig config) : base(config)
        {
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this._fileAppenderConfig = (FileAppenderConfig)base._config;
            this._maxFileSize = this._fileAppenderConfig.MaxFileSize * 1024L;
            this._pathManager = new FileAppenderPathManager(this._fileAppenderConfig);
        }

        /// <summary>
        /// 创建配置对象实例
        /// </summary>
        /// <param name="ele">配置元素</param>
        /// <returns>配置对象实例</returns>
        protected override BaseConfig CreateConfig(XElement ele)
        {
            return new FileAppenderConfig(ele);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        protected override void PrimitiveWriteLog(LogItem item)
        {
            if (this._fileAppenderConfig == null || !base.Validate(this._fileAppenderConfig, item) || !this._status)
            {
                return;
            }

            switch (this._fileAppenderConfig.LockingModel)
            {
                case LockingModel.Exclusive:
                    this.ExclusiveWriteLog(item);
                    break;
                case LockingModel.Minimal:
                    this.MinimalWriteLog(item);
                    break;
                case LockingModel.InterProcess:
                    this.InterProcessWriteLog(item);
                    break;
                default:
                    LogSysInnerLog.OnRaiseLog(this, new Exception(string.Format("不支持的锁模型:{0}", this._fileAppenderConfig.LockingModel.ToString())));
                    break;
            }
        }

        private StreamWriter _sw = null;
        private void ExclusiveWriteLog(LogItem item)
        {
            try
            {
                if (this._sw != null && this._sw.BaseStream.Length >= this._maxFileSize)
                {
                    this._sw.Close();
                    this._sw = null;
                }

                if (this._sw == null)
                {
                    //获得日志文件路径
                    string logFilePath = this.GetLogFilePath();
                    if (string.IsNullOrWhiteSpace(logFilePath))
                    {
                        return;
                    }

                    this._sw = File.AppendText(logFilePath);
                }

                this.WriteLogToFile(item, this._sw);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }

        private void MinimalWriteLog(LogItem item)
        {
            try
            {
                //获得日志文件路径
                string logFilePath = this.GetLogFilePath();
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    return;
                }

                using (var sw = File.AppendText(logFilePath))
                {
                    this.WriteLogToFile(item, sw);
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }

        private void InterProcessWriteLog(LogItem item)
        {
            Mutex mutex = null;
            try
            {
                //获得日志文件路径
                string logFilePath = this.GetLogFilePath();
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    return;
                }

                mutex = this.GetMutex(logFilePath);
                using (var sw = File.AppendText(logFilePath))
                {
                    //日志处理
                    this.WriteLogToFile(item, sw);
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
            finally
            {
                this.ReleaseMutex(mutex);
            }
        }

        private void WriteLogToFile(LogItem item, StreamWriter sw)
        {
            string logMsg = LayoutManager.LayoutLog(item, this._fileAppenderConfig);
            if (this._securityPolicy != null)
            {
                logMsg = this._securityPolicy.Encryption(logMsg);
            }

            sw.WriteLine(logMsg);
            sw.Flush();
            this._fileSize = sw.BaseStream.Length;
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

        /// <summary>
        /// 获得日志文件路径
        /// </summary>
        /// <returns>日志文件路径</returns>
        private string GetLogFilePath()
        {
            /********************************************************************
            * Log\*yyyy-MM-dd_HH_mm_ss*_flow.log  =>  Log\2018-08-19_17_05_12_flow.log
            * *yyyy-MM-dd*\info.log  =>  2018-08-19\info_1.log 或 2018-08-19\info_n.log
            * *yyyy-MM-dd*\*yyyy-MM-dd_HH_mm_ss*_flow.log  =>  2018-08-19\2018-08-19_17_05_12_flow.log
            * 或
            * *yyyy-MM-dd*\*HH_mm_ss*_flow.log  =>  2018-08-19\17_05_12_flow.log
            ********************************************************************/

            if (!string.IsNullOrWhiteSpace(this._filePath) && this._fileSize < this._maxFileSize)
            {
                //前一次写入的文件名尚可用
                return this._filePath;
            }

            this._fileSize = 0;
            this._filePath = this._pathManager.CreateLogFilePath();
            return this._filePath;
        }
    }
}
