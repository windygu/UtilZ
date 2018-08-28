﻿using System;
using System.IO;
using System.Text;
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
                //Array array = Enum.GetValues(typeof(Environment.SpecialFolder));//特殊目录集合
                //StringBuilder sb = new StringBuilder();
                //foreach (var item in array)
                //{
                //    sb.AppendLine(item.ToString());
                //}

                //File.WriteAllText("SpecialFolder.txt", sb.ToString());

                Console.WriteLine("ane key continue...");
                Console.ReadKey();

                Loger.LoadConfig(@"logconfig.xml");

                var subItem = new RedirectOutputSubscribeItem();
                subItem.LogOutput += SubItem_LogOutput;
                RedirectOuputCenter.Instance.AddLogOutput(subItem);

                //string format = @"yyyy-MM-dd_HH_mm_ss.fffffff";
                //string timeStr = DateTime.Now.ToString(format);

                //DateTime time;
                //bool xx = DateTime.TryParseExact(timeStr, format, null, System.Globalization.DateTimeStyles.None, out time);


              

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
                Console.WriteLine("SubItem_LogOutput:" + e.Item.Content);
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
