using System;
using System.IO;
using UtilZ.Dotnet.SEx.Log;
using UtilZ.Dotnet.SEx.Log.Core;
using UtilZ.Dotnet.SEx.Log.RedirectOuput;

namespace TestSE
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var subItem = new RedirectOutputSubscribeItem();
                subItem.LogOutput += SubItem_LogOutput;
                RedirectOuputCenter.Instance.AddLogOutput(subItem);

                //string format = @"yyyy-MM-dd_HH_mm_ss.fffffff";
                //string timeStr = DateTime.Now.ToString(format);

                //DateTime time;
                //bool xx = DateTime.TryParseExact(timeStr, format, null, System.Globalization.DateTimeStyles.None, out time);


                //Console.WriteLine("ane key continue...");
                //Console.ReadKey();

                LogSysInnerLog.Log += LogSysInnerLog_Log;
                Loger.Debug("dsfa");
                //ILoger loger = Loger.GetLoger(null);
                //loger.Error("sadfsdafdsf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Any key exit..");
            Console.ReadKey();
        }

        private static void SubItem_LogOutput(object sender, RedirectOuputArgs e)
        {
            try
            {
                Console.Write(e.Item.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void LogSysInnerLog_Log(object sender, UtilZ.Dotnet.SEx.Log.Model.InnerLogOutputArgs e)
        {
            try
            {
                Console.Write(e.SEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
