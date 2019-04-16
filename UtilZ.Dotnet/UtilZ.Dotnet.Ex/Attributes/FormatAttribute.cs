using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Attributes
{
    /// <summary>
    /// 单位特性
    /// </summary>
    public class FormatAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatString">格式化字符串</param>
        public FormatAttribute(string formatString)
        {            
            this.FormatString = formatString;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        public string FormatString { get; private set; }
    }
}
