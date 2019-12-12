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
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace WinFormA
{
    public partial class FUdp : Form
    {
        public FUdp()
        {
            InitializeComponent();
        }

        private LogLevel _logShowLevel = LogLevel.Trace;
        private readonly ITransferNet _transfer = new UdpTransferNet();
        private AsynQueue<ReceiveDatagramInfo> _proBufferQueue;
        private const int headSize = 17;

        private byte[] _data = null;
        private Stream _stream = null;
        private long _packageLen = 0;
        private string _filePath = null;
        private long _dataLen = 0;

        private void FUdp_Load(object sender, EventArgs e)
        {
            var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, "RedirectToUI");
            if (redirectAppenderToUI != null)
            {
                redirectAppenderToUI.RedirectOuput += RedirectOuput;
            }
            DropdownBoxHelper.BindingEnumToComboBox<LogLevel>(comboBoxLogLevel, this._logShowLevel);
            _proBufferQueue = new AsynQueue<ReceiveDatagramInfo>(ProRevBuffer, "处理接收到数据队列", true, true);
            this.FormClosing += FTcp_FormClosing;
        }

        private void ProRevBuffer(ReceiveDatagramInfo item)
        {
            try
            {
                byte[] buffer = item.Data;
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
                                return;
                                //_data = new byte[_dataLen];
                                //_stream = new MemoryStream(_data);
                                break;
                            case 3:
                                Loger.Info($"文件长度:{_dataLen}");
                                return;
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
                        //string filePath = $@"E:\Tmp\{DateTime.Now.ToFileTime()}.dat";
                        //Loger.Trace($"收到数据内容,写入文件{filePath}");
                        //File.WriteAllBytes(filePath, _data);
                        //Loger.Trace($"收到数据内容,写入文件{filePath}完成");
                        Loger.Trace("收到数据内容");
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

        private void FTcp_FormClosing(object sender, FormClosingEventArgs e)
        {
            _transfer.Dispose();
            _proBufferQueue.Dispose();
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

        private IPEndPoint _remotEP;
        private void btnInit_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = txtDataDir.Text;
                if (string.IsNullOrWhiteSpace(dir))
                {
                    Loger.Error("数据存在目录不能为空");
                    return;
                }

                if (!Directory.Exists(dir))
                {
                    Loger.Error("数据存在目录不存在");
                    return;
                }

                IPAddress sendIpAddr;
                if (!IPAddress.TryParse(txtSendIp.Text, out sendIpAddr))
                {
                    Loger.Error("发送端IP无效");
                    return;
                }

                IPAddress dstIpAddr;
                if (!IPAddress.TryParse(txtDstIp.Text, out dstIpAddr))
                {
                    Loger.Error("目的地IP无效");
                    return;
                }

                //this._transfer = new TransferCore(this._config, new IPEndPoint(IPAddress.Any, (int)numSrcPort.Value), new IPEndPoint(dstIpAddr, (int)numDstPort.Value), this.Rev);
                this._remotEP = new IPEndPoint(dstIpAddr, (int)numDstPort.Value);
                var config = new NetConfig();
                config.ListenEP = new IPEndPoint(sendIpAddr, (int)numSrcPort.Value);
                this._transfer.Init(config, this.Rev);
                Loger.Info("启动成功...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "程序启动失败");
            }
        }

        private void Rev(ReceiveDatagramInfo obj)
        {
            _proBufferQueue.Enqueue(obj);
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
                _transfer.Send(buffer, this._remotEP);
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

                byte[] data = File.ReadAllBytes(ofd.FileName);
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        byte[] buffer = new byte[headSize + data.Length];
                        using (var ms = new MemoryStream(buffer))
                        {
                            var bw = new BinaryWriter(ms);
                            bw.Write((long)buffer.Length);
                            bw.Write((byte)2);
                            bw.Write((long)data.Length);
                            bw.Write(data, 0, data.Length);

                            Loger.Error("发送资源数据开始");
                            int mtu = 64512;
                            byte[] sendBuf = new byte[mtu];
                            long ep;
                            ms.Seek(0, SeekOrigin.Begin);
                            while (ms.Position < buffer.Length)
                            {
                                ep = buffer.Length - ms.Position;
                                if (ep < sendBuf.Length)
                                {
                                    sendBuf = new byte[ep];
                                }

                                ms.Read(sendBuf, 0, sendBuf.Length);
                                _transfer.Send(sendBuf, this._remotEP);
                            }
                        }

                        Loger.Error("发送资源数据完成");
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
                        int mtu = 64512;
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
                            _transfer.Send(buffer.Take(headSize).ToArray(), this._remotEP);

                            long ep, p = 0;
                            while (p < fs.Length)
                            {
                                ep = fs.Length - p;
                                if (ep < mtu)
                                {
                                    mtu = (int)ep;
                                    buffer = new byte[mtu];
                                }

                                fs.Read(buffer, 0, mtu);
                                _transfer.Send(buffer, this._remotEP);
                                p += mtu;
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
