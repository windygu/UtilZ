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
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace WinFormA
{
    public partial class FUdpNet : Form
    {
        private TransferConfig _config;
        private TransferChannel _transfer;
        private LogLevel _logShowLevel = LogLevel.Info;

        public FUdpNet()
        {
            InitializeComponent();
        }

        private void FUdpNet_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                const int maxLogCount = 100000;
                logControl.MaxItemCount = maxLogCount;
                logControl.SetLogRefreshInfo(3, maxLogCount);
                var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, "RedirectToUI");
                if (redirectAppenderToUI != null)
                {
                    redirectAppenderToUI.RedirectOuput += RedirectOuput;
                }
                this._config = new TransferConfig();
                numParallelThreadCount.Maximum = TransferConstant.PARALLEL_THREAD_MAX_COUN;
                checkBoxLockLog.Checked = logControl.IsLock;
                DropdownBoxHelper.BindingEnumToComboBox<LogLevel>(comboBoxLogLevel, this._logShowLevel);
                this.SetButtonEnable(true);
                this.FormClosing += FA_FormClosing;
            }
            catch(Exception ex)
            {

            }
        }

        private void FA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._transfer != null)
            {
                _transfer.Dispose();
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

        private void SetButtonEnable(bool enable)
        {
            btnInit.Enabled = enable;
            btnSendText.Enabled = !enable;
            btnSendData.Enabled = btnSendText.Enabled;
            btnSendFile.Enabled = btnSendText.Enabled;
            //numParallelThreadCount.Enabled = btnSendText.Enabled;
        }

        private void btnChoiceDataDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtDataDir.Text = fbd.SelectedPath;
        }

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

                IPAddress dstIpAddr;
                if (!IPAddress.TryParse(txtDstIp.Text, out dstIpAddr))
                {
                    Loger.Error("目的地IP无效");
                    return;
                }

                this._config.LocalFileDirectory = dir;

                var remoteEP = new IPEndPoint(dstIpAddr, (int)numDstPort.Value);
                int timeout = (int)numTimeout.Value;
                string[] priorityStrs = rtxtMsg.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                short priority;
                var policys = new List<TransferPolicy>();
                foreach (var priorityStr in priorityStrs)
                {
                    if (short.TryParse(priorityStr, out priority))
                    {
                        policys.Add(new TransferPolicy(remoteEP, priority, timeout));
                    }
                }

                DropdownBoxHelper.BindingIEnumerableGenericToComboBox<TransferPolicy>(comPri, policys, nameof(TransferPolicy.Priority));
                rtxtMsg.Text = string.Empty;

                this._config.ParseDataMaxThreadCount = this._config.TransferThreadCount + 2;
                //this._config.NetConfig.ListenEP = new IPEndPoint(IPAddress.Any, (int)numSrcPort.Value);
                this._config.NetConfig.ListenEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)numSrcPort.Value);
                //this._config.NetConfig.Protocal = TransferProtocal.Tcp;
                this._transfer = new TransferChannel(this._config, this.Rev);
                this._transfer.Start();
                this.SetButtonEnable(false);
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
                    if (data.Length > 1024)
                    {
                        bool isWriteFile = false;
                        this.Invoke(new Action(() =>
                        {
                            isWriteFile = cbDataWriteFile.Checked;
                        }));

                        if (isWriteFile)
                        {
                            //文件
                            string filePath = Path.Combine(this._config.LocalFileDirectory, $@"{DateTime.Now.ToFileTime()}.dat");
                            Loger.Info($"收到数据内容,写入文件{filePath}");
                            File.WriteAllBytes(filePath, data);
                            Loger.Info($"收到数据内容,写入文件{filePath}完成");
                        }
                        else
                        {
                            Loger.Info("收到数据内容");
                        }
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

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logControl.Clear();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        private void numParallelThreadCount_ValueChanged(object sender, EventArgs e)
        {
            _config.TransferThreadCount = (byte)numParallelThreadCount.Value;
        }

        private void comboBoxLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._logShowLevel = DropdownBoxHelper.GetEnumFromComboBox<LogLevel>(comboBoxLogLevel);
        }

        private void checkBoxLockLog_CheckedChanged(object sender, EventArgs e)
        {
            logControl.IsLock = checkBoxLockLog.Checked;
        }

        private TransferPolicy GetPolicy()
        {
            return DropdownBoxHelper.GetGenericFromComboBox<TransferPolicy>(comPri);
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
                Task.Factory.StartNew((obj) =>
                {
                    try
                    {
                        Loger.Trace("发送消息数据开始");
                        this._transfer.SendData(data, (TransferPolicy)obj);
                        Loger.Trace("发送消息数据完成");
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送消息数据异常");
                    }
                }, this.GetPolicy());
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "发送文本消息异常");
            }
        }

        private int GetSendCount()
        {
            int sendCount = 0;
            this.Invoke(new Action(() => { sendCount = (int)numSendCount.Value; }));
            return sendCount;
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
                        byte[] data = File.ReadAllBytes(ofd.FileName);
                        int sendCount = GetSendCount();
                        for (int i = 0; i < sendCount; i++)
                        {
                            Loger.Error($"发送资源数据开始[{i + 1}]");
                            this._transfer.SendData(data, (TransferPolicy)obj);
                            Loger.Error("发送资源数据完成");
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送资源数据异常");
                    }
                }, this.GetPolicy());
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

                Task.Factory.StartNew((obj) =>
                {
                    try
                    {
                        int sendCount = GetSendCount();
                        for (int i = 0; i < sendCount; i++)
                        {
                            Loger.Error($"发送文件开始[{i + 1}]");
                            this._transfer.SendFile(ofd.FileName, (TransferPolicy)obj);
                            Loger.Error("发送文件完成");
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送文件异常");
                    }
                }, this.GetPolicy());
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "发送文件异常");
            }
        }

        private void btnStream_Click(object sender, EventArgs e)
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
                        int sendCount = GetSendCount();
                        for (int i = 0; i < sendCount; i++)
                        {
                            Loger.Error($"发送文件开始[{i + 1}]");
                            using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                this._transfer.SendData(fs, 0, fs.Length, (TransferPolicy)obj);
                            }
                            Loger.Error("发送文件完成");
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, "发送文件异常");
                    }
                }, this.GetPolicy());
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "发送文件异常");
            }
        }

        private void comPri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
