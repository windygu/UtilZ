using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UtilZ.DotnetStd.Ex.Model;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart.Series
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
        public Func<PointInfo, FrameworkElement> CreatePointFunc
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

        protected ChartCollection<IChartItem> _values = null;
        public ChartCollection<IChartItem> Values
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



        public void GetAxisValueArea(AxisAbs axis, out double min, out double max)
        {
            this.PrimitiveGetAxisValueArea(axis, out min, out max);
        }
        protected abstract void PrimitiveGetAxisValueArea(AxisAbs axis, out double min, out double max);


        private Style _style = null;
        public Style Style
        {
            get { return _style; }
            set
            {
                _style = value;
                base.OnRaisePropertyChanged(nameof(Style));
            }
        }

        private string _title = null;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                base.OnRaisePropertyChanged(nameof(Title));
            }
        }


        protected Canvas _canvas;
        protected Rect _chartArea;
        public void Draw(Canvas canvas, Rect chartArea)
        {
            this._canvas = canvas;
            this._chartArea = chartArea;
            this.PrimitiveDraw(canvas, chartArea);
        }
        protected abstract void PrimitiveDraw(Canvas canvas, Rect chartArea);

        public void Clear()
        {
            this.PrimitiveClear();
        }
        protected abstract void PrimitiveClear();


        public void Update()
        {
            this.PrimitiveUpdate();
        }

        protected virtual void PrimitiveUpdate()
        {
            this.PrimitiveClear();
            this.PrimitiveDraw(this._canvas, this._chartArea);
        }



        public void FillLegendItemToList(List<SeriesLegendItem> legendBrushList)
        {
            this.PrimitiveFillLegendItemToList(legendBrushList);
        }

        protected abstract void PrimitiveFillLegendItemToList(List<SeriesLegendItem> legendBrushList);


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

        protected virtual void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {

        }


        public object Tag { get; set; }
    }
}
