using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Config;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 日志追加器基类
    /// </summary>
    public abstract class AppenderBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppenderBase()
        {

        }

        private string _name = null;
        /// <summary>
        /// 日志追加器名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ele"></param>
        public abstract void Init(XElement ele);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public abstract void WriteLog(LogItem item);

        /// <summary>
        /// 验证日志是否允许输出[返回值true:允许输出;false:丢弃]
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="item">要验证输出的日志项</param>
        /// <returns>true:允许输出;false:丢弃</returns>
        protected virtual bool Validate(BaseConfig config, LogItem item)
        {
            if (item == null)
            {
                return false;
            }

            if (!config.Enable)
            {
                return false;
            }

            if (config.Levels != null && !config.Levels.Contains(item.Level))
            {
                return false;
            }

            if (config.EventIdMin != LogConstant.DefaultEventId && item.EventID < config.EventIdMin)
            {
                return false;
            }

            if (config.EventIdMax != LogConstant.DefaultEventId && item.EventID > config.EventIdMax)
            {
                return false;
            }

            var matchString = config.MatchString;
            if (!string.IsNullOrEmpty(matchString) &&
                !string.IsNullOrEmpty(item.Message) &&
                !Regex.IsMatch(item.Message, matchString))
            {
                return false;
            }

            var matchExceptionType = config.MatchExceptionType;
            if (matchExceptionType != null && item.Exception != null)
            {
                Type exType = item.Exception.GetType();
                if (exType != matchExceptionType && !exType.IsSubclassOf(matchExceptionType))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 重写Equals,日志追加器名称相同认为同一个日志追加器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var ex = obj as AppenderBase;
            if (ex == null)
            {
                return false;
            }

            return string.Equals(ex.Name, this._name);
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this._name == null ? base.GetHashCode() : this._name.GetHashCode();
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._name == null ? base.ToString() : this._name;
        }
    }
}
