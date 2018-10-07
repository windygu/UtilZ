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

            TestDBAccess.Test();

            //while (true)
            //{
            //    Loger.Info("//RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));//RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));//RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));");
            //    if (Directory.GetFiles("Log").Length > 10)
            //    {
            //        break;
            //    }
            //}
            Console.WriteLine("any key exist...");
            Console.ReadKey();
        }
    }
}
