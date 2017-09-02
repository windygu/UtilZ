using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Winform.PropertyGrid.TypeConverters;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    [TypeConverter(typeof(PropertyGridDropDownListConverter))]
    public class NAddress
    {
        public string Text { get; set; }

        public int Value { get; set; }
    }
}
