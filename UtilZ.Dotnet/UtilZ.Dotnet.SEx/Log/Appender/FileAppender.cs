using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Config;
using UtilZ.Dotnet.SEx.Log.Layout;
using UtilZ.Dotnet.SEx.Log.Model;
using UtilZ.Dotnet.SEx.Log.Security;

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
        private bool _status;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileAppender() : base()
        {
            this._config = new FileAppenderConfig();
            this._maxFileSize = this._config.MaxFileSize * 1024L;
            this._pathManager = new FileAppenderPathManager(this._config);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ele"></param>
        public override void Init(XElement ele)
        {
            if (ele == null)
            {
                return;
            }

            this._config.Parse(ele);
            this._maxFileSize = this._config.MaxFileSize * 1024L;
            try
            {
                this._pathManager = new FileAppenderPathManager(this._config);
                this._securityPolicy = LogUtil.CreateInstance(this._config.SecurityPolicy) as ILogSecurityPolicy;
                this._status = true;
            }
            catch (Exception ex)
            {
                this._status = false;
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            if (this._config == null || !base.Validate(this._config, item) || !this._status)
            {
                return;
            }

            Mutex mutex = null;
            try
            {
                mutex = this.GetMutex();
                //获得日志文件路径
                string logFilePath = this.GetLogFilePath();
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    return;
                }

                string logMsg;
                using (var sw = File.AppendText(logFilePath))
                {
                    //日志处理
                    logMsg = LayoutManager.LayoutLog(item, this._config);
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
        /// 获取进程锁
        /// </summary>
        /// <returns>进程锁</returns>
        private Mutex GetMutex()
        {
            string mutexName = this._config.MutexName;
            if (string.IsNullOrWhiteSpace(mutexName))
            {
                return null;
            }

            Mutex mutex = null;
            try
            {
                try
                {
                    //如果此命名互斥对象已存在则请求打开
                    mutex = Mutex.OpenExisting(mutexName);
                }
                catch (WaitHandleCannotBeOpenedException)
                {
                    //如果初次运行没有已命名的互斥对象则创建一个
                    mutex = new Mutex(false, mutexName);
                }

                mutex.WaitOne();
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
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
