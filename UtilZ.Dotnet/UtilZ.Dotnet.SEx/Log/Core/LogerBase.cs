using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.Core
{
    /// <summary>
    /// 日志记录器基类
    /// </summary>
    public abstract class LogerBase : ILoger
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogerBase()
        {

        }

        #region ILoger接口
        /// <summary>
        /// 日志记录器名称
        /// </summary>
        protected string _logerName;
        /// <summary>
        /// 获取日志记录器名称
        /// </summary>
        string ILoger.LogerName
        {
            get { return _logerName; }
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        protected LogLevel _level = LogLevel.Off;
        /// <summary>
        /// 获取获取日志级别
        /// </summary>
        LogLevel ILoger.Level
        {
            get { return _level; }
        }

        /// <summary>
        /// 实例添加日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        protected void InsAddLog(LogLevel level, string msg, Exception ex, int eventID, params object[] args)
        {
            this.PrimitiveAddLog(4, level, msg, ex, eventID, args);
        }

        /// <summary>
        /// 静态方法添加日志的方法
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        internal abstract void ObjectAddLog(LogLevel level, string msg, Exception ex, int eventID, params object[] args);

        /// <summary>
        /// 实例添加日志
        /// </summary>
        /// <param name="skipFrames">跳过堆栈帧数</param>
        /// <param name="level">日志级别</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">格式参数</param>
        protected abstract void PrimitiveAddLog(int skipFrames, LogLevel level, string msg, Exception ex, int eventID, params object[] args);

        #region 记录日志方法
        #region Trace
        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void ILoger.Trace(string formatMsg, params object[] args)
        {
            this.InsAddLog(LogLevel.Trace, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Trace(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Trace, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void ILoger.Trace(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            this.InsAddLog(LogLevel.Trace, null, ex, eventID, null);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Trace(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Trace, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void ILoger.Debug(string formatMsg, params object[] args)
        {
            this.InsAddLog(LogLevel.Debug, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Debug(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Debug, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void ILoger.Debug(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            this.InsAddLog(LogLevel.Debug, null, ex, eventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Debug(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Debug, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Info
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void ILoger.Info(string formatMsg, params object[] args)
        {
            this.InsAddLog(LogLevel.Info, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Info(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Info, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void ILoger.Info(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            this.InsAddLog(LogLevel.Info, null, ex, eventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Info(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Info, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void ILoger.Warn(string formatMsg, params object[] args)
        {
            this.InsAddLog(LogLevel.Warn, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Warn(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Warn, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void ILoger.Warn(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            this.InsAddLog(LogLevel.Warn, null, ex, eventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Warn(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Warn, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void ILoger.Error(string formatMsg, params object[] args)
        {
            this.InsAddLog(LogLevel.Error, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Error(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Error, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void ILoger.Error(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            this.InsAddLog(LogLevel.Error, null, ex, eventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Error(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Error, formatMsg, ex, eventID, args);
        }
        #endregion

        #region Faltal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void ILoger.Faltal(string formatMsg, params object[] args)
        {
            this.InsAddLog(LogLevel.Fatal, formatMsg, null, LogConstant.DefaultEventId, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Faltal(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Fatal, formatMsg, null, eventID, args);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void ILoger.Faltal(Exception ex, int eventID = LogConstant.DefaultEventId)
        {
            this.InsAddLog(LogLevel.Fatal, null, ex, eventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void ILoger.Faltal(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args)
        {
            this.InsAddLog(LogLevel.Fatal, formatMsg, ex, eventID, args);
        }
        #endregion
        #endregion
        #endregion

        #region IDisposable接口
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 释放资源方法
        /// </summary>
        /// <param name="isDisposing">是否释放标识</param>
        protected virtual void Dispose(bool isDisposing)
        {

        }
        #endregion
    }
}
