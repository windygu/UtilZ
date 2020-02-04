﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class DateTimeAxis : AxisAbs
    {
        private TimeSpan? _labelStep = null;
        /// <summary>
        /// 坐标轴刻度值间隔,为null时自动计算
        /// </summary>
        public TimeSpan? LabelStep
        {
            get { return _labelStep; }
            set
            {
                _labelStep = value;
                base.OnRaisePropertyChanged(nameof(LabelStep));
            }
        }


        private DateTime? _minValue = null;
        /// <summary>
        /// 坐标轴刻度最小值,为null时自动计算
        /// </summary>
        public DateTime? MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                base.OnRaisePropertyChanged(nameof(MinValue));
            }
        }

        private DateTime? _maxValue = null;
        /// <summary>
        /// 坐标轴刻度最大值,为null时自动计算
        /// </summary>
        public DateTime? MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                base.OnRaisePropertyChanged(nameof(MaxValue));
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





        public Func<DateTime, string> CustomAxisTextFormatCunc;
        private DateTimeAxisData _axisData = null;



        public DateTimeAxis()
            : base()
        {

        }


        private Rect _labelTextSize;

        protected override double PrimitiveGetXAxisHeight()
        {
            this._labelTextSize = this.MeasureLabelTextSize();
            return base.CalculateAxisSize(this._labelTextSize.Height);
        }

        private Rect MeasureLabelTextSize()
        {
            string labelText = this.CreateAxisText(DateTime.Parse("2020-12-12 22:22:22"));
            return AxisHelper.MeasureLabelTextSize(this, labelText);
        }

        private string CreateAxisText(DateTime value)
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

        private DateTimeAxisData CreateAxisData(ChartCollection<ISeries> seriesCollection)
        {
            if (this._minValue == null || this._maxValue == null)
            {
                var result = this.GetMinAndMaxValue(seriesCollection);
                if (this._minValue != null)
                {
                    result.MinTime = this._minValue.Value;
                }

                if (this._maxValue != null)
                {
                    result.MaxTime = this._maxValue.Value;
                }

                if (result.MinTime == null || result.MaxTime == null)
                {
                    return null;
                }
                else
                {
                    return new DateTimeAxisData(result.MinTime.Value, result.MaxTime.Value);
                }
            }
            else
            {
                return new DateTimeAxisData(this._minValue.Value, this._maxValue.Value);
            }
        }

        private (DateTime? MinTime, DateTime? MaxTime) GetMinAndMaxValue(ChartCollection<ISeries> seriesCollection)
        {
            DateTime? min = null, max = null;
            IChartDateTimeItem chartDateTimeItem;

            foreach (var series in seriesCollection)
            {
                if (series.AxisX != this && series.AxisY != this ||
                    series.Values == null ||
                    series.Values.Count == 0)
                {
                    continue;
                }

                foreach (var value in series.Values)
                {
                    chartDateTimeItem = (IChartDateTimeItem)value;
                    if (chartDateTimeItem == null)
                    {
                        continue;
                    }

                    if (min == null || chartDateTimeItem.Time < min.Value)
                    {
                        min = chartDateTimeItem.Time;
                    }

                    if (max == null || chartDateTimeItem.Time > max.Value)
                    {
                        max = chartDateTimeItem.Time;
                    }
                }
            }

            return (min, max);
        }




        private double CalculateXLabelStep(DateTimeAxisData axisData)
        {
            double labelStepMilliseconds = this._labelStep != null ? this._labelStep.Value.TotalMilliseconds : double.NaN;
            if (double.IsNaN(labelStepMilliseconds))
            {
                const double INTERVAL = 20d;
                double labelTextSpace = INTERVAL + this._labelTextSize.Width * 1.5d;
                int count = (int)(this._axisCanvas.Width / labelTextSpace);
                if (count == 0)
                {
                    labelStepMilliseconds = axisData.Area.TotalMilliseconds;
                }
                else
                {
                    TimeSpan intervalTimeLength = TimeSpan.FromMilliseconds(axisData.Area.TotalMilliseconds / count);//一个刻度内时长
                    labelStepMilliseconds = this.AdjustIntervalTimeLength(intervalTimeLength);
                }
            }

            return labelStepMilliseconds;
        }

        private double CalculateYLabelStep(DateTimeAxisData axisData)
        {
            double labelStepMilliseconds = this._labelStep != null ? this._labelStep.Value.TotalMilliseconds : double.NaN;
            if (double.IsNaN(labelStepMilliseconds))
            {
                int labelCount = (int)(this._axisCanvas.Height / AxisConstant.DEFAULT_STEP_SIZE);
                if (this._axisCanvas.Height % AxisConstant.DEFAULT_STEP_SIZE > AxisConstant.ZERO_D)
                {
                    labelCount += 1;
                }

                if (labelCount == 0)
                {
                    labelStepMilliseconds = axisData.Area.TotalMilliseconds;
                }
                else
                {
                    TimeSpan intervalTimeLength = TimeSpan.FromMilliseconds(axisData.Area.TotalMilliseconds / labelCount);//一个刻度内时长
                    labelStepMilliseconds = this.AdjustIntervalTimeLength(intervalTimeLength);
                }
            }

            return labelStepMilliseconds;
        }

        private double AdjustIntervalTimeLength(TimeSpan intervalTimeLength)
        {
            double stepMilliseconds;

            const int YEAR_DAYS = 365;
            const int MONTH_DAYS = 365;
            const int ZERO = 0;
            const double HALF = 0.5d;
            const double DAY_MILLISECONDES = 86400000d;
            const double HOUR_MILLISECONDES = 3600000d;
            const double MINUT_MILLISECONDES = 60000d;
            const double SECOND_MILLISECONDES = 1000d;
            const double OFFSET = 1.0d;

            if (intervalTimeLength.Days >= YEAR_DAYS)
            {
                double totalYears = intervalTimeLength.TotalDays / YEAR_DAYS;
                long intergerYears = (long)totalYears;
                if (totalYears - intergerYears > +HALF)
                {
                    stepMilliseconds = Math.Ceiling(intervalTimeLength.TotalDays / YEAR_DAYS) * YEAR_DAYS * DAY_MILLISECONDES;
                }
                else
                {
                    stepMilliseconds = (HALF + (double)intergerYears) * YEAR_DAYS * DAY_MILLISECONDES;
                }
            }
            else if (intervalTimeLength.Days >= MONTH_DAYS)
            {
                double totalMonths = intervalTimeLength.TotalDays / MONTH_DAYS;
                long intergerMonths = (long)totalMonths;
                if (totalMonths - intergerMonths >= HALF)
                {
                    stepMilliseconds = Math.Ceiling(intervalTimeLength.TotalDays / MONTH_DAYS) * MONTH_DAYS * DAY_MILLISECONDES;
                }
                else
                {
                    stepMilliseconds = (HALF + (double)intergerMonths) * MONTH_DAYS * DAY_MILLISECONDES;
                }
            }
            else if (intervalTimeLength.Days > ZERO)
            {
                if (intervalTimeLength.TotalDays - intervalTimeLength.Days >= HALF)
                {
                    stepMilliseconds = Math.Ceiling(intervalTimeLength.TotalDays) * DAY_MILLISECONDES;
                }
                else
                {
                    stepMilliseconds = HALF * DAY_MILLISECONDES + intervalTimeLength.Days * DAY_MILLISECONDES;
                }
            }
            else if (intervalTimeLength.Hours > ZERO)
            {
                double hours = intervalTimeLength.TotalMilliseconds / HOUR_MILLISECONDES;
                double hoursMilliseconds = hours - intervalTimeLength.Hours;

                if (hoursMilliseconds == ZERO)
                {
                    stepMilliseconds = intervalTimeLength.TotalMilliseconds;
                }
                else if (hoursMilliseconds >= HALF)
                {
                    stepMilliseconds = Math.Ceiling(hours) * HOUR_MILLISECONDES;
                }
                else
                {
                    stepMilliseconds = ((double)intervalTimeLength.Hours + HALF) * HOUR_MILLISECONDES;
                }
            }
            else if (intervalTimeLength.Minutes > ZERO)
            {
                const double HALF_MINUT_MILLISECONDES = 30000d;
                double minutesSurplusMilliseconds = intervalTimeLength.TotalMilliseconds - intervalTimeLength.Minutes * MINUT_MILLISECONDES;
                if (minutesSurplusMilliseconds == ZERO)
                {
                    stepMilliseconds = intervalTimeLength.TotalMilliseconds;
                }
                else if (minutesSurplusMilliseconds >= HALF_MINUT_MILLISECONDES)
                {
                    stepMilliseconds = ((double)intervalTimeLength.Minutes + OFFSET) * MINUT_MILLISECONDES;
                }
                else
                {
                    stepMilliseconds = ((double)intervalTimeLength.Minutes + HALF) * MINUT_MILLISECONDES;
                }
            }
            else if (intervalTimeLength.Seconds > ZERO)
            {
                const double HALF_SECONDE_MILLISECONDES = 500d;
                double secondsSurplusMilliseconds = intervalTimeLength.TotalMilliseconds - intervalTimeLength.Seconds * SECOND_MILLISECONDES;
                if (secondsSurplusMilliseconds == ZERO)
                {
                    stepMilliseconds = intervalTimeLength.TotalMilliseconds;
                }
                else if (secondsSurplusMilliseconds >= HALF_SECONDE_MILLISECONDES)
                {
                    stepMilliseconds = ((double)intervalTimeLength.Minutes + OFFSET) * MINUT_MILLISECONDES;
                }
                else
                {
                    stepMilliseconds = ((double)intervalTimeLength.Minutes + HALF) * MINUT_MILLISECONDES;
                }
            }
            else
            {
                stepMilliseconds = Math.Ceiling(intervalTimeLength.TotalMilliseconds);
                var muilt = AxisHelper.CalDoubleToIntegerMuilt(stepMilliseconds);
                var step2 = AxisHelper.DoubleToCeilingInteger(stepMilliseconds, muilt);
                while (step2 >= intervalTimeLength.TotalMilliseconds && muilt >= 1)
                {
                    muilt = muilt / 10;
                    step2 = AxisHelper.DoubleToCeilingInteger(stepMilliseconds, muilt);
                }

                if (!double.IsNaN(step2))
                {
                    stepMilliseconds = step2;
                }
            }

            return stepMilliseconds;
        }








        protected override double PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection)
        {
            this._axisData = this.CreateAxisData(seriesCollection);
            if (this._axisData == null)
            {
                return AxisConstant.AXIS_DEFAULT_SIZE;
            }

            this._labelTextSize = this.MeasureLabelTextSize();
            axisCanvas.Width = base.CalculateAxisSize(this._labelTextSize.Width);
            double labelStepMilliseconds = this.CalculateYLabelStep(this._axisData);
            double labelStepSize = AxisHelper.CalculateLabelStepSize(this._axisData.Area.TotalMilliseconds, axisCanvas.Height, labelStepMilliseconds);
            List<double> yList;
            switch (base.Orientation)
            {
                case AxisOrientation.BottomToTop:
                    yList = this.DrawYAxisBottomToTop(axisCanvas, this._axisData, labelStepMilliseconds, labelStepSize);
                    break;
                case AxisOrientation.TopToBottom:
                    yList = this.DrawXAxisTopToBottom(axisCanvas, this._axisData, labelStepMilliseconds, labelStepSize);
                    break;
                default:
                    throw new ArgumentException($"未知的{base.Orientation.ToString()}");
            }

            AxisHelper.DrawYAxisLabelLine(this, axisCanvas, yList);
            return axisCanvas.Width;
        }

        private List<double> DrawXAxisTopToBottom(Canvas axisCanvas, DateTimeAxisData axisData, double labelStepMilliseconds, double labelStepSize)
        {
            List<double> yList = new List<double>();
            double axisHeight = axisCanvas.Height;
            double top = AxisConstant.ZERO_D, top2;
            DateTime time = axisData.MinValue;
            double y = AxisConstant.ZERO_D;
            TextBlock label;
            Rect labelSize = this._labelTextSize;
            double heightHalf = labelSize.Height / 2;
            AxisLabelTextLocation labelTextLocation = AxisLabelTextLocation.First;
            bool addLabelControl;
            double lastLabelY = axisHeight;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(time));

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
                            top2 = top - heightHalf;
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

                if (labelTextLocation == AxisLabelTextLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        yList.Add(y);
                    }
                    break;
                }
                yList.Add(y);

                time = time.AddMilliseconds(labelStepMilliseconds);
                if (time >= axisData.MaxValue)
                //if (tmp - maxValue > _PRE)
                {
                    labelStepSize = (labelStepMilliseconds - (time - axisData.MaxValue).TotalMilliseconds) * labelStepSize / labelStepMilliseconds;
                    double labelHeight = AxisHelper.MeasureLabelTextSize(this, axisData.MaxValue.ToString()).Height + 10d;
                    if (labelStepSize < labelHeight)
                    {
                        break;
                    }

                    time = axisData.MaxValue;
                    y = axisHeight;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    y += labelStepSize;
                }

                top += labelStepSize;
            }

            return yList;
        }

        private List<double> DrawYAxisBottomToTop(Canvas axisCanvas, DateTimeAxisData axisData, double labelStepMilliseconds, double labelStepSize)
        {
            List<double> yList = new List<double>();
            double axisHeight = axisCanvas.Height;
            double bottom = AxisConstant.ZERO_D, bottom2;
            DateTime time = axisData.MinValue;
            double y = axisHeight;
            TextBlock label;
            Rect labelSize = this._labelTextSize;
            double heightHalf = labelSize.Height / 2;
            AxisLabelTextLocation labelTextLocation = AxisLabelTextLocation.First;
            bool addLabelControl;
            double lastLabelY = axisHeight;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(time));

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
                            bottom2 = bottom - heightHalf;
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

                if (labelTextLocation == AxisLabelTextLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        yList.Add(y);
                    }
                    break;
                }
                yList.Add(y);

                time = time.AddMilliseconds(labelStepMilliseconds);
                if (time >= axisData.MaxValue)
                {
                    labelStepSize = (labelStepMilliseconds - (time - axisData.MaxValue).TotalMilliseconds) * labelStepSize / labelStepMilliseconds;
                    double labelHeight = AxisHelper.MeasureLabelTextSize(this, axisData.MaxValue.ToString()).Height + 10d;
                    if (labelStepSize < labelHeight)
                    {
                        break;
                    }

                    time = axisData.MaxValue;
                    y = AxisConstant.ZERO_D;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    y -= labelStepSize;
                }

                bottom += labelStepSize;
            }

            return yList;
        }

        protected override void PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection)
        {
            this._axisData = this.CreateAxisData(seriesCollection);
            if (this._axisData == null)
            {
                return;
            }

            double labelStepMilliseconds = this.CalculateXLabelStep(this._axisData);
            double labelStepSize = AxisHelper.CalculateLabelStepSize(this._axisData.Area.TotalMilliseconds, axisCanvas.Width, labelStepMilliseconds);
            List<double> xList;
            switch (base.Orientation)
            {
                case AxisOrientation.LeftToRight:
                    xList = this.DrawXAxisLeftToRight(axisCanvas, this._axisData, labelStepMilliseconds, labelStepSize);
                    break;
                case AxisOrientation.RightToLeft:
                    xList = this.DrawXAxisRightToLeft(axisCanvas, this._axisData, labelStepMilliseconds, labelStepSize);
                    break;
                default:
                    throw new ArgumentException($"未知的{base.Orientation.ToString()}");
            }
            AxisHelper.DrawXAxisLabelLine(this, axisCanvas, xList);
        }

        private List<double> DrawXAxisRightToLeft(Canvas axisCanvas, DateTimeAxisData axisData, double labelStepMilliseconds, double labelStepSize)
        {
            List<double> xList = new List<double>();
            double axisWidth = axisCanvas.Width;
            double right = AxisConstant.ZERO_D;
            double lastLabelX = axisWidth;
            DateTime time = axisData.MinValue;
            AxisLabelTextLocation labelTextLocation = AxisLabelTextLocation.First;
            double labelTextWidth = this._labelTextSize.Width;
            double labelTextWidthHalf = labelTextWidth / 2;
            double offset = labelTextWidth / 2;
            TextBlock label;
            bool addLabelControl;
            double x = axisWidth;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(time));

                if (axisWidth - labelTextWidth > AxisConstant.LABEL_TEXT_INTERVAL)
                {
                    addLabelControl = false;

                    switch (labelTextLocation)
                    {
                        case AxisLabelTextLocation.First:
                            this._axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetRight(label, right);
                            lastLabelX = right + labelTextWidth;
                            labelTextLocation = AxisLabelTextLocation.Middle;
                            break;
                        case AxisLabelTextLocation.Middle:
                            right += labelStepSize;
                            offset = right - labelTextWidthHalf - lastLabelX;
                            if (offset >= AxisConstant.LABEL_TEXT_INTERVAL)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetRight(label, right - labelTextWidthHalf);
                                lastLabelX = right + labelTextWidth;
                            }
                            else if (offset > 0)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetRight(label, right - labelTextWidthHalf + offset);
                                lastLabelX = right + labelTextWidth + offset;
                            }
                            break;
                        case AxisLabelTextLocation.Last:
                            if (right > AxisConstant.ZERO_D && labelTextWidth + AxisConstant.LABEL_TEXT_INTERVAL <= lastLabelX)
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

                if (labelTextLocation == AxisLabelTextLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        xList.Add(x);
                    }
                    break;
                }
                xList.Add(x);

                time = time.AddMilliseconds(labelStepMilliseconds);
                if (time >= axisData.MaxValue)
                {
                    x = AxisConstant.ZERO_D;
                    time = axisData.MaxValue;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    x -= labelStepSize;
                }
            }

            return xList;
        }

        private List<double> DrawXAxisLeftToRight(Canvas axisCanvas, DateTimeAxisData axisData, double labelStepMilliseconds, double labelStepSize)
        {
            List<double> xList = new List<double>();
            double axisWidth = axisCanvas.Width;
            double left = AxisConstant.ZERO_D;
            double lastLabelX = AxisConstant.ZERO_D;
            DateTime time = axisData.MinValue;
            AxisLabelTextLocation labelTextLocation = AxisLabelTextLocation.First;
            double labelTextWidth = this._labelTextSize.Width;
            double labelTextWidthHalf = labelTextWidth / 2;
            double offset = labelTextWidth / 2;
            TextBlock label;
            bool addLabelControl;
            double x = left;

            while (true)
            {
                label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(time));

                if (axisWidth - labelTextWidth > AxisConstant.LABEL_TEXT_INTERVAL)
                {
                    addLabelControl = false;

                    switch (labelTextLocation)
                    {
                        case AxisLabelTextLocation.First:
                            this._axisCanvas.Children.Add(label);
                            addLabelControl = true;
                            Canvas.SetLeft(label, left);
                            lastLabelX = left + labelTextWidth;
                            labelTextLocation = AxisLabelTextLocation.Middle;
                            break;
                        case AxisLabelTextLocation.Middle:
                            left += labelStepSize;
                            offset = left - labelTextWidthHalf - lastLabelX;
                            if (offset >= AxisConstant.LABEL_TEXT_INTERVAL)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetLeft(label, left - labelTextWidthHalf);
                                lastLabelX = left + labelTextWidth;
                            }
                            else if (offset > 0)
                            {
                                axisCanvas.Children.Add(label);
                                addLabelControl = true;

                                Canvas.SetLeft(label, left - labelTextWidthHalf + offset);
                                lastLabelX = left + labelTextWidth + offset;
                            }
                            break;
                        case AxisLabelTextLocation.Last:
                            if (lastLabelX + labelTextWidth + AxisConstant.LABEL_TEXT_INTERVAL <= axisWidth)
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

                if (labelTextLocation == AxisLabelTextLocation.Last)
                {
                    if (this._showLastLabel)
                    {
                        xList.Add(x);
                    }
                    break;
                }
                xList.Add(x);

                time = time.AddMilliseconds(labelStepMilliseconds);
                if (time >= axisData.MaxValue)
                {
                    x = this._axisCanvas.Width;
                    time = axisData.MaxValue;
                    labelTextLocation = AxisLabelTextLocation.Last;
                }
                else
                {
                    x += labelStepSize;
                }
            }

            return xList;
        }





        protected override double PrimitiveGetX(IChartItem chartItem)
        {
            if (this._axisData == null || chartItem == null)
            {
                return double.NaN;
            }

            object obj = chartItem.GetXValue();
            DateTime? value = AxisHelper.ConvertToDateTime(obj);
            if (value == null)
            {
                return double.NaN;
            }

            //默认AxisOrientation.LeftToRight
            double result = base._axisCanvas.Width * (value.Value - this._minValue.Value).TotalMilliseconds / this._axisData.Area.TotalMilliseconds;
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

            object obj = chartItem.GetYValue();
            DateTime? value = AxisHelper.ConvertToDateTime(obj);
            if (value == null)
            {
                return double.NaN;
            }

            double result = base._axisCanvas.Height * (value.Value - this._minValue.Value).TotalMilliseconds / this._axisData.Area.TotalMilliseconds;
            if (base.Orientation == AxisOrientation.BottomToTop)
            {
                result = base._axisCanvas.Height - result;
            }

            return result;
        }
    }


    internal class DateTimeAxisData
    {
        public DateTime MinValue { get; private set; }
        public DateTime MaxValue { get; private set; }

        public TimeSpan Area { get; private set; }

        public DateTimeAxisData(DateTime minValue, DateTime maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Area = maxValue - minValue;
        }
    }
}
