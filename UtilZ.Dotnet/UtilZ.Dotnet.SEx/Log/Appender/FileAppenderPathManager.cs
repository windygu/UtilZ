﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Config;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal class FileAppenderPathManager
    {
        private readonly static char[] _pathSplitChars = new char[] { '\\', '/' };
        private readonly FileAppenderConfig _config;
        private readonly FileAppenderPathInfo[] _subPathInfos;
        private bool _isAbsolutePath;
        private bool _isFirstGetFilePath = true;
        public FileAppenderPathManager(FileAppenderConfig config)
        {
            this._config = config;
            string filePath = config.FilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("文件路径不能为空");
            }

            this._isAbsolutePath = !string.IsNullOrWhiteSpace(Path.GetPathRoot(filePath));
            string[] paths = filePath.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length == 0)
            {
                throw new ArgumentNullException("文件路径不能为空");
            }

            this._subPathInfos = new FileAppenderPathInfo[paths.Length];
            this._subPathInfos[0] = new FileAppenderPathInfo(paths[0], true);
            for (int i = 1; i < paths.Length; i++)
            {
                this._subPathInfos[i] = new FileAppenderPathInfo(paths[i], false);
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

        private string GetLastLogFilePath(string dir, string searchPattern)
        {
            string[] existlogFilePaths = Directory.GetFiles(dir, searchPattern, SearchOption.TopDirectoryOnly);
            if (existlogFilePaths.Length == 0)
            {
                return null;
            }

            //[Key:创建时间;Value:日志文件路径]
            var orderLogFilePath = new SortedList<DateTime, string>();
            DateTime time;
            string logRootDirFullPath = null;
            foreach (var existLogFilePath in existlogFilePaths)
            {
                if (this.CheckInvalidLogFilePath(existLogFilePath, ref logRootDirFullPath))
                {
                    time = File.GetCreationTime(existLogFilePath);
                    orderLogFilePath.Add(time, existLogFilePath);
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

            time = DateTime.Now;
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
            string[] paths = this._subPathInfos.Select(t => { return t.CreatePath(); }).ToArray();
            string logFileFullPath = Path.GetFullPath(Path.Combine(paths));
            return logFileFullPath;
        }

        /// <summary>
        /// 清理过期的日志文件
        /// </summary>
        private void ClearExpireLogFile(string searchPattern, string currentLogDir)
        {
            try
            {
                string logRootFullPath = Path.GetFullPath(this._subPathInfos[0].CreatePath());                
                List<FileInfo> fileInfos = this.GetAllLogFileInfos(logRootFullPath, searchPattern);
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
                this.DeleteEmptyDirectory(logRootFullPath, hsDelLogFileFullPathDirs);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("清除过期日志异常", ex);
            }
        }

        private void DeleteEmptyDirectory(string logRootFullPath, HashSet<string> hsDelLogFileFullPathDirs)
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
                            if (string.Equals(logRootFullPath, delDirInfo.FullName, StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                        delDirInfo = delDirInfo.Parent;
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

        private List<FileInfo> GetAllLogFileInfos(string logRootFullPath, string searchPattern)
        {
            DirectoryInfo logRootDirParentDirInfo;
            if (this._subPathInfos.Length >= 2)
            {
                //"Log\*yyyy-MM-dd_HH_mm_ss.fffffff*.log"
                //日志存放单独文件夹
                logRootDirParentDirInfo = new DirectoryInfo(logRootFullPath);
            }
            else
            {
                //"*yyyy-MM-dd_HH_mm_ss.fffffff*.log"
                //日志文件存放根目录
                logRootDirParentDirInfo = Directory.GetParent(logRootFullPath);
            }

            FileInfo[] srcFileInfos = logRootDirParentDirInfo.GetFiles(searchPattern, SearchOption.AllDirectories);
            int rootDirLength = logRootDirParentDirInfo.FullName.Length;
            List<FileInfo> fileInfos = new List<FileInfo>();
            string logRootDirFullPath = null;
            foreach (var fileInfo in srcFileInfos)
            {
                if (this.CheckInvalidLogFilePath(fileInfo.FullName, ref logRootDirFullPath))
                {
                    fileInfos.Add(fileInfo);
                }
            }

            return fileInfos;
        }

        /// <summary>
        /// 检查日志文件路径是否有效[有效返回true;无效返回false]
        /// </summary>
        /// <param name="logFilePath"></param>
        /// <param name="logRootDirFullPath"></param>
        /// <returns></returns>
        private bool CheckInvalidLogFilePath(string logFilePath, ref string logRootDirFullPath)
        {
            if (!this.FormatLogFilePath(ref logFilePath, ref logRootDirFullPath))
            {
                return false;
            }

            string[] paths = logFilePath.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length != this._subPathInfos.Length)
            {
                return false;
            }

            for (int i = 0; i < paths.Length; i++)
            {
                if (!this._subPathInfos[i].CheckInvailidLogFilePath(paths[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool FormatLogFilePath(ref string logFilePath, ref string logRootDirFullPath)
        {
            bool isRelativePath = string.IsNullOrWhiteSpace(Path.GetPathRoot(logFilePath));
            if (this._isAbsolutePath)
            {
                if (isRelativePath)
                {
                    //原路径为绝对路径, 目标路径为相对路径,则转换为绝对路径
                    logFilePath = Path.GetFullPath(logFilePath);
                }
            }
            else
            {
                if (!isRelativePath)
                {
                    //原路径为相对路径, 目标路径绝相对路径,则转换为相对路径
                    //Log\123.log
                    //D:\App\Log\22.log
                    //D:\App\Log\Abc\22.log

                    if (string.IsNullOrWhiteSpace(logRootDirFullPath))
                    {
                        string[] pathItems = this._subPathInfos.Select(t => { return t.CreatePath(); }).ToArray();
                        string srcRelativePath = Path.Combine(pathItems);
                        string srcAbsolutePath = Path.GetFullPath(srcRelativePath);
                        logRootDirFullPath = srcAbsolutePath.Substring(0, srcAbsolutePath.Length - srcRelativePath.Length);
                    }

                    if (logFilePath.Length < logRootDirFullPath.Length)
                    {
                        return false;
                    }

                    logFilePath = logFilePath.Substring(logRootDirFullPath.Length);
                }
            }

            return true;
        }
    }
}