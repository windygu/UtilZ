using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.SHPAgentBLL;
using UtilZ.Dotnet.SHPBase.ServiceBasic;

namespace WinFormB
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //    var para = new ServiceInsListenInfo();
                //    para.ListenPort = 123;
                //    var str = RestFullClientHelper.Post("http://192.168.10.96:20011/QueryRouteServiceUrl/para", para);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
