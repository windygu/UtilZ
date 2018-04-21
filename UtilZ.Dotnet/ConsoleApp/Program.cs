using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UtilZ.Dotnet.Ex.Base;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TelnetServer ts = new TelnetServer(IPAddress.Parse("0.0.0.0"), 14002, null, 3);
            Console.WriteLine("Press any key exit");
            Console.ReadKey();
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
