﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.WindowEx.Base;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls
{
    /// <summary>
    /// 日志显示控件
    /// </summary>
    public sealed class LogControl : RichTextBox, ILogControl, IDisposable
    {
        #region 依赖属性
        /// <summary>
        /// 最多显示项数依赖属性
        /// </summary>
        private static readonly DependencyProperty MaxItemCountProperty =
            DependencyProperty.RegisterAttached(nameof(MaxItemCount), typeof(int), typeof(LogControl), new PropertyMetadata(100, PropertyChanged));

        /// <summary>
        /// 是否锁定滚动依赖属性
        /// </summary>
        private static readonly DependencyProperty IsLockProperty =
            DependencyProperty.RegisterAttached(nameof(IsLock), typeof(bool), typeof(LogControl), new PropertyMetadata(false, PropertyChanged));

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
        #endregion




        private AsynQueue<ShowLogItem> _logShowQueue = null;
        /// <summary>
        /// 单次最大刷新日志条数
        /// </summary>
        private int _refreshCount = 5;
        private readonly object _logLock = new object();

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



        private readonly Paragraph content;
        private readonly Delegate _primitiveShowLogDelegate;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogControl()
            : base()
        {
            this.IsReadOnly = true;
            this.Background = Brushes.Black;
            this.BorderThickness = new Thickness(0d);
            this.content = new Paragraph();
            base.Document.Blocks.Clear();
            base.Document.Blocks.Add(this.content);

            this._primitiveShowLogDelegate = new Action<List<ShowLogItem>>(this.PrimitiveShowLog);

            this._defaultStyle = new LogShowStyle(0, Colors.Gray) { Name = "默认样式" };
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

            lock (this._logLock)
            {
                if (this._refreshCount == refreshCount && this._cacheCapcity == cacheCapcity)
                {
                    //参数相同,忽略
                    return;
                }

                this._refreshCount = refreshCount;
                this._cacheCapcity = cacheCapcity;
                this.StartRefreshLogThread();
            }
        }

        /// <summary>
        /// 启动刷新日志线程
        /// </summary>
        private void StartRefreshLogThread()
        {
            lock (this._logLock)
            {
                if (this._logShowQueue != null)
                {
                    this._logShowQueue.Stop();
                    this._logShowQueue.Dispose();
                    this._logShowQueue = null;
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

                DispatcherOperation dispatcherOperation = this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, this._primitiveShowLogDelegate, items);
                dispatcherOperation.Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void PrimitiveShowLog(List<ShowLogItem> items)
        {
            try
            {
                FontFamily fontFamily;
                foreach (var item in items)
                {
                    LogShowStyle style = this.GetStyleById(item.StyleID);
                    var run = new Run()
                    {
                        Text = item.LogText,
                        Foreground = style.ForegroundBrush,
                        FontSize = style.FontSize,
                    };
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

                base.ScrollToEnd();
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
            lock (this._logLock)
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

            lock (this._logLock)
            {
                var id = style.ID;
                if (this._styleDic.ContainsKey(id))
                {
                    this._styleDic.Remove(id);
                }
            }
        }

        /// <summary>
        /// 清空样式
        /// </summary>
        public void ClearStyle()
        {
            lock (this._logLock)
            {
                this._styleDic.Clear();
            }
        }

        /// <summary>
        /// 获取当前所有样式数组
        /// </summary>
        /// <returns>当前所有样式数组</returns>
        public LogShowStyle[] GetStyles()
        {
            lock (this._logLock)
            {
                return this._styleDic.Values.ToArray();
            }
        }

        /// <summary>
        /// 根据样式标识ID获取样式
        /// </summary>
        /// <param name="id">样式标识ID</param>
        /// <returns>获取样式</returns>
        public LogShowStyle GetStyleById(int id)
        {
            lock (this._logLock)
            {
                if (this._styleDic.ContainsKey(id))
                {
                    return this._styleDic[id];
                }

                return this._defaultStyle;
            }
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


        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">显示内容</param>
        /// <param name="styleId">样式标识ID</param>
        public void AddLog(string logText, int styleId)
        {
            var item = new ShowLogItem(logText, styleId);
            lock (this._logLock)
            {
                if (this._logShowQueue == null)
                {
                    return;
                }

                this._logShowQueue.Enqueue(item);
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
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                lock (this._logLock)
                {
                    if (this._logShowQueue != null)
                    {
                        this._logShowQueue.Stop();
                        this._logShowQueue.Dispose();
                        this._logShowQueue = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }





    /// <summary>
    /// 显示的日志项
    /// </summary>
    internal class ShowLogItem
    {
        /// <summary>
        /// 样式标识ID
        /// </summary>
        public int StyleID { get; private set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string LogText { get; private set; }

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

            this.LogText = logText;
            this.StyleID = styleId;
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
