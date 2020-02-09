using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public interface IChartNoAxisValue : IChartValue
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 样式
        /// </summary>
        Style Style { get; }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        object GetValue();
    }
}
