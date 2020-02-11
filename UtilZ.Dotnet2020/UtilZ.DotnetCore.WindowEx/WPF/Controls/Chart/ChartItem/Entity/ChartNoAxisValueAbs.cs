using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class ChartNoAxisValueAbs : ChartValueAbs, IChartNoAxisValue
    {
        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; private set; }

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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="title"></param>
        /// <param name="style"></param>
        /// <param name="tooltip"></param>
        public ChartNoAxisValueAbs(object value, string label, string title, Style style, string tooltip)
            : base(tooltip, null)
        {
            this.Value = value;
            this.Label = label;
            this.Title = title;
            this.Style = style;
        }
    }
}
