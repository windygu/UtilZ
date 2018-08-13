using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;

namespace TestE.Common
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
            logControlF1.AddLog(str, 1);
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
                Thread.Sleep(500);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_ext != null)
            {
                _ext.Stop(checkBoxSync.Checked);
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
