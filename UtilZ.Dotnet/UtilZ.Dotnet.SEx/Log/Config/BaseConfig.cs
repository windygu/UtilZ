using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UtilZ.Dotnet.SEx.Log.Config
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
        public string Name { get; set; }

        /// <summary>
        /// 日志布局
        /// </summary>
        public string Layout { get; set; } = null;

        /// <summary>
        /// 时间格式
        /// </summary>
        public string DateFormat { get; set; } = null;

        private int _separatorCount = 140;
        /// <summary>
        /// 分隔线长度
        /// </summary>
        public int SeparatorCount
        {
            get { return _separatorCount; }
            set { _separatorCount = value; }
        }

        /// <summary>
        /// 分隔线
        /// </summary>
        private string _separatorLine = new string('-', 140);
        /// <summary>
        /// 获取分隔线
        /// </summary>
        public string SeparatorLine
        {
            get
            {
                return _separatorLine;
            }
        }
        #endregion

        #region 过滤
        private bool _enable = true;
        /// <summary>
        /// 是否启用日志追加器
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 过滤日志级别
        /// </summary>
        public LogLevel[] Levels { get; set; }

        private int _eventIdMin = LogConstant.DefaultEventId;
        /// <summary>
        /// 事件ID最小值(包含该值,默认值为不限)
        /// </summary>
        public int EventIdMin
        {
            get { return _eventIdMin; }
            set { _eventIdMin = value; }
        }

        private int _eventIdMax = LogConstant.DefaultEventId;
        /// <summary>
        /// 事件ID最大值(包含该值,默认值为不限)
        /// </summary>
        public int EventIdMax
        {
            get { return _eventIdMax; }
            set { _eventIdMax = value; }
        }

        /// <summary>
        /// 消息匹配指定的字符串才被记录,为空或null不匹配(默认为null)
        /// </summary>
        public string MatchString { get; set; } = null;

        private Type _matchExceptionType = null;
        /// <summary>
        /// 要记录的异常的类型为指定类型或其子类才被记录,为null不匹配(默认为null)
        /// </summary>
        public Type MatchExceptionType
        {
            get { return _matchExceptionType; }
            set { _matchExceptionType = value; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素节点</param>
        public BaseConfig()
        {

        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="ele"></param>
        public virtual void Parse(XElement ele)
        {
            if (ele == null)
            {
                return;
            }

            this.Name = LogUtil.GetAttributeValue(ele, "name");

            bool enable;
            if (bool.TryParse(LogUtil.GetAttributeValue(ele, "enable"), out enable))
            {
                this._enable = enable;
            }

            this.Layout = LogUtil.GetChildXElementValue(ele, "Layout");
            this.DateFormat = LogUtil.GetChildXElementValue(ele, "DateFormat");
            int separatorCount;
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, "SeparatorCount"), out separatorCount))
            {
                this._separatorCount = separatorCount;
                if (separatorCount > 0)
                {
                    this._separatorLine = new string('-', separatorCount);
                }
            }

            string levels = LogUtil.GetChildXElementValue(ele, "Levels").Trim();
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
            if (int.TryParse(LogUtil.GetChildXElementValue(ele, "EventIdMin"), out eventId))
            {
                if (eventId < LogConstant.DefaultEventId)
                {
                    eventId = LogConstant.DefaultEventId;
                }

                this._eventIdMin = eventId;
            }

            if (int.TryParse(LogUtil.GetChildXElementValue(ele, "EventIdMax"), out eventId))
            {
                if (eventId < LogConstant.DefaultEventId)
                {
                    eventId = LogConstant.DefaultEventId;
                }

                this._eventIdMax = eventId;
            }

            this.MatchString = LogUtil.GetChildXElementValue(ele, "MatchString");
            string matchExceptionTypeName = LogUtil.GetChildXElementValue(ele, "MatchExceptionType").Trim();
            if (!string.IsNullOrWhiteSpace(matchExceptionTypeName))
            {
                try
                {
                    this._matchExceptionType = Type.GetType(matchExceptionTypeName);
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }
    }
}
