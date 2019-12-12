﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace WinFormA
{
    public partial class FTcp : Form
    {
        public FTcp()
        {
            InitializeComponent();
        }

        private LogLevel _logShowLevel = LogLevel.Trace;
        private Socket _listen;
        private Socket _client;
        private AsynQueue<byte[]> _proBufferQueue;
        private void FTcp_Load(object sender, EventArgs e)
        {
            var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, "RedirectToUI");
            if (redirectAppenderToUI != null)
            {
                redirectAppenderToUI.RedirectOuput += RedirectOuput;
            }
            DropdownBoxHelper.BindingEnumToComboBox<LogLevel>(comboBoxLogLevel, this._logShowLevel);
            _proBufferQueue = new AsynQueue<byte[]>(ProRevBuffer, "处理接收到数据队列", true, true);
            this.FormClosing += FTcp_FormClosing;
        }

        private void FTcp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_listen != null)
            {
                _listen.Dispose();
            }

            if (_client != null)
            {
                _client.Disconnect(false);
                _client.Dispose();
            }
        }

        private byte[] _data = null;
        private Stream _stream = null;
        private long _packageLen = 0;
        private string _filePath = null;
        private long _dataLen = 0;
        const int headSize = 17;

        private void ProRevBuffer(byte[] buffer)
        {
            try
            {
                return;
                int dataLen = 0;
                if (_stream == null)
                {
                    using (var ms = new MemoryStream(buffer))
                    {
                        var br = new BinaryReader(ms);
                        _packageLen = br.ReadInt64();
                        var flag = br.ReadByte();
                        _dataLen = br.ReadInt64();

                        switch (flag)
                        {
                            case 1:
                                string txt = Encoding.UTF8.GetString(buffer.Skip(headSize).ToArray());
                                Loger.Info($"收到文本消息:{txt}");
                                return;
                            case 2:
                                Loger.Info($"数据长度:{_dataLen}");
                                _data = new byte[_dataLen];
                                _stream = new MemoryStream(_data);
                                break;
                            case 3:
                                Loger.Info($"文件长度:{_dataLen}");
                                _filePath = $@"E:\Tmp\{DateTime.Now.ToFileTime()}.dat";
                                _stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.Write);
                                break;
                        }

                        dataLen = buffer.Length - headSize;
                        _stream.Write(buffer, headSize, dataLen);
                        _stream.Flush();
                        _dataLen -= dataLen;
                    }
                }
                else
                {
                    _stream.Write(buffer, 0, buffer.Length);
                    _stream.Flush();
                    _dataLen -= buffer.Length;
                }

                if (_dataLen == 0)
                {
                    if (_stream is MemoryStream)
                    {
                        //文件
                        string filePath = $@"E:\Tmp\{DateTime.Now.ToFileTime()}.dat";
                        Loger.Trace($"收到数据内容,写入文件{filePath}");
                        File.WriteAllBytes(filePath, _data);
                        Loger.Trace($"收到数据内容,写入文件{filePath}完成");
                    }
                    else
                    {
                        Loger.Trace($"收到文件{_filePath}");
                    }

                    _stream.Close();
                    _stream = null;
                    _data = null;
                    _filePath = null;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "处理收到数据异常");
            }
        }

        private void RedirectOuput(object sender, RedirectOuputArgs e)
        {
            try
            {
                if (e.Item.Level < this._logShowLevel)
                {
                    return;
                }

                string logInfo = string.Format("{0} {1} {2}", e.Item.Time.ToString("yyyy-MM-dd HH:mm:ss"), LogConstant.GetLogLevelName(e.Item.Level), e.Item.Content);
                logControl.AddLog(logInfo, e.Item.Level);
            }
            catch (Exception ex)
            {
                logControl.AddLog(ex.Message, LogLevel.Error);
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            IPAddress listenAddr;
            if (!IPAddress.TryParse(txtSendIp.Text, out listenAddr))
            {
                Loger.Error("发送端IP无效");
                return;
            }

            _listen = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //new IPEndPoint(dstIpAddr, (int)numDstPort.Value)
            _listen.Bind(new IPEndPoint(IPAddress.Any, (int)numSrcPort.Value));
            btnInit.Enabled = false;
            Task.Factory.StartNew(AcceptClient, TaskCreationOptions.LongRunning);
        }

        private void AcceptClient()
        {
            _listen.Listen(10);
            while (true)
            {
                try
                {
                    _client = _listen.Accept();
                    Task.Factory.StartNew(Rev, _client, TaskCreationOptions.LongRunning);
                    Loger.Info("客户端连接上");

                }
                catch (Exception ex)
                {
                    Loger.Error(ex, "_listen.Accept异常");
                }
            }
        }

        private void Rev(object obj)
        {
            byte[] buffer = new byte[65536];
            Socket client = (Socket)obj;
            int len;

            while (true)
            {
                try
                {
                    len = client.Receive(buffer);
                    if (len > 0)
                    {
                        _proBufferQueue.Enqueue(buffer.Take(len).ToArray());
                    }
                }
                catch (SocketException sex)
                {
                    Loger.Error(sex, "接收数据异常SocketException");
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, "接收数据异常");
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress dstIpAddr;
            if (!IPAddress.TryParse(txtDstIp.Text, out dstIpAddr))
            {
                Loger.Error("目的地IP无效");
                return;
            }

            _client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _client.Connect(new IPEndPoint(dstIpAddr, (int)numDstPort.Value));
            Task.Factory.StartNew(Rev, _client, TaskCreationOptions.LongRunning);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logControl.Clear();
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
                byte[] buffer = new byte[headSize + data.Length];
                using (var ms = new MemoryStream(buffer))
                {
                    var bw = new BinaryWriter(ms);
                    bw.Write((long)buffer.Length);
                    bw.Write((byte)1);
                    bw.Write((long)data.Length);
                    bw.Write(data, 0, data.Length);
                }

                Loger.Trace("发送消息数据开始");
                _client.Send(buffer);
                Loger.Trace("发送消息数据完成");
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


                Task.Factory.StartNew((obj) =>
                {
                    try
                    {
                        byte[] data = File.ReadAllBytes((string)obj);
                        byte[] buffer = new byte[headSize + data.Length];
                        using (var ms = new MemoryStream(buffer))
                        {
                            var bw = new BinaryWriter(ms);
                            bw.Write((long)buffer.Length);
                            bw.Write((byte)2);
                            bw.Write((long)data.Length);
                            bw.Write(data, 0, data.Length);
                        }

                        for (int i = 0; i < 100; i++)
                        {
                            Loger.Error("发送资源数据开始");
                            _client.Send(buffer);
                            Loger.Error("发送资源数据完成");
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送资源数据异常");
                    }
                }, ofd.FileName);
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
                        int mtu = 4096;
                        using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            Loger.Error("发送资源数据开始");

                            byte[] buffer = new byte[mtu];
                            using (var ms = new MemoryStream(buffer))
                            {
                                var bw = new BinaryWriter(ms);
                                bw.Write((long)headSize);
                                bw.Write((byte)3);
                                bw.Write(fs.Length);
                            }
                            //_client.Send(buffer.Take(9).ToArray());
                            _client.Send(buffer, 0, headSize, SocketFlags.None);

                            long ep, p = 0;
                            int sendCount = 0;
                            while (p < fs.Length)
                            {
                                ep = fs.Length - p;
                                if (ep < mtu)
                                {
                                    mtu = (int)ep;
                                }

                                fs.Read(buffer, 0, mtu);
                                sendCount = 0;
                                try
                                {
                                    while (sendCount < mtu)
                                    {
                                        sendCount += _client.Send(buffer, sendCount, mtu, SocketFlags.None);
                                    }

                                    p += mtu;
                                }
                                catch (Exception exi)
                                {
                                    Loger.Error(exi, "发送资源数据异常");
                                }
                            }

                            Loger.Error("发送资源数据完成");
                        }
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

        private void comboBoxLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._logShowLevel = DropdownBoxHelper.GetEnumFromComboBox<LogLevel>(comboBoxLogLevel);
        }


    }
}
