using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class LabelAxisItem
    {
        public string Label { get; private set; }

        public LabelAxisItem(string label)
        {
            this.Label = label;
        }

        public override string ToString()
        {
            return this.Label;
        }
    }
}
