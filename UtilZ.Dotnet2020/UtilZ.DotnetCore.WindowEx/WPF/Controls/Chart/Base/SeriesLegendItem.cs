using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// SeriesLegendItem
    /// </summary>
    public class SeriesLegendItem
    {
        /// <summary>
        /// Series Brush
        /// </summary>
        public Brush Brush { get; private set; }

        /// <summary>
        /// Series Title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Series
        /// </summary>
        public ISeries Series { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="brush">Series Brush</param>
        /// <param name="title">Series Title</param>
        /// <param name="series">Series</param>
        public SeriesLegendItem(Brush brush, string title, ISeries series)
        {
            this.Brush = brush;
            this.Title = title;
            this.Series = series;
        }
    }
}
