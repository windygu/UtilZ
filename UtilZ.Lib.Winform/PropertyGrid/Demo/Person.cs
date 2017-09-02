using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Person
    {
        [DisplayName("年龄")]
        public int Age { get; set; }

        [DisplayName("第一个名字")]
        public string FirstName { get; set; }

        [DisplayName("最后一个名字")]
        public string LastName { get; set; }

        public override string ToString()
        {
            return this.LastName + ", " + this.FirstName + " (" + this.Age.ToString() + ")";
            //return base.ToString();
        }
    }
}
