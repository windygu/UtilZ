using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class TestMemoryMappedFile
    {
        public static void TT()
        {

        }

        public static void Test()
        {

            string filePath = @"G:\Tmp\cef_binary_1.1180.832_windows.zip";
            filePath = @"G:\Tmp\DB\db2.sqlite";
            string filePath2 = @"G:\Tmp\DB\db23.sqlite";
            int length = (int)(new FileInfo(filePath).Length);
            var mmf = MemoryMappedFile.CreateFromFile(filePath);

            for (int i = 0; i < 3; i++)
            {
                byte[] buffer = new byte[length];
                var wath = Stopwatch.StartNew();
                using (var readStream = mmf.CreateViewStream())
                {
                    var bs = new BufferedStream(readStream);
                    //readStream.Read(buffer, 0, buffer.Length);
                    bs.Read(buffer, 0, buffer.Length);
                }
                wath.Stop();
                Console.WriteLine("CreateViewStream,time:{0}", wath.Elapsed.TotalMilliseconds);

                wath.Restart();
                using (var readAccessor = mmf.CreateViewAccessor())
                {
                    readAccessor.ReadArray<byte>(0, buffer, 0, buffer.Length);
                }
                wath.Stop();
                Console.WriteLine("CreateViewAccessor,time:{0}", wath.Elapsed.TotalMilliseconds);
                //

                wath.Restart();
                buffer = File.ReadAllBytes(filePath2);
                wath.Stop();
                Console.WriteLine("ReadAllBytes,time:{0}", wath.Elapsed.TotalMilliseconds);
            }

            mmf.Dispose();
        }

        public static void Test_bk()
        {
            string srcFilePath = @"F:\Movie\《加菲猫2》Garfield.A.Tail.Of.Two.Kitties.2006.720p.BluRay.x264-AVS720\Garfield.II.A.Tail.Of.Two.Kitties.2006.x264.DTS.4AUDIO-WAF.chs.mkv";
            string dstFilePath = @"G:\Tmp\Garfield.II.A.Tail.Of.Two.Kitties.2006.x264.DTS.4AUDIO-WAF.chs.mkv";

            Console.WriteLine("begin  map");
            var wath = Stopwatch.StartNew();
            TestMap(srcFilePath, dstFilePath);
            wath.Stop();
            Console.WriteLine("end  map,time:{0}", wath.Elapsed.TotalMilliseconds);


            //dstFilePath = @"G:\Tmp\Garfield.II.A.Tail.Of.Two.Kitties.2006.x264.DTS.4AUDIO-WAF.chs2222.mkv";
            //Console.WriteLine("begin  IO");
            //var wath = Stopwatch.StartNew();
            //TestIO(srcFilePath, dstFilePath);
            //wath.Stop();
            //Console.WriteLine("end  IO,time:{0}", wath.Elapsed.TotalMilliseconds);
        }

        private static void TestIO(string srcFilePath, string dstFilePath)
        {
            File.Copy(srcFilePath, dstFilePath);
        }

        private static void TestMap(string srcFilePath, string dstFilePath)
        {
            var mmfSrc = MemoryMappedFile.CreateFromFile(srcFilePath);
            var mmfDst = MemoryMappedFile.CreateFromFile(dstFilePath, FileMode.CreateNew, "aa", new FileInfo(srcFilePath).Length);
            using (var readStream = mmfSrc.CreateViewStream())
            {
                using (var writeStream = mmfDst.CreateViewStream())
                {
                    int count;
                    byte[] buffer = new byte[1024 * 1024 * 100];
                    while (true)
                    {
                        count = readStream.Read(buffer, 0, buffer.Length);
                        if (count < 1)
                        {
                            break;
                        }
                        else
                        {
                            writeStream.Write(buffer, 0, count);
                        }
                    }
                }
            }
        }
    }
}
