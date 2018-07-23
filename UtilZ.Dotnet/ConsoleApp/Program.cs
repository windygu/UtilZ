using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Model;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var subLog = new UtilZ.Dotnet.Ex.Log.LogOutput.LogOutputSubscribeItem(null, null);
            //subLog.LogOutput += SubLog_LogOutput;
            //Loger.LogOutput.AddLogOutput(subLog);
            //Loger.LogOutput.Enable = true;


            //TelnetServer ts = new TelnetServer(IPAddress.Parse("0.0.0.0"), 14002, "测试服务", ProCallback, 3);
            //ts.Start();

            TestArray64();

            Console.WriteLine("Press any key exit");
            Console.ReadKey();
        }

        private static void TestArray64()
        {
            try
            {
                Array64<byte> array = new Array64<byte>(102, 20, 5, 4);
                long beginIndex = 0;
                byte[] buffer = new byte[5];
                array.Set(beginIndex, buffer, buffer.Length);
                beginIndex += buffer.Length;

                buffer = new byte[34];
                array.Set(beginIndex, buffer, buffer.Length);
                beginIndex += buffer.Length;

                buffer = new byte[50];
                array.Set(beginIndex, buffer, buffer.Length);
                beginIndex += buffer.Length;

                buffer = new byte[3];
                array.Set(beginIndex, buffer, buffer.Length);
                beginIndex += buffer.Length;
            }
            catch(Exception ex)
            {

            }
        }

        private static void SubLog_LogOutput(object sender, UtilZ.Dotnet.Ex.Log.Model.LogOutputArgs e)
        {
            string str;
            try
            {
                str = string.Format("{0} {1}", DateTime.Now, e.Item.Content);
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }

            Console.WriteLine(str);
        }

        private static void TestFtpUrl()
        {
            string regStr = @"^[f,F]{1}[t,T]{1}[p,P]{1}://(?<ip>" + RegexConstant.IPV4Reg + "):" + @"(?<port>" + RegexConstant.Port + ")/";
            regStr = RegexConstant.FtpUrl;
            TestFtpUrl(@"ftp://0.0.0.0:65530/ori", regStr);
            TestFtpUrl(@"ftp://255.255.255.255:65533/ori", regStr);
            TestFtpUrl(@"ftp://250.253.255.200:65535/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:65536/ori", regStr);

            TestFtpUrl(@"ftp://10.99.55.0:65500/ori", regStr);
            TestFtpUrl(@"ftp://9.99.1.199:65526/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:65529/ori", regStr);

            TestFtpUrl(@"ftp://0.0.0.0:65400/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:65499/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:65002/ori", regStr);

            TestFtpUrl(@"ftp://0.0.0.0:64000/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:64259/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:64999/ori", regStr);

            TestFtpUrl(@"ftp://0.0.0.0:50000/", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:59999", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:51234/`~2134$%^/abc/sadf/sdafg/fffg", regStr);

            TestFtpUrl(@"ftp://0.0.0.0:1/ori/134$%^/abc/sadf/sdafg/fffg/", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:1000/ori", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:9999/ori/sadfb/", regStr);
            TestFtpUrl(@"ftp://0.0.0.0:5134/ori/abc", regStr);

            TestFtpUrl(@"ftp://0.0.0.0/ori", regStr);
        }

        private static void TestFtpUrl(string ftpUrl, string regStr)
        {
            if (string.IsNullOrWhiteSpace(ftpUrl))
            {
                throw new ArgumentNullException("ftpUrl");
            }

            Console.WriteLine(ftpUrl);
            try
            {
                new FtpEx(ftpUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------------");
            return;
            ftpUrl = ftpUrl.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);


            Match match = Regex.Match(ftpUrl, regStr);
            if (match.Success)
            {
                Console.WriteLine("IP:{0,-15},Port:{1,-5},Dir:{2}", match.Groups["ip"].Value, match.Groups["port"].Value, match.Groups["dir"].Value);
            }
            else
            {
                Console.WriteLine("匹配失败,{0}", ftpUrl);
            }

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------------");
        }


        private static void TestPortReg()
        {
            string regStr = @"(?<port>" + RegexConstant.Port + ")";

            TestPortReg(@"ftp://0.0.0.0:65530/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65533/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65535/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65536/ori", regStr);

            TestPortReg(@"ftp://0.0.0.0:65500/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65526/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65529/ori", regStr);

            TestPortReg(@"ftp://0.0.0.0:65400/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65499/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:65002/ori", regStr);

            TestPortReg(@"ftp://0.0.0.0:64000/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:64259/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:64999/ori", regStr);

            TestPortReg(@"ftp://0.0.0.0:50000/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:59999/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:51234/ori", regStr);

            TestPortReg(@"ftp://0.0.0.0:1/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:1000/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:9999/ori", regStr);
            TestPortReg(@"ftp://0.0.0.0:5134/ori", regStr);

        }
        private static void TestPortReg(string inputStr, string regStr)
        {
            Match match = Regex.Match(inputStr, regStr);
            if (match.Success)
            {
                Console.WriteLine("Port:{0,-5}", match.Groups["port"].Value);
            }
            else
            {
                Console.WriteLine("匹配失败,{0}", inputStr);
            }
        }

        private static void TestIPReg()
        {
            string regStr = @"(?<ip>" + RegexConstant.IPV4Reg + ")";
            TestIPReg(@"ftp://0.0.0.0/ori", regStr);
            TestIPReg(@"ftp://255.255.255.255/ori", regStr);


            TestIPReg(@"ftp://250.253.255.200/ori", regStr);
            TestIPReg(@"ftp://249.100.199.159/ori", regStr);
            TestIPReg(@"ftp://10.99.55.0/ori", regStr);
            TestIPReg(@"ftp://9.99.1.199/ori", regStr);
        }

        private static void TestIPReg(string inputStr, string regStr)
        {
            Match match = Regex.Match(inputStr, regStr);
            if (match.Success)
            {
                Console.WriteLine("IP:{0,-15}", match.Groups["ip"].Value);
            }
            else
            {
                Console.WriteLine("匹配失败,{0}", inputStr);
            }
        }



        private static void TestTelnet()
        {
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            //IPAddress ip = IPAddress.Parse("192.168.0.101");
            IPAddress ip = IPAddress.Parse("0.0.0.0");
            IPEndPoint serverEP = new IPEndPoint(ip, 14002);
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Bind(serverEP);
            sck.Listen(443);

            try
            {
                Console.WriteLine("Listening for clients...");
                Socket client = sck.Accept();

                while (true)
                {
                    // Send a welcome greet
                    byte[] buffer = Encoding.Default.GetBytes("Welcome to the server of Kobernicus!!");
                    client.Send(buffer, 0, buffer.Length, 0);
                    buffer = new byte[255];

                    // Read the sended command
                    int rec = client.Receive(buffer, 0, buffer.Length, 0);
                    byte[] bufferReaction = Encoding.Default.GetBytes(rec.ToString());

                    // Run the command
                    Process prcsCMD = new Process();
                    prcsCMD.StartInfo.FileName = bufferReaction.ToString();
                    prcsCMD.StartInfo.UseShellExecute = false;
                    prcsCMD.StartInfo.Arguments = string.Empty;
                    prcsCMD.StartInfo.RedirectStandardOutput = true;
                    prcsCMD.Start();

                    string output = prcsCMD.StandardOutput.ReadToEnd();
                    byte[] cmdOutput = Encoding.Default.GetBytes(output);
                    client.Send(cmdOutput, 0, cmdOutput.Length, 0);
                    cmdOutput = new byte[255];
                }
                sck.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
