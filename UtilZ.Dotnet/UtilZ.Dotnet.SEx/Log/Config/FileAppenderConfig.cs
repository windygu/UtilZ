using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UtilZ.Dotnet.SEx.Log.Config
{
    /// <summary>
    /// 日志追加器配置
    /// </summary>
    [Serializable]
    public class FileAppenderConfig : BaseConfig
    {
        private int _days = 7;
        /// <summary>
        /// 日志保留天数小于1表示永不清除
        /// </summary>
        public int Days
        {
            get { return _days; }
        }

        private int _maxFileCount = -1;
        /// <summary>
        /// 最多产生的日志文件数，超过则只保留最新的n个,小于1为不限文件数
        /// </summary>
        public int MaxFileCount
        {
            get { return _maxFileCount; }
        }

        private int _maxFileSize = 10 * 1024 * 1024;
        /// <summary>
        /// 日志文件上限大小,当文件超过此值则分隔成多个日志文件,单位/MB
        /// </summary>
        public int MaxFileSize
        {
            get { return _maxFileSize; }
        }

        /********************************************************************
         * Log\yyyy-MM-dd_HH_mm_ss;_flow.log  =>  Log\2018-08-19_17_05_12_flow.log
         * yyyy-MM-dd\info.log  =>  2018-08-19\info_1.log 或 2018-08-19\info_n.log
         * yyyy-MM-dd\yyyy-MM-dd_HH_mm_ss;_flow.log  =>  2018-08-19\2018-08-19_17_05_12_flow.log
         * 或
         * yyyy-MM-dd\HH_mm_ss;_flow.log  =>  2018-08-19\17_05_12_flow.log
         ********************************************************************/

        private string _filePath = @"Log/*yyyy-MM-dd_HH_mm_ss.fffffff*.log";
        /// <summary>
        /// 日志存放路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        private bool _isAppend = true;
        /// <summary>
        /// 是否追加日志
        /// </summary>
        public bool IsAppend
        {
            get { return _isAppend; }
        }

        /// <summary>
        /// 日志安全策略,该类型为实现接口ILogSecurityPolicy的子类,必须实现Encryption方法
        /// </summary>
        public string SecurityPolicy { get; private set; }

        /// <summary>
        /// 进程同步锁名称
        /// </summary>
        public string MutexName { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileAppenderConfig() : base()
        {

        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="ele"></param>
        public override void Parse(XElement ele)
        {
            if (ele == null)
            {
                return;
            }

            base.Parse(ele);
            int days;
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, "Days"), out days))
            {
                if (days < 1)
                {
                    days = 7;
                }

                this._days = days;
            }

            int maxFileCount;
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, "MaxFileCount"), out maxFileCount))
            {
                if (maxFileCount < 1 && maxFileCount != -1)
                {
                    maxFileCount = -1;
                }

                this._maxFileCount = maxFileCount;
            }

            int maxFileSize;
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, "MaxFileSize"), out maxFileSize))
            {
                if (maxFileSize < 1)
                {
                    maxFileSize = 10;
                }

                this._maxFileSize = maxFileSize * 1024 * 1024;
            }

            string filePath = LogUtil.GetChildXElementValue(ele, "FilePath").Trim();
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                this._filePath = filePath.Trim();
            }

            bool isAppend;
            if (bool.TryParse(LogUtil.GetChildXElementValue(ele, "IsAppend").Trim(), out isAppend))
            {
                this._isAppend = isAppend;
            }

            this.SecurityPolicy = LogUtil.GetChildXElementValue(ele, "SecurityPolicy").Trim();
            this.MutexName = LogUtil.GetChildXElementValue(ele, "MutexName ").Trim();
        }
    }
}
