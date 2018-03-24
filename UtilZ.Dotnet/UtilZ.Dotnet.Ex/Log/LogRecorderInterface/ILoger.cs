using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Log.LogRecorderInterface
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILoger
    {
        #region 记录日志方法
        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        void Debug(string msg);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(string msg, object extendInfo = null);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(string msg, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        void Debug(Exception ex);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(Exception ex, object extendInfo = null);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(Exception ex, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Debug(string msg, Exception ex);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(string msg, Exception ex, object extendInfo = null);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(string msg, Exception ex, int eventID = 0, object extendInfo = null);
        #endregion

        #region Info
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        void Info(string msg);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(string msg, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(string msg, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        void Info(Exception ex);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(Exception ex, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(Exception ex, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Info(string msg, Exception ex);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(string msg, Exception ex, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(string msg, Exception ex, int eventID = 0, object extendInfo = null);
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        void Warn(string msg);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(string msg, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(string msg, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        void Warn(Exception ex);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(Exception ex, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(Exception ex, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Warn(string msg, Exception ex);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(string msg, Exception ex, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(string msg, Exception ex, int eventID = 0, object extendInfo = null);
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        void Error(string msg);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(string msg, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(string msg, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        void Error(Exception ex);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(Exception ex, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(Exception ex, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Error(string msg, Exception ex);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(string msg, Exception ex, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(string msg, Exception ex, int eventID = 0, object extendInfo = null);
        #endregion

        #region Faltal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        void Faltal(string msg);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(string msg, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(string msg, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        void Faltal(Exception ex);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(Exception ex, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(Exception ex, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Faltal(string msg, Exception ex);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(string msg, Exception ex, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(string msg, Exception ex, int eventID = 0, object extendInfo = null);
        #endregion
        #endregion
    }
}
