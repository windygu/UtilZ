using System;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Appender;
using UtilZ.Dotnet.Ex.Log.Config;
using Xunit;

namespace XUnitTestEx
{
    public class UnitTestLog
    {
        [Fact]
        public void TestDefault()
        {
            Loger.Info("Hello!");
        }

        [Fact]
        public void TestConfig()
        {
            Loger.LoadConfig("logconfig.xml");
            Loger.Info("Hello wwww!");
        }

        [Fact]
        public void TestManualAddAppender()
        {
            ILoger loger = Loger.GetLoger(null);
            loger.AddAppender(new FileAppender(new FileAppenderConfig(null) { FilePath = @"LogFix\abc.log" }));
            loger.AddAppender(new FileAppender(new FileAppenderConfig(null) { FilePath = @"Log3\*yyyy-MM-dd_HH_mm_ss.fffffff*.log" }));
            loger.Info("TestManualAddAppender");
        }

        [Fact]
        public void TestRedirectOuput()
        {
            Loger.LoadConfig("logconfig.xml");
            RedirectOuputCenter.Add(new RedirectOutputChannel((e) => { Console.WriteLine(e.Item.Format); }, "redirectAppender"));
            Loger.Info("TestRedirectOuput!");

        }
    }
}
