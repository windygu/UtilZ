using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.SEx.Log.Appender;

namespace UtilZ.Dotnet.SEx.Log.Appender
{
    /// <summary>
    /// 操作系统日志输出追加器
    /// </summary>
    public class SystemLogAppender : AppenderBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemLogAppender() : base()
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
