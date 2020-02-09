using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UtilZ.DotnetStd.Ex.Model;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class SeriesAbs : BaseModelAbs, ISeries
    {
        private AxisAbs _axisX = null;
        public virtual AxisAbs AxisX
        {
            get { return _axisX; }
            set
            {
                _axisX = value;
                base.OnRaisePropertyChanged(nameof(AxisX));
            }
        }

        private AxisAbs _axisY = null;
        public virtual AxisAbs AxisY
        {
            get { return _axisY; }
            set
            {
                _axisY = value;
                base.OnRaisePropertyChanged(nameof(AxisY));
            }
        }

        private Func<PointInfo, FrameworkElement> _createPointFunc = null;
        public virtual Func<PointInfo, FrameworkElement> CreatePointFunc
        {
            get { return _createPointFunc; }
            set
            {
                _createPointFunc = value;
                base.OnRaisePropertyChanged(nameof(CreatePointFunc));
            }
        }




        public event NotifyCollectionChangedEventHandler ValuesCollectionChanged;
        private void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var handler = this.ValuesCollectionChanged;
            handler?.Invoke(sender, e);
        }

        protected ChartCollection<IChartValue> _values = null;
        public ChartCollection<IChartValue> Values
        {
            get { return _values; }
            set
            {
                if (this._values != null)
                {
                    this._values.CollectionChanged -= Values_CollectionChanged;
                }

                _values = value;
                if (this._values != null)
                {
                    this._values.CollectionChanged += Values_CollectionChanged;
                }

                base.OnRaisePropertyChanged(nameof(Values));
            }
        }





        private Style _style = null;
        public virtual Style Style
        {
            get { return _style; }
            set
            {
                if (_style == value)
                {
                    return;
                }

                _style = value;
                this.StyleChanged(_style);
            }
        }

        protected abstract void StyleChanged(Style style);

        private bool _enableTooltip = false;
        /// <summary>
        /// true:启用Tooltip;false:禁用Tooltip
        /// </summary>
        public bool EnableTooltip
        {
            get { return _enableTooltip; }
            set
            {
                if (_enableTooltip == value)
                {
                    return;
                }

                _enableTooltip = value;
                this.EnableTooltipChanged(_enableTooltip);
            }
        }

        protected virtual void EnableTooltipChanged(bool enableTooltip)
        {

        }

        private double _tooltipArea = AxisConstant.TOOLTIP_PRE;
        /// <summary>
        /// 鼠标点周围范围内有点则触发Tooltip,小于0使用默认值
        /// </summary>
        public virtual double TooltipArea
        {
            get { return _tooltipArea; }
            set
            {
                if (value < AxisConstant.ZERO_D)
                {
                    value = AxisConstant.TOOLTIP_PRE;
                }
                _tooltipArea = value;
            }
        }

        private string _title = null;
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                base.OnRaisePropertyChanged(nameof(Title));
            }
        }


        private readonly List<SeriesLegendItem> _currentSeriesLegendItemList = new List<SeriesLegendItem>();
        private List<SeriesLegendItem> _chartSeriesLegendItemList = null;
        public SeriesAbs()
        {

        }







        protected Canvas _canvas;
        public void Add(Canvas canvas)
        {
            this._canvas = canvas;
            this._currentSeriesLegendItemList.Clear();
            this.PrimitiveAdd(canvas);
        }
        protected abstract void PrimitiveAdd(Canvas canvas);





        public bool Remove()
        {
            if (this.PrimitiveRemove(this._canvas))
            {
                return true;
            }

            if (this._chartSeriesLegendItemList != null)
            {
                foreach (var legendItem in this._currentSeriesLegendItemList)
                {
                    this._currentSeriesLegendItemList.Remove(legendItem);
                }
                this._currentSeriesLegendItemList.Clear();
            }
            this._chartSeriesLegendItemList = null;

            return false;
        }
        protected abstract bool PrimitiveRemove(Canvas canvas);





        public void Update()
        {
            this.PrimitiveRemove(this._canvas);
            this.PrimitiveAdd(this._canvas);
        }




        public void FillLegendItemToList(List<SeriesLegendItem> legendBrushList)
        {
            legendBrushList.AddRange(this._currentSeriesLegendItemList);
        }

        protected void AddOrReplaceLegendItem(SeriesLegendItem legendItem)
        {
            this.RemoveLegendItem();
            this._currentSeriesLegendItemList.Add(legendItem);
        }

        protected void AddLegendItem(SeriesLegendItem legendItem)
        {
            this._currentSeriesLegendItemList.Add(legendItem);
        }


        protected void RemoveLegendItem()
        {
            this._currentSeriesLegendItemList.Clear();
        }






        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility == value)
                {
                    return;
                }

                Visibility oldVisibility = _visibility;
                _visibility = value;
                this.VisibilityChanged(oldVisibility, _visibility);
            }
        }

        protected abstract void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility);


        public object Tag { get; set; }
    }
}
