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
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls
{
    /// <summary>
    /// LogControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl
    {
        /// <summary>
        /// 最多显示项数
        /// </summary>
        private int _maxItemCount = 100;
        /// <summary>
        /// 获取或设置最多显示项数
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置最多显示项数,默认为100项,超过之后则会将旧的项移除")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxItemCount
        {
            get { return _maxItemCount; }
            set
            {
                if (_maxItemCount == value)
                {
                    return;
                }

                if (value < 1)
                {
                    _maxItemCount = 1;
                }
                else
                {
                    _maxItemCount = value;
                }

                this.RemoveOutElements();
            }
        }

        private bool _isLock = false;
        /// <summary>
        /// 是否锁定滚动
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("是否锁定滚动[true:锁定;false:不锁定;默认为false]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsLock
        {
            get { return _isLock; }
            set
            {
                if (_isLock == value)
                {
                    return;
                }

                _isLock = value;
                if (_isLock)
                {
                    this._logShowQueue.Stop();
                }
                else
                {
                    this._logShowQueue.Start();
                }
            }
        }

        private AsynQueue<ShowLogItem> _logShowQueue = null;
        private readonly object _logShowQueueLock = new object();
        /// <summary>
        /// 单次最大刷新日志条数
        /// </summary>
        private int _refreshCount = 10;

        /// <summary>
        /// 日志缓存容量
        /// </summary>
        private int _cacheCapcity = 100;

        private readonly int _millisecondsTimeout = 10;
        private readonly List<Inline> _lines = new List<Inline>();
        private readonly Dictionary<LogLevel, LogShowStyle> _leveStyle = new Dictionary<LogLevel, LogShowStyle>();
        private readonly LogShowStyle _defaultStyle;
        /// <summary>
        /// 获取控件的同步上下文
        /// </summary>
       // private System.Threading.SynchronizationContext _synContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogControl()
        {
            InitializeComponent();

            this._defaultStyle = new LogShowStyle(Colors.Gray, this.GetFontFamily(null), 15);
            // this._synContext = System.Threading.SynchronizationContext.Current;     
            this.Loaded += LogControl_Loaded;
        }

        private void LogControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartRefreshLogThread();
        }

        /*
        private static DispatcherOperationCallback exitFrameCallback = new
                                DispatcherOperationCallback(ExitFrame);

        /// <summary>
        /// Processes all UI messages currently in the message queue.
        /// </summary>

        public static void DoEvents()
        {

            // Create new nested message pump.

            DispatcherFrame nestedFrame = new DispatcherFrame();

            // Dispatch a callback to the current message queue, when getting called,

            // this callback will end the nested message loop.

            // note that the priority of this callback should be lower than the that of UI event messages.

            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(

                                                  DispatcherPriority.Background, exitFrameCallback, nestedFrame);

            // pump the nested message loop, the nested message loop will

            // immediately process the messages left inside the message queue.
            Dispatcher.PushFrame(nestedFrame);

            // If the "exitFrame" callback doesn't get finished, Abort it.

            if (exitOperation.Status != DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }

        private static Object ExitFrame(Object state)
        {
            DispatcherFrame frame = state as DispatcherFrame;
            // Exit the nested message loop.
            frame.Continue = false;
            return null;
        }
        */

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

                this._logShowQueue = new AsynQueue<ShowLogItem>(this.ShowLog, this._refreshCount, "日志显示线程", true, true, this._cacheCapcity);
            }
        }

        private void ShowLog(List<ShowLogItem> items)
        {
            try
            {
                //this._synContext.Post(new System.Threading.SendOrPostCallback(this.ShowLog), items);
                //DoEvents();
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
                foreach (var item in items)
                {
                    LogShowStyle style = this.GetStyle(item.Level);
                    var run = new Run();
                    run.Text = item.LogText;
                    run.Foreground = style.Foreground;
                    if (style.FontSize > 0)
                    {
                        run.FontSize = style.FontSize;
                    }

                    if (style.FontFamily != null)
                    {
                        run.FontFamily = style.FontFamily;
                    }

                    content.Inlines.Add(run);
                    this._lines.Add(run);

                    if (!this._isLock)
                    {
                        this.RemoveOutElements();
                    }
                }

                rtxt.ScrollToEnd();

                //if (items.LastOrDefault().Level == LogLevel.Faltal)
                //{
                //    UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ.LMQCenter.Publish("123", null);
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void ShowLog_bk(object state)
        {
            try
            {
                List<ShowLogItem> items = (List<ShowLogItem>)state;
                foreach (var item in items)
                {
                    LogShowStyle style = this.GetStyle(item.Level);

                    var span = new Span();
                    var run = new Run();
                    run.Text = item.LogText;
                    run.Foreground = style.Foreground;
                    if (style.FontSize > 0)
                    {
                        run.FontSize = style.FontSize;
                    }

                    if (style.FontFamily != null)
                    {
                        run.FontFamily = style.FontFamily;
                    }

                    span.Inlines.Add(run);
                    span.Inlines.Add(new LineBreak());
                    content.Inlines.Add(span);
                    this._lines.Add(span);

                    if (!this._isLock)
                    {
                        this.RemoveOutElements();
                    }
                }

                rtxt.ScrollToEnd();


                //if (items.LastOrDefault().Level == LogLevel.Faltal)
                //{
                //    UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ.LMQCenter.Publish("123", null);
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void RemoveOutElements()
        {
            var maxItemCount = this._maxItemCount;
            while (this._lines.Count > maxItemCount)
            {
                content.Inlines.Remove(this._lines[0]);
                this._lines.RemoveAt(0);
            }
        }

        /// <summary>
        /// 添加样式
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="foreground">字体颜色</param>
        /// <param name="fontFamilyName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        public void AddStyle(LogLevel level, Color foreground, string fontFamilyName = null, double fontSize = 15d)
        {
            FontFamily fontFamily = this.GetFontFamily(fontFamilyName);
            var style = new LogShowStyle(foreground, fontFamily, fontSize);
            if (this._leveStyle.ContainsKey(level))
            {
                this._leveStyle[level] = style;
            }
            else
            {
                this._leveStyle.Add(level, style);
            }
        }

        private FontFamily GetFontFamily(string fontName)
        {
            FontFamily fontFamily;
            if (string.IsNullOrWhiteSpace(fontName))
            {
                fontFamily = null;
                //fontName = "Microsoft YaHei UI";
            }
            else
            {
                var fonts = UtilZ.Dotnet.Ex.Base.FontEx.GetSystemInstallFonts();
                bool existYahei = (from tmpItem in fonts where string.Equals(tmpItem.Name, fontName, StringComparison.OrdinalIgnoreCase) select tmpItem).Count() > 0;
                if (existYahei)
                {
                    //fontName = fonts[0].Name;
                    fontFamily = new FontFamily(fontName);
                }
                else
                {
                    fontFamily = null;
                }
            }

            return fontFamily;
        }

        /// <summary>
        /// 移除样式
        /// </summary>
        /// <param name="level">日志级别</param>
        public void RemoveStyle(LogLevel level)
        {
            if (this._leveStyle.ContainsKey(level))
            {
                this._leveStyle.Remove(level);
            }
        }

        /// <summary>
        /// 清除样式
        /// </summary>
        public void ClearStyle()
        {
            this._leveStyle.Clear();
        }

        private LogShowStyle GetStyle(LogLevel level)
        {
            if (this._leveStyle.ContainsKey(level))
            {
                return this._leveStyle[level];
            }

            return this._defaultStyle;
        }

        private readonly object _addLogLock = new object();
        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">日志文本</param>
        /// <param name="level">日志级别</param>
        public void AddLog(string logText, LogLevel level)
        {
            var item = new ShowLogItem(logText, level);
            bool result;
            lock (this._addLogLock)
            {
                while (true)
                {
                    result = this._logShowQueue.Enqueue(item, this._millisecondsTimeout);
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

        /*
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Player1.Child = CreateMediaElementOnWorkerThread();
            //Player2.Child = CreateMediaElementOnWorkerThread();
            //Player3.Child = CreateMediaElementOnWorkerThread();
        }

        private HostVisual CreateMediaElementOnWorkerThread()
        {
            // Create the HostVisual that will "contain" the VisualTarget
            // on the worker thread.
            HostVisual hostVisual = new HostVisual();

            // Spin up a worker thread, and pass it the HostVisual that it
            // should be part of.
            Thread thread = new Thread(new ParameterizedThreadStart(MediaWorkerThread));
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start(hostVisual);
            // Wait for the worker thread to spin up and create the VisualTarget.
            s_event.WaitOne();
            return hostVisual;
        }

        private FrameworkElement CreateMediaElement()
        {
            // Create a MediaElement, and give it some video content.
            MediaElement mediaElement = new MediaElement();
            mediaElement.BeginInit();
            mediaElement.Source = new Uri("http://download.microsoft.com/download/2/C/4/2C433161-F56C-4BAB-BBC5-B8C6F240AFCC/SL_0410_448x256_300kb_2passCBR.wmv?amp;clcid=0x409");
            mediaElement.Width = 200;
            mediaElement.Height = 100;
            mediaElement.EndInit();
            return mediaElement;
        }

        private void MediaWorkerThread(object arg)
        {
            // Create the VisualTargetPresentationSource and then signal the
            // calling thread, so that it can continue without waiting for us.
            HostVisual hostVisual = (HostVisual)arg;


            VisualTargetPresentationSource visualTargetPS = new VisualTargetPresentationSource(hostVisual);
            s_event.Set();

            // Create a MediaElement and use it as the root visual for the
            // VisualTarget.
            visualTargetPS.RootVisual = CreateMediaElement();

            // Run a dispatcher for this worker thread.  This is the central
            // processing loop for WPF.
            System.Windows.Threading.Dispatcher.Run();
        }

        private static AutoResetEvent s_event = new AutoResetEvent(false);
        */
    }

    internal class LogShowStyle
    {
        public Brush Foreground { get; private set; }
        public double FontSize { get; private set; }
        public FontFamily FontFamily { get; private set; }

        public LogShowStyle(Color foreground, FontFamily fontFamily, double fontSize)
        {
            this.Foreground = new SolidColorBrush(foreground);
            this.FontFamily = fontFamily;
            this.FontSize = fontSize;
        }
    }

    internal class ShowLogItem
    {
        private LogLevel _level;
        public LogLevel Level { get { return _level; } }

        private string _logText;
        public string LogText { get { return _logText; } }

        private const string _newLine = "\r\n";

        public ShowLogItem(string logText, LogLevel level)
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
            this._level = level;
        }
    }
}
