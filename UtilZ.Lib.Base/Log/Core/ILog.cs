using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.Core
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 获取或设置日志级别
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
        /// 开启日志记录
        /// </summary>
        void Start();

        /// <summary>
        /// 停止日志记录
        /// </summary>
        void Stop();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        void LoadConfig(string configFilePath);

        /// <summary>
        /// 根据日志记录器名称获取日志记录器
        /// </summary>
        /// <param name="name">日志记录器名称</param>
        /// <returns>日志记录器</returns>
        ILoger GetLoger(string name);

        #region 记录日志方法
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Debug(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Info(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Warn(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Error(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        void Faltal(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);
        #endregion
    }
}
