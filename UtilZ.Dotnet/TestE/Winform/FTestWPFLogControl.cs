using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.WindowEx.Base;

namespace TestE.Winform
{
    public partial class FTestWPFLogControl : Form
    {
        private readonly ThreadEx _thread;
        public FTestWPFLogControl()
        {
            InitializeComponent();
            this._thread = new ThreadEx(this.AddLog, null, true);
        }

        private void AddLog(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                logControl1.AddLog(string.Format("{0}_{1} Info asdfsdafsdafsdafdfgfdsgdfsg logControl1.AddStyle(LogLevel.Debug, System.Windows.Media.Colors.Gray" +
                    "sadf", DateTime.Now, _index++), LogLevel.Info);
                Thread.Sleep(30);
            }
        }
        private Stopwatch _watch;
        private void FTestWPFLogControl_Load(object sender, EventArgs e)
        {
            //logControl1.AddStyle(LogLevel.Debug, System.Windows.Media.Colors.Gray, "Vladimir Script", 15);
            //logControl1.AddStyle(LogLevel.Error, System.Windows.Media.Colors.Red, "Wide Latin", 13);
            //logControl1.AddStyle(LogLevel.Faltal, System.Windows.Media.Colors.Blue, "STXingkai", 15);
            //logControl1.AddStyle(LogLevel.Info, System.Windows.Media.Colors.Green, "", 13);
            //logControl1.AddStyle(LogLevel.Warn, System.Windows.Media.Colors.YellowGreen, "Nirmala UI", 15);

            logControl1.SetStyle(new LogShowStyle(LogLevel.Debug, System.Windows.Media.Colors.Gray));
            logControl1.SetStyle(new LogShowStyle(LogLevel.Error, System.Windows.Media.Colors.Red));
            logControl1.SetStyle(new LogShowStyle(LogLevel.Fatal, System.Windows.Media.Colors.Red));
            logControl1.SetStyle(new LogShowStyle(LogLevel.Info, System.Windows.Media.Colors.WhiteSmoke));
            logControl1.SetStyle(new LogShowStyle(LogLevel.Warn, System.Windows.Media.Colors.Yellow));

            //logControl1.MaxItemCount = 8;
            cbIsLock.Checked = logControl1.IsLock;

            var sub = new UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ.SubscibeItem("123");
            sub.MessageNotify = CompletedNotify;
            UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ.LMQCenter.Subscibe(sub);
        }

        private void CompletedNotify(SubscibeItem sub, object obj)
        {
            _watch.Stop();
            this.Invoke(new Action(() =>
            {
                textBox1.Text = _watch.Elapsed.TotalMilliseconds.ToString();
            }));
        }

        private int _index = 0;
        private void btnTest_Click(object sender, EventArgs e)
        {
            logControl1.AddLog(string.Format("{0}_{1} Debug", DateTime.Now, _index++), LogLevel.Debug);
            logControl1.AddLog(string.Format("{0}_{1} Error", DateTime.Now, _index++), LogLevel.Error);
            logControl1.AddLog(string.Format("{0}_{1} Faltal", DateTime.Now, _index++), LogLevel.Fatal);
            logControl1.AddLog(string.Format("{0}_{1} Info", DateTime.Now, _index++), LogLevel.Info);
            logControl1.AddLog(string.Format("{0}_{1} Warn", DateTime.Now, _index++), LogLevel.Warn);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            logControl1.AddLog(string.Format("{0}_{1} Debugsdafasdfsda sdag agfdgdsfg  private void btnThreadTest_Click(" +
                "object sender, EventArgs e) private void btnThreadTest_Click(object sender, EventArgs e)", DateTime.Now, _index++), LogLevel.Debug);
        }

        private void btnThreadTest_Click(object sender, EventArgs e)
        {
            if (this._thread.IsRuning)
            {
                this._thread.Stop();
            }
            else
            {
                this._thread.Start();
            }
        }

        private void cbIsLock_CheckedChanged(object sender, EventArgs e)
        {
            logControl1.IsLock = cbIsLock.Checked;
        }

        private void btnPer_Click(object sender, EventArgs e)
        {
            logControl1.MaxItemCount = 200;
            int count = 10000;
            logControl1.SetLogRefreshInfo(10, count + 10);
            Task.Factory.StartNew(() =>
            {
                _watch = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                {
                    logControl1.AddLog(string.Format("{0}_{1} Debugsdafasdfsda sdag agfdgdsfg  private void btnThreadTest_Click(" +
                        "object sender, EventArgs e) private void btnThreadTest_Click(object sender, EventArgs e)", DateTime.Now, _index++), LogLevel.Debug);
                }

                logControl1.AddLog(string.Format("{0}_{1} Faltal", DateTime.Now, _index++), LogLevel.Fatal);
            });
        }
    }
}
