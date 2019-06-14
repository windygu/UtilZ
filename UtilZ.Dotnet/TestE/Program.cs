using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestE.Common;
using TestE.Winform;
using UtilZ.Dotnet.Ex.Log;

namespace TestE
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Loger.LoadConfig(@"logconfig.xml");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FTestLoger());
        }
    }
}
