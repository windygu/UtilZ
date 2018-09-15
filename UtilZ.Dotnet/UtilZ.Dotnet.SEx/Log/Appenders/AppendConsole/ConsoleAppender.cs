using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.AppenderConfig;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 控制台日志输出追加器
    /// </summary>
    public class ConsoleAppender : AppenderBase
    {
        private readonly ConsoleAppenderConfig _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConsoleAppender() : base()
        {
            this._config = new ConsoleAppenderConfig();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ele">配置元素</param>
        public override void Init(XElement ele)
        {
            try
            {
                this._config.Parse(ele);
                base._status = true;
            }
            catch (Exception)
            {
                base._status = false;
                throw;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            try
            {
                if (this._config == null || !base.Validate(this._config, item))
                {
                    return;
                }

                string logMsg = LayoutManager.LayoutLog(item, this._config);
                Console.WriteLine(logMsg);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }
    }
}
