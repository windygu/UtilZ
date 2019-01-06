using System;
using System.IO;
using UtilZ.Dotnet.Ex.Log;

namespace TestE
{
    class Program
    {
        static void Main(string[] args)
        {
            RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));
            Loger.Info("Hello!");

            //Console.WriteLine("continue...");
            //Console.ReadKey();

            Loger.Info("Word!");

            //TestDBAccess.Test();

            //while (true)
            //{
            //    Loger.Info("//RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));//RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));//RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));");
            //    if (Directory.GetFiles("Log").Length > 10)
            //    {
            //        break;
            //    }
            //}

            //ILoger loger = Loger.GetLoger("");

            //loger.Trace("", 1, 2, 3, 4);

            //loger.Trace(2, new Program(), "", 1, 2, 3, 4, 5);

            //loger.Trace(new Exception());
            //loger.Trace(new Exception(), 1);
            //loger.Trace(new Exception(), 1, new Program());

            //loger.Trace(new Exception(), "", 1, 2, 3, 4);

            //loger.Trace(1, new Program(), new Exception(), "", 1, 2, 3, 4);



            Console.WriteLine("any key exist...");
            Console.ReadKey();
        }
    }
}
