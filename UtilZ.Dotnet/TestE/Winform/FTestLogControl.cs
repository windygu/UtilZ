using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ;

namespace TestE.Winform
{
    public partial class FTestLogControl : Form
    {
        public FTestLogControl()
        {
            InitializeComponent();
        }

        private Stopwatch _watch;
        private void FTestLogControl_Load(object sender, EventArgs e)
        {
            logControl1.MaxItemCount = 10;
            checkBox1.Checked = logControl1.IsLock;

            var sub = new UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ.SubscibeItem("123");
            sub.MessageNotify = this.CompletedNotify;
            UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ.LMQCenter.Subscibe(sub);
            logControl1.StartRefreshLogThread(5, 15000);
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
            logControl1.AddLogStyleForColor(string.Format("{0}_{1}_safsdf", DateTime.Now, _index++), Color.Red);
            logControl1.AddLogStyleForClass(string.Format("{0}_{1} Error", DateTime.Now, _index++), "error");
            logControl1.AddLogStyleForClass(string.Format("{0}_{1} Warn", DateTime.Now, _index++), "warn");
            logControl1.AddLogStyleForClass(string.Format("{0}_{1} Info", DateTime.Now, _index++), "info");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            logControl1.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            logControl1.IsLock = checkBox1.Checked;
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            //logControl1.SetTemplate(@"LogControlTemplate1.html", "bid", "p");
            logControl1.SetTemplate(@"LogControlTemplate2.html", "uid", "li");
        }

        private void btnPer_Click(object sender, EventArgs e)
        {
            logControl1.MaxItemCount = 10050;
            Task.Factory.StartNew(() =>
            {
                _watch = Stopwatch.StartNew();
                for (int i = 0; i < 100000; i++)
                {
                    logControl1.AddLogStyleForColor(string.Format("{0} Hello Word{1}", DateTime.Now, _index++), Color.Red);
                }

                logControl1.AddLogStyleForColor(string.Format("{0}_{1} Faltal", DateTime.Now, _index++), Color.Green);
            });
        }
    }
}
