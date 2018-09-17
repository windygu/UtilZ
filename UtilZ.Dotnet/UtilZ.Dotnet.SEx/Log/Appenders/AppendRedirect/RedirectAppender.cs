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
        private readonly RedirectAppendConfig _redirectAppendConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public RedirectAppender(XElement ele) : base(ele)
        {
            this._redirectAppendConfig = (RedirectAppendConfig)base._config;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置对象</param>
        public RedirectAppender(BaseConfig config) : base(config)
        {
            this._redirectAppendConfig = (RedirectAppendConfig)base._config;
        }

        /// <summary>
        /// 创建配置对象实例
        /// </summary>
        /// <returns>配置对象实例</returns>
        protected override BaseConfig CreateConfig()
        {
            return new RedirectAppendConfig();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            try
            {
                if (this._redirectAppendConfig == null || !base.Validate(this._redirectAppendConfig, item))
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
