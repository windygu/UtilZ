using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base
{

    /// <summary>
    /// 标记配置项为忽略
    /// </summary>
    public class ConfigIgnoreAttribute : ConfigAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigIgnoreAttribute()
            : base()
        {

        }
    }
}
