using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// 扩展DirectoryInfo类
    /// </summary>
    public static class DirectoryInfoEx
    {
        /// <summary>
        /// 复制文件夹内容到指定目录
        /// </summary>
        /// <param name="srcDir">原目录</param>
        /// <param name="dstDir">目标目录</param>
        public static void CopyFolder(string srcDir, string dstDir)
        {
            if (string.IsNullOrWhiteSpace(srcDir))
            {
                throw new ArgumentNullException("srcDir");
            }

            CopyFolder(new DirectoryInfo(srcDir), dstDir);
        }

        /// <summary>
        /// 复制文件夹内容到指定目录
        /// </summary>
        /// <param name="srcDirInfo">原目录信息</param>
        /// <param name="dstDir">目标目录</param>
        public static void CopyFolder(this DirectoryInfo srcDirInfo, string dstDir)
        {
            if (srcDirInfo == null)
            {
                throw new ArgumentNullException("srcDirInfo");
            }

            if (!srcDirInfo.Exists)
            {
                throw new ArgumentException("源目录不存在");
            }

            string srcFullName = srcDirInfo.FullName;
            if (srcFullName.EndsWith(@"\"))
            {
                srcFullName = srcFullName.Remove(srcFullName.Length - 1);
            }
            int startIndex = srcFullName.Length + 1;
            FileSystemInfo[] fsInfos = srcDirInfo.GetFileSystemInfos("*.*", SearchOption.AllDirectories);

            foreach (var info in fsInfos)
            {
                if (info is DirectoryInfo)
                {
                    string dstSubDir = Path.Combine(dstDir, info.FullName.Substring(startIndex));
                    var dirInfo = new DirectoryInfo(dstSubDir);
                    if (!dirInfo.Exists)
                    {
                        dirInfo.Create();
                    }
                }
                else if (info is FileInfo)
                {
                    string dstFile = Path.Combine(dstDir, info.FullName.Substring(startIndex));
                    ((FileInfo)info).CopyTo(dstFile, true);
                }
                else
                {
                    throw new NotSupportedException(string.Format("未知的文件系统类型:{0}", info.GetType().Name));
                }
            }
        }

        /// <summary>
        /// 打开指定文件所在目录[如果文件不存在,但目录存在则打开该文件上一层目录,如果目录不存在,则直接返回]
        /// </summary>
        /// <param name="filePath">指定文件路径</param>
        public static void OpenFileDirectory(string filePath)
        {
            string selectPath;
            if (File.Exists(filePath))
            {
                selectPath = filePath;
            }
            else
            {
                string dir = Path.GetDirectoryName(filePath);
                if (Directory.Exists(dir))
                {
                    selectPath = dir;
                }
                else
                {
                    return;
                }
            }

            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,{0}", selectPath));
        }

        /// <summary>
        /// 检查目录是否存在,如果不存在则创建
        /// </summary>
        /// <param name="dir">目录</param>
        public static void CheckDirectory(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                throw new ArgumentNullException(nameof(dir));
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 检查文件路径中所包含的目录是否存在,如果不存在则创建
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void CheckFilePathDirectory(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
