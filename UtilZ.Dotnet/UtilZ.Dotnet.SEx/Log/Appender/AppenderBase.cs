using System;
using System.Collections.Generic;
using System.Text;
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
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public abstract void WriteLog(LogItem item);

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
