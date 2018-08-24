using System;
using UtilZ.Dotnet.SEx.Log;
using UtilZ.Dotnet.SEx.Log.Core;

namespace TestSE
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ane key continue...");
                Console.ReadKey();

                LogSysInnerLog.Log += LogSysInnerLog_Log;
                Loger.Debug("dsfa");
                ILoger loger = Loger.GetLoger(null);
                loger.Error("sadfsdafdsf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static void LogSysInnerLog_Log(object sender, UtilZ.Dotnet.SEx.Log.Model.InnerLogOutputArgs e)
        {
            try
            {
                Console.Write(e.SEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
