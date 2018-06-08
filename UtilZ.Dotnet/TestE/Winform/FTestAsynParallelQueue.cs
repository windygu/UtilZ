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
using UtilZ.Dotnet.Ex.DataStruct;

namespace TestE.Winform
{
    public partial class FTestAsynParallelQueue : Form
    {
        public FTestAsynParallelQueue()
        {
            InitializeComponent();
        }

        private AsynQueue<List<string>> _retShowQueue;
        private AsynParallelQueue<int, string> _apQueue;
        private ThreadEx _createThread;
        private void FTestAsynParallelQueue_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this._apQueue = new AsynParallelQueue<int, string>(Pro, ProResult, 4, 10, true);
            this._retShowQueue = new AsynQueue<List<string>>(this.ProShow, "结果显示线程", true, true);
            this._createThread = new ThreadEx(this.Create, "生产线程", true);
        }

        private readonly Random _rnd = new Random();
        private void Create(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                this._apQueue.Enqueue(_rnd.Next(1, 100));
            }
        }

        private void ProShow(List<string> rets)
        {
            string ret = string.Join("\r\n", rets);
            try
            {
                Thread.Sleep(1000);
                this.Invoke(new Action(() =>
                {
                    richTextBox1.Text = ret;
                }));
                Application.DoEvents();
            }
            catch (ObjectDisposedException)
            {

            }
        }

        private string Pro(int p, CancellationToken token)
        {
            Thread.SpinWait(_rnd.Next(5, 15));
            return string.Format("ThreadId:{0};Value:{1}", Thread.CurrentThread.ManagedThreadId, p * 10);
        }

        private void ProResult(List<string> rets)
        {
            this._retShowQueue.Enqueue(rets);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (this._createThread.IsRuning)
            {
                this._createThread.Stop();
                btnTest.Text = "开始";
            }
            else
            {
                this._createThread.Start();
                btnTest.Text = "停止";
            }
        }
    }
}
