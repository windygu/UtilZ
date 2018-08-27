using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Config;
using UtilZ.Dotnet.SEx.Log.Model;
using UtilZ.Dotnet.SEx.Log.RedirectOuput;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 重定向日志输出
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
        /// <param name="ele"></param>
        public override void Init(XElement ele)
        {
            this._config.Parse(ele);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            if (this._config == null || !this._config.Validate(item))
            {
                return;
            }

            RedirectOuputCenter.Instance.AddOutputLog(this.Name, item);
        }
    }
}
