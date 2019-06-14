using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.WindowEx.Base;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls
{
    /// <summary>
    /// LogControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl, ILogControl
    {
        /// <summary>
        /// 最多显示项数依赖属性
        /// </summary>
        private static readonly DependencyProperty MaxItemCountProperty =
            DependencyProperty.RegisterAttached(nameof(LogControl.MaxItemCount), typeof(int), typeof(LogControl), new PropertyMetadata(100, PropertyChanged));

        /// <summary>
        /// 是否锁定滚动依赖属性
        /// </summary>
        private static readonly DependencyProperty IsLockProperty =
            DependencyProperty.RegisterAttached(nameof(LogControl.IsLock), typeof(bool), typeof(LogControl), new PropertyMetadata(false, PropertyChanged));

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LogControl;
            if (e.Property == LogControl.MaxItemCountProperty)
            {
                control.RemoveOutElements();
            }
            else if (e.Property == LogControl.IsLockProperty)
            {
                if ((bool)e.NewValue)
                {
                    control._logShowQueue.Stop();
                }
                else
                {
                    control._logShowQueue.Start();
                }
            }
        }

        /// <summary>
        /// 获取或设置最多显示项数
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置最多显示项数,默认为100项,超过之后则会将旧的项移除")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxItemCount
        {
            get
            {
                return (int)this.GetValue(LogControl.MaxItemCountProperty);
            }
            set
            {
                this.SetValue(LogControl.MaxItemCountProperty, value);
            }
        }

        /// <summary>
        /// 是否锁定滚动
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("是否锁定滚动[true:锁定;false:不锁定;默认为false]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsLock
        {
            get
            {
                return (bool)this.GetValue(LogControl.IsLockProperty);
            }
            set
            {
                this.SetValue(LogControl.IsLockProperty, value);
            }
        }

        private AsynQueue<ShowLogItem> _logShowQueue = null;
        private readonly object _logShowQueueLock = new object();
        /// <summary>
        /// 单次最大刷新日志条数
        /// </summary>
        private int _refreshCount = 5;

        /// <summary>
        /// 日志缓存容量
        /// </summary>
        private int _cacheCapcity = 100;

        private readonly List<Inline> _lines = new List<Inline>();

        /// <summary>
        /// 样式字典集合[key:样式key;value:样式]
        /// </summary>
        private readonly Dictionary<int, LogShowStyle> _styleDic = new Dictionary<int, LogShowStyle>();
        private readonly LogShowStyle _defaultStyle;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogControl()
        {
            InitializeComponent();

            this._defaultStyle = new LogShowStyle(0, Colors.Gray);
            this._defaultStyle.Name = "默认样式";
            this.AddDefaultStyle();
            this.StartRefreshLogThread();
        }

        /// <summary>
        /// 添加默认样式
        /// </summary>
        private void AddDefaultStyle()
        {
            this.SetStyle(new LogShowStyle(LogLevel.Debug, Colors.Gray));
            this.SetStyle(new LogShowStyle(LogLevel.Error, Colors.Red));
            this.SetStyle(new LogShowStyle(LogLevel.Fatal, Colors.Red));
            this.SetStyle(new LogShowStyle(LogLevel.Info, Colors.WhiteSmoke));
            this.SetStyle(new LogShowStyle(LogLevel.Warn, Colors.Yellow));
        }

        /// <summary>
        /// 设置日志刷新信息
        /// </summary>
        /// <param name="refreshCount">单次最大刷新日志条数</param>
        /// <param name="cacheCapcity">日志缓存容量,建议等于日志最大项数</param>
        public void SetLogRefreshInfo(int refreshCount, int cacheCapcity)
        {
            if (refreshCount < 1)
            {
                throw new ArgumentException(string.Format("单次最大刷新日志条数参数值:{0}无效,该值不能小于1", refreshCount), "refreshCount");
            }

            if (cacheCapcity < refreshCount)
            {
                throw new ArgumentException(string.Format("日志缓存容量参数值:{0}无效,该值不能小于单次最大刷新日志条数参数值:{1}", cacheCapcity, refreshCount), "cacheCapcity");
            }

            if (this._refreshCount == refreshCount && this._cacheCapcity == cacheCapcity)
            {
                //参数相同,忽略
                return;
            }

            this._refreshCount = refreshCount;
            this._cacheCapcity = cacheCapcity;

            this.StartRefreshLogThread();
        }

        /// <summary>
        /// 启动刷新日志线程
        /// </summary>
        private void StartRefreshLogThread()
        {
            lock (this._logShowQueueLock)
            {
                if (this._logShowQueue != null)
                {
                    this._logShowQueue.Stop();
                    this._logShowQueue.Dispose();
                }

                this._logShowQueue = new AsynQueue<ShowLogItem>(this.ShowLog, this._refreshCount, 10, "日志显示线程", true, true, this._cacheCapcity);
            }
        }

        private void ShowLog(List<ShowLogItem> items)
        {
            try
            {
                if (items == null || items.Count == 0)
                {
                    return;
                }

                this.Dispatcher.Invoke(new Action(() => { this.ShowLog((object)items); }));
                Thread.Sleep(15);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void ShowLog(object state)
        {
            try
            {
                List<ShowLogItem> items = (List<ShowLogItem>)state;
                FontFamily fontFamily;
                foreach (var item in items)
                {
                    LogShowStyle style = this.GetStyleById(item.StyleID);
                    var run = new Run();
                    run.Text = item.LogText;
                    run.Foreground = style.ForegroundBrush;
                    run.FontSize = style.FontSize;
                    fontFamily = style.FontFamily;
                    if (fontFamily != null)
                    {
                        run.FontFamily = fontFamily;
                    }

                    content.Inlines.Add(run);
                    this._lines.Add(run);

                    if (!this.IsLock)
                    {
                        this.RemoveOutElements();
                    }
                }

                rtxt.ScrollToEnd();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void RemoveOutElements()
        {
            var maxItemCount = this.MaxItemCount;
            while (this._lines.Count > maxItemCount)
            {
                content.Inlines.Remove(this._lines[0]);
                this._lines.RemoveAt(0);
            }
        }

        /// <summary>
        /// 设置样式,不存在添加,存在则用新样式替换旧样式
        /// </summary>
        /// <param name="style">样式</param>
        public void SetStyle(LogShowStyle style)
        {
            var id = style.ID;
            if (this._styleDic.ContainsKey(id))
            {
                this._styleDic[id] = style;
            }
            else
            {
                this._styleDic.Add(id, style);
            }
        }

        /// <summary>
        /// 移除样式
        /// </summary>
        /// <param name="style">样式标识</param>
        public void RemoveStyle(LogShowStyle style)
        {
            if (style == null)
            {
                return;
            }

            var id = style.ID;
            if (this._styleDic.ContainsKey(id))
            {
                this._styleDic.Remove(id);
            }
        }

        /// <summary>
        /// 清空样式
        /// </summary>
        public void ClearStyle()
        {
            this._styleDic.Clear();
        }

        /// <summary>
        /// 获取当前所有样式数组
        /// </summary>
        /// <returns>当前所有样式数组</returns>
        public LogShowStyle[] GetStyles()
        {
            return this._styleDic.Values.ToArray();
        }

        /// <summary>
        /// 根据样式标识ID获取样式
        /// </summary>
        /// <param name="id">样式标识ID</param>
        /// <returns>获取样式</returns>
        public LogShowStyle GetStyleById(int id)
        {
            if (this._styleDic.ContainsKey(id))
            {
                return this._styleDic[id];
            }

            return this._defaultStyle;
        }

        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">显示内容</param>
        /// <param name="level">日志级别</param>
        public void AddLog(string logText, LogLevel level)
        {
            this.AddLog(logText, (int)level);
        }

        private readonly object _addLogLock = new object();
        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">显示内容</param>
        /// <param name="styleId">样式标识ID</param>
        public void AddLog(string logText, int styleId)
        {
            var item = new ShowLogItem(logText, styleId);
            bool result;
            lock (this._addLogLock)
            {
                while (true)
                {
                    result = this._logShowQueue.Enqueue(item);
                    if (result)
                    {
                        break;
                    }
                    else
                    {
                        this._logShowQueue.Remove(1);
                    }
                }
            }
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public void Clear()
        {
            this.content.Inlines.Clear();
            this._lines.Clear();
        }

        /// <summary>
        /// 显示的日志项
        /// </summary>
        [Serializable]
        internal class ShowLogItem
        {
            private int _styleId;
            /// <summary>
            /// 样式标识ID
            /// </summary>
            public int StyleID
            {
                get { return _styleId; }
            }

            private string _logText;
            /// <summary>
            /// 文本内容
            /// </summary>
            public string LogText
            {
                get { return _logText; }
            }

            private const string _newLine = "\r\n";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="logText"></param>
            /// <param name="styleId"></param>
            public ShowLogItem(string logText, int styleId)
            {
                logText += _newLine;
                //if (logText == null)
                //{
                //    logText = string.Empty;
                //}
                //else
                //{
                //    if (!logText.EndsWith(_newLine))
                //    {
                //        logText += _newLine;
                //    }
                //}

                this._logText = logText;
                this._styleId = styleId;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="logText"></param>
            /// <param name="level">日志级别</param>
            public ShowLogItem(string logText, LogLevel level) :
                this(logText, (int)level)
            {
            }
        }
    }
}
