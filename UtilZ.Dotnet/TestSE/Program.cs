using System;
using System.IO;
using System.Text;
using UtilZ.Dotnet.SEx.Log;
using UtilZ.Dotnet.SEx.Log.Appender;
using UtilZ.Dotnet.SEx.Log.Config;

namespace TestSE
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var xxxxx = System.Threading.Thread.GetDomain().FriendlyName;
                //string appenderName = "a";
                //var ret = string.Equals(appenderName, "abc");

                //*MyDocuments*
                //string firstPath = "*MyDocuments*";
                //string wildcardStr = firstPath.Substring(1, firstPath.Length - 2);
                int a = 123;


                //string logRootFullPath = @"G:\Tmp\test\123.log";
                //var isRelativePath = string.IsNullOrWhiteSpace(Path.GetPathRoot(logRootFullPath));


                //string srcRelativePath = @"test\123.log";
                //string srcAbsolutePath = Path.GetFullPath(srcRelativePath);
                //string rootDir = srcAbsolutePath.Substring(0,srcAbsolutePath.Length - srcRelativePath.Length);


                //string logFilePath = @"G:\Tmp\test\124.log";
                //string parentDir = Directory.GetParent(Path.GetDirectoryName(logRootFullPath)).FullName;
                //if (logFilePath.Length < parentDir.Length)
                //{

                //}

                //logFilePath = logFilePath.Substring(parentDir.Length);

                //string[] allDirs = Directory.GetDirectories(logRootFullPath, "*.*", SearchOption.AllDirectories);
                //var dirInfos = new DirectoryInfo(logRootFullPath).GetDirectories("*.*", SearchOption.AllDirectories);

                //Array array = Enum.GetValues(typeof(Environment.SpecialFolder));//特殊目录集合
                //StringBuilder sb = new StringBuilder();
                //foreach (var item in array)
                //{
                //    sb.AppendLine(item.ToString());
                //}

                //File.WriteAllText("SpecialFolder.txt", sb.ToString());

                //Console.WriteLine("ane key continue...");
                //Console.ReadKey();

                Loger.LoadConfig(@"logconfig.xml");

                var subItem = new RedirectOutputSubscribeItem("redirectAppender");
                subItem.LogOutput += SubItem_LogOutput;
                RedirectOuputCenter.Add(subItem);

                //string format = @"yyyy-MM-dd_HH_mm_ss.fffffff";
                //string timeStr = DateTime.Now.ToString(format);

                //DateTime time;
                //bool xx = DateTime.TryParseExact(timeStr, format, null, System.Globalization.DateTimeStyles.None, out time);


                LogSysInnerLog.Log += LogSysInnerLog_Log;
                Loger.Debug("dsfa", 10);

                //Loger.Debug("dsfa", 100);
                ILoger loger = Loger.GetLoger(null);
                loger.AddAppender(new FileAppender(new FileAppenderConfig(null) { FilePath = @"Log2\*yyyy-MM-dd_HH_mm_ss.fffffff*.log" }));
                loger.Error("新版本");
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

        private static void LogSysInnerLog_Log(object sender, InnerLogOutputArgs e)
        {
            try
            {
                Console.WriteLine(e.Ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
