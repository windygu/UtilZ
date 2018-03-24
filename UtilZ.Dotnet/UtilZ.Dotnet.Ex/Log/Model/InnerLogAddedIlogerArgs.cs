using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Log.Model
{
    /// <summary>
    /// 内部日志添加到ILoger记录事件参数
    /// </summary>
    public class InnerLogAddedIlogerArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public InnerLogAddedIlogerArgs(LogLevel level, string msg, Exception ex, string name, int eventID, object extendInfo)
        {
            this.EventID = eventID;
            this.Level = level;
            this.Message = msg;
            this.Exception = ex;
            this.Name = name;
            this.ExtendInfo = extendInfo;
        }

        #region 属性
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// 日志信息对象
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 日志记录器名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 事件ID
        /// </summary>
        public int EventID { get; private set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public object ExtendInfo { get; private set; }
        #endregion
    }
}
