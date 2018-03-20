using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base.Log.LogRecorderInterface;
using UtilZ.Lib.Base.Log.Model;

namespace UtilZ.Lib.Base.Log.Core
{
    /// <summary>
    /// 日志记录基类
    /// </summary>
    public abstract class BaseLog : ILog, IDisposable
    {
        #region ILog
        /// <summary>
        /// 获取或设置日志级别
        /// </summary>
        private LogLevel _level = LogLevel.Debug;

        /// <summary>
        /// 获取或设置日志级别
        /// </summary>
        public virtual LogLevel Level
        {
            get { return this._level; }
            set
            {
                if (this._level == value)
                {
                    return;
                }

                this._level = value;
            }
        }

        /// <summary>
        /// 日志跳过堆栈调用层数属性名称
        /// </summary>
        public int SkipFrames { get; set; }

        /// <summary>
        /// 开启日志记录
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 停止日志记录
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public abstract void LoadConfig(string configFilePath);

        /// <summary>
        /// 根据日志记录器名称获取日志记录器
        /// </summary>
        /// <param name="name">日志记录器名称</param>
        /// <returns>日志记录器</returns>
        public abstract ILoger GetLoger(string name);

        #region 记录日志方法
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public abstract void Debug(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public abstract void Info(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public abstract void Warn(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public abstract void Error(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public abstract void Faltal(string msg, Exception ex, string name = null, int eventID = 0, object extendInfo = null);
        #endregion
        #endregion

        #region IDisposable
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
        /// <param name="isDispose">是否释放标识</param>
        protected virtual void Dispose(bool isDispose)
        {
           
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseLog()
        {

        }
    }
}
