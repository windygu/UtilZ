﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Config;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal class FileLogPathInfo
    {
        private readonly static char[] _pathSplitChars = new char[] { '\\', '/' };
        private readonly FileAppenderConfig _config;
        private readonly FileLogSubPathInfo[] _subPathInfos;
        private readonly bool _status;
        public bool Status
        {
            get { return _status; }
        }

        private bool _isFirstGetFilePath = true;
        public FileLogPathInfo(FileAppenderConfig config)
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

                string[] paths = filePath.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
                if (paths.Length == 0)
                {
                    this._status = false;
                    return;
                }

                this._subPathInfos = new FileLogSubPathInfo[paths.Length];
                this._subPathInfos[0] = new FileLogSubPathInfo(paths[0], true);
                for (int i = 1; i < paths.Length; i++)
                {
                    this._subPathInfos[i] = new FileLogSubPathInfo(paths[i], false);
                }
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
                string tmpLogFileFullPath = this.GetPlatformLogFilePath();
                string dir = Path.GetDirectoryName(tmpLogFileFullPath);
                string searchPattern = string.Format("*{0}", Path.GetExtension(tmpLogFileFullPath));
                string logFilePath;

                if (this._isFirstGetFilePath && this._config.IsAppend)
                {
                    this._isFirstGetFilePath = false;
                    if (Directory.Exists(dir))
                    {
                        logFilePath = this.GetLastLogFilePath(dir, searchPattern);
                        if (string.IsNullOrWhiteSpace(logFilePath))
                        {
                            logFilePath = tmpLogFileFullPath;
                        }
                        else
                        {
                            this.ClearExpireLogFile(searchPattern);
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
                        logFilePath = tmpLogFileFullPath;
                        this.ClearExpireLogFile(searchPattern);
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

        private string GetLastLogFilePath(string dir, string searchPattern)
        {
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

        private string GetPlatformLogFilePath()
        {
            string logFileFullPath = this.PrimitiveGetPlatformLogFilePath();
            while (File.Exists(logFileFullPath))
            {
                logFileFullPath = this.PrimitiveGetPlatformLogFilePath();
            }

            return logFileFullPath;
        }

        private string PrimitiveGetPlatformLogFilePath()
        {
            string[] paths = this._subPathInfos.Select(t => { return t.GetPath(); }).ToArray();
            string logFileFullPath = Path.GetFullPath(Path.Combine(paths));
            return logFileFullPath;
        }

        private DateTime _lastClearExpireDaysTime = DateTime.Now.AddMonths(-1);
        /// <summary>
        /// 清理过期的日志文件
        /// </summary>
        private void ClearExpireLogFile(string searchPattern)
        {
            try
            {
                int days = this._config.Days;
                string logRootFullPath = Path.GetFullPath(this._subPathInfos[0].GetPath());
                DirectoryInfo logRootDirParentDirInfo = Directory.GetParent(logRootFullPath);
                List<FileInfo> fileInfos = this.GetAllLogFileInfos(logRootFullPath, logRootDirParentDirInfo, searchPattern);
                var currentClearTime = DateTime.Now;
                var hsDelLogFileDirs = new HashSet<string>();

                //按日志保留天数删除
                this.DeleteLogFileByDays(fileInfos, currentClearTime, hsDelLogFileDirs);

                //按日志文件个数删除日志
                this.DeleteLogFileByFileCount(fileInfos, hsDelLogFileDirs);

                //删除空目录
                this.DeleteEmptyLogDir(logRootDirParentDirInfo.FullName.Length, hsDelLogFileDirs);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("清除过期日志异常", ex);
            }
        }

        private void DeleteEmptyLogDir(int rootDirLength, HashSet<string> hsDelLogFileDirs)
        {
            if (hsDelLogFileDirs.Count > 0)
            {
                foreach (var delLogFileDir in hsDelLogFileDirs)
                {
                    try
                    {
                        this.DelEmptyLogDir(delLogFileDir, rootDirLength);
                    }
                    catch (Exception ex)
                    {
                        LogSysInnerLog.OnRaiseLog(this, ex);
                    }
                }
            }
        }

        private void DeleteLogFileByFileCount(List<FileInfo> fileInfos, HashSet<string> hsDelLogFileDirs)
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
                            hsDelLogFileDirs.Add(delFileInfo.Directory.FullName);
                        }
                        catch (Exception ex)
                        {
                            LogSysInnerLog.OnRaiseLog(this, ex);
                        }
                    }
                }
            }
        }

        private void DeleteLogFileByDays(List<FileInfo> fileInfos, DateTime currentClearTime, HashSet<string> hsDelLogFileDirs)
        {
            if (currentClearTime.Year != this._lastClearExpireDaysTime.Year ||
                currentClearTime.Month != this._lastClearExpireDaysTime.Month ||
                currentClearTime.Day != this._lastClearExpireDaysTime.Day)
            {
                this._lastClearExpireDaysTime = currentClearTime;
                TimeSpan tsDuration;
                foreach (var fileInfo in fileInfos.ToArray())
                {
                    tsDuration = fileInfo.CreationTime - currentClearTime;
                    if (tsDuration.TotalDays - this._config.Days > 0)
                    {
                        try
                        {
                            fileInfo.Delete();
                            fileInfos.Remove(fileInfo);
                            hsDelLogFileDirs.Add(fileInfo.Directory.FullName);
                        }
                        catch (Exception ex)
                        {
                            LogSysInnerLog.OnRaiseLog(this, ex);
                        }
                    }
                }
            }
        }

        private void DelEmptyLogDir(string logFileDir, int rootDirLength)
        {
            string logFullDir = logFileDir.Substring(rootDirLength);
            string[] dirs = logFullDir.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
            string logDir;
            for (int i = dirs.Length - 1; i >= 0; i--)
            {
                logDir = Path.Combine(dirs.Take(i).ToArray());
                if (Directory.GetFiles(logDir, "*.*", SearchOption.AllDirectories).Length == 0)
                {
                    try
                    {
                        Directory.Delete(logFileDir, true);
                    }
                    catch (Exception ex)
                    {
                        LogSysInnerLog.OnRaiseLog(this, ex);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private List<FileInfo> GetAllLogFileInfos(string logRootFullPath, DirectoryInfo logRootDirParentDirInfo, string searchPattern)
        {
            List<FileInfo> srcFileInfos = new List<FileInfo>();
            if (this._subPathInfos.Length > 1)
            {
                DirectoryInfo[] dirInfos = new DirectoryInfo(logRootFullPath).GetDirectories();
                dirInfos = dirInfos.Where(t => { return !this._subPathInfos[1].CheckInvailidLogFilePath(t.Name); }).ToArray();
                foreach (var dirInfo in dirInfos)
                {
                    try
                    {
                        srcFileInfos.AddRange(dirInfo.GetFiles(searchPattern, SearchOption.AllDirectories));
                    }
                    catch (Exception ex)
                    {
                        LogSysInnerLog.OnRaiseLog("获取日志文件信息异常", ex);
                    }
                }
            }
            else
            {
                var fileInfoArray = logRootDirParentDirInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                srcFileInfos.AddRange(fileInfoArray);
            }

            List<FileInfo> fileInfos = srcFileInfos.ToList();
            int rootDirLength = logRootDirParentDirInfo.FullName.Length;
            string filePath;
            foreach (var fileInfo in srcFileInfos)
            {
                filePath = fileInfo.FullName.Substring(rootDirLength);
                if (this.CheckInvailidLogFilePath(filePath))
                {
                    fileInfos.Remove(fileInfo);
                }
            }

            return fileInfos;
        }

        /// <summary>
        /// 检查日志文件路径是否是无效路径[无效返回true;有效返回false]
        /// </summary>
        /// <param name="logFilePath"></param>
        /// <returns></returns>
        private bool CheckInvailidLogFilePath(string logFilePath)
        {
            string[] paths = logFilePath.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length != this._subPathInfos.Length)
            {
                return true;
            }

            for (int i = 0; i < paths.Length; i++)
            {
                if (this._subPathInfos[i].CheckInvailidLogFilePath(paths[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
