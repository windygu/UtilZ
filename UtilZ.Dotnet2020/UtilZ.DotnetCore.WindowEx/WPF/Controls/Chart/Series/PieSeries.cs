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

        public override Style Style
        {
            get => throw new NotSupportedException("饼图不支持此属性");
            set => throw new NotSupportedException("饼图不支持此属性");
        }


        private double _pushOut = double.NaN;
        /// <summary>
        /// 选中的饼向外突出的距离,单位:像素.小于等于0或为IsInfinity或NaN此值无效
        /// </summary>
        public double PushOut
        {
            get { return _pushOut; }
            set { _pushOut = value; }
        }

        private double _radius = double.NaN;
        /// <summary>
        /// 饼图半径,小于等于0或为IsInfinity或NaN使用控件高度和宽度中的最小值
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                base.OnRaisePropertyChanged(nameof(Radius));
            }
        }


        public PieSeries()
            : base()
        {

        }

        protected override void PrimitiveAdd(Canvas canvas)
        {
            //IChartNoAxisValue
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
