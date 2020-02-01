using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilZ.DotnetStd.Ex.Model;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public abstract class AxisAbs : BaseModelAbs
    {
        private ChartDockOrientation _dockOrientation;
        /// <summary>
        /// 坐标轴停靠方向
        /// </summary>
        public ChartDockOrientation DockOrientation
        {
            get { return _dockOrientation; }
            set
            {
                _dockOrientation = value;
                base.OnRaisePropertyChanged(nameof(DockOrientation));
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


        private AxisOrientation _orientation;
        public AxisOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                base.OnRaisePropertyChanged(nameof(Orientation));
            }
        }


        private double _size = 30d;
        /// <summary>
        /// 为X轴时表高度; 为Y轴时表示宽度
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

        private Style _labelStyle = null;
        /// <summary>
        /// 刻度值样式
        /// </summary>
        public Style LabelStyle
        {
            get { return _labelStyle; }
            set
            {
                _labelStyle = value;
                base.OnRaisePropertyChanged(nameof(LabelStyle));
            }
        }


        private double _labelSize = 30d;
        /// <summary>
        /// 刻度线宽度或高度小于0使用默认值30,单位:像素
        /// </summary>
        public double LabelSize
        {
            get { return _labelSize; }
            set
            {
                _labelSize = value;
                base.OnRaisePropertyChanged(nameof(LabelSize));
            }
        }






        private bool _axisLine = true;
        /// <summary>
        /// true:绘制坐标线;false:不绘制坐标线
        /// </summary>
        public bool AxisLine
        {
            get { return _axisLine; }
            set
            {
                _axisLine = value;
                base.OnRaisePropertyChanged(nameof(AxisLine));
            }
        }

        private Style _axisLineStyle = null;
        /// <summary>
        /// 坐标线样式
        /// </summary>
        public Style AxisLineStyle
        {
            get { return _axisLineStyle; }
            set
            {
                _axisLineStyle = value;
                base.OnRaisePropertyChanged(nameof(AxisLineStyle));
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

        private Style _titleStyle = null;
        /// <summary>
        /// 标题样式
        /// </summary>
        public Style TitleStyle
        {
            get { return _titleStyle; }
            set
            {
                _titleStyle = value;
                base.OnRaisePropertyChanged(nameof(TitleStyle));
            }
        }


        protected double _PRE = 0.00000001d;
        /// <summary>
        /// 浮点数比较精度
        /// </summary>
        public double PRE
        {
            get { return _PRE; }
            set { _PRE = value; }
        }


        private readonly Canvas _axisCanvas = new Canvas();
        public AxisAbs()
            : base()
        {
            this._axisCanvas.Background = ColorBrushHelper.GetNextColor();
        }


        internal void UpdateLabelStyle()
        {
            throw new NotImplementedException();
        }

        internal void UpdateTickMark()
        {
            this.Draw(null);
        }

        internal void UpdateAxisLine()
        {
            throw new NotImplementedException();
        }

        internal void UpdateTitle()
        {
            throw new NotImplementedException();
        }

        internal void UpdateTitleStyle()
        {
            throw new NotImplementedException();
        }

        internal bool JustUpdateAxis(string propertyName)
        {
            throw new NotImplementedException();
        }



        internal void Draw(ChartCollection<ISeries> seriesCollection)
        {
            this._axisCanvas.Children.Clear();
            this.PrimitiveDraw(seriesCollection, this._axisCanvas);
        }

        protected abstract void PrimitiveDraw(ChartCollection<ISeries> seriesCollection, Canvas axisCanvas);





        public FrameworkElement GetAxisControl()
        {
            return this.PrimitiveGetAxisControl();
        }
        protected virtual FrameworkElement PrimitiveGetAxisControl()
        {
            return this._axisCanvas;
        }







        private TextBlock _titleControl = null;
        private TextBlock GetTitleControl()
        {
            if (string.IsNullOrWhiteSpace(this._title))
            {
                return null;
            }

            if (this._titleControl == null)
            {
                TextBlock titleControl = new TextBlock();

                titleControl.Style = this._titleStyle;
                if (titleControl.Style == null)
                {
                    titleControl.Style = ChartStyleHelper.CreateAxisTitleStyle(this._dockOrientation);
                }

                titleControl.Text = this._title;
                this._titleControl = titleControl;
            }

            return this._titleControl;
        }

        protected void AddAxisXTitle(Canvas axisCanvas)
        {
            //TextBlock titleControl = this._titleControl;
            //if (titleControl == null)
            //{
            //    return;
            //}

            //axisCanvas.Children.Add(titleControl);

            //var titleSize = UITextHelper.MeasureTextSize(titleControl);
            //double left = (width - titleSize.Width) / 2;
            //Canvas.SetLeft(titleControl, left);

            //double top;
            //if (this.IsAxisXBottom())
            //{
            //    top = 0d;
            //}
            //else
            //{
            //    top = axisCanvas.Height - titleSize.Height;
            //}

            //Canvas.SetTop(titleControl, top);
        }

        protected void AddAxisYTitle(Canvas axisCanvas)
        {
            //TextBlock titleControl = this.GetTitleControl();
            //if (titleControl == null)
            //{
            //    return;
            //}

            ////axisCanvas.Background = Brushes.Yellow;
            ////titleLabel.Background = Brushes.Red;

            //axisCanvas.Children.Add(titleControl);
            //var titleSize = UITextHelper.MeasureTextSize(titleControl);
            //double top = (height - titleSize.Height) / 2;
            //Canvas.SetTop(titleControl, top);

            //if (this.IsAxisYLeft())
            //{
            //    TransformGroup transformGroup = new TransformGroup();
            //    var rotateTransformLeft = new RotateTransform();
            //    rotateTransformLeft.CenterX = titleSize.Width / 2;
            //    rotateTransformLeft.CenterY = titleSize.Height / 2;
            //    rotateTransformLeft.Angle = -90d;
            //    transformGroup.Children.Add(rotateTransformLeft);

            //    var translateTransformLeft = new TranslateTransform();
            //    translateTransformLeft.X = 0 - titleSize.Height;
            //    translateTransformLeft.Y = 0d;
            //    transformGroup.Children.Add(translateTransformLeft);

            //    titleControl.RenderTransform = transformGroup;

            //    Canvas.SetLeft(titleControl, 0d);
            //}
            //else
            //{
            //    TransformGroup transformGroup = new TransformGroup();
            //    var rotateTransformRight = new RotateTransform();
            //    rotateTransformRight.CenterX = titleSize.Width / 2;
            //    rotateTransformRight.CenterY = titleSize.Height / 2;
            //    rotateTransformRight.Angle = 90d;
            //    transformGroup.Children.Add(rotateTransformRight);

            //    var translateTransformRight = new TranslateTransform();
            //    translateTransformRight.X = axisCanvas.Width;
            //    translateTransformRight.Y = 0d;
            //    transformGroup.Children.Add(translateTransformRight);

            //    titleControl.RenderTransform = transformGroup;

            //    Canvas.SetRight(titleControl, 0d);
            //}
        }





        protected bool IsAxisYLeft()
        {
            return this._dockOrientation == ChartDockOrientation.Left;
        }

        protected bool IsAxisXBottom()
        {
            return this._dockOrientation == ChartDockOrientation.Bottom;
        }


    }
}
