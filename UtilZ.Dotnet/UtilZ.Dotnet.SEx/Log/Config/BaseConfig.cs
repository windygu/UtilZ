using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using UtilZ.Dotnet.SEx.Log.Model;

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
        /// 获取节点指定特性值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="attriName"></param>
        /// <returns></returns>
        protected string GetAttributeValue(XElement ele, string attriName)
        {
            string value;
            var attri = ele.Attribute(attriName);
            if (attri != null)
            {
                value = attri.Value;
            }
            else
            {
                value = string.Empty;
            }

            return value;

        }

        /// <summary>
        /// 获取节点下指定名称子节点特性值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="childName"></param>
        /// <param name="attriName"></param>
        /// <returns></returns>
        protected string GetChildXElementValue(XElement ele, string childName, string attriName = null)
        {
            string value;
            var childEle = ele.XPathSelectElement(childName);
            if (childEle == null)
            {
                value = string.Empty;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(attriName))
                {
                    attriName = "value";
                }

                value = this.GetAttributeValue(childEle, attriName);
            }

            return value;
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

            this.Name = this.GetAttributeValue(ele, "name");
            bool.TryParse(this.GetAttributeValue(ele, "enable"), out this._enable);
            this.Layout = this.GetChildXElementValue(ele, "Layout");
            this.DateFormat = this.GetChildXElementValue(ele, "Layout");
            int.TryParse(this.GetChildXElementValue(ele, "SeparatorCount"), out this._separatorCount);
            if (this._separatorCount > 0)
            {
                this._separatorLine = new string('-', this._separatorCount);
            }

            string levels = this.GetChildXElementValue(ele, "Levels").Trim();
            if (!string.IsNullOrWhiteSpace(levels))
            {
                string[] levelStrs = levels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var logLevels = new List<LogLevel>();
                LogLevel level;
                foreach (var levelStr in levelStrs)
                {
                    if (Enum.TryParse<LogLevel>(levelStr, out level))
                    {
                        logLevels.Add(level);
                    }
                }

                this.Levels = logLevels.ToArray();
            }

            if (int.TryParse(this.GetChildXElementValue(ele, "EventIdMin"), out this._eventIdMin))
            {
                if (this._eventIdMin < LogConstant.DefaultEventId)
                {
                    this._eventIdMin = LogConstant.DefaultEventId;
                }
            }

            if (int.TryParse(this.GetChildXElementValue(ele, "EventIdMax"), out this._eventIdMax))
            {
                if (this._eventIdMax < LogConstant.DefaultEventId)
                {
                    this._eventIdMax = LogConstant.DefaultEventId;
                }
            }

            this.MatchString = this.GetChildXElementValue(ele, "MatchString");
            string matchExceptionTypeName = this.GetChildXElementValue(ele, "MatchExceptionType").Trim();
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

        /// <summary>
        /// 验证日志是否允许输出[返回值true:允许输出;false:丢弃]
        /// </summary>
        /// <param name="item">要验证输出的日志项</param>
        /// <returns>true:允许输出;false:丢弃</returns>
        public bool Validate(LogItem item)
        {
            if (item == null)
            {
                return false;
            }

            if (!this.Enable)
            {
                return false;
            }

            if (this.Levels != null && !this.Levels.Contains(item.Level))
            {
                return false;
            }

            if (this.EventIdMin != LogConstant.DefaultEventId && item.EventID < this.EventIdMin)
            {
                return false;
            }

            if (this.EventIdMax != LogConstant.DefaultEventId && item.EventID > this.EventIdMax)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.MatchString) &&
                !string.IsNullOrEmpty(item.Message) &&
                !Regex.IsMatch(item.Message, MatchString))
            {
                return false;
            }

            if (this._matchExceptionType != null && item.Exception != null)
            {
                Type exType = item.Exception.GetType();
                if (exType != this._matchExceptionType && !exType.IsSubclassOf(this._matchExceptionType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
