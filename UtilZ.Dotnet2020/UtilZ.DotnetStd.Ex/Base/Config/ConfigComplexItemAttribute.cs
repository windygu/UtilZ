using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base
{
    /// <summary>
    /// 标记配置项是复合类型
    /// </summary>
    public class ConfigComplexItemAttribute : ConfigAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="des">描述</param>
        /// <param name="allowNullValueElement">值为null时节点是否存在[true:存在;false:不存在],默认值false</param>
        public ConfigComplexItemAttribute(string name, string des, bool allowNullValueElement = false)
            : base(name, des, allowNullValueElement)
        {

        }
    }
}
