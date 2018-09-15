using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SEx.Log.AppenderConfig;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal class FileAppenderPathManager
    {
        static FileAppenderPathManager()
        {
            Array specialFolderArray = Enum.GetValues(typeof(Environment.SpecialFolder));//特殊目录集合
            foreach (var specialFolder in specialFolderArray)
            {
                _hsSpecialFolders.Add(specialFolder.ToString().ToLower());
            }
        }

        private readonly static HashSet<string> _hsSpecialFolders = new HashSet<string>();
        private readonly static char[] _pathSplitChars = new char[] { '\\', '/' };
        private readonly FileAppenderPathBuilderBase _pathBuilder;
        public FileAppenderPathManager(FileAppenderConfig config)
        {
            string filePath = config.FilePath;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("文件路径不能为空");
            }

            var fullPath = this.ConvertToFullPath(filePath);
            string[] paths = fullPath.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
            paths[0] = paths[0] + Path.DirectorySeparatorChar;
            if (paths[paths.Length - 1].Contains(LogConstant.DatePatternFlagChar))
            {
                this._pathBuilder = new FileAppenderVariateFileNameBuilder(config, paths, _pathSplitChars);
            }
            else
            {
                this._pathBuilder = new FileAppenderFixFileNameBuilder(config, paths, _pathSplitChars);
            }
        }

        private string ConvertToFullPath(string filePath)
        {
            //*MyDocuments*\Log\*yyyy-MM-dd_HH_mm_ss.fffffff*.log
            //* MyDocuments*\Log\abc.log

            // G:\Tmp\Log\*yyyy-MM-dd_HH_mm_ss.fffffff*.log
            // G:\Tmp\Log\abc.log

            // * yyyy-MM-dd_HH_mm_ss.fffffff*.log
            //abc*.log

            //Log\*yyyy*\*MM*\*dd*\*HH_mm_ss.fffffff*.log
            //Log\*yyyy*\*MM*\*dd*\abc.log


            //G:\Tmp\Log\abc.log
            //G:\TmpLog\*yyyy*\*MM*\*dd*\abc.log
            //G:\Tmp\Log\*yyyy-MM-dd_HH_mm_ss.fffffff*.log

            var testTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(Path.GetPathRoot(filePath)))
            {
                string[] paths = filePath.Split(_pathSplitChars, StringSplitOptions.RemoveEmptyEntries);
                string firstPath = paths[0];
                if (firstPath[0] == LogConstant.DatePatternFlagChar && firstPath[firstPath.Length - 1] == LogConstant.DatePatternFlagChar)
                {
                    //*MyDocuments*
                    string pattern = firstPath.Substring(1, firstPath.Length - 2).ToLower();
                    if (_hsSpecialFolders.Contains(pattern))
                    {
                        var specialFolder = (Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), pattern, true);
                        paths[0] = Environment.GetFolderPath(specialFolder);
                        filePath = Path.Combine(paths);
                    }
                    else
                    {
                        filePath = Path.GetFullPath(filePath);
                    }
                }
                else if (firstPath.Contains(LogConstant.DatePatternFlagChar))
                {
                    string dir = Path.GetDirectoryName(typeof(FileAppenderPathManager).Assembly.Location);
                    filePath = string.Format(@"{0}/{1}", dir, filePath);
                }
                else
                {
                    filePath = string.Format(@"{0}/{1}", Directory.GetParent(firstPath).FullName, filePath);
                }
            }
            else
            {
                //已经是全路径,不再处理
            }

            return filePath;
        }

        public string CreateLogFilePath()
        {
            return this._pathBuilder.CreateLogFilePath();
        }
    }
}
