using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.WindowEx.Base;

namespace TestE.Winform
{
    public partial class FTestLogControlF : Form
    {
        private readonly ThreadEx _thread;
        public FTestLogControlF()
        {
            InitializeComponent();

            cbIsLock.Checked = logControl1.IsLock;
            this.cbIsLock.CheckedChanged += new System.EventHandler(this.cbIsLock_CheckedChanged);
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

        private void FTestLogControlF_Load(object sender, EventArgs e)
        {
            //logControl1.SetStyle(LogLevel.Debug, Color.Gray, null, 0);
            //logControl1.SetStyle(LogLevel.Error, Color.Red, "", 0);
            //logControl1.SetStyle(LogLevel.Faltal, Color.Red, "", 0);
            //logControl1.SetStyle(LogLevel.Info, Color.WhiteSmoke, "", 0);
            //logControl1.SetStyle(LogLevel.Warn, Color.Yellow, null, 0);
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
    }
}
