using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UtilZ.DotnetCore.WindowEx.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
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


        private bool _showLastLabel = true;
        public bool ShowLastLabel
        {
            get { return _showLastLabel; }
            set
            {
                _showLastLabel = value;
                base.OnRaisePropertyChanged(nameof(ShowLastLabel));
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
            double axisHeight = AxisHelper.MeasureLabelTextSize(this, labelText).Height;
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
            NumberAxisValueArea result = this.GetMinAndMaxValue(seriesCollection);
            long minMuilt, maxMuilt;
            if (AxisHelper.DoubleHasValue(this._minValue))
            {
                result.Min = this._minValue;

                if (AxisHelper.DoubleHasValue(this._maxValue))
                {
                    result.Max = this._maxValue;
                }
                else
                {
                    if (AxisHelper.DoubleHasValue(result.Max))
                    {
                        maxMuilt = AxisHelper.CalDoubleToIntegerMuilt(result.Max);
                        result.Max = AxisHelper.DoubleToCeilingInteger(result.Max, maxMuilt);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (AxisHelper.DoubleHasValue(result.Min))
                {
                    minMuilt = AxisHelper.CalDoubleToIntegerMuilt(result.Min);
                    if (AxisHelper.DoubleHasValue(this._maxValue))
                    {
                        result.Min = AxisHelper.DoubleToFloorInteger(result.Min, minMuilt);
                        result.Max = this._maxValue;
                    }
                    else
                    {
                        if (AxisHelper.DoubleHasValue(result.Max))
                        {
                            maxMuilt = AxisHelper.CalDoubleToIntegerMuilt(result.Max);
                            long muilt = minMuilt > maxMuilt ? minMuilt : maxMuilt;
                            result.Min = AxisHelper.DoubleToFloorInteger(result.Min, muilt);
                            result.Max = AxisHelper.DoubleToCeilingInteger(result.Max, muilt);
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

            if (result.Max - result.Min <= base._PRE)
            {
                return null;
            }

            return new NumberAxisData(result.Min, result.Max);
        }

        private NumberAxisValueArea GetMinAndMaxValue(ChartCollection<ISeries> seriesCollection)
        {
            double min = double.NaN, max = double.NaN;
            if (seriesCollection == null || seriesCollection.Count == AxisConstant.ZERO_I)
            {
                return new NumberAxisValueArea(min, max);
            }

            double tmpMin, tmpMax;
            foreach (var series in seriesCollection)
            {
                if (series.AxisX != this && series.AxisY != this)
                {
                    continue;
                }

                this.PrimitiveGetMinAndMaxValue(this, series.Values, out tmpMin, out tmpMax);
                if (double.IsNaN(min) || tmpMin - min < base._PRE)
                {
                    min = tmpMin;
                }

                if (double.IsNaN(max) || tmpMax - max > base._PRE)
                {
                    max = tmpMax;
                }
            }

            return new NumberAxisValueArea(min, max);
        }

        private void PrimitiveGetMinAndMaxValue(AxisAbs axis, ChartCollection<IChartValue> values, out double min, out double max)
        {
            min = double.NaN;
            max = double.NaN;

            if (values == null || values.Count == 0)
            {
                return;
            }

            double pre = double.IsNaN(axis.PRE) ? AxisConstant.ZERO_D : axis.PRE;
            object obj;
            IEnumerable enumerable;
            double tmp, tmpTotalChild;

            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                switch (axis.AxisType)
                {
                    case AxisType.X:
                        obj = value.GetXValue();
                        break;
                    case AxisType.Y:
                        obj = value.GetYValue();
                        break;
                    default:
                        throw new NotImplementedException(axis.AxisType.ToString());
                }

                if (obj == null)
                {
                    continue;
                }

                if (obj is IEnumerable)
                {
                    enumerable = (IEnumerable)obj;
                    tmpTotalChild = double.NaN;
                    foreach (var item in enumerable)
                    {
                        if (item == null || !(item is IChartChildValue))
                        {
                            continue;
                        }

                        tmp = AxisHelper.ConvertToDouble(((IChartChildValue)item).GetValue());
                        if (!AxisHelper.DoubleHasValue(tmp))
                        {
                            continue;
                        }

                        if (double.IsNaN(min) || tmp - min < pre)
                        {
                            min = tmp;
                        }

                        if (AxisHelper.DoubleHasValue(tmpTotalChild))
                        {
                            tmpTotalChild += tmp;
                        }
                        else
                        {
                            tmpTotalChild = tmp;
                        }
                    }

                    if (!AxisHelper.DoubleHasValue(tmpTotalChild))
                    {
                        continue;
                    }

                    if (double.IsNaN(max) || tmpTotalChild - max > pre)
                    {
                        max = tmpTotalChild;
                    }
                }
                else
                {
                    tmp = AxisHelper.ConvertToDouble(obj);
                    if (!AxisHelper.DoubleHasValue(tmp))
                    {
                        continue;
                    }

                    if (double.IsNaN(min) || tmp - min < pre)
                    {
                        min = tmp;
                    }

                    if (double.IsNaN(max) || tmp - max > pre)
                    {
                        max = tmp;
                    }
                }
            }
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
        /// 必须设置Y轴宽度
        /// </summary>
        /// <param name="axisCanvas"></param>
        /// <param name="seriesCollection"></param>
        protected override List<double> PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection)
        {
            this._axisData = this.CreateAxisData(seriesCollection);
            if (this._axisData == null)
            {
                axisCanvas.Width = AxisConstant.AXIS_DEFAULT_SIZE;
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
            return yList;
        }
        private List<double> DrawYAxisTopToBottom(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> yList = new List<double>();
            double axisHeight = axisCanvas.Height;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisHeight);
            double labelStepSize = AxisHelper.CalculateLabelStepSize(axisData.Area, axisHeight, labelStep);
            double labelTextLineInterval = base.GetAxisYLabelTextLineInterval();
            double top = AxisConstant.ZERO_D, top2;
            double value = axisData.MinValue;
            double y = AxisConstant.ZERO_D;
            TextBlock label;
            Size labelSize;
            double labelWidth = AxisConstant.ZERO_D;
            AxisLabelLocation labelTextLocation = AxisLabelLocation.First;
            bool addLabelControl;
            double lastLabelY = axisHeight;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(value));
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
                        case AxisLabelLocation.First:
                            axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetTop(label, top);
                            lastLabelY = top + labelSize.Height;
                            labelTextLocation = AxisLabelLocation.Middle;
                            break;
                        case AxisLabelLocation.Middle:
                            top2 = top - labelSize.Height / 2;
                            if (top2 <= axisHeight)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetTop(label, top2);
                                lastLabelY = top2 + labelSize.Height;
                            }
                            break;
                        case AxisLabelLocation.Last:
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
                            Canvas.SetRight(label, labelTextLineInterval);
                        }
                        else
                        {
                            Canvas.SetLeft(label, labelTextLineInterval);
                        }
                    }
                }

                if (labelTextLocation == AxisLabelLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        yList.Add(y);
                    }
                    break;
                }
                yList.Add(y);

                value += labelStep;
                if (value >= axisData.MaxValue)
                //if (tmp - maxValue > _PRE)
                {
                    labelStepSize = (labelStep - (value - axisData.MaxValue)) * labelStepSize / labelStep;
                    double labelHeight = AxisHelper.MeasureLabelTextSize(this, axisData.MaxValue.ToString()).Height + 10d;
                    if (labelStepSize < labelHeight)
                    {
                        break;
                    }

                    value = axisData.MaxValue;
                    y = axisHeight;
                    labelTextLocation = AxisLabelLocation.Last;
                }
                else
                {
                    y += labelStepSize;
                }

                top += labelStepSize;
            }

            axisCanvas.Width = base.CalculateAxisSize(labelWidth);
            return yList;
        }

        private List<double> DrawYAxisBottomToTop(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> yList = new List<double>();
            double axisHeight = axisCanvas.Height;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisHeight);
            double labelStepSize = AxisHelper.CalculateLabelStepSize(axisData.Area, axisHeight, labelStep);
            double labelTextLineInterval = base.GetAxisYLabelTextLineInterval();
            double bottom = AxisConstant.ZERO_D, bottom2;
            double value = axisData.MinValue;
            double y = axisHeight;
            TextBlock label;
            Size labelSize;
            double labelWidth = AxisConstant.ZERO_D;
            AxisLabelLocation labelTextLocation = AxisLabelLocation.First;
            bool addLabelControl;
            double lastLabelY = axisHeight;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(value));
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
                        case AxisLabelLocation.First:
                            axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetBottom(label, bottom);
                            lastLabelY = axisHeight - labelSize.Height;
                            labelTextLocation = AxisLabelLocation.Middle;
                            break;
                        case AxisLabelLocation.Middle:
                            bottom2 = bottom - labelSize.Height / 2;
                            if (bottom2 > AxisConstant.ZERO_D)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;
                                Canvas.SetBottom(label, bottom2);
                                lastLabelY = bottom2 - labelSize.Height;
                            }
                            break;
                        case AxisLabelLocation.Last:
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
                            Canvas.SetRight(label, labelTextLineInterval);
                        }
                        else
                        {
                            Canvas.SetLeft(label, labelTextLineInterval);
                        }
                    }
                }

                if (labelTextLocation == AxisLabelLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        yList.Add(y);
                    }
                    break;
                }
                yList.Add(y);

                value += labelStep;
                if (value >= axisData.MaxValue)
                {
                    labelStepSize = (labelStep - (value - axisData.MaxValue)) * labelStepSize / labelStep;
                    double labelHeight = AxisHelper.MeasureLabelTextSize(this, axisData.MaxValue.ToString()).Height + 10d;
                    if (labelStepSize < labelHeight)
                    {
                        break;
                    }

                    value = axisData.MaxValue;
                    y = AxisConstant.ZERO_D;
                    labelTextLocation = AxisLabelLocation.Last;
                }
                else
                {
                    y -= labelStepSize;
                }

                bottom += labelStepSize;
            }

            axisCanvas.Width = base.CalculateAxisSize(labelWidth);
            return yList;
        }




        protected override List<double> PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection)
        {
            this._axisData = this.CreateAxisData(seriesCollection);
            if (this._axisData == null)
            {
                return null;
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
            return xList;
        }

        private List<double> DrawXAxisRightToLeft(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> xList = new List<double>();
            double axisWidth = axisCanvas.Width;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisWidth);
            double labelStepSize = AxisHelper.CalculateLabelStepSize(axisData.Area, axisWidth, labelStep);
            double right = AxisConstant.ZERO_D;
            double lastLabelX = axisWidth;
            double x = axisWidth;
            double value = axisData.MinValue;
            TextBlock label;
            Size labelSize;
            AxisLabelLocation labelTextLocation = AxisLabelLocation.First;
            bool addLabelControl;
            double offset;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(value));
                labelSize = UITextHelper.MeasureTextSize(label);
                if (axisWidth - labelSize.Width > AxisConstant.LABEL_TEXT_INTERVAL)
                {
                    addLabelControl = false;
                    switch (labelTextLocation)
                    {
                        case AxisLabelLocation.First:
                            axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetRight(label, right);
                            lastLabelX = right + labelSize.Width;
                            labelTextLocation = AxisLabelLocation.Middle;
                            break;
                        case AxisLabelLocation.Middle:
                            right += labelStepSize;
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
                        case AxisLabelLocation.Last:
                            if (right > AxisConstant.ZERO_D && labelSize.Width + AxisConstant.LABEL_TEXT_INTERVAL <= lastLabelX)
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
                            Canvas.SetBottom(label, AxisConstant.LABEL_TEXT_INTERVAL);
                        }
                        else
                        {
                            Canvas.SetTop(label, AxisConstant.LABEL_TEXT_INTERVAL);
                        }
                    }
                }

                if (labelTextLocation == AxisLabelLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        xList.Add(x);
                    }
                    break;
                }
                xList.Add(x);

                value += labelStep;

                if (value >= axisData.MaxValue)
                //if (tmp - axisData.MaxValue > _PRE)
                {
                    value = axisData.MaxValue;
                    x = AxisConstant.ZERO_D;
                    labelTextLocation = AxisLabelLocation.Last;
                }
                else
                {
                    x -= labelStepSize;
                }
            }

            return xList;
        }

        private List<double> DrawXAxisLeftToRight(Canvas axisCanvas, NumberAxisData axisData)
        {
            List<double> xList = new List<double>();
            double axisWidth = axisCanvas.Width;
            double labelStep = this.CalculateLabelStep(axisData.Area, axisWidth);
            double labelStepSize = AxisHelper.CalculateLabelStepSize(axisData.Area, axisWidth, labelStep);
            double left = AxisConstant.ZERO_D;
            double lastLabelX = AxisConstant.ZERO_D;
            double x = AxisConstant.ZERO_D;
            double value = axisData.MinValue;
            TextBlock label;
            Size labelSize;
            AxisLabelLocation labelTextLocation = AxisLabelLocation.First;
            bool addLabelControl;
            double offset;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(value));
                labelSize = UITextHelper.MeasureTextSize(label);
                if (axisWidth - labelSize.Width > AxisConstant.LABEL_TEXT_INTERVAL)
                {
                    addLabelControl = false;
                    switch (labelTextLocation)
                    {
                        case AxisLabelLocation.First:
                            axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetLeft(label, left);
                            lastLabelX = left + labelSize.Width;
                            labelTextLocation = AxisLabelLocation.Middle;
                            break;
                        case AxisLabelLocation.Middle:
                            left += labelStepSize;
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
                        case AxisLabelLocation.Last:
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
                            Canvas.SetBottom(label, AxisConstant.LABEL_TEXT_INTERVAL);
                        }
                        else
                        {
                            Canvas.SetTop(label, AxisConstant.LABEL_TEXT_INTERVAL);
                        }
                    }
                }

                if (labelTextLocation == AxisLabelLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        xList.Add(x);
                    }
                    break;
                }
                xList.Add(x);

                value += labelStep;

                if (value >= axisData.MaxValue)
                //if (tmp - axisData.MaxValue > _PRE)
                {
                    value = axisData.MaxValue;
                    x = axisWidth;
                    labelTextLocation = AxisLabelLocation.Last;
                }
                else
                {
                    x += labelStepSize;
                }
            }

            return xList;
        }





        protected override double PrimitiveGetX(IChartItem item)
        {
            return this.GetAxis(item, true, base._axisCanvas.Width);
        }

        protected override double PrimitiveGetY(IChartItem item)
        {
            return this.GetAxis(item, false, base._axisCanvas.Height);
        }

        private double GetAxis(IChartItem item, bool x, double axisSize)
        {
            if (this._axisData == null)
            {
                return double.NaN;
            }

            object obj = AxisHelper.GetChartItemAxisValue(item, x);
            if (item == null)
            {
                return double.NaN;
            }

            double value = AxisHelper.ConvertToDouble(obj);
            if (!AxisHelper.DoubleHasValue(value))
            {
                return double.NaN;
            }

            double result = axisSize * (value - this._axisData.MinValue) / this._axisData.Area;
            if (base.Orientation == AxisOrientation.BottomToTop ||
                base.Orientation == AxisOrientation.RightToLeft)
            {
                result = axisSize - result;
            }

            return result;
        }
    }

    internal class NumberAxisValueArea
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public NumberAxisValueArea(double min, double max)
        {
            this.Min = min;
            this.Max = max;
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
