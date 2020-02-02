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


        private double _labelSize = 10d;
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









        internal double GetXAxisHeight()
        {
            this._axisCanvas.Height = this.PrimitiveGetXAxisHeight();
            return this._axisCanvas.Height;
        }
        protected abstract double PrimitiveGetXAxisHeight();

        protected double CalculateAxisSize(double labelTextSize)
        {
            double axisSize = labelTextSize;
            if (AxisHelper.DoubleHasValue(this._labelSize) && this._labelSize > AxisConstant.ZERO_D)
            {
                axisSize = labelTextSize + this._labelSize + AxisConstant.LABEL_TEXT_INTERVAL;
            }

            return axisSize;
        }


        internal void DrawX(ChartCollection<ISeries> seriesCollection, double axisWidth)
        {
            this._axisCanvas.Children.Clear();
            this._axisCanvas.Width = axisWidth;
            this.AddAxisXTitle(this._axisCanvas);
            this.PrimitiveDrawX(this._axisCanvas, seriesCollection, axisWidth);
        }
        protected abstract void PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisWidth);


        internal double DrawY(ChartCollection<ISeries> seriesCollection, double axisHeight)
        {
            this._axisCanvas.Children.Clear();
            this._axisCanvas.Height = axisHeight;
            this.AddAxisYTitle(this._axisCanvas);
            return this.PrimitiveDrawY(this._axisCanvas, seriesCollection, axisHeight);
        }

        /// <summary>
        /// 子类重写此函数时,必须设置Y轴宽度以及返回Y轴宽度
        /// </summary>
        /// <param name="axisCanvas"></param>
        /// <param name="seriesCollection"></param>
        /// <param name="axisHeight"></param>
        /// <returns>Y轴宽度</returns>
        protected abstract double PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisHeight);








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

        private void AddAxisXTitle(Canvas axisCanvas)
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

        private void AddAxisYTitle(Canvas axisCanvas)
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





        public bool IsAxisYLeft()
        {
            return this._dockOrientation == ChartDockOrientation.Left;
        }

        public bool IsAxisXBottom()
        {
            return this._dockOrientation == ChartDockOrientation.Bottom;
        }


        public void Validate()
        {
            this.PrimitiveValidate();
        }
        protected virtual void PrimitiveValidate()
        {
            switch (this._axisType)
            {
                case AxisType.X:
                    if (this._dockOrientation != ChartDockOrientation.Top &&
                        this._dockOrientation != ChartDockOrientation.Bottom)
                    {
                        throw new ArgumentException($"X坐标轴停靠只能为{nameof(ChartDockOrientation.Top)}或{nameof(ChartDockOrientation.Bottom)},值{this._dockOrientation.ToString()}无效");
                    }

                    if (this._orientation != AxisOrientation.LeftToRight &&
                        this._orientation != AxisOrientation.RightToLeft)
                    {
                        throw new ArgumentException($"X坐标轴坐标方向只能为{nameof(AxisOrientation.LeftToRight)}或{nameof(AxisOrientation.RightToLeft)},值{this._orientation.ToString()}无效");
                    }
                    break;
                case AxisType.Y:
                    if (this._dockOrientation != ChartDockOrientation.Left &&
                        this._dockOrientation != ChartDockOrientation.Right)
                    {
                        throw new ArgumentException($"X坐标轴停靠只能为{nameof(ChartDockOrientation.Left)}或{nameof(ChartDockOrientation.Right)},值{this._dockOrientation.ToString()}无效");
                    }

                    if (this._orientation != AxisOrientation.TopToBottom &&
                        this._orientation != AxisOrientation.BottomToTop)
                    {
                        throw new ArgumentException($"X坐标轴坐标方向只能为{nameof(AxisOrientation.TopToBottom)}或{nameof(AxisOrientation.BottomToTop)},值{this._orientation.ToString()}无效");
                    }
                    break;
                default:
                    throw new NotImplementedException(this._axisType.ToString());
            }
        }
    }
}
