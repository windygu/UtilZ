using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log.Appender;

namespace UtilZ.Dotnet.Ex.Log.Appenders
{
    /// <summary>
    /// 邮件日志输出追加器
    /// </summary>
    public class MailAppender : AppenderBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailAppender() : base()
        {
            throw new NotImplementedException();
            //this._config = new ConsoleAppenderConfig();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ele">配置元素</param>
        public override void Init(XElement ele)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            throw new NotImplementedException();
        }
    }
}
