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

            this._defaultStyle = new LogShowStyle(Colors.Gray, this.GetFontFamily(null), 15);
            this.StartRefreshLogThread();
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
                foreach (var item in items)
                {
                    LogShowStyle style = this.GetStyle(item.StyleKey);
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
            this.AddStyle((int)level, foreground, fontFamilyName, fontSize);
        }

        /// <summary>
        /// 添加样式
        /// </summary>
        /// <param name="styleKey">样式key</param>
        /// <param name="foreground">字体颜色</param>
        /// <param name="fontFamilyName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        public void AddStyle(int styleKey, Color foreground, string fontFamilyName = null, double fontSize = 15d)
        {
            FontFamily fontFamily = this.GetFontFamily(fontFamilyName);
            var style = new LogShowStyle(foreground, fontFamily, fontSize);
            if (this._styleDic.ContainsKey(styleKey))
            {
                this._styleDic[styleKey] = style;
            }
            else
            {
                this._styleDic.Add(styleKey, style);
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
            this.RemoveStyle((int)level);
        }

        /// <summary>
        /// 移除样式
        /// </summary>
        /// <param name="styleKey">样式key</param>
        public void RemoveStyle(int styleKey)
        {
            if (this._styleDic.ContainsKey(styleKey))
            {
                this._styleDic.Remove(styleKey);
            }
        }

        /// <summary>
        /// 清除样式
        /// </summary>
        public void ClearStyle()
        {
            this._styleDic.Clear();
        }

        private LogShowStyle GetStyle(int styleKey)
        {
            if (this._styleDic.ContainsKey(styleKey))
            {
                return this._styleDic[styleKey];
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
            this.AddLog(logText, (int)level);
        }

        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">日志文本</param>
        /// <param name="styleKey">样式key</param>
        public void AddLog(string logText, int styleKey)
        {
            var item = new ShowLogItem(logText, styleKey);
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
        private int _styleKey;
        public int StyleKey { get { return _styleKey; } }

        private string _logText;
        public string LogText { get { return _logText; } }

        private const string _newLine = "\r\n";

        public ShowLogItem(string logText, int styleKey)
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
            this._styleKey = styleKey;
        }
    }
}
