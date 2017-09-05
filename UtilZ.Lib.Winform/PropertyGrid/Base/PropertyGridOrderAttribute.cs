using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid.Base
{
    /// <summary>
    /// 属性排序
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyGridOrderAttribute : Attribute
    {
        /// <summary>
        /// 顺序索引
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="order">顺序索引</param>
        public PropertyGridOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
