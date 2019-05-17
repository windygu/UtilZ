using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.CEF.Base;
using Xilium.CefGlue;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            winCefBrowser1.ProcessMessageReceived += WinCefBrowser1_ProcessMessageReceived;
            winCefBrowser1.BeforeContextMenu += WinCefBrowser1_BeforeContextMenu;
        }

        private void WinCefBrowser1_BeforeContextMenu(object sender, BeforeContextMenuEventArgs e)
        {
            e.MenuModel.Clear();
        }


        private void WinCefBrowser1_ProcessMessageReceived(object sender, ProcessMessageEventArgs e)
        {
            var pro = Process.GetCurrentProcess();
            if (e.Message.Name == "JavascriptExecutedResult")
            {
                var reult = e.Message.Arguments.GetString(0);
                MessageBox.Show("WinCefBrowser1_ProcessMessageReceived:" + pro.ProcessName + pro.Id + Environment.NewLine + reult);
            }
            else
            {
                //这里接收进程消息
                MessageBox.Show("WinCefBrowser1_ProcessMessageReceived:" + pro.ProcessName + pro.Id);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoadPage_Click(object sender, EventArgs e)
        {
            string url = @"file:///D:\Projects\Self\ServiceHostedPlatform\trunk\Code\UtilZ.Dotnet.SHPSln\WindowsFormsApplication1\HTMLPage1.html";
            //url = url.Replace('\\', '/');
            //winCefBrowser1.NavigateTo(url);
            winCefBrowser1.StartUrl = url;
        }

        private void btnExcuteJS_Click(object sender, EventArgs e)
        {
            string js = @"sum(123, 456)";
            winCefBrowser1.ExecuteJS(js);
        }

        private void btnGetJSValue_Click(object sender, EventArgs e)
        {

        }

        private void btnSendProcessMessage_Click(object sender, EventArgs e)
        {
            var msg = CefProcessMessage.Create("ExecuteJavaScript");
            msg.Arguments.SetString(0, "Add(5,6)");
            winCefBrowser1.SendProcessMessage(msg);
        }
    }
}
