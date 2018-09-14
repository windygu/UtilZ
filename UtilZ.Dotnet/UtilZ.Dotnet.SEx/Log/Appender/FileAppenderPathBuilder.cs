using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Config;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal class FileAppenderPathBuilder : IFileAppenderPathBuilder
    {
        private readonly char[] _pathSplitChars;
        private readonly FileAppenderConfig _config;
        private readonly string _rootDir;
        private readonly FileAppenderPathItem[] _pathItems;
        private bool _isFirstGetFilePath = true;

        public FileAppenderPathBuilder(FileAppenderConfig config, string[] paths, char[] pathSplitChars)
        {
            this._config = config;
            this._pathSplitChars = pathSplitChars;
            int rootDirPathCount = paths.Length - 1;
            for (int i = 0; i < paths.Length - 1; i++)
            {
                if (paths[i].Contains(LogConstant.DatePatternFlagChar))
                {
                    rootDirPathCount = i + 1;
                    break;
                }
            }

            string[] rootPaths = paths.Take(rootDirPathCount).ToArray();
            this._rootDir = Path.Combine(rootPaths);
            string[] relativePaths = paths.Skip(rootDirPathCount).ToArray();
            this._pathItems = new FileAppenderPathItem[relativePaths.Length];
            for (int i = 0; i < relativePaths.Length; i++)
            {
                this._pathItems[i] = new FileAppenderPathItem(relativePaths[i]);
            }
        }

        public string CreateLogFilePath()
        {
            string tmpFilePath = this.PrimitiveCreateLogFilePath();
            string dir = Path.GetDirectoryName(tmpFilePath);
            string searchPattern = string.Format("*{0}", Path.GetExtension(tmpFilePath));
            string logFilePath;

            if (this._isFirstGetFilePath && this._config.IsAppend)
            {
                this._isFirstGetFilePath = false;
                if (Directory.Exists(dir))
                {
                    logFilePath = this.GetLastLogFilePath(dir, searchPattern);
                    if (string.IsNullOrWhiteSpace(logFilePath))
                    {
                        logFilePath = tmpFilePath;
                    }
                    else
                    {
                        this.ClearExpireLogFile(searchPattern, dir);
                    }
                }
                else
                {
                    logFilePath = tmpFilePath;
                    Directory.CreateDirectory(dir);
                }
            }
            else
            {
                logFilePath = tmpFilePath;
                if (Directory.Exists(dir))
                {
                    this.ClearExpireLogFile(searchPattern, dir);
                }
                else
                {
                    Directory.CreateDirectory(dir);
                }
            }

            return logFilePath;
        }

        /// <summary>
        /// 清理过期的日志文件
        /// </summary>
        private void ClearExpireLogFile(string searchPattern, string currentLogDir)
        {
            try
            {
                List<FileInfo> fileInfos = this.GetAllLogFileInfos(searchPattern);
                var hsDelLogFileFullPathDirs = new HashSet<string>();

                //按日志保留天数删除
                this.DeleteLogFileByDays(fileInfos, hsDelLogFileFullPathDirs);

                //按日志文件个数删除日志
                this.DeleteLogFileByFileCount(fileInfos, hsDelLogFileFullPathDirs);

                //排除本次写日志目录
                if (hsDelLogFileFullPathDirs.Contains(currentLogDir))
                {
                    hsDelLogFileFullPathDirs.Remove(currentLogDir);
                }

                //删除空目录
                this.DeleteEmptyDirectory(hsDelLogFileFullPathDirs);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("清除过期日志异常", ex);
            }
        }

        private void DeleteEmptyDirectory(HashSet<string> hsDelLogFileFullPathDirs)
        {
            if (hsDelLogFileFullPathDirs.Count == 0)
            {
                return;
            }

            foreach (var delLogFileFullPathDir in hsDelLogFileFullPathDirs)
            {
                try
                {
                    //级联删除空目录
                    var delDirInfo = new DirectoryInfo(delLogFileFullPathDir);
                    while (true)
                    {
                        if (!delDirInfo.Exists)
                        {
                            delDirInfo = delDirInfo.Parent;
                            continue;
                        }

                        if (delDirInfo.GetFileSystemInfos("*.*", SearchOption.AllDirectories).Length == 0)
                        {
                            delDirInfo.Delete();
                        }
                        else
                        {
                            break;
                        }

                        delDirInfo = delDirInfo.Parent;
                        if (string.Equals(this._rootDir, delDirInfo.FullName, StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        private void DeleteLogFileByFileCount(List<FileInfo> fileInfos, HashSet<string> hsDelLogFileFullPathDirs)
        {
            if (this._config.MaxFileCount > -1 && fileInfos.Count > this._config.MaxFileCount)
            {
                int delCount = fileInfos.Count - this._config.MaxFileCount;
                if (delCount > 0)
                {
                    var delFileInfos = fileInfos.OrderBy(t => { return t.CreationTime; }).Take(delCount).ToArray();
                    foreach (var delFileInfo in delFileInfos)
                    {
                        try
                        {
                            delFileInfo.Delete();
                            hsDelLogFileFullPathDirs.Add(delFileInfo.Directory.FullName);
                        }
                        catch (Exception ex)
                        {
                            LogSysInnerLog.OnRaiseLog(this, ex);
                        }
                    }
                }
            }
        }

        private DateTime _lastClearExpireDaysTime = DateTime.Now.AddMonths(-1);
        private void DeleteLogFileByDays(List<FileInfo> fileInfos, HashSet<string> hsDelLogFileFullPathDirs)
        {
            int days = this._config.Days;
            if (days < 1)
            {
                return;
            }

            var currentClearTime = DateTime.Now;
            if (currentClearTime.Year != this._lastClearExpireDaysTime.Year ||
            currentClearTime.Month != this._lastClearExpireDaysTime.Month ||
            currentClearTime.Day != this._lastClearExpireDaysTime.Day)
            {
                TimeSpan tsDuration;
                foreach (var fileInfo in fileInfos.ToArray())
                {
                    tsDuration = currentClearTime - fileInfo.CreationTime;
                    if (tsDuration.TotalDays - days > 0)
                    {
                        try
                        {
                            fileInfo.Delete();
                            fileInfos.Remove(fileInfo);
                            hsDelLogFileFullPathDirs.Add(fileInfo.Directory.FullName);
                        }
                        catch (Exception ex)
                        {
                            LogSysInnerLog.OnRaiseLog(this, ex);
                        }
                    }
                }

                this._lastClearExpireDaysTime = currentClearTime;
            }
        }

        private List<FileInfo> GetAllLogFileInfos(string searchPattern)
        {
            var rootDirInfo = new DirectoryInfo(this._rootDir);
            List<FileInfo> srcFileInfos = thisGetAllLogFileInfo(searchPattern, rootDirInfo);
            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (var fileInfo in srcFileInfos)
            {
                if (this.CheckInvalidLogFilePath(fileInfo.FullName))
                {
                    fileInfos.Add(fileInfo);
                }
            }

            return fileInfos;
        }

        /// <summary>
        /// 检查日志文件路径是否是有效路径[有效返回true;无效返回false]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckInvalidLogFilePath(string path)
        {
            if (!string.IsNullOrWhiteSpace(Path.GetPathRoot(path)))
            {
                path = path.Substring(this._rootDir.Length);
            }

            var paths = path.Split(this._pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length != this._pathItems.Length)
            {
                return false;
            }

            for (int i = 0; i < paths.Length; i++)
            {
                if (!this._pathItems[i].CheckPath(paths[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private List<FileInfo> thisGetAllLogFileInfo(string searchPattern, DirectoryInfo rootDirInfo)
        {
            List<FileInfo> srcFileInfos = new List<FileInfo>();
            try
            {
                if (this._pathItems.Length == 1)
                {
                    //存放于日志根目录
                    srcFileInfos.AddRange(rootDirInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly));
                }
                else
                {
                    //存放于需要实时创建目录的子目录
                    var dirInfos = rootDirInfo.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
                    foreach (var dirInfo in dirInfos)
                    {
                        try
                        {
                            srcFileInfos.AddRange(dirInfo.GetFiles(searchPattern, SearchOption.AllDirectories));
                        }
                        catch (Exception exi)
                        {
                            LogSysInnerLog.OnRaiseLog(this, exi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }

            return srcFileInfos;
        }

        private string GetLastLogFilePath(string dir, string searchPattern)
        {
            string[] existlogFilePaths = Directory.GetFiles(dir, searchPattern, SearchOption.TopDirectoryOnly);
            if (existlogFilePaths.Length == 0)
            {
                return null;
            }

            var fileNamePathItem = this._pathItems.Last();
            //[Key:创建时间;Value:日志文件路径]
            var orderLogFilePath = new SortedList<DateTime, string>();
            foreach (var existLogFilePath in existlogFilePaths)
            {
                if (fileNamePathItem.CheckPath(Path.GetFileName(existLogFilePath)))
                {
                    orderLogFilePath.Add(File.GetCreationTime(existLogFilePath), existLogFilePath);
                }
            }

            if (orderLogFilePath.Count == 0)
            {
                //当前日志目录下没有符合路径标准的日志文件
                return null;
            }

            KeyValuePair<DateTime, string> existLastLogFilePathInfo = orderLogFilePath.ElementAt(orderLogFilePath.Count - 1);
            DateTime createTime = existLastLogFilePathInfo.Key;
            var existLastLogFilePath = existLastLogFilePathInfo.Value;
            var existLastLogFileInfo = new FileInfo(existLastLogFilePath);

            var time = DateTime.Now;
            if (existLastLogFileInfo.Length < this._config.MaxFileSize &&
                createTime.Year == time.Year &&
                createTime.Month == time.Month &&
                createTime.Day == time.Day)
            {
                //最后一个文件是当天创建且小于目标大小
                return existLastLogFilePath;
            }
            else
            {
                return null;
            }
        }

        private string PrimitiveCreateLogFilePath()
        {
            string logFileFullPath = this.PrimitiveCreateLogFilePath2();
            while (File.Exists(logFileFullPath))
            {
                logFileFullPath = this.PrimitiveCreateLogFilePath2();
            }

            return logFileFullPath;
        }

        private string PrimitiveCreateLogFilePath2()
        {
            var paths = this._pathItems.Select(t => { return t.CreatePath(); }).ToList();
            paths.Insert(0, this._rootDir);
            return Path.Combine(paths.ToArray());
        }
    }
}
