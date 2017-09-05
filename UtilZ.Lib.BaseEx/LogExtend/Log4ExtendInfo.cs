using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.BaseEx.LogExtend
{
    /// <summary>
    /// log4net扩展信息
    /// </summary>
    internal class Log4ExtendInfo
    {
        public object ExtendInfo { get; private set; }

        public Log4ExtendInfo(object extendInfo)
        {
            this.ExtendInfo = extendInfo;
        }
    }
}
