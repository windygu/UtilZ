using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Winform.PropertyGrid.Base;
using UtilZ.Lib.Winform.PropertyGrid.TypeConverters;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    /// <summary>
    /// Person
    /// </summary>
    [TypeConverter(typeof(PropertyGridSortConverter))]
    public class Person
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Person()
        {

        }

        /// <summary>
        /// 最后一个名字
        /// </summary>
        [DisplayName("最后一个名字")]
        [PropertyGridOrderAttribute(2)]
        public string LastName { get; set; }

        /// <summary>
        /// 第一个名字
        /// </summary>
        [DisplayName("第一个名字")]
        [PropertyGridOrderAttribute(1)]
        public string FirstName { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [DisplayName("年龄P")]
        [PropertyGridOrderAttribute(0)]
        public int Age { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString()
        {
            return this.LastName + ", " + this.FirstName + " (" + this.Age.ToString() + ")";
            //return base.ToString();
        }
    }
}
