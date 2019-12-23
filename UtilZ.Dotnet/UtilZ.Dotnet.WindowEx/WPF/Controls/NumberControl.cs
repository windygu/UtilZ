using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls
{
    /// <summary>
    /// 数字编辑控件
    /// </summary>
    public class NumberControl : TextBox
    {
        private const string _POINT = ".";
        private const string _SUBTRACT = "-";
        private const char _ZEROR = '0';

        #region 依赖属性
        /// <summary>
        /// 值依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumberControl),
                new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 最小值依赖属性
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberControl),
                new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 最大值依赖属性
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberControl),
                new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 数字显示框（也称作 up-down 控件）中要显示的十进制位数依赖属性
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumberControl),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnPropertyChangedCallback)));

        ///// <summary>
        ///// 单击向上或向下按钮时，数字显示框（也称作 up-down 控件）递增或递减的值依赖属性
        ///// </summary>
        //public static readonly DependencyProperty IncrementProperty =
        //    DependencyProperty.Register(nameof(Increment), typeof(double), typeof(NumberControl),
        //        new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 数字显示宽度依赖属性
        /// </summary>
        public static readonly DependencyProperty NumberWidthProperty =
            DependencyProperty.Register(nameof(NumberWidth), typeof(int), typeof(NumberControl),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 数字文本不足够显示宽度时0的填充方向依赖属性
        /// </summary>
        public static readonly DependencyProperty NumberWidthFillDirectionProperty =
            DependencyProperty.Register(nameof(NumberWidthFillDirection), typeof(NumberWidthFillDirection), typeof(NumberControl),
                new FrameworkPropertyMetadata(NumberWidthFillDirection.Right, new PropertyChangedCallback(OnPropertyChangedCallback)));





        /// <summary>
        /// 值改变事件
        /// </summary>
        public event EventHandler<NumberValueChangedArgs> ValueChanged;

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public double Value
        {
            get
            {
                return (double)base.GetValue(ValueProperty);
            }
            set
            {
                if (!double.IsNaN(value))
                {
                    if (value - this.Minimum < this._precision || value - this.Maximum > this._precision)
                    {
                        throw new ArgumentOutOfRangeException($"值超出最大最小值范围{this.Minimum}-{this.Maximum}");
                    }

                    string valueStr = value.ToString();
                    int pointIndex = valueStr.IndexOf(_POINT);
                    int decimalPlaces = this.DecimalPlaces;
                    if (pointIndex > 0 && valueStr.Length - (pointIndex + 1) > decimalPlaces)
                    {
                        valueStr = valueStr.Substring(0, pointIndex + 1 + decimalPlaces);
                        value = double.Parse(valueStr);
                    }
                }

                var oldValue = this.Value;
                base.SetValue(ValueProperty, value);

                var handler = this.ValueChanged;
                if (handler != null)
                {
                    handler(this, new NumberValueChangedArgs(oldValue, value));
                }
            }
        }

        /// <summary>
        /// 获取或设置数字显示框的最小允许值,为double.NaN时无限制
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)base.GetValue(MinimumProperty);
            }
            set
            {
                base.SetValue(MinimumProperty, value);

                if (!double.IsNaN(value) && this.Value - value < this._precision)
                {
                    this.Value = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置数字显示框的最大允许值,为double.NaN时无限制
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)base.GetValue(MaximumProperty);
            }
            set
            {
                base.SetValue(MaximumProperty, value);

                if (!double.IsNaN(value) && this.Value - value > this._precision)
                {
                    this.Value = value;
                }
            }
        }

        private double _precision = 0d;
        /// <summary>
        /// 获取或设置数字显示框（也称作 up-down 控件）中要显示的十进制位数
        /// </summary>
        public int DecimalPlaces
        {
            get
            {
                return (int)base.GetValue(DecimalPlacesProperty);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("值不能小于0");
                }

                this._precision = 1d / Math.Pow(10d, (double)(value + 1));
                base.SetValue(DecimalPlacesProperty, value);
            }
        }


        /// <summary>
        /// 获取或设置数字显示宽度,即固定字符长度,小于1表示无限制
        /// </summary>
        public int NumberWidth
        {
            get
            {
                return (int)base.GetValue(NumberWidthProperty);
            }
            set
            {
                base.SetValue(NumberWidthProperty, value);
            }
        }

        /// <summary>
        /// 获取或设置当数字文本不足够显示宽度时0的填充方向
        /// </summary>
        public NumberWidthFillDirection NumberWidthFillDirection
        {
            get
            {
                return (NumberWidthFillDirection)base.GetValue(NumberWidthFillDirectionProperty);
            }
            set
            {
                base.SetValue(NumberWidthFillDirectionProperty, value);
            }
        }


        ///// <summary>
        ///// 获取或设置单击向上或向下按钮时，数字显示框（也称作 up-down 控件）递增或递减的值
        ///// </summary>
        //public double Increment
        //{
        //    get
        //    {
        //        return (double)base.GetValue(IncrementProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(IncrementProperty, value);
        //    }
        //}

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selfControl = (NumberControl)d;
            if (e.Property == ValueProperty ||
                e.Property == DecimalPlacesProperty ||
                e.Property == NumberWidthProperty ||
                e.Property == NumberWidthFillDirectionProperty)
            {
                selfControl.UpdateTextByValue();
            }
        }

        private void UpdateTextByValue()
        {
            double value = this.Value;
            if (double.IsNaN(value))
            {
                this.Text = string.Empty;
                return;
            }

            this.Text = value.ToString();

            int numberWidth = this.NumberWidth;
            if (numberWidth > 0 && this.Text.Length < numberWidth)
            {
                int count = numberWidth - this.Text.Length;
                string fillStr = new string('0', count);

                switch (this.NumberWidthFillDirection)
                {
                    case NumberWidthFillDirection.Left:
                        this.Text = this.Text.Insert(0, fillStr);
                        break;
                    case NumberWidthFillDirection.Right:
                        this.Text = this.Text.Insert(this.Text.Length, fillStr);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            this.SelectionStart = this.Text.Length;
        }
        #endregion






        private bool _enablePreviewMouseDown = true;
        private bool _valueNoComit = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NumberControl()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            InputMethod.SetPreferredImeState(this, InputMethodState.Off);//禁止输入中文

            var pasteCommand = new CommandBinding(ApplicationCommands.Paste);
            pasteCommand.Executed += PasteCommand_Executed;
            this.CommandBindings.Add(pasteCommand);

            //var copyCommand = new CommandBinding(ApplicationCommands.Copy);
            //copyCommand.Executed += CopyCommand_Executed; ;
            //this.CommandBindings.Add(copyCommand);
        }

        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            object data;
            if (this.SelectionLength == 0)
            {
                data = this.Text;
            }
            else
            {
                data = this.SelectedText;
            }

            Clipboard.SetData(System.Windows.DataFormats.Text, data);
            if (e != null)
            {
                e.Handled = true;
            }
        }

        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e != null)
            {
                e.Handled = true;
            }

            string text = Clipboard.GetText(TextDataFormat.Text);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            double value;
            if (!double.TryParse(text, out value))
            {
                return;
            }

            if (value - this.Minimum < this._precision || value - this.Maximum > this._precision)
            {
                return;
            }

            this.Value = value;
        }






        /// <summary>
        /// 重写OnPreviewMouseDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (e.Handled)
            {
                return;
            }

            if (this._enablePreviewMouseDown)
            {
                this.Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 重写OnGotFocus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            this.SelectAll();
            this._enablePreviewMouseDown = false;
        }

        /// <summary>
        /// 重写OnLostFocus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            this._enablePreviewMouseDown = true;
            this.CommitValue();
        }

        private static Key[] _allowInputKeyArr = null;
        private bool _lastPressDownCtrl = false;
        /// <summary>
        /// 重写OnPreviewKeyDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Handled)
            {
                //已验证,不再验证
                return;
            }

            if (e.Key == Key.Tab ||
                e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Back || e.Key == Key.Delete)
            {
                return;
            }

            if (e.Key == Key.Enter)
            {
                this.CommitValue();
                return;
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                this._lastPressDownCtrl = true;
                return;
            }

            if (this._lastPressDownCtrl)
            {
                switch (e.Key)
                {
                    case Key.C:
                        this.CopyCommand_Executed(this, null);
                        break;
                    case Key.V:
                        this.PasteCommand_Executed(this, null);
                        break;
                    default:
                        break;
                }

                this._lastPressDownCtrl = false;
                return;
            }

            if (_allowInputKeyArr == null)
            {
                _allowInputKeyArr = new Key[]
                {
                    Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9,  //小键盘数字
                    Key.D0,Key.D1,Key.D2,Key.D3,Key.D4,Key.D5,Key.D6,Key.D7,Key.D8,Key.D9,  //数字
                    Key.OemPeriod,Key.Decimal, //小数点
                    Key.OemMinus, Key.Subtract,  //减号
                };
            }

            if (!_allowInputKeyArr.Contains(e.Key))
            {
                e.Handled = true;
                return;
            }

            if (this.SelectionLength > 0)
            {
                this.Text = string.Empty;
            }
        }

        private void CommitValue()
        {
            if (!this._valueNoComit)
            {
                return;
            }

            double value;
            string text = this.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                value = double.NaN;
            }
            else
            {
                if (text.EndsWith(_POINT))
                {
                    text = text.Substring(0, text.Length - 1);
                }

                value = double.Parse(text);
            }

            this.Value = value;
            this._valueNoComit = false;
        }


        private static string[] _allowInputStrArr = null;
        /// <summary>
        /// 重写OnPreviewTextInput
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (e.Handled)
            {
                //已验证,不再验证
                return;
            }

            if (_allowInputStrArr == null)
            {
                _allowInputStrArr = new string[]
                {
                    "-",".","0","1","2","3","4","5","6","7","8","9"
                };
            }

            if (!_allowInputStrArr.Contains(e.Text))
            {
                //输入了非数字相关的字符
                e.Handled = true;
                return;
            }


            if (this.Minimum >= 0 && string.Equals(e.Text, _SUBTRACT))
            {
                //最小值为非负数时,输入了减号
                e.Handled = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Text) && string.Equals(e.Text, _SUBTRACT))
            {
                //起始位置输入了减号
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.Text) && string.Equals(e.Text, _SUBTRACT))
            {
                //非起始位置输入了减号
                e.Handled = true;
                return;
            }

            int decimalPlaces = this.DecimalPlaces;
            if (decimalPlaces == 0 && string.Equals(e.Text, _POINT))
            {
                //小数位数为0,输入了小数点
                e.Handled = true;
                return;
            }


            //string text = this.Text + e.Text;
            string text = this.Text.Insert(this.SelectionStart, e.Text);

            if (string.IsNullOrWhiteSpace(text))
            {
                //没有值,不验证
                return;
            }

            if (text.StartsWith(_POINT))
            {
                //以小数开始
                e.Handled = true;
                return;
            }

            if (text.Length >= 2 && text[0] == _ZEROR && text[1] == _ZEROR)
            {
                //起始输入多个0
                e.Handled = true;
                return;
            }

            int pointIndex = text.IndexOf(_POINT);
            if (pointIndex > 0 && pointIndex != text.LastIndexOf(_POINT))
            {
                //多个小数点
                e.Handled = true;
                return;
            }

            if (text.EndsWith(_POINT))
            {
                //以小数结尾,不验证
                return;
            }


            double value = double.Parse(text);
            if (value - this.Minimum < this._precision)
            {
                e.Handled = true;
                return;
            }

            if (value - this.Maximum > this._precision)
            {
                e.Handled = true;
                return;
            }


            if (decimalPlaces == 0 && pointIndex > 0)
            {
                e.Handled = true;
                return;
            }

            if (pointIndex > 0 && text.Length - (pointIndex + 1) > decimalPlaces)
            {
                e.Handled = true;
                return;
            }
        }


        /// <summary>
        /// 重写OnTextChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            this._valueNoComit = true;
        }
    }


    /// <summary>
    /// 数字宽度填充方向
    /// </summary>
    public enum NumberWidthFillDirection
    {
        /// <summary>
        /// 左侧补0
        /// </summary>
        Left,

        /// <summary>
        /// 右侧补0
        /// </summary>
        Right
    }

    /// <summary>
    /// 数据改变事件参数
    /// </summary>
    public class NumberValueChangedArgs : EventArgs
    {
        /// <summary>
        /// 旧值
        /// </summary>
        public double OldValue { get; private set; }

        /// <summary>
        /// 新值
        /// </summary>
        public double NewValue { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public NumberValueChangedArgs(double oldValue, double newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}
