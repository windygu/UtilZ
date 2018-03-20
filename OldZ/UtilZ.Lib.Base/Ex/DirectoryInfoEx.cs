using UtilZ.Lib.Base.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Ex
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
                    Loger.Error(string.Format("未知的文件系统类型:{0}", info.GetType().Name));
                }
            }
        }

        /// <summary>
        /// 获取路径中有携带特殊目录的路径转换为完整路径
        /// </summary>
        /// <param name="srcPath">有携带特殊目录的路径</param>
        /// <returns>完整路径</returns>
        public static string GetFullPath(this string srcPath)
        {
            if (string.IsNullOrWhiteSpace(srcPath) || !string.IsNullOrEmpty(Path.GetPathRoot(srcPath)))
            {
                return srcPath;
            }

            //验证目录是否是特殊目录
            string tmpLogDirectory = srcPath.ToLower();
            Array array = Enum.GetValues(typeof(Environment.SpecialFolder));//特殊目录集合
            string specialFolderReg;//特殊目录正则表达式
            bool isFind = false;//是否找到已特殊目录开始标识[true:找到;false:未找到]
            foreach (var item in array)
            {
                specialFolderReg = string.Format("(?<name>^%{0}%[\\|/]?)", item.ToString()).ToLower();
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(tmpLogDirectory, specialFolderReg);
                if (match.Success)
                {
                    isFind = true;
                    //替换日志存放目录中的特殊目录
                    string specialFolder = match.Groups["name"].Value;
                    srcPath = srcPath.Remove(0, specialFolder.Length);
                    srcPath = string.Format("{0}/{1}", Environment.GetFolderPath((Environment.SpecialFolder)item), srcPath);
                    break;
                }
            }

            //移除目录最后的/或\
            if (srcPath.EndsWith(@"\") || srcPath.EndsWith(@"/"))
            {
                srcPath = srcPath.Remove(srcPath.Length - 1, 1);
            }

            if (!isFind)
            {
                //如果日志目录中不存在特殊目录,则根据日志分组策略设定目录

                //移除目录起始位置的/或\
                if (srcPath.StartsWith(@"\") || srcPath.StartsWith(@"/"))
                {
                    srcPath = srcPath.Remove(0, 1);
                }

                //拼接日志目录
                srcPath = Path.Combine(Environment.CurrentDirectory, srcPath);
            }

            return srcPath;
        }
    }
}
