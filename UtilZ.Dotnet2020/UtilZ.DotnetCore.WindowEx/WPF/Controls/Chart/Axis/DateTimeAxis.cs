using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
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





        public Func<DateTime, string> CustomAxisTextFormatCunc;
        private DateTimeAxisData _axisData = null;



        public DateTimeAxis()
            : base()
        {

        }


        private Rect _labelTextSize;

        protected override double PrimitiveGetXAxisHeight()
        {
            string labelText = this.CreateAxisText(DateTime.Now);
            this._labelTextSize = AxisHelper.MeasureScaleTextSize(this, labelText);
            return base.CalculateAxisSize(this._labelTextSize.Height);
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

        //private double CalculateLabelStep()
        //{
        //    double stepMilliseconds = this._labelStep != null ? this._labelStep.Value.TotalMilliseconds : double.NaN;
        //    if (double.IsNaN(stepMilliseconds))
        //    {
        //        double labelMinInterval = base.GetLabelMinInterval(this._labelTextSize.Width);
        //        int count = (int)(this._axisCanvas.Width / labelMinInterval);
        //        if (count == 0)
        //        {
        //            this.UpdateAxisXLeftToRight(this._beginDateTime.Value, this._endDateTime.Value, this._area.TotalMilliseconds, width, labelSize.Width);
        //            return;
        //        }

        //        TimeSpan intervalTimeLength = TimeSpan.FromMilliseconds(this._axisData.Area.TotalMilliseconds / count);//一个刻度内时长
        //        stepMilliseconds = this.AdjustIntervalTimeLength(intervalTimeLength);
        //    }
        //}

        //private int CalculateAxisLabelCount(double labelTextSize, double axisSize)
        //{
        //    var labelAndSpaceWidth = this._labelMinInterval;
        //    if (double.IsNaN(labelAndSpaceWidth))
        //    {
        //        labelAndSpaceWidth = AxisConstant.LABEL_TEXT_INTERVAL + labelTextSize;
        //    }

        //    return (int)Math.Floor(axisSize / labelAndSpaceWidth);
        //}




        protected override double PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisHeight)
        {
            this._axisData = null;
            throw new NotImplementedException();
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

        private List<double> DrawXAxisRightToLeft(Canvas axisCanvas, DateTimeAxisData axisData)
        {
            throw new NotImplementedException();
        }

        private List<double> DrawXAxisLeftToRight(Canvas axisCanvas, DateTimeAxisData axisData)
        {
            throw new NotImplementedException();
        }



        protected override double PrimitiveGetX(IChartItem chartItem)
        {
            var chartDateTimeItem = chartItem as IChartDateTimeItem;
            if (this._axisData == null || chartDateTimeItem == null)
            {
                return double.NaN;
            }

            //默认AxisOrientation.LeftToRight
            double result = base._axisCanvas.Width * (chartDateTimeItem.Time - this._minValue.Value).TotalMilliseconds / this._axisData.Area.TotalMilliseconds;
            if (base.Orientation == AxisOrientation.RightToLeft)
            {
                result = base._axisCanvas.Width - result;
            }

            return result;
        }

        protected override double PrimitiveGetY(IChartItem chartItem)
        {
            var chartDateTimeItem = chartItem as IChartDateTimeItem;
            if (this._axisData == null || chartDateTimeItem == null)
            {
                return double.NaN;
            }

            double result = base._axisCanvas.Height * (chartDateTimeItem.Time - this._minValue.Value).TotalMilliseconds / this._axisData.Area.TotalMilliseconds;
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
