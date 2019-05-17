using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace SHPTestTerminal1
{
    public class TestTerminl1BLL
    {
        private readonly ISHPServiceBasic _serviceBasic;
        public TestTerminl1BLL()
        {
            this._serviceBasic = SHPServiceBasicFactory.CreateBasicTerminalObject(TransferProtocal.Udp, "http://192.168.10.96:20001/", null);
        }

        internal void Request()
        {
            byte[] data = Encoding.UTF8.GetBytes("喜欢你");
            var transferBasicPolicy = new TransferBasicPolicy(3);
            transferBasicPolicy.MillisecondsTimeout = 1000;
            SHPServiceTransferData serviceTransferData = this._serviceBasic.Request(data, transferBasicPolicy);

            string str = Encoding.UTF8.GetString(serviceTransferData.Data);
            Loger.Info($"Request-收到[{str}]");
        }

        internal void Post()
        {
            byte[] data = Encoding.UTF8.GetBytes("天不正南");
            var transferBasicPolicy = new TransferBasicPolicy(2);
            transferBasicPolicy.MillisecondsTimeout = 1000;
            this._serviceBasic.Post(data, transferBasicPolicy);
            Loger.Info($"Post-成功");
        }
    }
}
