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
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.FileTransfer;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Model;

namespace ConsoleApp
{
    class Program
    {
        private static void TM()
        {
            SpinWait.SpinUntil(() => { return true; }, 100);
        }

        static void Main(string[] args)
        {
            //Thread tre = new Thread(TM);

            try
            {
                Task task = new Task(TM);
                task.Start();

                task.Wait();

                task.Start();
                task.Wait();

            }
            catch(Exception ex)
            {

            }

            //RedirectOuputCenter.Add(new RedirectOutputChannel(SubLog_LogOutput));


            //TelnetServer ts = new TelnetServer(IPAddress.Parse("0.0.0.0"), 14002, "测试服务", ProCallback, 3);
            //ts.Start();

            //TestArray64_basic();

            //TestArray64();

            //TestByteArray64();

            //TestConvertEx();
            string frt = "yyyy-MM-dd_HH_mm_ss.fffffff";
            string str = DateTime.Now.ToString(frt);

            TestMemoryMappedFile.Test();
            Console.WriteLine("Press any key exit");
            Console.ReadKey();
        }

        private static void TestConvertEx()
        {
            string str = "123";
            int a = ConvertEx.ToNumber<int>(str);

            str = "123xs";
            int b = ConvertEx.ToNumber<int>(str, 10);
        }


        private static void TestByteArray64()
        {
            try
            {
                string srcFilePath, destFilePath;
                srcFilePath = @"E:\Music\凡心大动.ape";
                srcFilePath = @"F:\Movie\黑客帝国\黑客帝国1_开始.RMVB";
                srcFilePath = @"F:\Movie\《加菲猫1》Garfield.2004.720p.BluRay.x264-AVS720-人人影视高清发布组\加菲猫Garfield.2004.720p.mkv";
                destFilePath = Path.Combine(@"G:\Tmp", Path.GetFileName(srcFilePath));

                using (Stream stream = File.OpenRead(srcFilePath))
                {
                    //var array = new ByteArray64(stream, 0, (long)(stream.Length * 0.6), int.MaxValue / 10);
                    var array = new ByteArray64(stream, 0, stream.Length);
                    int bufferSize = 1024 * 1024 * 10;
                    long offset = 0;
                    using (Stream fw = File.OpenWrite(destFilePath))
                    {
                        while (true)
                        {
                            try
                            {
                                byte[] buffer = array.Get(offset, bufferSize);
                                if (buffer.Length == 0)
                                {
                                    break;
                                }

                                offset += buffer.Length;
                                fw.Write(buffer, 0, buffer.Length);
                                fw.Flush();
                                //File.WriteAllBytes(destFilePath, buffer);
                            }
                            catch (Exception exi)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            GC.WaitForFullGCComplete();
        }

        private static byte _index = 0;
        private static void TestArray64()
        {
            var cm = new Microsoft.VisualBasic.Devices.ComputerInfo();
            ulong needMemorySize = cm.AvailablePhysicalMemory - 500 * 1024 * 1024;
            long length = (long)(needMemorySize / sizeof(long) / 20);
            //long length = (long)(needMemorySize);

            int colSize = int.MaxValue / (sizeof(long) + 1);
            int rowSize = int.MaxValue / 10;
            try
            {
                _index = 0;
                //var array = new Array64<long>(length, colSize, rowSize);
                var array = new Array64<long>(length);
                //var array = new Array64<byte>(102);
                long beginIndex = 0;
                long[] buffer = GetBuffer2(length / 2);
                int ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer2(length / 4);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer2(length / 8);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer2(length / 4);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer2(654);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = array.Get(0, (int)beginIndex + 10);
                if (buffer.Length != beginIndex)
                {
                    Console.WriteLine("Error");
                    return;
                }

                for (int i = 0; i < buffer.Length; i++)
                {
                    try
                    {
                        Console.WriteLine(i);
                        if (buffer[i] != array[i])
                        {
                            Console.WriteLine("Error");
                            return;
                        }
                    }
                    catch (Exception exi)
                    {
                        Console.WriteLine(exi.Message);
                        return;
                    }
                }

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = _index;
                    buffer[i] = _index++;
                }

                for (int i = 1; i < buffer.Length; i++)
                {
                    if (array[i] != buffer[i])
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                }

                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static long _lindex = 0;
        private static long[] GetBuffer2(long length)
        {
            long[] buffer = new long[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = _lindex++;
            }

            return buffer;
        }

        private static void TestArray64_basic()
        {
            TestArray64_1(102, 1, 7);
            TestArray64_1(102, 2, 7);
            TestArray64_1(102, 3, 7);
            TestArray64_1(102, 4, 7);
            TestArray64_1(102, 5, 7);
            TestArray64_1(102, 6, 7);
            TestArray64_1(102, 7, 7);
            TestArray64_1(102, 8, 7);
            TestArray64_1(102, 9, 7);
            TestArray64_1(102, 10, 7);
            TestArray64_1(102, 11, 7);
            TestArray64_1(102, 12, 7);


            TestArray64_1(102, 7, 1);
            TestArray64_1(102, 7, 2);
            TestArray64_1(102, 7, 3);
            TestArray64_1(102, 7, 4);
            TestArray64_1(102, 7, 5);
            TestArray64_1(102, 7, 6);
            TestArray64_1(102, 7, 7);
            TestArray64_1(102, 7, 8);
            TestArray64_1(102, 7, 9);
            TestArray64_1(102, 7, 10);
            TestArray64_1(102, 7, 11);
            TestArray64_1(102, 7, 12);

            TestArray64_2(102, 1, 7);
            TestArray64_2(102, 2, 7);
            TestArray64_2(102, 3, 7);
            TestArray64_2(102, 4, 7);
            TestArray64_2(102, 5, 7);
            TestArray64_2(102, 6, 7);
            TestArray64_2(102, 7, 7);
            TestArray64_2(102, 8, 7);
            TestArray64_2(102, 9, 7);
            TestArray64_2(102, 10, 7);
            TestArray64_2(102, 11, 7);
            TestArray64_2(102, 12, 7);

            TestArray64_2(102, 7, 1);
            TestArray64_2(102, 7, 2);
            TestArray64_2(102, 7, 3);
            TestArray64_2(102, 7, 4);
            TestArray64_2(102, 7, 5);
            TestArray64_2(102, 7, 6);
            TestArray64_2(102, 7, 7);
            TestArray64_2(102, 7, 8);
            TestArray64_2(102, 7, 9);
            TestArray64_2(102, 7, 10);
            TestArray64_2(102, 7, 11);
            TestArray64_2(102, 7, 12);
        }

        private static void TestArray64_2(long length, int colSize, int rowSize)
        {
            try
            {
                _index = 0;
                //var array = new Array64<byte>(length, colSize, rowSize);
                var array = new ByteArray64(length, colSize, rowSize);
                //var array = new Array64<byte>(102);
                long beginIndex = 0;
                byte[] buffer = GetBuffer(7);
                int ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(34);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(59);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(13);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(3);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = array.Get(0, (int)beginIndex + 10);
                if (buffer.Length != beginIndex)
                {
                    Console.WriteLine("Error");
                    return;
                }

                for (int i = 0; i < buffer.Length; i++)
                {
                    try
                    {
                        Console.WriteLine(i);
                        if (buffer[i] != array[i])
                        {
                            Console.WriteLine("Error");
                            return;
                        }
                    }
                    catch (Exception exi)
                    {
                        Console.WriteLine(exi.Message);
                        return;
                    }
                }

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = _index;
                    buffer[i] = _index++;
                }

                for (int i = 1; i < buffer.Length; i++)
                {
                    if (array[i] != buffer[i])
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                }

                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void TestArray64_1(long length, int colSize, int rowSize)
        {
            try
            {
                _index = 0;
                var array = new Array64<byte>(length, colSize, rowSize);
                //var array = new Array64<byte>(102);
                long beginIndex = 0;
                byte[] buffer = GetBuffer(7);
                int ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(34);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(59);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(13);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = GetBuffer(3);
                ret = array.Set(beginIndex, buffer, 0, buffer.Length);
                beginIndex += ret;

                buffer = array.Get(0, (int)beginIndex + 10);
                if (buffer.Length != beginIndex)
                {
                    Console.WriteLine("Error");
                    return;
                }

                for (int i = 1; i < buffer.Length; i++)
                {
                    if (buffer[i - 1] + 1 != buffer[i])
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                }

                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static byte[] GetBuffer(int length)
        {
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = _index++;
            }
            return buffer;
        }

        private static void SubLog_LogOutput(RedirectOuputItem e)
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
                new FtpFileTransfer(ftpUrl);
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
