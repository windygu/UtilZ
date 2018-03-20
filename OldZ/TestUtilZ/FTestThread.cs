using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UtilZ.Lib.Base.DataStruct.Threading;

namespace TestUtilZ
{
    public partial class FTestThread : Form
    {
        public FTestThread()
        {
            InitializeComponent();
        }

        private IThreadEx _ext = null;
        private void FTestThread_Load(object sender, EventArgs e)
        {
            //_ext = new ThreadEx(this.TestThreadMethod);
            _ext = new ThreadEx(this.TestThreadMethod2);
            _ext.Completed += _ext_Completed;
        }

        private void _ext_Completed(object sender, ThreadExCompletedArgs e)
        {
            ApendText(DateTime.Now.ToString() + " ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + " " + e.Type.ToString());
        }

        private void ApendText(string str)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => { ApendText(str); }));
            }
            else
            {
                if (richTextBox1.Lines.Count() > 50)
                {
                    richTextBox1.Clear();
                }

                richTextBox1.AppendText(str);
                richTextBox1.AppendText(Environment.NewLine);
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            //_ext = ThreadEx.Start(this.TestThreadMethod);
            _ext.Start("yf");
        }

        private void TestThreadMethod(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                ApendText(DateTime.Now.ToString() + " ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString());
                Thread.Sleep(500);
            }
        }

        private void TestThreadMethod2(CancellationToken token, object obj)
        {
            while (!token.IsCancellationRequested)
            {
                ApendText(DateTime.Now.ToString() + " " + obj.ToString() + " ThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString());
                Thread.Sleep(5000);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_ext != null)
            {
                _ext.Stop();
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (_ext != null)
            {
                _ext.Abort();
            }
        }
    }
}
