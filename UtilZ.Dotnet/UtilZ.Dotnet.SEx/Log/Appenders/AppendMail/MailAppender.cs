using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Appender;
using UtilZ.Dotnet.SEx.Log.Config;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 邮件日志输出追加器
    /// </summary>
    public class MailAppender : AppenderBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public MailAppender(XElement ele) : base(ele)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置对象</param>
        public MailAppender(BaseConfig config) : base(config)
        {

        }

        /// <summary>
        /// 创建配置对象实例
        /// </summary>
        /// <returns>配置对象实例</returns>
        protected override BaseConfig CreateConfig()
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
