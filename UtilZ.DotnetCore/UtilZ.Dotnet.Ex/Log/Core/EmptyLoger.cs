using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.Ex.Log
{
    /// <summary>
    /// 空日志记录器,不作任何输出
    /// </summary>
    internal class EmptyLoger : LogerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmptyLoger() : base()
        {

        }

        /// <summary>
        /// 静态方法添加日志的方法
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        internal override void ObjectAddLog(LogLevel level, string msg, Exception ex, int eventID, params object[] args)
        {

        }

        /// <summary>
        /// 实例添加日志
        /// </summary>
        /// <param name="skipFrames">跳过堆栈帧数</param>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        protected override void PrimitiveAddLog(int skipFrames, LogLevel level, string msg, Exception ex, int eventID, params object[] args)
        {

        }
    }
}
