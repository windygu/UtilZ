using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.Ex.Log.Appender
{
    /// <summary>
    /// 数据库日志输出追加器
    /// </summary>
    public class DatabaseAppender : AppenderBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素</param>
        public DatabaseAppender(XElement ele) : base(ele)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置对象</param>
        public DatabaseAppender(BaseConfig config) : base(config)
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
