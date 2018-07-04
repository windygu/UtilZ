using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.LogOutput;

namespace TestE.Common
{
    public partial class FTestAsynQueue : Form
    {
        public FTestAsynQueue()
        {
            InitializeComponent();
        }

        private AsynQueue<int> _asynQueue;
        private ThreadEx _thread;
        private int _index = 0;
        private void FTestAsynQueue_Load(object sender, EventArgs e)
        {
            _asynQueue = new AsynQueue<int>((i) =>
            {
                Thread.Sleep(100);

                this.Invoke(new Action(() =>
                {
                    textBox1.Text = i.ToString();
                }));

                if (_asynQueue.Count > 10)
                {
                    var removeItems = _asynQueue.Remove((t) => { return t % 10 > 4; });
                    var removeCount = removeItems.Count();
                    if (removeCount > 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = string.Format("移除:{0}项", removeCount);
                        }));

                        Thread.Sleep(2000);
                    }
                }

                Application.DoEvents();
            }, "消费者线程", true, false);

            _thread = new ThreadEx((token) =>
            {
                bool ret;
                while (!token.IsCancellationRequested)
                {
                    ret = _asynQueue.Enqueue(_index++);
                    Thread.Sleep(20);
                }
            }, "生产者线程", true);

            var subLog = new LogOutputSubscribeItem(null, null);
            subLog.LogOutput += SubLog_LogOutput;
            Loger.LogOutput.AddLogOutput(subLog);
            Loger.LogOutput.Enable = true;
        }

        private void SubLog_LogOutput(object sender, UtilZ.Dotnet.Ex.Log.Model.LogOutputArgs e)
        {
            this.Invoke(new Action(() =>
            {
                //logControl1.AddLogStyleForColor(e.Item.Content, Color.Gray);
                logControl1.AddLog(e.Item.Content);
            }));
        }

        private void btnPro_Click(object sender, EventArgs e)
        {
            if (_thread.IsRuning)
            {
                _thread.Stop();
                btnPro.Text = "已停止生产";
            }
            else
            {
                _thread.Start();
                btnPro.Text = "正在生产";
            }
        }

        private void btnCons_Click(object sender, EventArgs e)
        {
            bool isAbort = checkBoxStopCons.Checked;
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                if (_asynQueue.Status)
                {
                    _asynQueue.Stop(isAbort);
                    this.Invoke(new Action(() => { btnCons.Text = "已停止消费"; }));
                }
                else
                {
                    _asynQueue.Start();
                    this.Invoke(new Action(() => { btnCons.Text = "正在消费"; }));
                }
            });
        }
    }
}
