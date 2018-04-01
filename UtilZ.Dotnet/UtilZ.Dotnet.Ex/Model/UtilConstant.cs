using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Model
{
    /// <summary>
    /// 通用常量定义类
    /// </summary>
    public class UtilConstant
    {
        /// <summary>
        /// IPV4正则表达式
        /// </summary>
        public readonly static string IPV4Reg = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        /// <summary>
        /// 日期格式字符串
        /// </summary>
        public readonly static string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss";
    }
}
