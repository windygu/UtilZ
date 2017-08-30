using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Log.Config.Interface;
using UtilZ.Lib.Base.Log.LogRecorderInterface;
using UtilZ.Lib.Base.Log.Model;

namespace UtilZ.Lib.Base.Log.LogRecorder
{
    /// <summary>
    /// 日志接口蕨类
    /// </summary>
    public abstract class BaseLoger : ILoger
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        public abstract IConfig BaseConfig { get; set; }

        /// <summary>
        /// 内部日志添加到ILoger记录事件
        /// </summary>
        public event EventHandler<InnerLogAddedIlogerArgs> InnerLogAddedIloger;

        /// <summary>
        /// ILoger日志记录事件记录日志信息
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        protected virtual void OnRaiseAddIlogerLog(LogLevel level, string msg, Exception ex, int eventID, object extendInfo)
        {
            var handler = this.InnerLogAddedIloger;
            if (handler != null)
            {
                handler(this, new InnerLogAddedIlogerArgs(level, msg, ex, this.BaseConfig.Name, eventID, extendInfo));
            }
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="logRecorderName">日志记录器名称</param>
        /// <param name="logItem">日志项</param>
        protected void OutputLog(string logRecorderName, LogItem logItem)
        {
            UtilZ.Lib.Base.Log.LogOutput.LogOutputCenter.Instance.AddOutputLog(logRecorderName, logItem);
        }

        #region 记录日志方法
        #region Debug
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        public void Debug(string msg)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, msg, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Debug(string msg, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, msg, null, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Debug(string msg, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, msg, null, eventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void Debug(Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, null, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Debug(Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, null, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Debug(Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, null, ex, eventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Debug(string msg, Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, msg, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Debug(string msg, Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, msg, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Debug(string msg, Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Debug, msg, ex, eventID, extendInfo);
        }
        #endregion

        #region Info
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        public void Info(string msg)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, msg, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Info(string msg, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, msg, null, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Info(string msg, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, msg, null, eventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void Info(Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, null, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Info(Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, null, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Info(Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, null, ex, eventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Info(string msg, Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, msg, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Info(string msg, Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, msg, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Info(string msg, Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Info, msg, ex, eventID, extendInfo);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        public void Warn(string msg)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, msg, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Warn(string msg, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, msg, null, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Warn(string msg, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, msg, null, eventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void Warn(Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, null, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Warn(Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, null, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Warn(Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, null, ex, eventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Warn(string msg, Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, msg, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Warn(string msg, Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, msg, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Warn(string msg, Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Warn, msg, ex, eventID, extendInfo);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        public void Error(string msg)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, msg, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Error(string msg, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, msg, null, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Error(string msg, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, msg, null, eventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void Error(Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, null, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Error(Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, null, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Error(Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, null, ex, eventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Error(string msg, Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, msg, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Error(string msg, Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, msg, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Error(string msg, Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Error, msg, ex, eventID, extendInfo);
        }
        #endregion

        #region Faltal
        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        public void Faltal(string msg)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, msg, null, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Faltal(string msg, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, msg, null, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Faltal(string msg, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, msg, null, eventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void Faltal(Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, null, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Faltal(Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, null, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Faltal(Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, null, ex, eventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Faltal(string msg, Exception ex)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, msg, ex, LogConstant.DefaultEventID, null);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Faltal(string msg, Exception ex, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, msg, ex, LogConstant.DefaultEventID, extendInfo);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public void Faltal(string msg, Exception ex, int eventID = 0, object extendInfo = null)
        {
            this.OnRaiseAddIlogerLog(LogLevel.Faltal, msg, ex, eventID, extendInfo);
        }
        #endregion
        #endregion
    }
}
