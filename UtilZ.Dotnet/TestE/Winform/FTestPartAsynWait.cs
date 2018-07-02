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
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace TestE.Winform
{
    public partial class FTestPartAsynWait : Form
    {
        public FTestPartAsynWait()
        {
            InitializeComponent();
        }

        private readonly List<CobITemInfo> items = new List<CobITemInfo>();
        private void FTestPartAsynWait_Load(object sender, EventArgs e)
        {
            items.Add(new CobITemInfo("yf", 23));
            items.Add(new CobITemInfo("zhn", 31));
            items.Add(new CobITemInfo("tq", 18));
            DropdownBoxHelper.BindingIEnumerableGenericToComboBox<CobITemInfo>(comboBox1, items);
            //DropdownBoxHelper.BindingIEnumerableGenericToComboBox<CobITemInfo>(comboBox1, items, string.Empty);
            //DropdownBoxHelper.BindingIEnumerableGenericToComboBox<CobITemInfo>(comboBox1, t => { return t.Name; }, items);

            //DropdownBoxHelper.BindingIEnumerableGenericToToolStripComboBox<CobITemInfo>(toolStripComboBox1, items, nameof(CobITemInfo.Name));
            //DropdownBoxHelper.BindingIEnumerableGenericToToolStripComboBox<CobITemInfo>(toolStripComboBox1, items);
            DropdownBoxHelper.BindingIEnumerableGenericToToolStripComboBox<CobITemInfo>(toolStripComboBox1, t => { return t.Name; }, items);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var para = new PartAsynWaitPara<int, string>();
            para.Para = 10;
            para.Function = (inp) =>
            {
                for (int i = 0; i < inp.Para; i++)
                {
                    inp.AsynWait.Hint = string.Format("正在处理:{0}项..", i);
                    Thread.Sleep(500);
                    if (inp.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    if (i > 5)
                    {
                        throw new NotSupportedException("XXX");
                    }
                }

                return "OK";
            };
            para.IsShowCancel = true;
            para.AsynWaitBackground = Color.Red;
            para.Completed = (p) =>
            {
                string str;
                switch (p.Status)
                {
                    case PartAsynExcuteStatus.Completed:
                        str = p.Result;
                        break;
                    case PartAsynExcuteStatus.Exception:
                        str = p.Exception.Message;
                        break;
                    case PartAsynExcuteStatus.Cancel:
                        str = "Cancel";
                        break;
                    default:
                        str = "none";
                        break;
                }

                MessageBox.Show(this, str);
            };

            PartAsynWaitHelper.Wait(para, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DropdownBoxHelper.SetGenericToComboBox(comboBox1, items[1]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = DropdownBoxHelper.GetGenericFromComboBox<CobITemInfo>(comboBox1).Name;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DropdownBoxHelper.SetGenericToToolStripComboBox(toolStripComboBox1, items[1]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = DropdownBoxHelper.GetGenericFromToolStripComboBox<CobITemInfo>(toolStripComboBox1).Name;
        }
    }

    public class CobITemInfo
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public CobITemInfo(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return string.Format("Name:{0},Age:{1}", Name, Age);
        }
    }
}
