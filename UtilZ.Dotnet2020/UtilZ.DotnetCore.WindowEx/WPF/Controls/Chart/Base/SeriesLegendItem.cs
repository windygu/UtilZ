using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class SeriesLegendItem
    {
        public Brush Brush { get; private set; }

        public string Title { get; private set; }

        public ISeries Series { get; private set; }

        public SeriesLegendItem(Brush brush, string title, ISeries series)
        {
            this.Brush = brush;
            this.Title = title;
            this.Series = series;
        }
    }
}
