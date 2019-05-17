using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Command;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    internal class SHPBasicTerminal : SHPServiceBasicBase
    {
        private readonly TransferProtocal _revDataTransferProtocal;
        private readonly string _routeServiceBasicUrl;
        private readonly IEnumerable<int> _notAllocatePortList;

        public SHPBasicTerminal(TransferProtocal revDataTransferProtocal,
            string routeServiceBasicUrl,
            IEnumerable<int> notAllocatePortList)
            : base()
        {
            if (string.IsNullOrWhiteSpace(routeServiceBasicUrl))
            {
                throw new ArgumentNullException(nameof(routeServiceBasicUrl));
            }

            if (notAllocatePortList == null)
            {
                notAllocatePortList = new List<int>();
            }

            this._revDataTransferProtocal = revDataTransferProtocal;
            this._routeServiceBasicUrl = routeServiceBasicUrl;
            this._notAllocatePortList = notAllocatePortList;
        }

        protected override void ParseDataCallback(ReceiveDataItem receiveDataItem)
        {
            try
            {
                var serviceBasicTransferCommand = new SHPServiceBasicTransferCommand(receiveDataItem.Data);
                switch (serviceBasicTransferCommand.Cmd)
                {
                    case SHPServiceBasicTransferCommandDefine.RESPONSE:
                        base.ProResponseData(serviceBasicTransferCommand);
                        break;
                    default:
                        throw new InvalidOperationException($"终端模式下不应该收到数据同,平台逻辑错误,来源[{receiveDataItem.SrcEndPoint.ToString()}]");
                }
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                Loger.Error(ex, "处理数据异常");
            }
        }

        protected override string GetRouteServiceBasicUrl()
        {
            return this._routeServiceBasicUrl;
        }

        protected override TransferProtocal GetReceiveDataTransferProtocal()
        {
            return this._revDataTransferProtocal;
        }

        protected override IEnumerable<int> GetNotAllocatePortList()
        {
            return this._notAllocatePortList;
        }

        /// <summary>
        /// 获取服务实例名称
        /// </summary>
        public override string ServiceInsName
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
