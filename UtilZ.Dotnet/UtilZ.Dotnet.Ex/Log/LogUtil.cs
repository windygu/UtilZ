using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 日志操作公共类
    /// </summary>
    internal class LogUtil
    {
        /// <summary>
        /// 创建需要分隔大小的文件路径[2014-10-27_1.log]
        /// </summary>
        /// <param name="createTime">创建文件日期</param>
        /// <param name="lastTime">上次记录日志文件的日期</param>
        /// <param name="index">当前文件索引,没有记录之前的初始值为-1</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="directory">文件存放目录</param>
        /// <param name="fileName">文件名</param>
        /// <param name="extension">文件扩展名</param>
        /// <param name="fileSize">文件分隔大小,单位/MB</param>
        /// <returns>文件路径</returns>
        public static string CreateFilePath(DateTime createTime, ref DateTime lastTime, ref int index, string dateFormat, string directory, string fileName, string extension, uint fileSize)
        {
            //参数检查
            CreateFilePathParaCheck(index, dateFormat, directory, fileSize);

            if (index == -1)
            {
                index = UpdateFileIndex(directory, extension, createTime, dateFormat, fileName);
            }
            else
            {
                if (lastTime.Year != createTime.Year || lastTime.DayOfYear != createTime.DayOfYear)
                {
                    //如果日期发生了变化,则重置为1
                    index = 1;
                    lastTime = createTime;
                }
            }

            string dayStr = createTime.ToString(dateFormat);
            string filePath = System.IO.Path.Combine(directory, string.Format(@"{0}{1}_{2}{3}", dayStr, fileName, index, extension));

            if (File.Exists(filePath))
            {
                while (true)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length / 1048576 >= fileSize)//如果文件大小大于了fileSize大小，就将文件区分索引+1，再重新创建一个文件路径
                        {
                            index++;
                            filePath = Path.Combine(directory, string.Format(@"{0}_{1}{2}", dayStr, index, extension));
                            if (!File.Exists(filePath))
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch
                    {
                        index++;
                        filePath = Path.Combine(directory, string.Format(@"{0}_{1}{2}", dayStr, index, extension));
                        if (!File.Exists(filePath))
                        {
                            break;
                        }
                    }
                }
            }

            return filePath;
        }

        /// <summary>
        /// 创建需要分隔大小的文件路径[2014-10-27_1.log]方法参数验证
        /// </summary>
        /// <param name="index">当前文件索引,没有记录之前的初始值为-1</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="directory">文件存放目录</param>
        /// <param name="fileSize">文件分隔大小</param>
        /// <returns>文件路径</returns>
        private static void CreateFilePathParaCheck(int index, string dateFormat, string directory, uint fileSize)
        {
            if (index == 0)
            {
                throw new ArgumentException(string.Format("文件索引:{0}不是有效值", index));
            }

            try
            {
                if (string.IsNullOrEmpty(dateFormat))
                {
                    throw new ArgumentException("日期转换格式参数不能为空");
                }

                DateTime.Now.ToString(dateFormat);
            }
            catch
            {
                throw new ArgumentException(string.Format("日期转换格式参数不是有效的格式化参数:{0}", dateFormat));
            }

            try
            {
                if (string.IsNullOrEmpty(directory))
                {
                    throw new ArgumentException("文件存放目录不能为空");
                }

                DirectoryInfo dirInfo = new DirectoryInfo(directory);
            }
            catch
            {
                throw new ArgumentException(string.Format("文件存放目录不是有效的参数:{0}", directory));
            }

            if (fileSize <= 0)
            {
                throw new ArgumentException("文件分隔大小不能小于0");
            }
        }

        /// <summary>
        /// 更新文件索引
        /// </summary>
        /// <param name="directory">文件存放目录</param>
        /// <param name="extension">文件扩展名</param>
        /// <param name="dt">日期时间</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="fileName">文件名</param>
        /// <returns>文件索引</returns>
        private static int UpdateFileIndex(string directory, string extension, DateTime dt, string dateFormat, string fileName)
        {
            int logIndex = 1;
            int tmpLogIndex = -1;
            string regLogFileName = string.Format(@"^{0}{1}_(?<index>\d+){2}$", dt.ToString(dateFormat), fileName, extension);
            Regex logPathRex = new Regex(regLogFileName);
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (!dirInfo.Exists)
            {
                return logIndex;
            }

            string[] filePaths = Directory.GetFiles(directory, "*" + extension);
            Match logPathMatch = null;
            foreach (string filePath in filePaths)
            {
                logPathMatch = logPathRex.Match(Path.GetFileName(filePath));
                if (!logPathMatch.Success)
                {
                    continue;
                }

                tmpLogIndex = int.Parse(logPathMatch.Groups["index"].Value);
                if (tmpLogIndex > logIndex)
                {
                    logIndex = tmpLogIndex;
                }
            }

            return logIndex;
        }

        /// <summary>
        /// 获取分区文件的创建日期字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>分区文件的创建日期字符串</returns>
        public static string GetFileCreateDate(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            int index = fileName.IndexOf("_");
            if (index != -1)
            {
                fileName = fileName.Substring(0, fileName.IndexOf("_"));
            }

            return fileName;
        }

        /// <summary>
        /// 获取路径中有携带特殊目录的路径转换为完整路径
        /// </summary>
        /// <param name="srcPath">有携带特殊目录的路径</param>
        /// <returns>完整路径</returns>
        public static string GetFullPath(string srcPath)
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
                srcPath = Path.Combine(DirectoryInfoEx.CurrentAssemblyDirectory, srcPath);
            }

            return srcPath;
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="typeFullName">类型名称[格式:类型名,程序集命名.例如:Oracle.ManagedDataAccess.Client.OracleConnection,Oracle.ManagedDataAccess, Version=4.121.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342]</param>
        /// <returns>实例</returns>
        public static object CreateInstance(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return null;
            }

            string[] segs = typeFullName.Split(',');
            if (segs.Length < 2)
            {
                throw new NotSupportedException(string.Format("不支持的格式{0}", typeFullName));
            }

            string assemblyFileName = segs[1].Trim();//程序集文件名称
            string assemblyPath;
            if (string.IsNullOrEmpty(Path.GetPathRoot(assemblyFileName)))
            {
                //相对工作目录的路径
                assemblyPath = Path.Combine(DirectoryInfoEx.CurrentAssemblyDirectory, assemblyFileName);
            }
            else
            {
                //全路径
                assemblyPath = assemblyFileName;
            }

            if (!File.Exists(assemblyPath))
            {
                string srcExtension = Path.GetExtension(assemblyPath).ToLower();
                List<string> extensions = new List<string> { ".dll", ".exe" };
                if (extensions.Contains(srcExtension))
                {
                    return null;
                }

                bool isFind = false;
                string tmpAssemblyPath;
                foreach (var extension in extensions)
                {
                    tmpAssemblyPath = assemblyPath + extension;
                    if (File.Exists(tmpAssemblyPath))
                    {
                        assemblyPath = tmpAssemblyPath;
                        isFind = true;
                        break;
                    }
                }

                if (!isFind)
                {
                    return null;
                }
            }

            string assemblyName = AssemblyName.GetAssemblyName(assemblyPath).FullName;
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = (from item in assemblys where assemblyName.Equals(item.FullName) select item).FirstOrDefault();
            if (assembly == null)
            {
                assembly = Assembly.LoadFile(assemblyPath);
            }

            Type type = assembly.GetType(segs[0].Trim(), false, true);
            return Activator.CreateInstance(type);
        }
    }
}
