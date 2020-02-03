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
        private NumberAxisData _axisData = null;



        public NumberAxis()
            : base()
        {
            base.DockOrientation = ChartDockOrientation.Left;
        }





        protected override double PrimitiveGetXAxisHeight()
        {
            string labelText = this.CreateAxisText(123d);
            double axisHeight = AxisHelper.MeasureScaleTextSize(this, labelText).Height;
            return base.CalculateAxisSize(axisHeight);
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

        private NumberAxisData CreateAxisData(ChartCollection<ISeries> seriesCollection)
        {
            var result = this.GetMinAndMaxValue(seriesCollection);
            long minMuilt, maxMuilt;
            if (AxisHelper.DoubleHasValue(this._minValue))
            {
                result.MinValue = this._minValue;

                if (AxisHelper.DoubleHasValue(this._maxValue))
                {
                    result.MaxValue = this._maxValue;
                }
                else
                {
                    if (AxisHelper.DoubleHasValue(result.MaxValue))
                    {
                        maxMuilt = AxisHelper.CalDoubleToIntegerMuilt(result.MaxValue);
                        result.MaxValue = AxisHelper.DoubleToCeilingInteger(result.MaxValue, maxMuilt);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (AxisHelper.DoubleHasValue(result.MinValue))
                {
                    minMuilt = AxisHelper.CalDoubleToIntegerMuilt(result.MinValue);
                    if (AxisHelper.DoubleHasValue(this._maxValue))
                    {
                        result.MinValue = AxisHelper.DoubleToFloorInteger(result.MinValue, minMuilt);
                        result.MaxValue = this._maxValue;
                    }
                    else
                    {
                        if (AxisHelper.DoubleHasValue(result.MaxValue))
                        {
                            maxMuilt = AxisHelper.CalDoubleToIntegerMuilt(result.MaxValue);
                            long muilt = minMuilt > maxMuilt ? minMuilt : maxMuilt;
                            result.MinValue = AxisHelper.DoubleToFloorInteger(result.MinValue, muilt);
                            result.MaxValue = AxisHelper.DoubleToCeilingInteger(result.MaxValue, muilt);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            if (result.MaxValue - result.MinValue <= base._PRE)
            {
                return null;
            }

            return new NumberAxisData(result.MinValue, result.MaxValue);
        }

        private (double MinValue, double MaxValue) GetMinAndMaxValue(ChartCollection<ISeries> seriesCollection)
        {
            double min = double.NaN, max = double.NaN;
            if (seriesCollection == null || seriesCollection.Count == AxisConstant.ZERO_I)
            {
                return (min, max);
            }

            double tmpMin, tmpMax;
            foreach (var series in seriesCollection)
            {
                if (series.AxisX != this && series.AxisY != this)
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

            return (min, max);
        }

        private double CalculateLabelStep(double valueArea, double axisSize)
        {
            double labelStep = this._labelStep;

            if (double.IsNaN(labelStep))
            {
                int labelCount = (int)(axisSize / AxisConstant.DEFAULT_STEP_SIZE);
                if (axisSize % AxisConstant.DEFAULT_STEP_SIZE > AxisConstant.ZERO_D)
                {
                    labelCount += 1;
                }

                labelStep = valueArea / labelCount;

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





        /// <summary>
        /// 必须设置Y轴宽度以及返回Y轴宽度
        /// </summary>
        /// <param name="axisCanvas"></param>
        /// <param name="seriesCollection"></param>
        /// <param name="axisHeight"></param>
        /// <returns>Y轴宽度</returns>
        protected override double PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisHeight)
        {
            this._axisData = this.CreateAxisData(seriesCollection);
            if (this._axisData == null)
            {
                return AxisConstant.AXIS_DEFAULT_SIZE;
            }

            List<double> yList;
            switch (base.Orientation)
            {
                case AxisOrientation.BottomToTop:
                    yList = this.DrawYAxisBottomToTop(axisCanvas, this._axisData);
                    break;
                case AxisOrientation.TopToBottom:
                    yList = this.DrawYAxisTopToBottom(axisCanvas, this._axisData);
                    break;
                default:
                    throw new ArgumentException($"未知的{base.Orientation.ToString()}");
            }
            AxisHelper.DrawYAxisLabelLine(this, axisCanvas, yList);
            return axisCanvas.Width;
        }
        private List<double> DrawYAxisTopToBottom(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> yList = new List<double>();
            double axisHeight = axisCanvas.Height;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisHeight);
            int separatorCount = AxisHelper.CalSeparatorCount(axisData.Area, axisHeight, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(axisData.Area, axisHeight, labelStep);

            double top = AxisConstant.ZERO_D, top2;
            double tmp = axisData.MinValue;
            double endValue = axisData.MaxValue + labelStep - base._PRE;
            double slidSeparatorSize = separatorSize;
            double y = AxisConstant.ZERO_D;
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
                            Canvas.SetTop(label, top);
                            lastLabelY = top + labelSize.Height;
                            labelTextLocation = AxisLabelTextLocation.Middle;
                            break;
                        case AxisLabelTextLocation.Middle:
                            top2 = top - labelSize.Height / 2;
                            if (top2 <= axisHeight)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetTop(label, top2);
                                lastLabelY = top2 + labelSize.Height;
                            }
                            break;
                        case AxisLabelTextLocation.Last:
                            if (lastLabelY + labelSize.Height < axisHeight)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetBottom(label, AxisConstant.ZERO_D);
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
                if (tmp >= axisData.MaxValue)
                //if (tmp - maxValue > _PRE)
                {
                    slidSeparatorSize = (labelStep - (tmp - axisData.MaxValue)) * separatorSize / labelStep;
                    double labelHeight = AxisHelper.MeasureScaleTextSize(this, axisData.MaxValue.ToString()).Height + 10d;
                    if (slidSeparatorSize < labelHeight)
                    {
                        break;
                    }

                    tmp = axisData.MaxValue;
                    y = axisHeight;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    y += slidSeparatorSize;
                }

                top += slidSeparatorSize;
            }

            axisCanvas.Width = base.CalculateAxisSize(labelWidth);
            return yList;
        }

        private List<double> DrawYAxisBottomToTop(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> yList = new List<double>();
            double axisHeight = axisCanvas.Height;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisHeight);
            int separatorCount = AxisHelper.CalSeparatorCount(axisData.Area, axisHeight, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(axisData.Area, axisHeight, labelStep);

            double bottom = AxisConstant.ZERO_D, bottom2;
            double tmp = axisData.MinValue;
            double endValue = axisData.MaxValue + labelStep - base._PRE;
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
                if (tmp >= axisData.MaxValue)
                //if (tmp - maxValue > _PRE)
                {
                    slidSeparatorSize = (labelStep - (tmp - axisData.MaxValue)) * separatorSize / labelStep;
                    double labelHeight = AxisHelper.MeasureScaleTextSize(this, axisData.MaxValue.ToString()).Height + 10d;
                    if (slidSeparatorSize < labelHeight)
                    {
                        break;
                    }

                    tmp = axisData.MaxValue;
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




        protected override void PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisWidth)
        {
            this._axisData = this.CreateAxisData(seriesCollection);
            if (this._axisData == null)
            {
                return;
            }

            List<double> xList;
            switch (base.Orientation)
            {
                case AxisOrientation.LeftToRight:
                    xList = this.DrawXAxisLeftToRight(axisCanvas, this._axisData);
                    break;
                case AxisOrientation.RightToLeft:
                    xList = this.DrawXAxisRightToLeft(axisCanvas, this._axisData);
                    break;
                default:
                    throw new ArgumentException($"未知的{base.Orientation.ToString()}");
            }
            AxisHelper.DrawXAxisLabelLine(this, axisCanvas, xList);
        }

        private List<double> DrawXAxisRightToLeft(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> xList = new List<double>();
            double axisWidth = axisCanvas.Width;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisWidth);
            int separatorCount = AxisHelper.CalSeparatorCount(axisData.Area, axisWidth, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(axisData.Area, axisWidth, labelStep);

            double right = AxisConstant.ZERO_D;
            double lastLabelX = axisWidth;
            double x = axisWidth;
            double tmp = axisData.MinValue;
            double endValue = axisData.MaxValue + labelStep - base._PRE;
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
                            Canvas.SetRight(label, right);
                            lastLabelX = right + labelSize.Width;
                            labelTextLocation = AxisLabelTextLocation.Middle;
                            break;
                        case AxisLabelTextLocation.Middle:
                            right += separatorSize;
                            offset = right - labelSize.Width / 2 - lastLabelX;
                            if (offset >= AxisConstant.LABEL_TEXT_INTERVAL)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetRight(label, right - labelSize.Width / 2);
                                lastLabelX = right + labelSize.Width;
                            }
                            else if (offset > 0)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetRight(label, right - labelSize.Width / 2 + offset);
                                lastLabelX = right + labelSize.Width + offset;
                            }
                            break;
                        case AxisLabelTextLocation.Last:
                            if (right>AxisConstant.ZERO_D&& labelSize.Width + AxisConstant.LABEL_TEXT_INTERVAL <= lastLabelX)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetLeft(label, AxisConstant.ZERO_D);
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

                if (tmp >= axisData.MaxValue)
                //if (tmp - axisData.MaxValue > _PRE)
                {
                    tmp = axisData.MaxValue;
                    x = AxisConstant.ZERO_D;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    x -= separatorSize;
                }
            }

            return xList;
        }

        private List<double> DrawXAxisLeftToRight(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> xList = new List<double>();
            double axisWidth = axisCanvas.Width;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisWidth);
            int separatorCount = AxisHelper.CalSeparatorCount(axisData.Area, axisWidth, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(axisData.Area, axisWidth, labelStep);

            double left = AxisConstant.ZERO_D;
            double lastLabelX = AxisConstant.ZERO_D;
            double x = AxisConstant.ZERO_D;
            double tmp = axisData.MinValue;
            double endValue = axisData.MaxValue + labelStep - base._PRE;
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

                if (tmp >= axisData.MaxValue)
                //if (tmp - axisData.MaxValue > _PRE)
                {
                    tmp = axisData.MaxValue;
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





        protected override double PrimitiveGetX(IChartItem chartItem)
        {
            var chartNumberItem = chartItem as IChartNumberItem;
            if (this._axisData == null || chartNumberItem == null)
            {
                return double.NaN;
            }

            //默认AxisOrientation.LeftToRight
            double result = base._axisCanvas.Width * (chartNumberItem.AxisXValue - this._minValue) / this._axisData.Area;
            if (base.Orientation == AxisOrientation.RightToLeft)
            {
                result = base._axisCanvas.Width - result;
            }

            return result;
        }

        protected override double PrimitiveGetY(IChartItem chartItem)
        {
            if (this._axisData == null || chartItem == null)
            {
                return double.NaN;
            }

            double result = base._axisCanvas.Height * (chartItem.Value - this._minValue) / this._axisData.Area;
            if (base.Orientation == AxisOrientation.BottomToTop)
            {
                result = base._axisCanvas.Height - result;
            }

            return result;
        }
    }

    internal class NumberAxisData
    {
        public double MinValue { get; private set; }
        public double MaxValue { get; private set; }

        public double Area { get; private set; }

        public NumberAxisData(double minValue, double maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Area = maxValue - minValue;
        }
    }
}
