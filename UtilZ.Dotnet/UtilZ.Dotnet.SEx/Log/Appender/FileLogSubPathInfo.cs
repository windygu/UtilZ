using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    internal class FileLogSubPathInfo
    {
        private const char DatePatternFlagChar = '*';
        private readonly int _pathLength;
        private readonly string _datePattern;
        private readonly string _targetPath;
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

        public FileLogSubPathInfo(string path)
        {
            /***********************************************************
             * datePattern:  
             * @yyyy-MM-dd@.log
             * Abc@yyyy-MM-dd@.log
             ***********************************************************/

            int begin = path.IndexOf(DatePatternFlagChar);
            if (begin > -1)
            {
                int end = path.LastIndexOf(DatePatternFlagChar);
                if (end < 0)
                {
                    throw new ArgumentException("日期匹配字符串无效");
                }

                int length = end - begin;
                string leftStr = path.Substring(0, begin);
                string rightStr = path.Substring(end + 1);
                this._datePatternIndex = begin;
                this._datePatternLength = end - begin - 1;
                string datePattern = path.Substring(begin + 1, this._datePatternLength);
                string str = DateTime.Now.ToString(datePattern);
                this._datePattern = datePattern;
                this._pathLength = path.Length - 2;
                this._targetPath = leftStr + "{0}" + rightStr;
                this._flag = true;
            }
            else
            {
                this._targetPath = path;
                this._pathLength = path.Length;
                this._flag = false;
            }
        }

        public string GetPath()
        {
            string path;
            if (this._flag)
            {
                path = string.Format(this._targetPath, DateTime.Now.ToString(this._datePattern));
            }
            else
            {
                path = this._targetPath;
            }

            return path;
        }


        public bool TryGetDateByFilePath(string path, out DateTime time)
        {
            time = DateTime.Now;
            if (!this._flag || string.IsNullOrWhiteSpace(path) || path.Length < (this._datePatternIndex + this._datePatternLength))
            {
                return false;
            }

            string timeStr = path.Substring(this._datePatternIndex, this._datePatternLength);
            return DateTime.TryParseExact(timeStr, this._datePattern, null, System.Globalization.DateTimeStyles.None, out time);
        }

        /// <summary>
        /// 检查日志文件路径是否是无效路径[无效返回true;有效返回false]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool CheckInvailidLogFilePath(string path)
        {
            if (!this._flag)
            {
                return false;
            }

            if (path.Length != this._pathLength)
            {
                return true;
            }

            string timeStr = path.Substring(this._datePatternIndex, this._datePatternLength);
            DateTime time;
            bool parseResult = DateTime.TryParseExact(timeStr, this._datePattern, null, System.Globalization.DateTimeStyles.None, out time);
            if (parseResult)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
