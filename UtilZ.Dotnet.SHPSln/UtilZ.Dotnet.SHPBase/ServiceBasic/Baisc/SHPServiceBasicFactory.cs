using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    /// <summary>
    /// 服务基础创建工厂
    /// </summary>
    public class SHPServiceBasicFactory
    {
        public static ISHPServiceBasic CreateBasicServiceObject(Action<SHPServiceTransferData> proRevDataFunc,
            Func<SHPServiceTransferData, SHPServiceResponseData> proRequestFunc,
            Func<ServiceStatusInfo> getServiceStatus,
            Func<PreReceiveDataInfo, bool> acceptRevDataFunc,
            Action closeServiceNotiry)
        {
            return new SHPBasicService(proRevDataFunc, proRequestFunc, getServiceStatus, acceptRevDataFunc, closeServiceNotiry);
        }

        public static ISHPServiceBasic CreateBasicTerminalObject(TransferProtocal revDataTransferProtocal,
            string routeServiceBasicUrl,
            IEnumerable<int> notAllocatePortList)
        {
            return new SHPBasicTerminal(revDataTransferProtocal, routeServiceBasicUrl, notAllocatePortList);
        }
    }
}
