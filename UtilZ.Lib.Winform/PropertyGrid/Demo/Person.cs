using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    /// <summary>
    /// Person
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Person
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Person()
        {

        }

        /// <summary>
        /// 年龄
        /// </summary>
        [DisplayName("年龄")]
        public int Age { get; set; }

        /// <summary>
        /// 第一个名字
        /// </summary>
        [DisplayName("第一个名字")]
        public string FirstName { get; set; }

        /// <summary>
        /// 最后一个名字
        /// </summary>
        [DisplayName("最后一个名字")]
        public string LastName { get; set; }

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
