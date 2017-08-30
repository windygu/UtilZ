using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base
{
    /// <summary>
    /// 单位特性
    /// </summary>
    public class NDisplayFormatAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatString">格式化字符串</param>
        public NDisplayFormatAttribute(string formatString)
        {
            this.FormatString = formatString;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        public string FormatString { get; private set; }
    }
}
