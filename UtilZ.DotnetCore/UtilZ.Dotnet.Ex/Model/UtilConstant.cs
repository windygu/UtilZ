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
        #region 正则表达式
        /// <summary>
        /// IPV4正则表达式
        /// </summary>
        public readonly static string IPV4RegStr = @"((25[0-5]{1}|2[0-4]{1}\d{1}|1\d{2}|[1-9]\d{1}|\d{1})\.){3}(25[0-5]{1}|2[0-4]{1}\d{1}|1\d{2}|[1-9]\d{1}|\d{1}){1}";

        /// <summary>
        /// 端口号正则表达式
        /// </summary>
        public readonly static string PortRegStr = @"6553[0-5]{1}|655[0-2]{1}\d{1}|65[0-4]{1}\d{2}|6[0-4]{1}\d{3}|[1-5]{1}\d{4}|[1-9]{1}\d{0,3}";

        /// <summary>
        /// FtpUrl正则表达式,可通过组取得IP,端口,以及之后的相对目录[ip,port,dir]
        /// </summary>
        public readonly static string FtpUrlRegStr = @"^[f,F]{1}[t,T]{1}[p,P]{1}://(?<ip>((25[0-5]{1}|2[0-4]{1}\d{1}|1\d{2}|[1-9]\d{1}|\d{1})\.){3}(25[0-5]{1}|2[0-4]{1}\d{1}|1\d{2}|[1-9]\d{1}|\d{1}){1})(:(?<port>6553[0-5]{1}|655[0-2]{1}\d{1}|65[0-4]{1}\d{2}|6[0-4]{1}\d{3}|[1-5]{1}\d{4}|[1-9]{1}\d{0,3}))*(/(?<dir>((([^\/:*?""<>|]+/)+([^\/:*?""<>|]+)?)|([^\/:*?""<>|]+)+))*)?$";
        #endregion

        /// <summary>
        /// 日期格式字符串
        /// </summary>
        public readonly static string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss";
    }
}
