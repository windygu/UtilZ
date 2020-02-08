using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class PieSeries : SeriesAbs
    {
        public override AxisAbs AxisX
        {
            get => throw new NotSupportedException("饼图不需要指定坐标轴");
            set => throw new NotSupportedException("饼图不需要指定坐标轴");
        }
        public override AxisAbs AxisY
        {
            get => throw new NotSupportedException("饼图不需要指定坐标轴");
            set => throw new NotSupportedException("饼图不需要指定坐标轴");
        }

        public override Func<PointInfo, FrameworkElement> CreatePointFunc
        {
            get => throw new NotSupportedException("饼图不支持创建自定义点标注");
            set => throw new NotSupportedException("饼图不支持创建自定义点标注");
        }

        public override double TooltipArea
        {
            get => throw new NotSupportedException("饼图不支持此属性");
            set => throw new NotSupportedException("饼图不支持此属性");
        }



        public PieSeries()
        {

        }

        protected override void PrimitiveAdd(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        protected override bool PrimitiveRemove(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        protected override void StyleChanged(Style style)
        {
            throw new NotImplementedException();
        }

        protected override void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {
            throw new NotImplementedException();
        }
    }
}
