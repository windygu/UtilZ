using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public class SeriesLegendItem
    {
        public Brush Brush { get; private set; }

        public string Title { get; private set; }

        public SeriesLegendItem(Brush brush, string title)
        {
            this.Brush = brush;
            this.Title = title;
        }
    }
}
