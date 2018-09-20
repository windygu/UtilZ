using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Config;

namespace UtilZ.Dotnet.ILEx.LogExtend
{
    /// <summary>
    /// log4net配置
    /// </summary>
    public class Log4AppenderConfig : BaseConfig
    {
        /// <summary>
        /// log4net日志配置文件路径
        /// </summary>
        public string Log4ConfigFilePath { get; set; } = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele"></param>
        public Log4AppenderConfig(XElement ele) : base(ele)
        {
            if (ele == null)
            {
                return;
            }

            this.Log4ConfigFilePath = LogUtil.GetChildXElementValue(ele, nameof(this.Log4ConfigFilePath));
        }
    }
}
