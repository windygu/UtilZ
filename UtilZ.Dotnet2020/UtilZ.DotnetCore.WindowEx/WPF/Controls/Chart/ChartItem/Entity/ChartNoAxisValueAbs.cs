using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class ChartNoAxisValueAbs : ChartValueAbs, IChartNoAxisValue
    {
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 样式
        /// </summary>
        public Style Style { get; private set; }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return this.Value;
        }


        public ChartNoAxisValueAbs(object value, string title, Style style, string tooltip)
            : base(tooltip, null)
        {
            this.Value = value;
            this.Title = title;
            this.Style = style;
        }
    }
}
