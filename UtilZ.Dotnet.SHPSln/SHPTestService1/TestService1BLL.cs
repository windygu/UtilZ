using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Command;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace SHPTestService1
{
    public class TestService1BLL
    {
        private readonly ISHPServiceBasic _serviceBasic;

        public string ServiceInsName
        {
            get
            {
                return this._serviceBasic.ServiceInsName;
            }
        }

        public TestService1BLL()
        {
            this._serviceBasic = SHPServiceBasicFactory.CreateBasicServiceObject(this.ProRevDataFunc, this.ProRequestFunc, this.GetServiceStatus, this.AcceptRevDataFunc, this.CloseServiceNotiry);
        }

        private void ProRevDataFunc(SHPServiceTransferData serviceTransferData)
        {
            if (serviceTransferData.DataCode == 2)
            {
                string str = Encoding.UTF8.GetString(serviceTransferData.Data);
                Loger.Info($"收到数据[{str}]");
            }
        }

        private SHPServiceResponseData ProRequestFunc(SHPServiceTransferData serviceTransferData)
        {
            if (serviceTransferData.DataCode == 3)
            {
                string str = Encoding.UTF8.GetString(serviceTransferData.Data);
                Loger.Info($"收到数据[{str}]");
            }

            var revRequestProResult = new SHPServiceResponseData();
            revRequestProResult.DataCode = 1;
            revRequestProResult.Data = Encoding.UTF8.GetBytes("abc");
            return revRequestProResult;

        }
        private ServiceStatusInfo GetServiceStatus()
        {
            Loger.Info($"收到数据[GetServiceStatus]");
            return new ServiceStatusInfo() { PayloadValue = 123 };
        }

        private bool AcceptRevDataFunc(PreReceiveDataInfo preReceiveDataInfo)
        {
            Loger.Info($"收到[AcceptRevDataFunc]数据");
            return true;
        }

        private void CloseServiceNotiry()
        {
            Loger.Error("收到CloseServiceNotiry");
            this.Stop();
        }

        public void Stop()
        {
            Loger.Info($"Stop");
        }
    }
}
