using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public abstract class AxisAbs : BaseModel
    {
        /* 依赖属性
        public static readonly DependencyProperty AxisTypeProperty =
            DependencyProperty.Register(nameof(AxisType), typeof(AxisType), typeof(AxisAbsControl),
                new FrameworkPropertyMetadata(AxisType.X, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty SeriesOrientationProperty =
            DependencyProperty.Register(nameof(SeriesOrientation), typeof(SeriesOrientation), typeof(AxisAbsControl),
                new FrameworkPropertyMetadata(SeriesOrientation.LeftToRight, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty EnableCoordinateLineProperty =
           DependencyProperty.Register(nameof(EnableCoordinateLine), typeof(bool), typeof(AxisAbsControl),
               new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty TitleProperty =
          DependencyProperty.Register(nameof(Title), typeof(string), typeof(AxisAbsControl),
              new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty TitleStyleProperty =
          DependencyProperty.Register(nameof(TitleStyle), typeof(Style), typeof(AxisAbsControl),
              new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty LabelStyleProperty =
          DependencyProperty.Register(nameof(LabelStyle), typeof(Style), typeof(AxisAbsControl),
              new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty AxisDockOrientationProperty =
          DependencyProperty.Register(nameof(AxisDockOrientation), typeof(AxisDockOrientation), typeof(AxisAbsControl),
              new FrameworkPropertyMetadata(AxisDockOrientation.Left, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty SeparatorEnableProperty =
         DependencyProperty.Register(nameof(SeparatorEnable), typeof(bool), typeof(AxisAbsControl),
             new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty SeparatorStyleProperty =
          DependencyProperty.Register(nameof(SeparatorStyle), typeof(Style), typeof(AxisAbsControl),
              new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 坐标轴类型
        /// </summary>
        public AxisType AxisType
        {
            get { return (AxisType)base.GetValue(AxisTypeProperty); }
            set { base.SetValue(AxisTypeProperty, value); }
        }


        public SeriesOrientation SeriesOrientation
        {
            get { return (SeriesOrientation)base.GetValue(SeriesOrientationProperty); }
            set { base.SetValue(SeriesOrientationProperty, value); }
        }

        public bool EnableCoordinateLine
        {
            get { return (bool)base.GetValue(EnableCoordinateLineProperty); }
            set { base.SetValue(EnableCoordinateLineProperty, value); }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 标题样式
        /// </summary>
        public Style TitleStyle
        {
            get { return (Style)base.GetValue(TitleStyleProperty); }
            set { base.SetValue(TitleStyleProperty, value); }
        }


        /// <summary>
        /// 刻度样式
        /// </summary>
        public Style LabelStyle
        {
            get { return (Style)base.GetValue(LabelStyleProperty); }
            set { base.SetValue(LabelStyleProperty, value); }
        }


        /// <summary>
        /// 坐标轴方向
        /// </summary>
        public AxisDockOrientation AxisDockOrientation
        {
            get { return (AxisDockOrientation)base.GetValue(AxisDockOrientationProperty); }
            set { base.SetValue(AxisDockOrientationProperty, value); }
        }


        public bool SeparatorEnable
        {
            get { return (bool)base.GetValue(SeparatorEnableProperty); }
            set { base.SetValue(SeparatorEnableProperty, value); }
        }

        /// <summary>
        /// 刻度分隔线Brush
        /// </summary>
        public Style SeparatorStyle
        {
            get { return (Style)base.GetValue(SeparatorStyleProperty); }
            set { base.SetValue(SeparatorStyleProperty, value); }
        }
       


        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selfControl = (AxisAbsControl)d;
            if (e.Property == AxisTypeProperty)
            {
                //selfControl.SetSmog((double)e.NewValue);
            }
            else if (e.Property == SeriesOrientationProperty)
            {

            }
            else if (e.Property == EnableCoordinateLineProperty)
            {

            }
            else if (e.Property == TitleProperty)
            {

            }
            else if (e.Property == TitleStyleProperty)
            {

            }
            else if (e.Property == LabelStyleProperty)
            {

            }
            else if (e.Property == AxisDockOrientationProperty)
            {

            }
            else if (e.Property == SeparatorEnableProperty)
            {

            }
            else if (e.Property == SeparatorStyleProperty)
            {

            }
        }
        */


        private Style _style = null;
        /// <summary>
        /// 坐标轴控件样式
        /// </summary>
        public Style Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                base.OnRaisePropertyChanged(nameof(Style));
            }
        }

        private Style _labelStyle = null;
        /// <summary>
        /// 刻度样式
        /// </summary>
        public Style LabelStyle
        {
            get
            {
                return _labelStyle;
            }
            set
            {
                _labelStyle = value;
                base.OnRaisePropertyChanged(nameof(LabelStyle));
            }
        }

        private Style _titleStyle = null;
        /// <summary>
        /// 标题样式
        /// </summary>
        public Style TitleStyle
        {
            get
            {
                return _titleStyle;
            }
            set
            {
                _titleStyle = value;
                base.OnRaisePropertyChanged(nameof(TitleStyle));
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


        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                base.OnRaisePropertyChanged(nameof(Visibility));
            }
        }

        private double _size = double.NaN;
        /// <summary>
        /// X轴高度或Y宽度
        /// </summary>
        public double Size
        {
            get { return _size; }
            set
            {
                _size = value;
                base.OnRaisePropertyChanged(nameof(Size));
            }
        }

        private AxisType _axisType;
        /// <summary>
        /// 坐标轴类型
        /// </summary>
        public AxisType AxisType
        {
            get { return _axisType; }
            set
            {
                _axisType = value;
                base.OnRaisePropertyChanged(nameof(AxisType));
            }
        }


        private SeriesOrientation _seriesOrientation;
        public SeriesOrientation SeriesOrientation
        {
            get { return _seriesOrientation; }
            set
            {
                _seriesOrientation = value;
                base.OnRaisePropertyChanged(nameof(SeriesOrientation));
            }
        }

       

        private AxisDockOrientation _axisDockOrientation;
        /// <summary>
        /// 坐标轴方向
        /// </summary>
        public AxisDockOrientation AxisDockOrientation
        {
            get { return _axisDockOrientation; }
            set
            {
                _axisDockOrientation = value;
                base.OnRaisePropertyChanged(nameof(AxisDockOrientation));
            }
        }

        //private bool _separatorEnable = false;
        //public bool SeparatorEnable
        //{
        //    get { return _separatorEnable; }
        //    set
        //    {
        //        if (_separatorEnable == value)
        //        {
        //            return;
        //        }

        //        _separatorEnable = value;
        //        base.OnRaisePropertyChanged(nameof(SeparatorEnable));
        //    }
        //}

        //private Style _separatorStyle = null;
        ///// <summary>
        ///// 刻度分隔线Brush
        ///// </summary>
        //public Style SeparatorStyle
        //{
        //    get { return _separatorStyle; }
        //    set
        //    {
        //        if (_separatorStyle == value)
        //        {
        //            return;
        //        }

        //        _separatorStyle = value;
        //        base.OnRaisePropertyChanged(nameof(SeparatorStyle));
        //    }
        //}

        





        protected readonly Canvas _axisCanvas = new Canvas();

        public AxisAbs()
        {

        }



        internal void InitBySeries(IEnumerable<ISeries> series)
        {
            this.PrimitiveInitBySeries(series);
        }
        protected virtual void PrimitiveInitBySeries(IEnumerable<ISeries> series)
        {

        }




        public double GetAxisXHeight()
        {
            return this.PrimitiveGetAxisXHeight();
        }

        protected abstract double PrimitiveGetAxisXHeight();





        public double GetAxisYWidth(double axisYHeight)
        {
            return this.PrimitiveGetAxisYWidth(axisYHeight);
        }
        protected abstract double PrimitiveGetAxisYWidth(double axisYHeight);



        public FrameworkElement GetAxisControl()
        {
            return this._axisCanvas;
        }



        public void UpdateAxis(Rect axisArea)
        {
            this.PrimitiveUpdateAxis(axisArea);
        }

        protected abstract void PrimitiveUpdateAxis(Rect axisArea);
    }
}
