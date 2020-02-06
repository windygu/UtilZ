﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilZ.DotnetCore.WindowEx.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class LabelAxis : AxisAbs
    {
        private double _axisSize = 100;
        /// <summary>
        /// X轴表示坐标轴表高度,Y轴表示坐标轴宽度
        /// </summary>
        public double AxisSize
        {
            get { return _axisSize; }
            set
            {
                if (!AxisHelper.DoubleHasValue(value) || value < AxisConstant.ZERO_D)
                {
                    value = AxisConstant.AXIS_DEFAULT_SIZE;
                }

                _axisSize = value;
                base.OnRaisePropertyChanged(nameof(AxisSize));
            }
        }

        private int _autoSizeFactor = 2;
        /// <summary>
        /// 自动计算条形图宽度因子,越大表示自动计算出来的条形图越宽,默认值2
        /// </summary>
        public int AutoSizeFactor
        {
            get { return _autoSizeFactor; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("值不能小于0");
                }

                _autoSizeFactor = value;
                base.OnRaisePropertyChanged(nameof(AutoSizeFactor));
            }
        }

        private double _angle = 310d;
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                base.OnRaisePropertyChanged(nameof(Angle));
            }
        }


        public Func<object, string> CustomAxisTextFormatCunc;
        /// <summary>
        /// [key:Label;value:LabelItem]
        /// </summary>
        private readonly Dictionary<object, LabelSeriesItem> _axisData = new Dictionary<object, LabelSeriesItem>();
        private readonly object _nullLabelAxisItem = new object();

        public LabelAxis()
            : base()
        {

        }



        protected override double PrimitiveGetXAxisHeight()
        {
            return this._axisSize;
        }



        private string CreateAxisText(object label)
        {
            if (label == this._nullLabelAxisItem)
            {
                return null;
            }

            string axisText;
            var customAxisTextFormatCunc = this.CustomAxisTextFormatCunc;
            if (customAxisTextFormatCunc != null)
            {
                axisText = customAxisTextFormatCunc(label);
            }
            else
            {
                if (label == null)
                {
                    axisText = null;
                }
                else
                {
                    axisText = label.ToString();
                }
            }

            return axisText;
        }

        /// <summary>
        /// 返回Column X:宽度或Y:高度
        /// </summary>
        /// <param name="seriesCollection"></param>
        /// <returns></returns>
        private void CreateAxisData(ChartCollection<ISeries> seriesCollection)
        {
            this._axisData.Clear();
            if (seriesCollection == null || seriesCollection.Count == 0)
            {
                return;
            }

            object axisValue;
            LabelSeriesItem labelAxisItem, labelAxisItemTmp;
            int preCount;

            foreach (var series in seriesCollection)
            {
                if (!(series is ColumnSeries) |
                    series.AxisX != this && series.AxisY != this ||
                    series.Values == null || series.Values.Count == 0)
                {
                    continue;
                }

                preCount = this._axisData.Count > 0 ? this._axisData.Values.ElementAt(0).Count : 0;
                foreach (var value in series.Values)
                {
                    if (value == null)
                    {
                        continue;
                    }

                    axisValue = null;
                    if (series.AxisX == this)
                    {
                        axisValue = value.GetXValue();
                    }
                    else if (series.AxisY == this)
                    {
                        axisValue = value.GetYValue();
                    }

                    if (axisValue == null)
                    {
                        axisValue = this._nullLabelAxisItem;
                    }

                    if (!this._axisData.TryGetValue(axisValue, out labelAxisItem))
                    {
                        labelAxisItem = new LabelSeriesItem((ColumnSeries)series);

                        if (this._axisData.Count > 0)
                        {
                            //补齐没有的项
                            labelAxisItemTmp = this._axisData.Values.ElementAt(0);
                            for (int i = 0; i < preCount; i++)
                            {
                                labelAxisItem.Add(null, double.NaN);
                            }
                        }
                        this._axisData.Add(axisValue, labelAxisItem);
                    }

                    labelAxisItem.Add(value, double.NaN);
                }

                //补齐没有的项
                foreach (var labelAxisItem2 in this._axisData.Values)
                {
                    while (labelAxisItem2.Count <= preCount)
                    {
                        labelAxisItem2.Add(null, double.NaN);
                    }
                }
            }
        }

        private Dictionary<ColumnSeries, double> GetSeriesSizeDic(ChartCollection<ISeries> seriesCollection)
        {
            Dictionary<ColumnSeries, double> seriesSizeDic = new Dictionary<ColumnSeries, double>();
            ColumnSeries columnSeries;
            Rectangle templateColumn;
            double seriesSize;
            foreach (var series in seriesCollection)
            {
                if (!(series is ColumnSeries) |
                   series.AxisX != this && series.AxisY != this)
                {
                    continue;
                }

                columnSeries = (ColumnSeries)series;
                if (seriesSizeDic.ContainsKey(columnSeries))
                {
                    continue;
                }

                columnSeries.Size = double.NaN;
                templateColumn = columnSeries.CreateColumn(null);
                seriesSize = double.NaN;
                if (series.AxisX == this)
                {
                    seriesSize = templateColumn.Width;
                }
                else if (series.AxisY == this)
                {
                    seriesSize = templateColumn.Height;
                }

                seriesSizeDic.Add(columnSeries, seriesSize);
            }

            return seriesSizeDic;
        }

        private double CalculateLabelStepSize(int labelCount, double axisSize)
        {
            return axisSize / labelCount;
        }


        private double CalculateSeriesSize(Dictionary<ColumnSeries, double> seriesSizeDic, double labelStepSize)
        {
            double totalSeriesSize = AxisConstant.ZERO_D;
            int seriesAutoSizeCount = 0;
            foreach (var seriesSize in seriesSizeDic.Values)
            {
                if (AxisHelper.DoubleHasValue(seriesSize))
                {
                    totalSeriesSize += seriesSize;
                }
                else
                {
                    seriesAutoSizeCount++;
                }
            }

            if (seriesAutoSizeCount > 0)
            {
                double seriesAutoSize = AxisConstant.ZERO_D;
                double availableSpace = labelStepSize - totalSeriesSize;
                if (availableSpace >= base._PRE)
                {
                    seriesAutoSize = availableSpace / (seriesAutoSizeCount + this._autoSizeFactor);

                    //计算固定大小的平均值
                    int fixSeriesSizeCount = seriesSizeDic.Count - seriesAutoSizeCount;
                    if (fixSeriesSizeCount > 0)
                    {
                        double avgSeriesSize = totalSeriesSize / fixSeriesSizeCount;
                        if (avgSeriesSize - seriesAutoSize < base._PRE)
                        {
                            //取固定大小平均值与可用空间自动计算值中的更小的值
                            seriesAutoSize = avgSeriesSize;
                        }
                    }
                }

                int index = -1;
                foreach (var kv in seriesSizeDic)
                {
                    index++;
                    if (AxisHelper.DoubleHasValue(kv.Value))
                    {
                        seriesAutoSize = kv.Value;
                    }

                    kv.Key.Size = seriesAutoSize;
                    totalSeriesSize += seriesAutoSize;
                }
            }

            return (labelStepSize - totalSeriesSize) / 2;
        }




        protected override List<double> PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection)
        {
            this.CreateAxisData(seriesCollection);
            if (this._axisData.Count == 0)
            {
                return null;
            }

            Dictionary<ColumnSeries, double> seriesSizeDic = this.GetSeriesSizeDic(seriesCollection);
            double labelStepSize = this.CalculateLabelStepSize(this._axisData.Count, axisCanvas.Width);
            double columnSeriesOffset = this.CalculateSeriesSize(seriesSizeDic, labelStepSize);
            TextBlock label;
            var angleQuadrantInfo = new AngleQuadrantInfo(this._angle, base._PRE);
            Size labelTextSize;
            double left = 0d, labelTextOffset;
            AxisLabelLocation labelLocation = AxisLabelLocation.First;
            object labelObj;
            LabelSeriesItem labelItem;
            List<double> xList = new List<double>();
            double labelStepSizeHalf = labelStepSize / 2;
            double x;

            for (int i = 0; i < this._axisData.Count; i++)
            {
                labelObj = this._axisData.ElementAt(i).Key;
                labelItem = this._axisData.ElementAt(i).Value;

                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(labelObj));
                labelTextSize = UITextHelper.MeasureTextSize(label);
                axisCanvas.Children.Add(label);
                labelTextOffset = (labelStepSize - labelTextSize.Width) / 2;
                Canvas.SetLeft(label, left + labelTextOffset);
                if (this.IsAxisXBottom())
                {
                    Canvas.SetTop(label, AxisConstant.ZERO_D);
                    this.RotateLabelBottom(label, angleQuadrantInfo, labelTextSize);
                }
                else
                {
                    Canvas.SetBottom(label, AxisConstant.ZERO_D);
                    this.RotateLabelTop(label, angleQuadrantInfo, labelTextSize);
                }

                switch (labelLocation)
                {
                    case AxisLabelLocation.First:
                        x = columnSeriesOffset;
                        labelLocation = AxisLabelLocation.Middle;
                        break;
                    case AxisLabelLocation.Middle:
                    case AxisLabelLocation.Last:
                        x = left + columnSeriesOffset;
                        break;
                    default:
                        throw new NotImplementedException(labelLocation.ToString());
                }

                foreach (var key in labelItem.Keys.ToArray())
                {
                    labelItem[key] = x;
                    x += labelItem.Series.Size;
                }

                xList.Add(left + labelStepSizeHalf);
                left += labelStepSize;
            }

            AxisHelper.DrawXAxisLabelLine(this, axisCanvas, AxisConstant.ZERO_D, axisCanvas.Height);
            return xList;
        }
        private void RotateLabelTop(TextBlock label, AngleQuadrantInfo angleQuadrantInfo, Size labelTextSize)
        {
            if (!angleQuadrantInfo.Rotate)
            {
                return;
            }

            var transformGroup = new TransformGroup();

            var rotateTransform = new RotateTransform();
            rotateTransform.CenterX = AxisConstant.ZERO_D;
            rotateTransform.CenterY = AxisConstant.ZERO_D;
            rotateTransform.Angle = angleQuadrantInfo.Angle;
            transformGroup.Children.Add(rotateTransform);

            var translateTransform = new TranslateTransform();
            double x, y;

            switch (angleQuadrantInfo.Quadrant)
            {
                case Quadrant.One:
                    x = labelTextSize.Height * Math.Cos(angleQuadrantInfo.Radians);
                    y = labelTextSize.Height * Math.Sin(angleQuadrantInfo.Radians);
                    translateTransform.X = labelTextSize.Width / 2 - x;
                    translateTransform.Y = labelTextSize.Height - y;
                    break;
                case Quadrant.Two:
                    translateTransform.X = labelTextSize.Width / 2;
                    translateTransform.Y = labelTextSize.Height;
                    break;
                case Quadrant.Three:
                    x = labelTextSize.Width * Math.Sin(angleQuadrantInfo.Radians);
                    y = labelTextSize.Width * Math.Cos(angleQuadrantInfo.Radians);
                    translateTransform.X = labelTextSize.Width / 2 + x;
                    translateTransform.Y = labelTextSize.Height - y;
                    break;
                case Quadrant.Four:
                    x = labelTextSize.Width * Math.Cos(angleQuadrantInfo.Radians) - labelTextSize.Height * Math.Sin(angleQuadrantInfo.Radians);
                    y = labelTextSize.Width * Math.Sin(angleQuadrantInfo.Radians) + labelTextSize.Height * Math.Cos(angleQuadrantInfo.Radians);
                    translateTransform.X = labelTextSize.Width / 2 - x;
                    translateTransform.Y = labelTextSize.Height - y;
                    break;
                default:
                    throw new NotImplementedException(angleQuadrantInfo.Quadrant.ToString());
            }

            transformGroup.Children.Add(translateTransform);
            label.RenderTransform = transformGroup;
        }

        private void RotateLabelBottom(TextBlock label, AngleQuadrantInfo angleQuadrantInfo, Size labelTextSize)
        {
            if (!angleQuadrantInfo.Rotate)
            {
                return;
            }

            var transformGroup = new TransformGroup();

            var rotateTransform = new RotateTransform();
            rotateTransform.CenterX = AxisConstant.ZERO_D;
            rotateTransform.CenterY = AxisConstant.ZERO_D;
            rotateTransform.Angle = angleQuadrantInfo.Angle;
            transformGroup.Children.Add(rotateTransform);

            var translateTransform = new TranslateTransform();
            double x, y;

            switch (angleQuadrantInfo.Quadrant)
            {
                case Quadrant.One:
                    x = labelTextSize.Width * Math.Sin(angleQuadrantInfo.Radians);
                    y = labelTextSize.Width * Math.Cos(angleQuadrantInfo.Radians);
                    translateTransform.X = labelTextSize.Width / 2 - x;
                    translateTransform.Y = y;
                    break;
                case Quadrant.Two:
                    x = labelTextSize.Width * Math.Cos(angleQuadrantInfo.Radians) - labelTextSize.Height * Math.Sin(angleQuadrantInfo.Radians);
                    y = labelTextSize.Width * Math.Sin(angleQuadrantInfo.Radians) + labelTextSize.Height * Math.Cos(angleQuadrantInfo.ModRadians);
                    translateTransform.X = labelTextSize.Width / 2 + x;
                    translateTransform.Y = y;
                    break;
                case Quadrant.Three:
                    x = labelTextSize.Height * Math.Sin(angleQuadrantInfo.ModRadians);
                    y = labelTextSize.Height * Math.Cos(angleQuadrantInfo.ModRadians); ;
                    translateTransform.X = labelTextSize.Width / 2 + x;
                    translateTransform.Y = y;
                    break;
                case Quadrant.Four:
                    translateTransform.X = labelTextSize.Width / 2;
                    translateTransform.Y = AxisConstant.ZERO_D;
                    break;
                default:
                    throw new NotImplementedException(angleQuadrantInfo.Quadrant.ToString());
            }

            transformGroup.Children.Add(translateTransform);
            label.RenderTransform = transformGroup;
        }





        /// <summary>
        /// 子类重写此函数时,必须设置Y轴宽度
        /// </summary>
        /// <param name="axisCanvas"></param>
        /// <param name="seriesCollection"></param>
        protected override List<double> PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection)
        {
            axisCanvas.Width = this._axisSize;
            this.CreateAxisData(seriesCollection);
            if (this._axisData.Count == 0)
            {
                return null;
            }

            Dictionary<ColumnSeries, double> seriesSizeDic = this.GetSeriesSizeDic(seriesCollection);
            double labelStepSize = this.CalculateLabelStepSize(this._axisData.Count, axisCanvas.Height);
            double columnSeriesOffset = this.CalculateSeriesSize(seriesSizeDic, labelStepSize);
            TextBlock label;
            Size labelTextSize;
            double top = 0d, labelTextOffset;
            AxisLabelLocation labelLocation = AxisLabelLocation.First;
            object labelObj;
            LabelSeriesItem labelItem;
            double y;
            List<double> yList = new List<double>();
            double labelStepSizeHalf = labelStepSize / 2;

            for (int i = 0; i < this._axisData.Count; i++)
            {
                labelObj = this._axisData.ElementAt(i).Key;
                labelItem = this._axisData.ElementAt(i).Value;

                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(labelObj));
                labelTextSize = UITextHelper.MeasureTextSize(label);
                axisCanvas.Children.Add(label);
                labelTextOffset = (labelStepSize - labelTextSize.Height) / 2;
                Canvas.SetTop(label, top + labelTextOffset);
                if (this.IsAxisYLeft())
                {
                    Canvas.SetRight(label, AxisConstant.ZERO_D);
                }
                else
                {
                    Canvas.SetLeft(label, AxisConstant.ZERO_D);
                }

                switch (labelLocation)
                {
                    case AxisLabelLocation.First:
                        y = columnSeriesOffset;
                        labelLocation = AxisLabelLocation.Middle;
                        break;
                    case AxisLabelLocation.Middle:
                    case AxisLabelLocation.Last:
                        y = top + columnSeriesOffset;
                        break;
                    default:
                        throw new NotImplementedException(labelLocation.ToString());
                }

                foreach (var key in labelItem.Keys.ToArray())
                {
                    labelItem[key] = y;
                    y += labelItem.Series.Size;
                }

                yList.Add(top + labelStepSizeHalf);
                top += labelStepSize;
            }

            AxisHelper.DrawYAxisLabelLine(this, axisCanvas, AxisConstant.ZERO_D, axisCanvas.Height);
            return yList;
        }









        protected override double PrimitiveGetX(IChartItem chartItem)
        {
            if (chartItem == null)
            {
                return double.NaN;
            }

            var labelAxisItem = chartItem.GetXValue();
            if (labelAxisItem == null)
            {
                labelAxisItem = this._nullLabelAxisItem;
            }

            LabelSeriesItem labelSeriesItem;
            if (!this._axisData.TryGetValue(labelAxisItem, out labelSeriesItem))
            {
                return double.NaN;
            }

            double result;
            if (labelSeriesItem.TryGetValue(chartItem, out result))
            {
                return result;
            }

            return double.NaN;
        }

        protected override double PrimitiveGetY(IChartItem chartItem)
        {
            if (chartItem == null)
            {
                return double.NaN;
            }

            var labelAxisItem = chartItem.GetYValue();
            if (labelAxisItem == null)
            {
                labelAxisItem = this._nullLabelAxisItem;
            }

            LabelSeriesItem labelSeriesItem;
            if (!this._axisData.TryGetValue(labelAxisItem, out labelSeriesItem))
            {
                return double.NaN;
            }

            double result;
            if (labelSeriesItem.TryGetValue(chartItem, out result))
            {
                return result;
            }

            return double.NaN;
        }
    }

    /// <summary>
    /// [key:IChartItem;value:坐标值]
    /// </summary>
    internal class LabelSeriesItem : Dictionary<IChartItem, double>
    {
        public ColumnSeries Series { get; private set; }

        public LabelSeriesItem(ColumnSeries series)
        {
            this.Series = series;
        }
    }

    internal class AngleQuadrantInfo
    {
        private bool _rotate = false;
        public bool Rotate
        {
            get { return _rotate; }
        }

        private double _angle = double.NaN;
        public double Angle
        {
            get { return _angle; }
        }

        private Quadrant _quadrant = Quadrant.One;
        public Quadrant Quadrant
        {
            get { return _quadrant; }
        }

        private double _radians = double.NaN;
        /// <summary>
        /// 角度对应的弧度
        /// </summary>
        public double Radians
        {
            get { return _radians; }
        }

        private double _modRadians = double.NaN;
        /// <summary>
        /// 用于计算值的余角弧度
        /// </summary>
        public double ModRadians
        {
            get { return _modRadians; }
        }


        private const double ANGLE_90 = 90d;
        private const double ANGLE_180 = 180d;
        private const double ANGLE_270 = 270d;
        private const double ANGLE_360 = 360d;


        public AngleQuadrantInfo(double angle, double pre)
        {
            this._rotate = AxisHelper.DoubleHasValue(angle);
            if (!this._rotate)
            {
                return;
            }

            //将角度转换为-360到360度之间
            angle = angle % ANGLE_360;
            if (angle < AxisConstant.ZERO_D)
            {
                //将负数角度转换为正数角度
                angle += ANGLE_360;
            }
            this._angle = angle;


            //角度值无效或小于精度值,则认为不需要旋转
            if (angle <= pre)
            {
                this._rotate = false;
                return;
            }

            double mathAngle;
            Quadrant quadrant;
            if (angle - ANGLE_270 > AxisConstant.ZERO_D)
            {
                //angle:270-360°
                quadrant = Quadrant.One;
                mathAngle = angle - ANGLE_270;
                this._modRadians = (ANGLE_180 - ANGLE_90 - mathAngle) * Math.PI / ANGLE_180;
            }
            else if (angle - ANGLE_180 > AxisConstant.ZERO_D)
            {
                //angle:180-270°
                quadrant = Quadrant.Two;
                mathAngle = angle - ANGLE_180;
                this._modRadians = (ANGLE_360 - ANGLE_90 - angle) * Math.PI / ANGLE_180;
            }
            else if (angle - ANGLE_90 > AxisConstant.ZERO_D)
            {
                //angle:90-180°
                quadrant = Quadrant.Three;
                mathAngle = angle - ANGLE_90;
                this._modRadians = (ANGLE_270 - ANGLE_90 - angle) * Math.PI / ANGLE_180;
            }
            else
            {
                //angle:0-90°
                quadrant = Quadrant.Four;
                mathAngle = angle;
            }

            this._radians = mathAngle * Math.PI / ANGLE_180;
            this._quadrant = quadrant;
        }
    }

    /// <summary>
    /// XY坐标系象限类型枚举
    /// </summary>
    internal enum Quadrant
    {
        /// <summary>
        /// 第一象限[270-360°]
        /// </summary>
        One,

        /// <summary>
        /// 第二象限[180-270°]
        /// </summary>
        Two,

        /// <summary>
        /// 第三象限[90-180°]
        /// </summary>
        Three,

        /// <summary>
        /// 第四象限[0-90°]
        /// </summary>
        Four
    }
}
