using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UtilZ.DotnetCore.WindowEx.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class NumberAxis : AxisAbs
    {
        private double _labelStep = double.NaN;
        /// <summary>
        /// 坐标轴刻度值间隔,为double.NaN时自动计算
        /// </summary>
        public double LabelStep
        {
            get { return _labelStep; }
            set
            {
                _labelStep = value;
                base.OnRaisePropertyChanged(nameof(LabelStep));
            }
        }


        private double _minValue = double.NaN;
        /// <summary>
        /// 坐标轴刻度最小值,为double.NaN时自动计算
        /// </summary>
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
            }
        }

        private double _maxValue = double.NaN;
        /// <summary>
        /// 坐标轴刻度最大值,为double.NaN时自动计算
        /// </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
            }
        }



        public Func<double, string> CustomAxisTextFormatCunc;

        public NumberAxis()
            : base()
        {
            base.DockOrientation = ChartDockOrientation.Left;
        }





        protected override double PrimitiveGetXAxisHeight()
        {
            double axisHeight = AxisHelper.MeasureScaleTextSize(this, "123").Height;
            return base.CalculateAxisSize(axisHeight);
        }

        /// <summary>
        /// 必须设置Y轴宽度以及返回Y轴宽度
        /// </summary>
        /// <param name="axisCanvas"></param>
        /// <param name="seriesCollection"></param>
        /// <param name="axisHeight"></param>
        /// <returns>Y轴宽度</returns>
        protected override double PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisHeight)
        {
            if (!this.UpdateMinAndMaxValue(seriesCollection))
            {
                return AxisConstant.AXIS_DEFAULT_SIZE;
            }

            List<double> yList;
            switch (base.Orientation)
            {
                case AxisOrientation.BottomToTop:
                    yList = this.DrawYAxisBottomToTop(axisCanvas, this._minValue, this._maxValue);
                    break;
                case AxisOrientation.TopToBottom:
                    yList = this.DrawYAxisTopToBottom(axisCanvas, this._minValue, this._maxValue);
                    break;
                default:
                    throw new ArgumentException($"未知的{base.Orientation.ToString()}");
            }
            AxisHelper.DrawYAxisLabelLine(this, axisCanvas, yList);
            return axisCanvas.Width;
        }


        protected override void PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisWidth)
        {
            if (!this.UpdateMinAndMaxValue(seriesCollection))
            {
                return;
            }

            List<double> xList;
            switch (base.Orientation)
            {
                case AxisOrientation.LeftToRight:
                    xList = this.DrawXAxisLeftToRight(axisCanvas, this._minValue, this._maxValue);
                    break;
                case AxisOrientation.RightToLeft:
                    xList = this.DrawXAxisRightToLeft(axisCanvas, this._minValue, this._maxValue);
                    break;
                default:
                    throw new ArgumentException($"未知的{base.Orientation.ToString()}");
            }
            AxisHelper.DrawXAxisLabelLine(this, axisCanvas, xList);
        }


        private bool UpdateMinAndMaxValue(ChartCollection<ISeries> seriesCollection)
        {
            if (!AxisHelper.DoubleHasValue(this._minValue) || !AxisHelper.DoubleHasValue(this._maxValue))
            {
                this.PrimitiveUpdateMinAndMax(seriesCollection);
            }

            if (AxisHelper.DoubleHasValue(this._minValue) || !AxisHelper.DoubleHasValue(this._maxValue))
            {
                return true;
            }

            return false;
        }






        private List<double> DrawYAxisTopToBottom(Canvas axisCanvas, double minValue, double maxValue)
        {
            throw new NotImplementedException();
        }

        private List<double> DrawYAxisBottomToTop(Canvas axisCanvas, double minValue, double maxValue)
        {
            List<double> yList = new List<double>();
            double area = maxValue - minValue;
            double axisHeight = axisCanvas.Height;
            double labelStep = this.CalculateLabelStep(area, axisHeight);
            int separatorCount = AxisHelper.CalSeparatorCount(area, axisHeight, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(area, axisHeight, labelStep);

            double bottom = AxisConstant.ZERO_D, bottom2;
            double tmp = minValue;
            double endValue = maxValue + labelStep - base._PRE;
            double slidSeparatorSize = separatorSize;
            double y = axisHeight;
            TextBlock label;
            Rect labelSize;
            double labelWidth = AxisConstant.ZERO_D;
            AxisLabelTextLocation labelTextLocation = AxisLabelTextLocation.First;
            bool addLabelControl;
            double lastLabelY = axisHeight;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(tmp));
                labelSize = UITextHelper.MeasureTextSize(label);
                if (labelSize.Width - labelWidth > base._PRE)
                {
                    labelWidth = labelSize.Width;
                }

                if (axisHeight > labelSize.Height)
                {
                    addLabelControl = false;
                    switch (labelTextLocation)
                    {
                        case AxisLabelTextLocation.First:
                            axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetBottom(label, bottom);
                            lastLabelY = axisHeight - labelSize.Height;
                            labelTextLocation = AxisLabelTextLocation.Middle;
                            break;
                        case AxisLabelTextLocation.Middle:
                            bottom2 = bottom - labelSize.Height / 2;
                            if (bottom2 > AxisConstant.ZERO_D)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetBottom(label, bottom2);
                                lastLabelY = bottom2 - labelSize.Height;
                            }
                            break;
                        case AxisLabelTextLocation.Last:
                            if (lastLabelY - labelSize.Height > AxisConstant.ZERO_D)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetTop(label, AxisConstant.ZERO_D);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    if (addLabelControl)
                    {
                        if (base.IsAxisYLeft())
                        {
                            Canvas.SetLeft(label, AxisConstant.ZERO_D);
                        }
                        else
                        {
                            Canvas.SetRight(label, AxisConstant.ZERO_D);
                        }
                    }
                }

                yList.Add(y);
                if (labelTextLocation == AxisLabelTextLocation.Last)
                {
                    break;
                }

                tmp += labelStep;
                if (tmp >= maxValue)
                //if (tmp - maxValue > _PRE)
                {
                    slidSeparatorSize = (labelStep - (tmp - maxValue)) * separatorSize / labelStep;
                    double labelHeight = AxisHelper.MeasureScaleTextSize(this, maxValue.ToString()).Height + 10d;
                    if (slidSeparatorSize < labelHeight)
                    {
                        break;
                    }

                    tmp = maxValue;
                    y = AxisConstant.ZERO_D;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    y -= slidSeparatorSize;
                }

                bottom += slidSeparatorSize;
            }

            axisCanvas.Width = base.CalculateAxisSize(labelWidth);
            return yList;
        }



        private List<double> DrawXAxisRightToLeft(Canvas axisCanvas, double minValue, double maxValue)
        {
            throw new NotImplementedException();
        }

        private List<double> DrawXAxisLeftToRight(Canvas axisCanvas, double minValue, double maxValue)
        {
            List<double> xList = new List<double>();
            double area = maxValue - minValue;
            double axisWidth = axisCanvas.Width;
            double labelStep = this.CalculateLabelStep(area, axisWidth);
            int separatorCount = AxisHelper.CalSeparatorCount(area, axisWidth, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(area, axisWidth, labelStep);

            double left = AxisConstant.ZERO_D;
            double lastLabelX = AxisConstant.ZERO_D;
            double x = AxisConstant.ZERO_D;
            double tmp = minValue;
            double endValue = maxValue + labelStep - base._PRE;
            TextBlock label;
            Rect labelSize;
            AxisLabelTextLocation labelTextLocation = AxisLabelTextLocation.First;
            bool addLabelControl;
            double offset;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(tmp));
                labelSize = UITextHelper.MeasureTextSize(label);
                if (axisWidth - labelSize.Width > AxisConstant.LABEL_TEXT_INTERVAL)
                {
                    addLabelControl = false;
                    switch (labelTextLocation)
                    {
                        case AxisLabelTextLocation.First:
                            axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetLeft(label, left);
                            lastLabelX = left + labelSize.Width;
                            labelTextLocation = AxisLabelTextLocation.Middle;
                            break;
                        case AxisLabelTextLocation.Middle:
                            left += separatorSize;
                            offset = left - labelSize.Width / 2 - lastLabelX;
                            if (offset >= AxisConstant.LABEL_TEXT_INTERVAL)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetLeft(label, left - labelSize.Width / 2);
                                lastLabelX = left + labelSize.Width;
                            }
                            else if (offset > 0)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetLeft(label, left - labelSize.Width / 2 + offset);
                                lastLabelX = left + labelSize.Width + offset;
                            }
                            break;
                        case AxisLabelTextLocation.Last:
                            if (lastLabelX + labelSize.Width + AxisConstant.LABEL_TEXT_INTERVAL <= axisWidth)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetRight(label, AxisConstant.ZERO_D);
                                lastLabelX = axisWidth;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    if (addLabelControl)
                    {
                        if (base.IsAxisXBottom())
                        {
                            Canvas.SetBottom(label, AxisConstant.ZERO_D);
                        }
                        else
                        {
                            Canvas.SetTop(label, AxisConstant.ZERO_D);
                        }
                    }
                }

                xList.Add(x);
                if (labelTextLocation == AxisLabelTextLocation.Last)
                {
                    break;
                }

                tmp += labelStep;

                if (tmp >= maxValue)
                //if (tmp - maxValue > _PRE)
                {
                    tmp = maxValue;
                    x = axisWidth;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    x += separatorSize;
                }
            }

            return xList;
        }

        private string CreateAxisText(double value)
        {
            string axisText;
            var customAxisTextFormatCunc = this.CustomAxisTextFormatCunc;
            if (customAxisTextFormatCunc != null)
            {
                axisText = customAxisTextFormatCunc(value);
            }
            else
            {
                axisText = value.ToString();
            }

            return axisText;
        }

        private double CalculateLabelStep(double valueArea, double axisSize)
        {
            double labelStep = this._labelStep;

            if (double.IsNaN(labelStep))
            {
                int separatorCount = (int)(axisSize / AxisConstant.DEFAULT_STEP_SIZE);
                if (axisSize % AxisConstant.DEFAULT_STEP_SIZE > AxisConstant.ZERO_D)
                {
                    separatorCount += 1;
                }

                labelStep = valueArea / separatorCount;

                var muilt = AxisHelper.CalDoubleToIntegerMuilt(labelStep);
                var step2 = AxisHelper.DoubleToCeilingInteger(labelStep, muilt);
                while (step2 >= valueArea && muilt >= 1)
                {
                    muilt = muilt / 10;
                    step2 = AxisHelper.DoubleToCeilingInteger(labelStep, muilt);
                }

                if (!double.IsNaN(step2))
                {
                    labelStep = step2;
                }

                labelStep = (double)((long)(labelStep * 100)) / 100;
            }

            return labelStep;
        }




        private void PrimitiveUpdateMinAndMax(ChartCollection<ISeries> seriesCollection)
        {
            double min, max;
            this.GetMinAndMaxValue(seriesCollection, out min, out max);
            if (!AxisHelper.DoubleHasValue(min) || !AxisHelper.DoubleHasValue(max))
            {
                return;
            }

            long minMuilt = AxisHelper.CalDoubleToIntegerMuilt(min);
            long maxMuilt = AxisHelper.CalDoubleToIntegerMuilt(max);
            long muilt = minMuilt > maxMuilt ? minMuilt : maxMuilt;

            this.UpdateMin(min, muilt);
            this.UpdateMax(max, muilt);
        }

        private void UpdateMin(double min, long muilt)
        {
            if (double.IsNaN(min))
            {
                return;
            }

            if (AxisHelper.DoubleHasValue(this._minValue))
            {
                return;
            }

            min = AxisHelper.DoubleToFloorInteger(min, muilt);
            if (min - this._minValue < base._PRE)
            {
                this._minValue = min;
            }
        }

        private void UpdateMax(double max, long muilt)
        {
            if (double.IsNaN(max))
            {
                return;
            }

            if (AxisHelper.DoubleHasValue(this._maxValue))
            {
                return;
            }

            max = AxisHelper.DoubleToCeilingInteger(max, muilt);
            if (max - this._maxValue > base._PRE)
            {
                this._maxValue = AxisHelper.DoubleToCeilingInteger(max, muilt);
            }
        }

        private void GetMinAndMaxValue(ChartCollection<ISeries> seriesCollection, out double min, out double max)
        {
            min = double.NaN;
            max = double.NaN;
            if (seriesCollection == null || seriesCollection.Count == AxisConstant.ZERO_I)
            {
                return;
            }

            double tmpMin, tmpMax;
            switch (this.AxisType)
            {
                case AxisType.X:
                    foreach (var series in seriesCollection)
                    {
                        if (series.AxisX != this)
                        {
                            continue;
                        }

                        series.GetAxisValueArea(this, out tmpMin, out tmpMax);
                        if (double.IsNaN(min) || tmpMin - min < base._PRE)
                        {
                            min = tmpMin;
                        }

                        if (double.IsNaN(max) || tmpMax - max > base._PRE)
                        {
                            max = tmpMax;
                        }
                    }
                    break;
                case AxisType.Y:
                    foreach (var series in seriesCollection)
                    {
                        if (series.AxisY != this)
                        {
                            continue;
                        }

                        series.GetAxisValueArea(this, out tmpMin, out tmpMax);
                        if (double.IsNaN(min) || tmpMin - min < base._PRE)
                        {
                            min = tmpMin;
                        }

                        if (double.IsNaN(max) || tmpMax - max > base._PRE)
                        {
                            max = tmpMax;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
