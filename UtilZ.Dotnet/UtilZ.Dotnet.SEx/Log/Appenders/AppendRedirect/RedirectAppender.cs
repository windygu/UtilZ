using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Config;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 重定向日志输出追加器
    /// </summary>
    public class RedirectAppender : AppenderBase
    {
        private readonly RedirectAppendConfig _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RedirectAppender() : base()
        {
            this._config = new RedirectAppendConfig();
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

                RedirectOuputCenter.Output(base.Name, item);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }
        }
    }
}
