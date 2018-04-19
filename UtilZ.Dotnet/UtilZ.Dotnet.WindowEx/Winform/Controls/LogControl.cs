using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.IO;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls
{
    /// <summary>
    /// 日志显示控件
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
            set { _isLock = value; }
        }

        /// <summary>
        /// 获取选中文本
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("是否锁定滚动[true:锁定;false:不锁定;默认为false]")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectText
        {
            get
            {
                IHTMLDocument2 doc2 = webBrowser.Document.DomDocument as IHTMLDocument2;
                dynamic range = doc2.selection.createRange();
                return range.htmlText;
            }
        }

        private HtmlElement _logContainerEle;
        private string _logContainerEleId;
        private string _logItemEleName;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogControl()
        {
            InitializeComponent();

            this.webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            string logHtmlTemplate = @"<!DOCTYPE html>
            <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
            <head>
                <meta charset=""utf-8"" />
                <title></title>
                <style type=""text/css"">
        body {
            background-color: lavender
        }
        ul {
            margin: 0px 0px 0px 0px;
        }
                    li {
                        list-style: none;
                    }
                </style>
            </head>
            <body id=""bid"">
            <ul  id=""uid"">
            </ul>
            </body>
            </html>";
            this.Init("uid", "li");
            this.webBrowser.DocumentText = logHtmlTemplate;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this._logContainerEle = this.webBrowser.Document.GetElementById(this._logContainerEleId);
        }

        private void Init(string logContainerEleId, string logItemEleName)
        {
            this._logContainerEleId = logContainerEleId;
            this._logItemEleName = logItemEleName;
        }

        /// <summary>
        /// 设置日志显示模板
        /// </summary>
        /// <param name="templateFilePath">日志模板html文件路径</param>
        /// <param name="logContainerEleId">显示日志的容器元素id</param>
        /// <param name="logItemEleName">日志项内容名称</param>
        public void SetTemplate(string templateFilePath, string logContainerEleId, string logItemEleName)
        {
            if (!File.Exists(templateFilePath))
            {
                throw new FileNotFoundException("日志模板文件不存在", templateFilePath);
            }

            this.Init(logContainerEleId, logItemEleName);
            //file://F:/Project/Git/UtilZ/UtilZ.Dotnet/TestE/bin/Debug/LogControl.html
            string urlPath = Uri.UriSchemeFile + Uri.SchemeDelimiter + Path.GetFullPath(templateFilePath).Replace(Path.DirectorySeparatorChar, '/');
            this.webBrowser.Navigate(urlPath);
        }

        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">日志文本</param>
        public void AddLog(string logText)
        {
            if (this.webBrowser.InvokeRequired)
            {
                this.webBrowser.Invoke(new Action(() =>
                {
                    this.AddLog(logText);
                }));
            }
            else
            {
                this.ShowLog(logText, null);
            }
        }

        /// <summary>
        /// 添加显示日志
        /// </summary>
        /// <param name="logText">日志文本</param>
        /// <param name="color">该条记录文本所显示的颜色</param>
        public void AddLog(string logText, Color color)
        {
            if (this.webBrowser.InvokeRequired)
            {
                this.webBrowser.Invoke(new Action(() =>
                {
                    this.AddLog(logText, color);
                }));
            }
            else
            {
                string style = string.Format("color:{0}", ColorTranslator.ToHtml(color));
                this.ShowLog(logText, style);
            }
        }

        /// <summary>
        /// 将日志显示到控件上
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="style"></param>
        private void ShowLog(string logInfo, string style)
        {
            if (this._logContainerEle == null)
            {
                return;
            }

            HtmlElement logEle = this.webBrowser.Document.CreateElement(this._logItemEleName);
            logEle.Style = style;
            logEle.InnerText = logInfo;

            this._logContainerEle.AppendChild(logEle);
            this.webBrowser.Document.Body.ScrollTop = this.webBrowser.Height;
            if (!this._isLock)
            {
                this.RemoveOutElements();
                this.webBrowser.Document.Window.ScrollTo(0, this.webBrowser.Document.Window.Size.Height);
            }
        }

        /// <summary>
        /// 移除超出限制的日志项
        /// </summary>
        private void RemoveOutElements()
        {
            if (this._logContainerEle == null)
            {
                return;
            }

            var logItemElementCollection = this._logContainerEle.Children;
            int offset = logItemElementCollection.Count - this._maxItemCount;
            if (offset <= 0)
            {
                return;
            }

            IHTMLDOMNode node;
            for (int i = 0; i < offset; i++)
            {
                node = logItemElementCollection[0].DomElement as IHTMLDOMNode;
                node.parentNode.removeChild(node);
            }
        }
    }
}
