using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base.Config
{
    /// <summary>
    /// 标记配置根
    /// </summary>
    public class ConfigRootAttribute : ConfigAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="des">描述</param>
        public ConfigRootAttribute(string name, string des = null)
            : base(name, des, false)
        {

        }
    }
}
