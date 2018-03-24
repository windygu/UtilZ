using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Config;
using UtilZ.Dotnet.Ex.Log.Config.Interface;
using UtilZ.Dotnet.Ex.Log.Layout;
using UtilZ.Dotnet.Ex.Log.LogRecorderInterface;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.LogRecorder
{
    /// <summary>
    /// 文件日志记录器
    /// </summary>
    public class FileLogRecorder : BaseLogRecorder, IFileLogRecorder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileLogRecorder()
        {
        }

        /// <summary>
        /// 日志安全策略
        /// </summary>
        private ILogSecurityPolicy _securityPolicy = null;

        /// <summary>
        /// 日志级别分类策略
        /// </summary>
        private readonly Dictionary<LogLevel, string> _dicLevelCategoryPolicys = new Dictionary<LogLevel, string>();

        /// <summary>
        /// 配置
        /// </summary>
        private IFileLogConfig _config = null;

        /// <summary>
        /// 配置
        /// </summary>
        public IFileLogConfig Config
        {
            get { return _config; }
            set
            {
                _config = value;
                if (_config == null)
                {
                    this._securityPolicy = null;
                    this._dicLevelCategoryPolicys.Clear();
                    return;
                }

                //加载日志安全策略
                if (!string.IsNullOrWhiteSpace(this._config.SecurityPolicy))
                {
                    try
                    {
                        this._securityPolicy = LogUtil.CreateInstance(this._config.SecurityPolicy.Trim()) as ILogSecurityPolicy;
                    }
                    catch
                    {
                        this._securityPolicy = null;
                    }
                }

                //日志级别分类策略
                if (!string.IsNullOrWhiteSpace(this._config.LevelCategoryPolicy))
                {
                    //Error,Faltal;Debug;Info;
                    string[] leveCategoryStrs = this._config.LevelCategoryPolicy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    LogLevel level;
                    string logDir;
                    foreach (var leveCategoryStr in leveCategoryStrs)
                    {
                        logDir = leveCategoryStr.Replace(",", string.Empty);
                        string[] leveStrs = leveCategoryStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var leveStr in leveStrs)
                        {
                            if (Enum.TryParse<LogLevel>(leveStr, out level))
                            {
                                this._dicLevelCategoryPolicys.Add(level, logDir);
                            }
                        }
                    }
                }

                this._config.LogDirectory = LogUtil.GetFullPath(this._config.LogDirectory);
            }
        }

        /// <summary>
        /// 基础配置
        /// </summary>
        public override IConfig BaseConfig
        {
            get { return this.Config; }
            set { this.Config = value as IFileLogConfig; }
        }

        /// <summary>
        /// 上次清除过期日志时间
        /// </summary>
        private DateTime _lastClearOverdueLogTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 清除过期日志
        /// </summary>
        /// <param name="days">保留日志天数</param>
        /// <param name="logDirectory">日志目录</param>
        /// <param name="currentTime">本次时间</param>
        /// <param name="logExtension">日志扩展名</param>
        private void ClearOverdueLog(int days, string logDirectory, DateTime currentTime, string logExtension)
        {
            //如果本次清理时间和上次清理时间是同一天,则不清理
            if (this._lastClearOverdueLogTime.Year == currentTime.Year &&
                this._lastClearOverdueLogTime.Month == currentTime.Month &&
                this._lastClearOverdueLogTime.Day == currentTime.Day)
            {
                return;
            }

            //清除过期日志
            this.ClearOverdueLog(days, logDirectory, logExtension);

            //设置本次清理时间为上次清理时间
            this._lastClearOverdueLogTime = currentTime;
        }

        /// <summary>
        /// 清除过期日志
        /// </summary>
        /// <param name="days">保留日志天数</param>
        /// <param name="logDirectory">日志目录</param>
        /// <param name="logExtension">日志扩展名</param>
        private void ClearOverdueLog(int days, string logDirectory, string logExtension)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(logDirectory);
            try
            {
                if (!dirInfo.Exists)
                {
                    return;
                }

                FileInfo[] logFiles = dirInfo.GetFiles("*" + logExtension);
                DateTime logFileCreateTime;

                var dtNow = DateTime.Now;
                DateTime deTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                foreach (var logFile in logFiles)
                {
                    try
                    {
                        if (!DateTime.TryParse(LogUtil.GetFileCreateDate(logFile.Name), out logFileCreateTime))
                        {
                            continue;
                        }

                        if ((deTime - logFileCreateTime).Days >= days)
                        {
                            logFile.Delete();
                        }
                    }
                    catch (Exception exi)
                    {
                        LogSysInnerLog.OnRaiseLog(this, exi);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }

        /// <summary>
        /// 上次创建日志文件路径的时间
        /// </summary>
        private DateTime _lastTime = DateTime.Now;

        /// <summary>
        /// 当日日志文件区分索引,-1为初始值
        /// </summary>
        private int _logIndex = -1;

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            if (item == null)
            {
                return;
            }

            this.WriteLog(new List<LogItem>() { item });
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        public override void WriteLog(List<LogItem> items)
        {
            if (this.Config == null || !this.Config.Enable || items == null || items.Count == 0)
            {
                return;
            }

            Mutex mutex = null;
            try
            {
                mutex = this.GetMutex();
                var groups = items.GroupBy((item) => { return item.Level; });
                string logRecorderName = this.Config.Name;

                foreach (var group in groups)
                {
                    //过滤条件验证
                    if (!base.FilterValidate(group.Key))
                    {
                        continue;
                    }

                    //获得日志文件路径
                    string logFilePath = this.GetLogFilePath(group.ElementAt(0), this._config.FilterName);
                    ILogSecurityPolicy securityPolicy = this._securityPolicy;
                    string logMsg;
                    using (var sw = File.AppendText(logFilePath))
                    {
                        foreach (var item in group)
                        {
                            //输出日志
                            this.OutputLog(logRecorderName, item);

                            //日志处理
                            logMsg = LayoutManager.LayoutLog(item, this._config, true);
                            if (securityPolicy != null)
                            {
                                logMsg = securityPolicy.Encryption(logMsg);
                            }

                            sw.WriteLine(logMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
            finally
            {
                //追加日志
                base.AppenderLog(items);
                this.ReleaseMutex(mutex);
            }
        }

        /// <summary>
        /// 获取进程锁
        /// </summary>
        /// <returns>进程锁</returns>
        private Mutex GetMutex()
        {
            Mutex mutex = null;
            try
            {
                string mutexName = this._config.MutexName;
                if (!string.IsNullOrWhiteSpace(mutexName))
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

        /// <summary>
        /// 获得日志文件路径
        /// </summary>
        /// <param name="item">日志项</param>
        /// <param name="fileName">文件名</param>
        /// <returns>日志文件路径</returns>
        private string GetLogFilePath(LogItem item, string fileName)
        {
            //日志存放目录
            string logDirectory = this._config.LogDirectory;
            if (this._dicLevelCategoryPolicys.ContainsKey(item.Level))
            {
                logDirectory = Path.Combine(logDirectory, this._dicLevelCategoryPolicys[item.Level]);
            }

            //创建日志文件路径
            string logFilePath = LogUtil.CreateFilePath(item.Time, ref this._lastTime, ref this._logIndex, LogConstant.LOGDATAFORMAT, logDirectory, fileName, LogConstant.LOGEXTENSION, Convert.ToUInt32(this.Config.LogFileSize));
            //清除过期日志
            this.ClearOverdueLog(this.Config.Days, logDirectory, item.Time, LogConstant.LOGEXTENSION);
            //目录是否存在
            this.CheckDirectory(logDirectory);
            return logFilePath;
        }

        /// <summary>
        /// 检测日志存放目录,如果不存在就创建该目录
        /// </summary>
        private void CheckDirectory(string logDirectory)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(logDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
    }
}
