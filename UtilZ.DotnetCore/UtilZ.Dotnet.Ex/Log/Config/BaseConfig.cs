﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UtilZ.Dotnet.Ex.Log.Config
{
    /// <summary>
    /// 基础配置
    /// </summary>
    [Serializable]
    public abstract class BaseConfig
    {
        #region 基础配置
        /// <summary>
        /// 日志记录器名称
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// 日志布局
        /// </summary>
        public string Layout { get; set; } = null;

        private string _dateFormat = null;
        /// <summary>
        /// 时间格式
        /// </summary>
        public string DateFormat
        {
            get { return _dateFormat; }
            set
            {
                try
                {
                    DateTime.Now.ToString(value);
                    _dateFormat = value;
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        private int _separatorCount = 0;
        /// <summary>
        /// 分隔线长度
        /// </summary>
        public int SeparatorCount
        {
            get { return _separatorCount; }
            set
            {
                if (value < 0 || value > 1000)
                {
                    return;
                }

                _separatorCount = value;
                this.SeparatorLine = new string('-', _separatorCount);
            }
        }

        /// <summary>
        /// 获取分隔线
        /// </summary>
        internal string SeparatorLine { get; private set; } = null;

        /// <summary>
        /// 是否启用日志输出缓存[true:启用;false:禁用]
        /// </summary>
        public bool EnableOutputCache { get; set; } = false;
        #endregion

        #region 过滤
        /// <summary>
        /// 是否启用日志追加器
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 过滤日志级别
        /// </summary>
        public LogLevel[] Levels { get; set; } = null;

        /// <summary>
        /// 事件ID最小值(包含该值,默认值为不限)
        /// </summary>
        public int EventIdMin { get; set; } = LogConstant.DEFAULT_EVENT_ID;

        /// <summary>
        /// 事件ID最大值(包含该值,默认值为不限)
        /// </summary>
        public int EventIdMax { get; set; } = LogConstant.DEFAULT_EVENT_ID;

        /// <summary>
        /// 消息匹配指定的字符串才被记录,为空或null不匹配(默认为null)
        /// </summary>
        public string MatchString { get; set; } = null;

        /// <summary>
        /// 要记录的异常的类型为指定类型或其子类才被记录,为null不匹配(默认为null)
        /// </summary>
        public Type MatchExceptionType { get; set; } = null;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public BaseConfig(XElement ele)
        {
            this.SeparatorCount = 140;
            if (ele == null)
            {
                return;
            }

            this.Name = LogUtil.GetAttributeValue(ele, nameof(this.Name).ToLower());

            bool enable;
            if (bool.TryParse(LogUtil.GetAttributeValue(ele, nameof(this.Enable).ToLower()), out enable))
            {
                this.Enable = enable;
            }

            this.Layout = LogUtil.GetChildXElementValue(ele, nameof(this.Layout));
            this.DateFormat = LogUtil.GetChildXElementValue(ele, nameof(this.DateFormat));
            int separatorCount;
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, nameof(this.SeparatorCount)), out separatorCount))
            {
                this.SeparatorCount = separatorCount;
            }

            bool enableOutputCache;
            if (bool.TryParse(LogUtil.GetAttributeValue(ele, nameof(this.EnableOutputCache)), out enableOutputCache))
            {
                this.EnableOutputCache = enableOutputCache;
            }

            string levels = LogUtil.GetChildXElementValue(ele, nameof(this.Levels)).Trim();
            if (!string.IsNullOrWhiteSpace(levels))
            {
                string[] levelStrs = levels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var logLevels = new List<LogLevel>();
                LogLevel level;
                foreach (var levelStr in levelStrs)
                {
                    if (Enum.TryParse<LogLevel>(levelStr, true, out level))
                    {
                        logLevels.Add(level);
                    }
                }

                this.Levels = logLevels.ToArray();
            }

            int eventId;
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, nameof(this.EventIdMin)), out eventId))
            {
                if (eventId < LogConstant.DEFAULT_EVENT_ID)
                {
                    eventId = LogConstant.DEFAULT_EVENT_ID;
                }

                this.EventIdMin = eventId;
            }

            if (int.TryParse(LogUtil.GetChildXElementValue(ele, nameof(this.EventIdMax)), out eventId))
            {
                if (eventId < LogConstant.DEFAULT_EVENT_ID)
                {
                    eventId = LogConstant.DEFAULT_EVENT_ID;
                }

                this.EventIdMax = eventId;
            }

            this.MatchString = LogUtil.GetChildXElementValue(ele, nameof(this.MatchString));
            string matchExceptionTypeName = LogUtil.GetChildXElementValue(ele, nameof(this.MatchExceptionType)).Trim();
            if (!string.IsNullOrWhiteSpace(matchExceptionTypeName))
            {
                try
                {
                    this.MatchExceptionType = LogUtil.GetType(matchExceptionTypeName);
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }
    }
}
