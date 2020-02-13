﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilZ.DotnetStd.Ex.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// 坐标基类
    /// </summary>
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


        private AxisLabelOrientation _orientation;
        /// <summary>
        /// 坐标方向
        /// </summary>
        public AxisLabelOrientation Orientation
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

        private double _labelMinInterval = double.NaN;
        /// <summary>
        /// 两个刻度值间最小间隔, 为double.NaN使用默认值,单位:像素
        /// </summary>
        public double LabelMinInterval
        {
            get { return _labelMinInterval; }
            set
            {
                _labelMinInterval = value;
                base.OnRaisePropertyChanged(nameof(LabelMinInterval));
            }
        }




        private bool _drawAxisAxisLine = true;
        /// <summary>
        /// true:绘制坐标线;false:不绘制坐标线
        /// </summary>
        public bool DrawAxisLine
        {
            get { return _drawAxisAxisLine; }
            set
            {
                _drawAxisAxisLine = value;
                base.OnRaisePropertyChanged(nameof(DrawAxisLine));
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


        //private double _minSize = double.NaN;
        ///// <summary>
        ///// 获取或设置最小大小,单位像素(X轴为最小高度,Y轴最小宽度),为double.NaN或小于等于0时此值无效
        ///// </summary>
        //public double MinSize
        //{
        //    get { return _minSize; }
        //    set
        //    {
        //        _minSize = value;
        //        base.OnRaisePropertyChanged(nameof(MinSize));
        //    }
        //}



        private bool _enableBackgroundLabelLine = false;
        /// <summary>
        /// 是否启用背景刻度线
        /// </summary>
        public bool EnableBackgroundLabelLine
        {
            get { return _enableBackgroundLabelLine; }
            set
            {
                _enableBackgroundLabelLine = value;
                base.OnRaisePropertyChanged(nameof(EnableBackgroundLabelLine));
            }
        }

        private Style _backgroundLabelLineStyle = null;
        /// <summary>
        /// 背景刻度线样式
        /// </summary>
        public Style BackgroundLabelLineStyle
        {
            get { return _backgroundLabelLineStyle; }
            set
            {
                _backgroundLabelLineStyle = value;
                base.OnRaisePropertyChanged(nameof(BackgroundLabelLineStyle));
            }
        }

        /// <summary>
        /// 创建图表中坐标背景线回调
        /// </summary>
        public Func<List<BackgroundLabelLineSegment>, Path> CreateBackgroundLabelLineFunc;

        internal Path CreateBackgroundLabelLine(List<BackgroundLabelLineSegment> labelLineSegments)
        {
            if (!this._enableBackgroundLabelLine ||
                labelLineSegments == null ||
                labelLineSegments.Count == 0)
            {
                return null;
            }

            var handler = this.CreateBackgroundLabelLineFunc;
            if (handler != null)
            {
                return handler(labelLineSegments);
            }

            Path backgroundLabelLine = new Path();
            if (this._backgroundLabelLineStyle == null)
            {
                backgroundLabelLine.Style = ChartStyleHelper.GetDefaultBackgroundLabelLineStyle();
            }
            else
            {
                backgroundLabelLine.Style = this._backgroundLabelLineStyle;
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            foreach (var labelLineSegment in labelLineSegments)
            {
                PathFigure segmentFigure = new PathFigure();
                segmentFigure.StartPoint = labelLineSegment.Point1;
                segmentFigure.Segments.Add(new LineSegment(labelLineSegment.Point2, true));
                geometryGroup.Children.Add(new PathGeometry()
                {
                    Figures = new PathFigureCollection(new PathFigure[] { segmentFigure })
                });
            }

            backgroundLabelLine.Data = geometryGroup;
            return backgroundLabelLine;
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


        /// <summary>
        /// 浮点数比较精度
        /// </summary>
        protected double _PRE = 0.00000001d;
        /// <summary>
        /// 浮点数比较精度
        /// </summary>
        public double PRE
        {
            get { return _PRE; }
            set
            {
                if (!AxisHelper.DoubleHasValue(value))
                {
                    throw new ArgumentException("精度值无效");
                }

                _PRE = value;
            }
        }



        /// <summary>
        /// 获取或设置坐标样式
        /// </summary>
        public Brush Background
        {
            get
            {
                return this._axisCanvas.Background;
            }
            set
            {
                this._axisCanvas.Background = value;
            }
        }


        /// <summary>
        /// 获取坐标轴宽度
        /// </summary>
        internal double Width
        {
            get { return this._axisCanvas.Width; }
        }

        /// <summary>
        /// 获取坐标轴高度
        /// </summary>
        internal double Height
        {
            get { return this._axisCanvas.Height; }
        }





        /// <summary>
        /// 绘制坐标的画布
        /// </summary>
        protected readonly Canvas _axisCanvas = new Canvas();
        /// <summary>
        /// 构造函数
        /// </summary>
        public AxisAbs()
            : base()
        {
            this._axisCanvas.Background = Brushes.Transparent;
            //test
            //this._axisCanvas.Background = ColorBrushHelper.GetNextColor();
        }



        internal void UpdateTitle()
        {
            if (this._titleControl != null)
            {
                this._titleControl.Text = this._title;
            }
        }

        internal void UpdateTitleStyle()
        {
            if (this._titleControl != null)
            {
                this._titleControl.Style = this._titleStyle;
            }
        }





        /// <summary>
        /// 计算X轴高度和Y轴宽度
        /// </summary>
        /// <param name="labelTextSize">Label文本X轴高度,Y轴宽度</param>
        /// <returns></returns>
        protected double CalculateAxisSize(double labelTextSize)
        {
            double axisSize = labelTextSize + ChartConstant.LABEL_TEXT_INTERVAL * 3;
            if (AxisHelper.DoubleHasValue(this._labelSize) && this._labelSize > ChartConstant.ZERO_D)
            {
                axisSize = axisSize + this._labelSize;
            }

            return axisSize;
        }


        /// <summary>
        /// 获取Y轴Label和文本之间的间隔
        /// </summary>
        /// <returns>Y轴Label和文本之间的间隔</returns>
        protected double GetAxisYLabelTextLineInterval()
        {
            double interval = ChartConstant.LABEL_TEXT_INTERVAL;
            if (AxisHelper.DoubleHasValue(this._labelSize) && this._labelSize > ChartConstant.ZERO_D)
            {
                interval = interval + this._labelSize;
            }

            return interval;
        }



        /// <summary>
        /// 获取X坐标轴高度
        /// </summary>
        /// <returns>X坐标轴高度</returns>
        internal double GetXAxisHeight()
        {
            this._axisCanvas.Height = this.PrimitiveGetXAxisHeight();
            return this._axisCanvas.Height;
        }
        /// <summary>
        /// 获取X坐标轴高度
        /// </summary>
        /// <returns>X坐标轴高度</returns>
        protected abstract double PrimitiveGetXAxisHeight();


        /// <summary>
        /// 绘制X轴
        /// </summary>
        /// <param name="seriesCollection">Series集合</param>
        /// <param name="axisWidth">X轴宽度</param>
        /// <returns>Label的X列表</returns>
        internal List<double> DrawX(ChartCollection<ISeries> seriesCollection, double axisWidth)
        {
            this._axisCanvas.Children.Clear();
            this._axisCanvas.Width = axisWidth;
            this.AddAxisXTitle();
            return this.PrimitiveDrawX(this._axisCanvas, seriesCollection);
        }
        /// <summary>
        /// 绘制X轴
        /// </summary>
        /// <param name="axisCanvas">画布</param>
        /// <param name="seriesCollection">Series集合</param>
        /// <returns>Label的X列表</returns>
        protected abstract List<double> PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection);




        /// <summary>
        /// 绘制Y轴
        /// </summary>
        /// <param name="seriesCollection">Series集合</param>
        /// <param name="axisHeight">Y轴高度</param>
        /// <returns>Label的Y列表</returns>

        internal List<double> DrawY(ChartCollection<ISeries> seriesCollection, double axisHeight)
        {
            this._axisCanvas.Children.Clear();
            this._axisCanvas.Height = axisHeight;
            this.AddAxisYTitle();
            return this.PrimitiveDrawY(this._axisCanvas, seriesCollection);
        }

        /// <summary>
        /// 子类重写此函数时,必须设置Y轴宽度
        /// </summary>
        /// <param name="axisCanvas">画布</param>
        /// <param name="seriesCollection">Series集合</param>
        /// <returns>Label的Y列表</returns>
        protected abstract List<double> PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection);







        /// <summary>
        /// 获取坐标轴控件
        /// </summary>
        public FrameworkElement AxisControl
        {
            get
            {
                return this._axisCanvas;
            }
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

        private void AddAxisXTitle()
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

        private void AddAxisYTitle()
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




        /// <summary>
        /// Y坐标轴是否左停靠
        /// </summary>
        /// <returns></returns>
        public bool IsAxisYLeft()
        {
            return this._dockOrientation == ChartDockOrientation.Left;
        }

        /// <summary>
        /// X坐标轴是否底部停靠
        /// </summary>
        /// <returns></returns>
        public bool IsAxisXBottom()
        {
            return this._dockOrientation == ChartDockOrientation.Bottom;
        }

        /// <summary>
        /// 验证坐标轴有效性,无效直接抛出ArgumentException异常
        /// </summary>
        public void Validate()
        {
            this.PrimitiveValidate();
        }

        /// <summary>
        /// 验证坐标轴有效性,无效直接抛出ArgumentException异常
        /// </summary>
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

                    if (this._orientation != AxisLabelOrientation.LeftToRight &&
                        this._orientation != AxisLabelOrientation.RightToLeft)
                    {
                        throw new ArgumentException($"X坐标轴坐标方向只能为{nameof(AxisLabelOrientation.LeftToRight)}或{nameof(AxisLabelOrientation.RightToLeft)},值{this._orientation.ToString()}无效");
                    }
                    break;
                case AxisType.Y:
                    if (this._dockOrientation != ChartDockOrientation.Left &&
                        this._dockOrientation != ChartDockOrientation.Right)
                    {
                        throw new ArgumentException($"X坐标轴停靠只能为{nameof(ChartDockOrientation.Left)}或{nameof(ChartDockOrientation.Right)},值{this._dockOrientation.ToString()}无效");
                    }

                    if (this._orientation != AxisLabelOrientation.TopToBottom &&
                        this._orientation != AxisLabelOrientation.BottomToTop)
                    {
                        throw new ArgumentException($"X坐标轴坐标方向只能为{nameof(AxisLabelOrientation.TopToBottom)}或{nameof(AxisLabelOrientation.BottomToTop)},值{this._orientation.ToString()}无效");
                    }
                    break;
                default:
                    throw new NotImplementedException(this._axisType.ToString());
            }
        }



        /// <summary>
        /// 获取指定项在X轴的坐标值
        /// </summary>
        /// <param name="item">目标项</param>
        /// <returns>指定项在X轴的坐标值</returns>
        internal double GetX(IChartItem item)
        {
            return this.PrimitiveGetX(item);
        }
        /// <summary>
        /// 获取指定项在X轴的坐标值
        /// </summary>
        /// <param name="item">目标项</param>
        /// <returns>指定项在X轴的坐标值</returns>
        protected abstract double PrimitiveGetX(IChartItem item);


        /// <summary>
        /// 获取指定项在Y轴的坐标值
        /// </summary>
        /// <param name="item">目标项</param>
        /// <returns>指定项在Y轴的坐标值</returns>
        internal double GetY(IChartItem item)
        {
            return this.PrimitiveGetY(item);
        }
        /// <summary>
        /// 获取指定项在Y轴的坐标值
        /// </summary>
        /// <param name="item">目标项</param>
        /// <returns>指定项在Y轴的坐标值</returns>
        protected abstract double PrimitiveGetY(IChartItem item);
    }
}
