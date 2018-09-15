using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SEx.Log.AppenderConfig;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal abstract class FileAppenderPathBuilderBase
    {
        protected readonly char[] _pathSplitChars;
        protected readonly FileAppenderConfig _config;
        protected readonly string _rootDir;
        protected readonly FileAppenderPathItem[] _pathItems;
        protected bool _isFirstGetFilePath = true;
        protected string _searchPattern;
        private DateTime _lastClearExpireDaysTime = DateTime.Now.AddMonths(-1);
        private DirectoryInfo _rootDirectoryInfo = null;
        protected DirectoryInfo RootDirectoryInfo
        {
            get
            {
                if (this._rootDirectoryInfo == null)
                {
                    this._rootDirectoryInfo = new DirectoryInfo(this._rootDir);
                }

                return this._rootDirectoryInfo;
            }
        }

        public FileAppenderPathBuilderBase(FileAppenderConfig config, string[] paths, char[] pathSplitChars)
        {
            this._config = config;
            this._pathSplitChars = pathSplitChars;
            int rootDirPathCount = paths.Length - 1;
            for (int i = 0; i < paths.Length - 1; i++)
            {
                if (paths[i].Contains(LogConstant.DatePatternFlagChar))
                {
                    rootDirPathCount = i;
                    break;
                }
            }

            string[] rootPaths = paths.Take(rootDirPathCount).ToArray();
            this._rootDir = Path.Combine(rootPaths);
            string[] relativePaths = paths.Skip(rootDirPathCount).ToArray();
            this._searchPattern = string.Format("*{0}", Path.GetExtension(relativePaths.Last()));
            this._pathItems = new FileAppenderPathItem[relativePaths.Length];
            for (int i = 0; i < relativePaths.Length; i++)
            {
                this._pathItems[i] = new FileAppenderPathItem(relativePaths[i]);
            }
        }

        public abstract string CreateLogFilePath();

        protected string PrimitiveCreateLogFilePath()
        {
            var paths = this._pathItems.Select(t => { return t.CreatePath(); }).ToList();
            paths.Insert(0, this._rootDir);
            return Path.Combine(paths.ToArray());
        }

        protected string GetLastLogFilePath(string dir)
        {
            try
            {
                string[] filePaths = Directory.GetFiles(dir, this._searchPattern, SearchOption.TopDirectoryOnly);
                if (filePaths.Length == 0)
                {
                    return null;
                }

                var orderLogFilePath = new SortedList<DateTime, string>();
                foreach (var filePath in filePaths)
                {
                    if (this.CheckPath(filePath))
                    {
                        orderLogFilePath.Add(File.GetCreationTime(filePath), filePath);
                    }
                }

                if (orderLogFilePath.Count == 0)
                {
                    //当前日志目录下没有符合路径标准的日志文件
                    return null;
                }

                var lastLogFilePath = orderLogFilePath.ElementAt(orderLogFilePath.Count - 1).Value;
                if (this.CompareLastLogFilePath(new FileInfo(lastLogFilePath)))
                {
                    //最后一个文件符合追加
                    return lastLogFilePath;
                }
                else
                {
                    //最后一个文件不符合追加
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("获取最后一个日志文件路径异常", ex);
                return null;
            }
        }

        protected abstract bool CompareLastLogFilePath(FileInfo lastLogFileInfo);

        /// <summary>
        /// 检查日志文件路径是否是有效路径[有效返回true;无效返回false]
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected abstract bool CheckPath(string filePath);

        protected void DeleteLogFileByFileCount(List<FileInfo> fileInfos, HashSet<string> hsDelLogFileFullPathDirs)
        {
            if (this._config.MaxFileCount < 1 || fileInfos.Count < this._config.MaxFileCount)
            {
                return;
            }

            int delCount = fileInfos.Count - this._config.MaxFileCount;
            if (delCount < 1)
            {
                return;
            }

            var delFileInfos = fileInfos.OrderBy(t => { return t.CreationTime; }).Take(delCount).ToArray();
            foreach (var delFileInfo in delFileInfos)
            {
                try
                {
                    delFileInfo.Delete();
                    if (hsDelLogFileFullPathDirs != null)
                    {
                        hsDelLogFileFullPathDirs.Add(delFileInfo.Directory.FullName);
                    }
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        protected void DeleteLogFileByDays(List<FileInfo> fileInfos, HashSet<string> hsDelLogFileFullPathDirs)
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
                            if (hsDelLogFileFullPathDirs != null)
                            {
                                hsDelLogFileFullPathDirs.Add(fileInfo.Directory.FullName);
                            }
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
    }
}
