using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.NetTransfer.Core;
using UtilZ.Dotnet.Ex.NetTransfer.Model;
using UtilZ.Dotnet.Ex.NetTransfer.Net;

namespace WinFormB
{
    public partial class FB : Form
    {
        private TransferCore _transfer;
        private readonly int _textMaxLength = 1024;
        private NetTransferConfig _config;

        public FB()
        {
            InitializeComponent();
        }

        private void FB_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            RedirectOuputCenter.Add(new RedirectOutputChannel(this.LogRedirectOuput, "RedirectToUI"));

            try
            {
                rtxtMsg.MaxLength = this._textMaxLength;
                _config = new NetTransferConfig();
                numParallelThreadCount.Value = _config.ParallelThreadMaxCount;
                _config.LocalFileDirectory = @"E:\Tmp";
                this._transfer = new TransferCore(_config, new IPEndPoint(IPAddress.Any, 6102), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6101), this.Rev);
                this._transfer.Start();
                Loger.Info("启动成功...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "程序启动失败");
            }
        }

        private void Rev(ReceiveDataItem item)
        {
            try
            {
                if (item == null)
                {
                    return;
                }

                if (item.Flag)
                {
                    var data = item.Data;
                    if (data.Length > this._textMaxLength)
                    {
                        //文件
                        string filePath = $@"E:\Tmp\{DateTime.Now.ToFileTime()}.dat";
                        Loger.Trace($"收到数据内容,写入文件{filePath}");
                        File.WriteAllBytes(filePath, data);
                        Loger.Trace($"收到数据内容,写入文件{filePath}完成");
                    }
                    else
                    {
                        //文本消息
                        string txt = Encoding.UTF8.GetString(data);
                        Loger.Info($"收到文本消息:{txt}");
                    }
                }
                else
                {
                    Loger.Info($"收到文件:{item.FilePath}");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "解析数据内容异常");
            }
        }

        private void LogRedirectOuput(RedirectOuputItem e)
        {
            try
            {
                //if (e.Item.Level < LogLevel.Info)
                //{
                //    return;
                //}

                string logInfo = string.Format("{0} {1} {2}", e.Item.Time.ToString("yyyy-MM-dd HH:mm:ss"), LogConstant.GetLogLevelName(e.Item.Level), e.Item.Content);
                logControlF1.AddLog(logInfo, e.Item.Level);
            }
            catch (Exception ex)
            {
                logControlF1.AddLog(ex.Message, LogLevel.Error);
            }
        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            string txt = rtxtMsg.Text;
            if (string.IsNullOrEmpty(txt))
            {
                Loger.Warn("消息内容不能为空");
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(txt);
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Loger.Trace("发送消息数据开始");
                        this._transfer.Send(data, 1000 * 1000);
                        Loger.Trace("发送消息数据完成");
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送消息数据异常");
                    }
                });
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "发送文本消息异常");
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                byte[] data = File.ReadAllBytes(ofd.FileName);
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Loger.Trace("发送资源数据开始");
                        this._transfer.Send(data, 1000 * 1000);
                        Loger.Trace("发送资源数据完成");
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送资源数据异常");
                    }
                });
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "发送数据异常");
            }
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Loger.Trace("发送文件开始");
                        this._transfer.SendFile(ofd.FileName, 1000 * 1000);
                        Loger.Trace("发送文件完成");
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送文件异常");
                    }
                });
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "发送文件异常");
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logControlF1.Clear();
        }

        private void numParallelThreadCount_ValueChanged(object sender, EventArgs e)
        {
            _config.ParallelThreadMaxCount = (byte)numParallelThreadCount.Value;
        }
    }
}
