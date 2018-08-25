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
        private readonly long _maxFileSize;
        private string _filePath;
        private long _fileSize = 0;
        private readonly LogFilePath _logFilePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        ///  <param name="ele">配置元素节点</param>
        public FileAppender(XElement ele) : base()
        {
            this._config = new FileAppenderConfig(ele);
            this._maxFileSize = this._config.MaxFileSize * 1024L;
            this._logFilePath = new LogFilePath(this._config);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            if (this._config == null || !this._config.Validate(item) || !this._logFilePath.Status)
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

                ILogSecurityPolicy securityPolicy = this._securityPolicy;
                string logMsg;
                using (var sw = File.AppendText(logFilePath))
                {
                    //日志处理
                    logMsg = LayoutManager.LayoutLog(item, this._config);
                    if (securityPolicy != null)
                    {
                        logMsg = securityPolicy.Encryption(logMsg);
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
            this._filePath = this._logFilePath.GetLogFilePath();
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

    internal class LogFilePath
    {
        private readonly FileAppenderConfig _config;
        private readonly SubPathInfo[] _subPathInfos;
        private readonly bool _status;
        public bool Status
        {
            get { return _status; }
        }

        private bool _isFirstGetFilePath = true;
        public LogFilePath(FileAppenderConfig config)
        {
            try
            {
                this._config = config;
                string filePath = config.FilePath;
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = @"Log\@yyyy-MM-dd_HH_mm_ss.fffffff@.log";
                }
                else
                {
                    filePath = filePath.Trim();
                }

                string[] paths = filePath.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                if (paths.Length == 0)
                {
                    this._status = false;
                    return;
                }

                this._subPathInfos = paths.Select(t => { return new SubPathInfo(t); }).ToArray();
                this._status = true;
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
                this._status = false;
            }
        }

        public string GetLogFilePath()
        {
            if (this._status)
            {
                string[] paths = this._subPathInfos.Select(t => { return t.GetPath(); }).ToArray();
                string tmpLogFileFullPath = Path.GetFullPath(Path.Combine(paths));
                string dir = Path.GetDirectoryName(tmpLogFileFullPath);
                string logFilePath;

                if (this._isFirstGetFilePath && this._config.IsAppend)
                {
                    this._isFirstGetFilePath = false;
                    if (Directory.Exists(dir))
                    {
                        logFilePath = this.GetLastLogFilePath(dir, Path.GetExtension(tmpLogFileFullPath));
                        if (string.IsNullOrWhiteSpace(logFilePath))
                        {
                            while (File.Exists(tmpLogFileFullPath))
                            {
                                paths = this._subPathInfos.Select(t => { return t.GetPath(); }).ToArray();
                                tmpLogFileFullPath = Path.GetFullPath(Path.Combine(paths));
                            }

                            logFilePath = tmpLogFileFullPath;
                        }
                        else
                        {
                            this.ClearExpireLogFile();
                        }
                    }
                    else
                    {
                        logFilePath = tmpLogFileFullPath;
                        Directory.CreateDirectory(dir);
                    }
                }
                else
                {
                    if (Directory.Exists(dir))
                    {
                        while (File.Exists(tmpLogFileFullPath))
                        {
                            paths = this._subPathInfos.Select(t => { return t.GetPath(); }).ToArray();
                            tmpLogFileFullPath = Path.GetFullPath(Path.Combine(paths));
                        }

                        logFilePath = tmpLogFileFullPath;
                        this.ClearExpireLogFile();
                    }
                    else
                    {
                        Directory.CreateDirectory(dir);
                        logFilePath = tmpLogFileFullPath;
                    }
                }

                return logFilePath;
            }
            else
            {
                return null;
            }
        }

        private string GetLastLogFilePath(string dir, string extension)
        {
            string searchPattern = string.Format("*{0}", extension);
            string[] existlogFilePaths = Directory.GetFiles(dir, searchPattern, SearchOption.TopDirectoryOnly);
            if (existlogFilePaths.Length == 0)
            {
                return null;
            }

            var fileNamePathInfo = this._subPathInfos[this._subPathInfos.Length - 1];
            //[Key:创建时间;Value:日志文件路径]
            var orderLogFilePath = new SortedList<DateTime, string>();
            DateTime time;
            string fileName;
            foreach (var existLogFilePath in existlogFilePaths)
            {
                fileName = Path.GetFileName(existLogFilePath);
                if (fileNamePathInfo.TryGetDateByFilePath(fileName, out time))
                {
                    orderLogFilePath.Add(time, existLogFilePath);
                }
            }

            if (orderLogFilePath.Count == 0)
            {
                //当前日志目录下没有符合路径标准的日志文件
                return null;
            }

            var existLastLogFilePath = orderLogFilePath.ElementAt(orderLogFilePath.Count - 1).Value;
            var existLastLogFileInfo = new FileInfo(existLastLogFilePath);
            var currentTime = DateTime.Now;
            if (existLastLogFileInfo.Length < this._config.MaxFileSize &&
                existLastLogFileInfo.CreationTime.Year == currentTime.Year &&
                existLastLogFileInfo.CreationTime.Month == currentTime.Month &&
                existLastLogFileInfo.CreationTime.Day == currentTime.Day)
            {
                //最后一个文件是当天创建且小于目标大小
                return existLastLogFilePath;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 清理过期的日志文件
        /// </summary>
        private void ClearExpireLogFile()
        {
            int days = this._config.Days;
            string logRootDirFullPath = Path.GetFullPath(this._subPathInfos[0].GetPath());
            DirectoryInfo logRootDirParentDirInfo = Directory.GetParent(logRootDirFullPath);
            //string appDir = Path.GetDirectoryName(logRootDirFullPath);
            foreach (var subPathInfo in this._subPathInfos)
            {
                if (subPathInfo.Flag)
                {
                    subPathInfo.
                }
                else
                {
                    subPathInfo.GetPath
                }
            }

            //string dir = Path.GetDirectoryName(logFilePath);
            //string extension = Path.GetExtension(logFilePath);
            //string[] oldLogFilePaths = Directory.GetFiles(dir, extension, SearchOption.TopDirectoryOnly);
            //if (oldLogFilePaths.Length > 日志文件个数)
            //{

            //}

        }
    }

    internal class SubPathInfo
    {
        private const char DatePatternFlagChar = '*';
        private readonly string _datePattern;
        private readonly string _path;
        private readonly int _datePatternIndex;
        private readonly int _datePatternLength;

        /// <summary>
        /// true:datePattern;false:path
        /// </summary>
        private readonly bool _flag;
        public bool Flag
        {
            get { return _flag; }
        }

        public SubPathInfo(string datePattern)
        {
            /***********************************************************
             * datePattern:  
             * @yyyy-MM-dd@.log
             * Abc@yyyy-MM-dd@.log
             ***********************************************************/
            int begin = datePattern.IndexOf(DatePatternFlagChar);
            if (begin > -1)
            {
                int end = datePattern.LastIndexOf(DatePatternFlagChar);
                if (end < 0)
                {
                    throw new ArgumentException("日期匹配字符串无效");
                }

                int length = end - begin;
                string leftStr = datePattern.Substring(0, begin);
                string rightStr = datePattern.Substring(end + 1);
                this._datePatternIndex = begin;
                this._datePatternLength = end - begin - 1;
                datePattern = datePattern.Substring(begin + 1, this._datePatternLength);
                string str = DateTime.Now.ToString(datePattern);
                this._datePattern = datePattern;
                this._path = leftStr + "{0}" + rightStr;
                this._flag = true;
            }
            else
            {
                this._path = datePattern;
                this._flag = false;
            }
        }

        public string GetPath()
        {
            string path;
            if (this._flag)
            {
                path = string.Format(this._path, DateTime.Now.ToString(this._datePattern));
            }
            else
            {
                path = this._path;
            }

            return path;
        }


        public bool TryGetDateByFilePath(string path, out DateTime time)
        {
            time = DateTime.Now;
            if (string.IsNullOrWhiteSpace(path) || path.Length < (this._datePatternIndex + this._datePatternLength))
            {
                return false;
            }

            string timeStr = path.Substring(this._datePatternIndex, this._datePatternLength);
            return DateTime.TryParseExact(timeStr, this._datePattern, null, System.Globalization.DateTimeStyles.None, out time);
        }
    }
}
