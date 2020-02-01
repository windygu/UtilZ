using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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


        protected override void PrimitiveDraw(ChartCollection<ISeries> seriesCollection, Canvas axisCanvas)
        {
            if (!AxisHelper.DoubleHasValue(this._minValue) || !AxisHelper.DoubleHasValue(this._maxValue))
            {
                this.UpdateMinAndMax(seriesCollection);
            }

            if (!AxisHelper.DoubleHasValue(this._minValue) || !AxisHelper.DoubleHasValue(this._maxValue))
            {
                return;
            }

            List<Point> axisPointList;
            switch (this.AxisType)
            {
                case AxisType.X:
                    base.AddAxisXTitle(axisCanvas);
                    axisPointList = this.DrawXAxis(axisCanvas, this._minValue, this._maxValue);
                    AxisHelper.DrawXAxisLabelLine(this, axisCanvas, axisPointList);
                    break;
                case AxisType.Y:
                    base.AddAxisYTitle(axisCanvas);
                    axisPointList = this.DrawYAxis(axisCanvas, this._minValue, this._maxValue);
                    AxisHelper.DrawYAxisLabelLine(this, axisCanvas, axisPointList);
                    break;
                default:
                    throw new NotImplementedException(this.AxisType.ToString());
            }
        }

        private List<Point> DrawYAxis(Canvas axisCanvas, double minValue, double maxValue)
        {
            List<Point> axisPointList = new List<Point>();
            double area = maxValue - minValue;
            double axisSize = axisCanvas.Height;
            double labelStep = this.CalculateLabelStep(area, axisSize);
            int separatorCount = AxisHelper.CalSeparatorCount(area, axisSize, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(area, axisSize, labelStep);

            double bottom = AxisConstant.ZERO_D;
            double tmp = minValue;
            double endValue = maxValue + labelStep - base._PRE;
            bool flag = false;
            double slidSeparatorSize = separatorSize;
            double y = axisSize;

            while (true)
            {
                var label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(tmp));
                axisCanvas.Children.Add(label);

                if (base.IsAxisYLeft())
                {
                    Canvas.SetRight(label, AxisConstant.ZERO_D);
                    axisPointList.Add(new Point(AxisConstant.ZERO_D, y));
                }
                else
                {
                    Canvas.SetLeft(label, AxisConstant.ZERO_D);
                    axisPointList.Add(new Point(axisCanvas.Width, y));
                }

                Canvas.SetBottom(label, bottom);

                if (flag)
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
                    flag = true;
                }

                bottom += slidSeparatorSize;
                y -= slidSeparatorSize;
            }

            return axisPointList;
        }

        private List<Point> DrawXAxis(Canvas axisCanvas, double minValue, double maxValue)
        {
            List<Point> axisPointList = new List<Point>();
            double area = maxValue - minValue;
            double axisSize = axisCanvas.Width;
            double labelStep = this.CalculateLabelStep(area, axisSize);
            int separatorCount = AxisHelper.CalSeparatorCount(area, axisSize, labelStep);
            double separatorSize = AxisHelper.CalSeparatorSize(area, axisSize, labelStep);

            double left = AxisConstant.ZERO_D;
            double tmp = minValue;
            double endValue = maxValue + labelStep - base._PRE;
            bool flag = false;

            while (true)
            {
                var label = AxisHelper.CreateLabelControl(this, this.CreateAxisText(tmp));
                axisCanvas.Children.Add(label);


                Canvas.SetLeft(label, left);
                if (base.IsAxisXBottom())
                {
                    Canvas.SetBottom(label, AxisConstant.ZERO_D);
                    axisPointList.Add(new Point(left, AxisConstant.ZERO_D));
                }
                else
                {
                    Canvas.SetTop(label, AxisConstant.ZERO_D);
                    axisPointList.Add(new Point(left, axisCanvas.Height));
                }

                if (flag)
                {
                    break;
                }

                tmp += labelStep;
                if (tmp >= maxValue)
                //if (tmp - maxValue > _PRE)
                {
                    left = left + (labelStep - (tmp - maxValue)) * separatorSize / labelStep;
                    tmp = maxValue;
                    flag = true;
                }
                else
                {
                    left += separatorSize;
                }
            }

            return axisPointList;
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




        private void UpdateMinAndMax(ChartCollection<ISeries> seriesCollection)
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
