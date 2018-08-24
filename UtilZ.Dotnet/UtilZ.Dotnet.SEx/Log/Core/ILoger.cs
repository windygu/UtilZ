using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.SEx.Log.Model;

namespace UtilZ.Dotnet.SEx.Log.Core
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILoger
    {
        /// <summary>
        /// 日志记录器名称
        /// </summary>
        string LogerName { get; }

        /// <summary>
        /// 获取日志级别
        /// </summary>
        LogLevel Level { get; }

        #region 记录日志方法
        #region Trace
        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void Trace(string formatMsg, params object[] args);

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Trace(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args);

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void Trace(Exception ex, int eventID = LogConstant.DefaultEventId);

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Trace(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args);
        #endregion

        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void Debug(string formatMsg, params object[] args);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Debug(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void Debug(Exception ex, int eventID = LogConstant.DefaultEventId);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Debug(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args);
        #endregion

        #region Info
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void Info(string formatMsg, params object[] args);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Info(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void Info(Exception ex, int eventID = LogConstant.DefaultEventId);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Info(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args);
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void Warn(string formatMsg, params object[] args);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Warn(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void Warn(Exception ex, int eventID = LogConstant.DefaultEventId);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Warn(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args);
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void Error(string formatMsg, params object[] args);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Error(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void Error(Exception ex, int eventID = LogConstant.DefaultEventId);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Error(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args);
        #endregion

        #region Faltal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="args">参数数组</param>
        void Faltal(string formatMsg, params object[] args);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Faltal(string formatMsg, int eventID = LogConstant.DefaultEventId, params object[] args);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        void Faltal(Exception ex, int eventID = LogConstant.DefaultEventId);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="formatMsg">格式化日志信息,参数为空或null表示无格式化</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数数组</param>
        void Faltal(string formatMsg, Exception ex, int eventID = LogConstant.DefaultEventId, params object[] args);
        #endregion
        #endregion
    }
}
