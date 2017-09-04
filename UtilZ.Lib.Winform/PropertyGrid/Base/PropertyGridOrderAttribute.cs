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
        //
        // Simple attribute to allow the order of a property to be specified
        //
        private int _order;

        public PropertyGridOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get
            {
                return _order;
            }
        }
    }
}
