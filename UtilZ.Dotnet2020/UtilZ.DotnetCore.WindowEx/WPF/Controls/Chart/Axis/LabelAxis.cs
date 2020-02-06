using System;
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
                    this.RotateLabelBottom(label);
                }
                else
                {
                    Canvas.SetBottom(label, AxisConstant.ZERO_D);
                    this.RotateLabelTop(label);
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
        private void RotateLabelTop(TextBlock label)
        {
            //todo..未测试,可能需要修改
            Size size = UITextHelper.MeasureTextSize(label);
            var transformGroup = new TransformGroup();

            const double ANGLE = -50d;

            var rotateTransform = new RotateTransform();
            rotateTransform.CenterX = size.Width;
            rotateTransform.CenterY = 0d;
            rotateTransform.Angle = ANGLE;
            transformGroup.Children.Add(rotateTransform);

            var translateTransform = new TranslateTransform();
            translateTransform.X = 0d - size.Width - size.Height / 2;
            translateTransform.Y = 0d;
            transformGroup.Children.Add(translateTransform);

            label.RenderTransform = transformGroup;
        }
        private void RotateLabelBottom(TextBlock label)
        {
            Size size = UITextHelper.MeasureTextSize(label);
            var transformGroup = new TransformGroup();

            const double ANGLE = -50d;

            var rotateTransform = new RotateTransform();
            rotateTransform.CenterX = size.Width;
            rotateTransform.CenterY = 0d;
            rotateTransform.Angle = ANGLE;
            transformGroup.Children.Add(rotateTransform);

            var translateTransform = new TranslateTransform();
            translateTransform.X = 0d - size.Width - size.Height / 2;
            translateTransform.Y = 0d;
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
}
