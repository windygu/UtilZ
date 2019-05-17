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
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace WinFormA
{
    public partial class FA : Form
    {
        private TransferCore _transfer;
        private NetTransferConfig _config;
        private LogLevel _logShowLevel = LogLevel.Trace;
        public FA()
        {
            InitializeComponent();
        }

        private void FA_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            RedirectOuputCenter.Add(new RedirectOutputChannel(this.LogRedirectOuput, "RedirectToUI"));
            numParallelThreadCount.Maximum = NetTransferConstant.PARALLEL_THREAD_MAX_COUN;
            checkBoxLockLog.Checked = logControl.IsLock;
            DropdownBoxHelper.BindingEnumToComboBox<LogLevel>(comboBoxLogLevel, this._logShowLevel);
            this.SetButtonEnable(true);
            this.FormClosing += FA_FormClosing;
        }

        private void FA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._transfer != null)
            {
                _transfer.Dispose();
            }
        }

        private void LogRedirectOuput(RedirectOuputItem e)
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
            numParallelThreadCount.Enabled = btnSendText.Enabled;
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

                _config = new NetTransferConfig();
                numParallelThreadCount.Value = _config.ParallelThreadMaxCount;
                this._config.LocalFileDirectory = dir;
                //this._transfer = new TransferCore(this._config, new IPEndPoint(IPAddress.Any, (int)numSrcPort.Value), new IPEndPoint(dstIpAddr, (int)numDstPort.Value), this.Rev);
                this._transfer = new TransferCore(this._config, new IPEndPoint(sendIpAddr, (int)numSrcPort.Value), new IPEndPoint(dstIpAddr, (int)numDstPort.Value), this.Rev);
                //this._transfer.Start();
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
                        this._transfer.Send(data, 1000 * 2);
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
                        Loger.Error("发送资源数据开始");
                        this._transfer.Send(data, 1000 * 20);
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
                        Loger.Error("发送文件开始");
                        this._transfer.SendFile(ofd.FileName, 1000 * 1000);
                        Loger.Error("发送文件完成");
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

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Loger.Error("发送文件开始");
                        using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            this._transfer.Send(fs, 0, fs.Length, 1000 * 1000);
                        }
                        Loger.Error("发送文件完成");
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
            logControl.Clear();
        }

        private void numParallelThreadCount_ValueChanged(object sender, EventArgs e)
        {
            _config.ParallelThreadMaxCount = (byte)numParallelThreadCount.Value;
        }

        private void checkBoxLockLog_CheckedChanged(object sender, EventArgs e)
        {
            logControl.IsLock = checkBoxLockLog.Checked;
        }

        private void comboBoxLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._logShowLevel = DropdownBoxHelper.GetEnumFromComboBox<LogLevel>(comboBoxLogLevel);
        }

        private void numPackageRate_ValueChanged(object sender, EventArgs e)
        {
            this._config.DiscardPackageRate = (float)(numPackageRate.Value / 100);
        }

        private void btnPacketInfo_Click(object sender, EventArgs e)
        {
            string packageInfo = $"丢包:{this._config.TotalDiscardPackageCount}个,总收到包:{this._config.TotalPackageCount}个";
            logControl.AddLog(packageInfo, LogLevel.Error);
        }

        private void btnResetPacketInfo_Click(object sender, EventArgs e)
        {
            this._config.TotalDiscardPackageCount = 0;
            this._config.TotalPackageCount = 0;
        }
    }
}
