using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls
{
    /// <summary>
    /// Winform日志控件,对WPF版本进行封装得到
    /// </summary>
    public partial class LogControlF : UserControl
    {
        /// <summary>
        /// 获取或设置最多显示项数
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("获取或设置最多显示项数,默认为100项,超过之后则会将旧的项移除")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxItemCount
        {
            get { return logControl.MaxItemCount; }
            set { logControl.MaxItemCount = value; }
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
            get { return logControl.IsLock; }
            set { logControl.IsLock = value; }
        }

        /// <summary>
        /// 设置日志刷新信息
        /// </summary>
        /// <param name="refreshCount">单次最大刷新日志条数</param>
        /// <param name="cacheCapcity">日志缓存容量,建议等于日志最大项数</param>
        public void SetLogRefreshInfo(int refreshCount, int cacheCapcity)
        {
            logControl.SetLogRefreshInfo(refreshCount, cacheCapcity);
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
            var color = System.Windows.Media.Color.FromArgb(foreground.A, foreground.R, foreground.G, foreground.B);
            logControl.AddStyle(level, color, fontFamilyName, fontSize);
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
            var color = System.Windows.Media.Color.FromArgb(foreground.A, foreground.R, foreground.G, foreground.B);
            logControl.AddStyle(styleKey, color, fontFamilyName, fontSize);
        }

        /// <summary>
        /// 移除样式
        /// </summary>
        /// <param name="level">日志级别</param>
        public void RemoveStyle(LogLevel level)
        {
            logControl.RemoveStyle(level);
        }

        /// <summary>
        /// 移除样式
        /// </summary>
        /// <param name="styleKey">样式key</param>
        public void RemoveStyle(int styleKey)
        {
            logControl.RemoveStyle(styleKey);
        }

        /// <summary>
        /// 清除样式
        /// </summary>
        public void ClearStyle()
        {
            logControl.ClearStyle();
        }

        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">日志文本</param>
        /// <param name="level">日志级别</param>
        public void AddLog(string logText, LogLevel level)
        {
            logControl.AddLog(logText, level);
        }

        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">日志文本</param>
        /// <param name="styleKey">样式key</param>
        public void AddLog(string logText, int styleKey)
        {
            logControl.AddLog(logText, styleKey);
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public void Clear()
        {
            logControl.Clear();
        }

        /// <summary>
        /// 获取或设置日志控件背景色
        /// </summary>
        public override Color BackColor
        {
            get
            {
                var brush = (System.Windows.Media.SolidColorBrush)logControl.Background;
                return Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
            }
            set
            {
                var color = System.Windows.Media.Color.FromArgb(value.A, value.R, value.G, value.B);
                logControl.Background = new System.Windows.Media.SolidColorBrush(color);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogControlF()
        {
            InitializeComponent();
        }
    }
}
