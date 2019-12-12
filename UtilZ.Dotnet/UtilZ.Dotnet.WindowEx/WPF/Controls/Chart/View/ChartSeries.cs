using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public class ChartSeries : BaseModel
    {
        private IEnumerable<ISeries> _series = null;
        public IEnumerable<ISeries> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                base.OnRaisePropertyChanged(nameof(Series));
            }
        }


        private AxesCollection _axes = null;
        public AxesCollection Axes
        {
            get { return _axes; }
            set
            {
                _axes = value;
                base.OnRaisePropertyChanged(nameof(Axes));
            }
        }

        private IChartLegend _chartLegend = null;
        public IChartLegend ChartLegend
        {
            get { return _chartLegend; }
            set
            {
                _chartLegend = value;
                base.OnRaisePropertyChanged(nameof(ChartLegend));
            }
        }

        private Style _chartLegendStyle = null;
        public Style ChartLegendStyle
        {
            get { return _chartLegendStyle; }
            set
            {
                _chartLegendStyle = value;
                base.OnRaisePropertyChanged(nameof(ChartLegendStyle));
            }
        }


        private bool _enableCoordinateLine = true;
        public bool EnableCoordinateLine
        {
            get { return _enableCoordinateLine; }
            set
            {
                _enableCoordinateLine = value;
                base.OnRaisePropertyChanged(nameof(EnableCoordinateLine));
            }
        }


        private Style _coordinateAxisStyle = null;
        public Style CoordinateAxisStyle
        {
            get { return _coordinateAxisStyle; }
            set
            {
                _coordinateAxisStyle = value;
                base.OnRaisePropertyChanged(nameof(CoordinateAxisStyle));
            }
        }


        public ChartSeries()
            : base()
        {

        }


        public void Validate()
        {
            if (this._axes == null)
            {
                throw new ArgumentNullException("未指定坐标");
            }

            this._axes.Validate();

            foreach (IEnumerable<AxisAbs> axisCollection in this._axes)
            {
                foreach (AxisAbs axis in axisCollection)
                {
                    //初始化坐标取值范围
                    axis.InitBySeries(this._series);
                }
            }
        }
    }
}
