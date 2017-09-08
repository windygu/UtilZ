using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base.Extend;

namespace TestUtilZ
{
    public partial class FTest : Form
    {
        public FTest()
        {
            InitializeComponent();
        }

        private void FTest_Load(object sender, EventArgs e)
        {

        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            try
            {
                var a1 = AppDomain.CurrentDomain.GetAssemblies();
                string dllPath = @"F:\Projects\Zhanghn\UtilZ\UtilZ.Lib.Winform\bin\Debug\UtilZ.Lib.Base.dll";
                int handle = NExtendUnmanagedDll.LoadLibraryEx(dllPath);

                var a2 = AppDomain.CurrentDomain.GetAssemblies();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
