using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Config;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 文件日志追加器
    /// </summary>
    public class FileAppender : AppenderBase
    {
        private readonly FileAppenderConfig _config;

        /// <summary>
        /// 日志安全策略
        /// </summary>
        private ILogSecurityPolicy _securityPolicy = null;
        private long _maxFileSize;
        private string _filePath;
        private long _fileSize = 0;
        private FileAppenderPathManager _pathManager;

        /// <summary>
        /// 日志写线程队列
        /// </summary>
        private LogAsynQueue<LogItem> _logWriteQueue;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileAppender() : base()
        {
            this._config = new FileAppenderConfig();
            this._maxFileSize = this._config.MaxFileSize * 1024L;
            this._pathManager = new FileAppenderPathManager(this._config);
            this._logWriteQueue = new LogAsynQueue<LogItem>(this.PrimitiveWriteLog, "默认日志输出线程");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ele">配置元素</param>
        public override void Init(XElement ele)
        {
            try
            {
                this._config.Parse(ele);
                this._maxFileSize = this._config.MaxFileSize * 1024L;
                this._pathManager = new FileAppenderPathManager(this._config);
                this._securityPolicy = LogUtil.CreateInstance(this._config.SecurityPolicy) as ILogSecurityPolicy;
                this._logWriteQueue.Dispose();
                this._logWriteQueue = new LogAsynQueue<LogItem>(this.PrimitiveWriteLog, string.Format("{0}日志输出线程", this._config.Name));
                base._status = true;
            }
            catch (Exception)
            {
                base._status = false;
                throw;
            }
        }

        ///// <summary>
        ///// 初始化
        ///// </summary>
        ///// <param name="config">配置元素</param>
        //public override void Init(BaseConfig config)
        //{
        //    try
        //    {
        //        this._config = config as FileAppenderConfig;
        //        if (this._config == null)
        //        {
        //            return;
        //        }

        //        this._maxFileSize = this._config.MaxFileSize * 1024L;
        //        this._pathManager = new FileAppenderPathManager(this._config);
        //        this._securityPolicy = LogUtil.CreateInstance(this._config.SecurityPolicy) as ILogSecurityPolicy;
        //        base._status = true;
        //    }
        //    catch (Exception)
        //    {
        //        base._status = false;
        //        throw;
        //    }
        //}

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            this._logWriteQueue.Enqueue(item);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        private void PrimitiveWriteLog(LogItem item)
        {
            if (this._config == null || !base.Validate(this._config, item) || !this._status)
            {
                return;
            }

            switch (this._config.LockingModel)
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
                    LogSysInnerLog.OnRaiseLog(this, new Exception(string.Format("不支持的锁模型:{0}", this._config.LockingModel.ToString())));
                    break;
            }
        }

        private StreamWriter _sw = null;
        private void ExclusiveWriteLog(LogItem item)
        {
            try
            {
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
                else
                {
                    if (this._sw.BaseStream.Length >= this._maxFileSize)
                    {
                        this._sw.Close();
                    }
                }

                //日志处理
                string logMsg = LayoutManager.LayoutLog(item, this._config);
                if (this._securityPolicy != null)
                {
                    logMsg = this._securityPolicy.Encryption(logMsg);
                }

                this._sw.WriteLine(logMsg);
                this._sw.Flush();
                this._fileSize = this._sw.BaseStream.Length;
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
                    //日志处理
                    string logMsg = LayoutManager.LayoutLog(item, this._config);
                    if (this._securityPolicy != null)
                    {
                        logMsg = this._securityPolicy.Encryption(logMsg);
                    }

                    sw.WriteLine(logMsg);
                    sw.Flush();
                    this._fileSize = sw.BaseStream.Length;
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
                    string logMsg = LayoutManager.LayoutLog(item, this._config);
                    if (this._securityPolicy != null)
                    {
                        logMsg = this._securityPolicy.Encryption(logMsg);
                    }

                    sw.WriteLine(logMsg);
                    sw.Flush();
                    this._fileSize = sw.BaseStream.Length;
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
                    Thread.Sleep(1);
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

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放资源标识</param>
        protected override void Dispose(bool disposing)
        {
            if (this._logWriteQueue == null)
            {
                return;
            }

            this._logWriteQueue.Dispose();
            this._logWriteQueue = null;
        }
    }
}
