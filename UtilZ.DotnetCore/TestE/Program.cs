using System;
using UtilZ.Dotnet.Ex.Log;

namespace TestE
{
    class Program
    {
        static void Main(string[] args)
        {
            RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Content); }, null));

            TestDBAccess.Test();
            Console.WriteLine("any key exist...");
            Console.ReadKey();
        }
    }
}
