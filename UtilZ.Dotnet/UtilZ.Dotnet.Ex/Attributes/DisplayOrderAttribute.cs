using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Attributes
{
    /// <summary>
    /// 显示顺序特性
    /// </summary>
    [Serializable]
    public class DisplayOrderAttribute : Attribute
    {
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int OrderIndex { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DisplayOrderAttribute(int orderIndex)
        {
            this.OrderIndex = orderIndex;
        }
    }
}
