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
                Application.DoEvents();
            }, "sadf", true, false);

            _thread = new ThreadEx((token) =>
            {
                bool ret;
                while (!token.IsCancellationRequested)
                {
                    ret = _asynQueue.Enqueue(_index++, 10);
                    Thread.Sleep(10);
                }
            }, "sadfcv", true);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (_thread.IsRuning)
            {
                _thread.Stop();
            }
            else
            {
                _thread.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //_asynQueue.Dispose();
            //_thread.Dispose();
            //if (_thread.IsRuning)
            //{
            //    _thread.Stop();
            //}
            //else
            //{
            //    _thread.Start();
            //}

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                if (_asynQueue.Status)
                {
                    _asynQueue.Stop(false);
                }
                else
                {
                    _asynQueue.Start();
                }
            });
        }
    }
}
