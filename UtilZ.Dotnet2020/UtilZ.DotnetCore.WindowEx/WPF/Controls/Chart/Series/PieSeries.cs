using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilZ.DotnetStd.Ex.Base;

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

        private Style _labelStyle = null;
        /// <summary>
        /// 标签样式
        /// </summary>
        public Style LabelStyle
        {
            get { return _labelStyle; }
            set
            {
                _labelStyle = value;
                base.OnRaisePropertyChanged(nameof(LabelStyle));
            }
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

        private Thickness _margin = new Thickness(0d);
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                base.OnRaisePropertyChanged(nameof(Margin));
            }
        }




        private readonly List<FrameworkElement> _elementList = new List<FrameworkElement>();

        public PieSeries()
            : base()
        {

        }


        protected override void PrimitiveAdd(Canvas canvas)
        {
            this._elementList.Clear();
            base.RemoveLegendItem();

            Dictionary<IChartNoAxisValue, double> valueDic = this.GetValueDic();
            if (valueDic == null || valueDic.Count == 0)
            {
                return;
            }


            double r = this.GetRadius(canvas);
            double R = r * 2;
            double yOffset = (canvas.Height - R) / 2;
            double xOffset = (canvas.Width - R) / 2;
            Size arcSegmentSize = new Size(r, r);
            double total = valueDic.Values.Sum();


            /******************************************************************************************
             * 步骤:
             * 圆的标准方程(x - a)²+(y - b)²= r²中，有三个参数a、b、r，即圆心坐标为(a，b)，半径为r 
             * 此处计算时,以a=0,b=0,即加以在(0,0)的圆 => 圆的方程x²+y²= r²
             * => x=±(r/(Math.Sqrt(1+Math.Power(Math.Tan(angle),2)))) 
             * => y=Math.Tan(anglr)*x 
             ******************************************************************************************/


            IChartNoAxisValue chartNoAxisValue;
            double value;
            Brush stroke = null;
            double angle = AxisConstant.ZERO_D, radians;
            //const double CLIP_ANLE = 30d;
            double x, y;


            Point center = new Point(r + xOffset, r + yOffset);
            Point lastPoint = new Point(R + xOffset, r + yOffset), point;

            for (int i = 0; i < valueDic.Count; i++)
            {
                chartNoAxisValue = valueDic.ElementAt(i).Key;
                Path path = new Path();
                path.Style = chartNoAxisValue.Style;
                if (path.Style == null)
                {
                    if (stroke == null)
                    {
                        stroke = ColorBrushHelper.GetColorByIndex(valueDic.Count);
                    }
                    path.Style = ChartStyleHelper.CreatePieSeriesStyle(stroke, ColorBrushHelper.GetColorByIndex(i));
                }

                base.AddLegendItem(new SeriesLegendItem(path.Fill.Clone(), chartNoAxisValue.Title, this));


                value = valueDic.ElementAt(i).Value;
                if (value <= AxisConstant.ZERO_D)
                {
                    continue;
                }

                path.ToolTip = chartNoAxisValue.TooltipText;
                path.Tag = value;
                angle = angle + value * MathEx.ANGLE_360 / total;
                radians = MathEx.AngleToRadians(angle);


                //+r是为了平移坐标
                x = Math.Cos(radians) * r + r + xOffset;
                y = Math.Sin(radians) * r + r + yOffset;
                point = new Point(x, y);

                List<PathSegment> pathSegments = new List<PathSegment>();
                pathSegments.Add(new LineSegment() { Point = lastPoint });
                pathSegments.Add(new ArcSegment() { Size = arcSegmentSize, Point = point, SweepDirection = SweepDirection.Clockwise });
                pathSegments.Add(new LineSegment() { Point = center });
                PathFigure pathFigure = new PathFigure(center, pathSegments, true);
                path.Data = new PathGeometry(new PathFigure[] { pathFigure });

                //if (angle - CLIP_ANLE < AxisConstant.ZERO_D)
                //{
                //    path.Clip = new RectangleGeometry(new Rect(x, y, Math.Abs(x - center.X), Math.Abs(y - center.Y)));
                //}


                path.Margin = this._margin;
                //path.MouseLeftButtonUp += Path_MouseLeftButtonUp;

                canvas.Children.Add(path);
                this._elementList.Add(path);
                lastPoint = point;
            }
        }

        private Quadrant GetQuadrantByAngle(double angle)
        {
            Quadrant quadrant;
            if (angle - MathEx.ANGLE_270 > AxisConstant.ZERO_D)
            {
                //angle:270-360°
                quadrant = Quadrant.Four;
            }
            else if (angle - MathEx.ANGLE_180 > AxisConstant.ZERO_D)
            {
                //angle:180-270°
                quadrant = Quadrant.Three;
            }
            else if (angle - MathEx.ANGLE_90 > AxisConstant.ZERO_D)
            {
                //angle:90-180°
                quadrant = Quadrant.Two;
            }
            else
            {
                //angle:0-90°
                quadrant = Quadrant.One;
            }

            return quadrant;
        }

        private Dictionary<IChartNoAxisValue, double> GetValueDic()
        {
            if (base._values == null || base._values.Count == 0)
            {
                return null;
            }

            IChartNoAxisValue chartNoAxisValue;
            double itemValue;
            Dictionary<IChartNoAxisValue, double> valueDic = new Dictionary<IChartNoAxisValue, double>();
            foreach (var value in base._values)
            {
                chartNoAxisValue = value as IChartNoAxisValue;
                if (chartNoAxisValue == null)
                {
                    continue;
                }

                itemValue = AxisHelper.ConvertToDouble(chartNoAxisValue.GetValue());
                if (!AxisHelper.DoubleHasValue(itemValue))
                {
                    itemValue = AxisConstant.ZERO_D;
                }

                valueDic[chartNoAxisValue] = itemValue;
            }

            return valueDic;
        }

        private double GetRadius(Canvas canvas)
        {
            double radius = this._radius;
            if (!AxisHelper.DoubleHasValue(radius))
            {
                radius = canvas.Width;
                if (canvas.Height - radius < AxisConstant.ZERO_D)
                {
                    radius = canvas.Height;
                }
                radius = radius / 2;
            }

            return radius;
        }

        protected override bool PrimitiveRemove(Canvas canvas)
        {
            foreach (var element in this._elementList)
            {
                canvas.Children.Remove(element);
            }
            this._elementList.Clear();
            return false;
        }


        protected override void StyleChanged(Style style)
        {
            throw new NotImplementedException();
        }


        protected override void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {
            foreach (var element in this._elementList)
            {
                element.Visibility = newVisibility;
            }
        }
    }
}
