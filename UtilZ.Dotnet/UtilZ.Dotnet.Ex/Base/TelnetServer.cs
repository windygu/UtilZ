using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// Telnet Server
    /// </summary>
    public class TelnetServer
    {
        private readonly Func<string, string> _proCallback;
        private Socket _listenerSocket;
        private Thread _thread;


        public TelnetServer(IPAddress ip, int port, Func<string, string> proCallback, int backlog = 3)
        {
            this._listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._listenerSocket.Bind(new IPEndPoint(ip, 14002));

            if (backlog < 1)
            {
                backlog = 1;
            }
            this._listenerSocket.Listen(backlog);

            this._thread = new Thread(this.StartListen);
            this._thread.IsBackground = true;
            this._thread.Name = "TelnetServer监听线程";
            this._thread.Start();
        }

        private void StartListen()
        {
            Socket client;
            while (true)
            {
                client = this._listenerSocket.Accept();
                this.SendWelcom(client);
                while (true)
                {
                    byte[] buffer = new byte[255];
                    int recCount = client.Receive(buffer, 0, buffer.Length, 0);
                    string recStr = Encoding.Default.GetString(buffer, 0, recCount);
                    if (recStr.EndsWith("\r\n"))
                    {
                        buffer = Encoding.Default.GetBytes("Res\r\n");
                        client.Send(buffer);
                    }
                }
            }
        }

        private void SendWelcom(Socket client)
        {
            byte[] buffer = Encoding.Default.GetBytes("Welcome to the server!!\r\n");
            client.Send(buffer);
        }
    }
}
