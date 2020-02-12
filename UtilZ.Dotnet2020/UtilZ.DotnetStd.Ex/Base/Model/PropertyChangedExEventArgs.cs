using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base
{
    public class PropertyChangedExEventArgs : PropertyChangedEventArgs
    {
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        public PropertyChangedExEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            this.OldValue = OldValue;
            this.NewValue = NewValue;
        }
    }
}
