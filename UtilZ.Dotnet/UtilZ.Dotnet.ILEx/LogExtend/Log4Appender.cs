using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.ILEx.LogExtend
{
    /// <summary>
    /// log4net日志追加器
    /// </summary>
    public class Log4Appender : AppenderBase
    {
        private readonly Log4AppenderConfig _log4AppenderConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public Log4Appender(XElement ele) : base(ele)
        {
            this._log4AppenderConfig = (Log4AppenderConfig)base._config;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置对象</param>
        public Log4Appender(BaseConfig config) : base(config)
        {
            this._log4AppenderConfig = (Log4AppenderConfig)base._config;
        }

        /// <summary>
        /// 创建配置对象实例
        /// </summary>
        /// <param name="ele">配置元素</param>
        /// <returns>配置对象实例</returns>
        protected override BaseConfig CreateConfig(XElement ele)
        {
            return new Log4AppenderConfig(ele);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        protected override void PrimitiveWriteLog(LogItem item)
        {
            try
            {
                if (item == null)
                {
                    return;
                }

                // 获取log4net日志记录器
                log4net.ILog log = log4net.LogManager.GetLogger(item.Name);
                if (log == null)
                {
                    return;
                }

                item.LogProcess();
                string content = LayoutManager.LayoutLog(item, this._config);
                switch (item.Level)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                        log.Debug(content);
                        break;
                    case LogLevel.Info:
                        log.Info(content);
                        break;
                    case LogLevel.Warn:
                        log.Warn(content);
                        break;
                    case LogLevel.Error:
                        log.Error(content);
                        break;
                    case LogLevel.Fatal:
                        log.Fatal(content);
                        break;
                    default:
                        throw new NotSupportedException(string.Format("不支持的日志级别:{0}", item.Level.ToString()));
                }
            }
            catch (Exception exi)
            {
                LogSysInnerLog.OnRaiseLog(this.GetType().FullName, exi);
            }
        }
    }
}
