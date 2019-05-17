using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Exceptions;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Command;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    internal class SHPBasicService : SHPServiceBasicBase
    {
        private readonly Action<SHPServiceTransferData> _proRevDataFunc;
        private readonly Func<SHPServiceTransferData, SHPServiceResponseData> _proRequestFunc;
        private readonly Func<ServiceStatus> _getServiceStatusFunc;
        protected readonly Func<PreReceiveDataInfo, bool> _acceptRevDataFunc;
        private readonly Action _closeServiceNotiry;

        private readonly SHPBasicServiceInfo _shpBasicServiceInfo;
        private readonly List<int> _hostNotAllocatePortList;
        private readonly ThreadEx _uploadPayloadThread;
        private int _denyRevDataCount = 0;
        private readonly SHPServiceRestfullService _serviceRestfullService;
        private readonly RestFullServiceLauncher<ISHPServiceRestfullService> _restFullServiceLauncher;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="proRevDataFunc">处理接收数据委托</param>
        /// <param name="proRequestFunc">处理请求委托</param>
        /// <param name="getServiceStatus">获取服务状态委托</param>
        /// <param name="acceptRevDataFunc">是否同意接收相关数据委托,为null则默认同意</param>
        public SHPBasicService(Action<SHPServiceTransferData> proRevDataFunc,
            Func<SHPServiceTransferData, SHPServiceResponseData> proRequestFunc,
            Func<ServiceStatus> getServiceStatus,
            Func<PreReceiveDataInfo, bool> acceptRevDataFunc, Action closeServiceNotiry)
            : base()
        {
            if (proRevDataFunc == null)
            {
                throw new ArgumentNullException(nameof(proRevDataFunc));
            }

            if (proRequestFunc == null)
            {
                throw new ArgumentNullException(nameof(proRequestFunc));
            }

            this._proRevDataFunc = proRevDataFunc;
            this._proRequestFunc = proRequestFunc;
            this._getServiceStatusFunc = getServiceStatus;
            this._acceptRevDataFunc = acceptRevDataFunc;
            this._closeServiceNotiry = closeServiceNotiry;

            this._shpBasicServiceInfo = this.QuerySHPBasicServiceInfo();
            this._hostNotAllocatePortList = this._shpBasicServiceInfo.HostNotAllocatePortList;
            if (this._hostNotAllocatePortList == null)
            {
                this._hostNotAllocatePortList = new List<int>();
            }

            int serviceListenPort;
            var transferChannel = base.CreateTransferChannel(this._shpBasicServiceInfo.TransferProtocal, this._hostNotAllocatePortList, out serviceListenPort);
            base.AddTransferChannel(this._shpBasicServiceInfo.TransferProtocal, transferChannel);

            //注册并启动RestFull服务
            int serviceRestfullServiceListenPort;
            this._serviceRestfullService = new SHPServiceRestfullService(this.CloseServiceNotifyCallback);
            this._restFullServiceLauncher = this.CreateRestFullServiceLauncher(this._serviceRestfullService,
                this._hostNotAllocatePortList, out serviceRestfullServiceListenPort);

            this.UploadServiceInsListenInfo(new ServiceInsListenInfo(this._shpBasicServiceInfo.DOID, this._shpBasicServiceInfo.Id, serviceListenPort, serviceRestfullServiceListenPort));

            this._uploadPayloadThread = new ThreadEx(this.UploadPayloadThreadMethod, "上报负载线程", true);
            this._uploadPayloadThread.Start();
        }

        private void CloseServiceNotifyCallback()
        {
            var handler = this._closeServiceNotiry;
            if (handler != null)
            {
                handler();
            }
        }

        private RestFullServiceLauncher<ISHPServiceRestfullService> CreateRestFullServiceLauncher(
            ISHPServiceRestfullService service, IEnumerable<int> notAllocatePortList, out int serviceListenPort)
        {
            RestFullServiceLauncher<ISHPServiceRestfullService> restFullServiceLauncher;
            const int portBeginIndex = 16000;
            serviceListenPort = portBeginIndex;
            string url;

            while (true)
            {
                try
                {
                    serviceListenPort++;
                    if (notAllocatePortList.Contains(serviceListenPort))
                    {
                        continue;
                    }

                    url = AgentServiceMethodNameConstant.GetServiceRestfullServiceUrl(serviceListenPort);
                    restFullServiceLauncher = new RestFullServiceLauncher<ISHPServiceRestfullService>(new Uri(url), service);
                    restFullServiceLauncher.Start();
                    break;
                }
                catch (AddressAlreadyInUseException)
                {
                    //地址已占用
                }
                catch (SocketException ex)
                {
                    //10048错误为端口已占用错误
                    if (ex.ErrorCode != 10048)
                    {
                        throw ex;
                    }
                }
            }

            return restFullServiceLauncher;
        }

        private void UploadPayloadThreadMethod(CancellationToken token)
        {
            var handler = this._getServiceStatusFunc;
            string url = AgentServiceMethodNameConstant.AGENT_SERVICE_BASE_URL + AgentServiceMethodNameConstant.UPLOAD_SERVICE_INS_Status;
            var uploadPayloadInterval = this._shpBasicServiceInfo.StatusUploadIntervalMilliseconds;
            string uploadPayloadIntervalStr;
            int uploadPayloadIntervalTmp;
            ServiceStatus serviceStatus;
            var serviceStatusInfo = new ServiceStatusInfo(this._shpBasicServiceInfo.DOID, this._shpBasicServiceInfo.Id);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(uploadPayloadInterval);

                    if (handler != null)
                    {
                        serviceStatus = handler();
                        serviceStatusInfo.UpdateServiceStatus(serviceStatus);
                    }

                    serviceStatusInfo.DenyCount = this._denyRevDataCount;
                    this._denyRevDataCount = 0;

                    uploadPayloadIntervalStr = RestFullClientHelper.Post(url, serviceStatusInfo);
                    if (int.TryParse(uploadPayloadIntervalStr, out uploadPayloadIntervalTmp))
                    {
                        if (uploadPayloadIntervalTmp > 0)
                        {
                            uploadPayloadInterval = uploadPayloadIntervalTmp;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, "获取服务状态信息异常");
                }
            }
        }

        private void UploadServiceInsListenInfo(ServiceInsListenInfo para)
        {
            string url = AgentServiceMethodNameConstant.AGENT_SERVICE_BASE_URL + AgentServiceMethodNameConstant.UPLOAD_SERVICE_INS_LISTEN_INFO;

            while (true)
            {
                try
                {
                    var json = RestFullClientHelper.Post(url, para);
                    var apiResult = SerializeEx.WebScriptJsonDeserializeObject<ApiResult>(json);
                    if (apiResult.Status == ApiResultStatus.Succes)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }

                Thread.Sleep(_QUERY_INTERVAL);
            }
        }

        private SHPBasicServiceInfo QuerySHPBasicServiceInfo()
        {
            string url = AgentServiceMethodNameConstant.AGENT_SERVICE_BASE_URL + AgentServiceMethodNameConstant.QUERY_SHP_BASIC_SERVICE_INFO;
            var para = new GetServiceInsInfoPara(Assembly.GetEntryAssembly().Location);

            while (true)
            {
                try
                {
                    var json = RestFullClientHelper.Post(url, para);
                    var apiResult = SerializeEx.WebScriptJsonDeserializeObject<ApiResult>(json);
                    switch (apiResult.Status)
                    {
                        case ApiResultStatus.Succes:
                            return SerializeEx.WebScriptJsonDeserializeObject<SHPBasicServiceInfo>(apiResult.Data);
                        case ApiResultStatus.Exception:
                        case ApiResultStatus.Fail:
                            throw new SHPException(apiResult.Data);
                    }
                }
                catch (SHPException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }

                Thread.Sleep(_QUERY_INTERVAL);
            }
        }

        #region ParseDataCallback
        protected override void ParseDataCallback(ReceiveDataItem receiveDataItem)
        {
            try
            {
                if (receiveDataItem.Flag)
                {
                    var serviceBasicTransferCommand = new SHPServiceBasicTransferCommand(receiveDataItem.Data);
                    switch (serviceBasicTransferCommand.Cmd)
                    {
                        case SHPServiceBasicTransferCommandDefine.REQUEST:
                            this.ProRequestData(receiveDataItem, serviceBasicTransferCommand);
                            break;
                        case SHPServiceBasicTransferCommandDefine.RESPONSE:
                            base.ProResponseData(serviceBasicTransferCommand);
                            break;
                        case SHPServiceBasicTransferCommandDefine.POST:
                            this.ProPostData(receiveDataItem, serviceBasicTransferCommand);
                            break;
                        case SHPServiceBasicTransferCommandDefine.ACCEPT_CHECK_REQ:
                            this.ProAcceptRequestRevData(receiveDataItem, serviceBasicTransferCommand);
                            break;
                        case SHPServiceBasicTransferCommandDefine.ACCEPT_CHECK_RES:
                            this.ProAcceptResRevData(serviceBasicTransferCommand);
                            break;
                        default:
                            throw new NotSupportedException($"不支持的内部命令[{serviceBasicTransferCommand.Cmd}]");
                    }
                }
                else
                {
                    this.ProPostData(receiveDataItem);
                }
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                Loger.Error(ex, "处理数据异常");
            }
        }

        private void ProAcceptResRevData(SHPServiceBasicTransferCommand serviceBasicTransferCommand)
        {
            var serviceBasicRevDataResponseCommand = new SHPServiceBasicRevDataResponseCommand();
            serviceBasicRevDataResponseCommand.Parse(serviceBasicTransferCommand.Data);
            base.PrimitiveProResponseData(serviceBasicRevDataResponseCommand.ContextId, serviceBasicRevDataResponseCommand);
        }

        private void ProAcceptRequestRevData(ReceiveDataItem receiveDataItem, SHPServiceBasicTransferCommand serviceBasicTransferCommand)
        {
            var serviceBasicRevDataRequestCommand = new SHPServiceBasicRevDataRequestCommand();
            serviceBasicRevDataRequestCommand.Parse(serviceBasicTransferCommand.Data);
            var preReceiveDataInfo = new PreReceiveDataInfo(serviceBasicRevDataRequestCommand.DataCode, serviceBasicRevDataRequestCommand.Size);
            bool accept = true;
            var handler = this._acceptRevDataFunc;
            if (handler != null)
            {
                accept = handler(preReceiveDataInfo);
                if (!accept)
                {
                    this._denyRevDataCount++;
                }
            }

            var serviceBasicTransferCommandRes = new SHPServiceBasicTransferCommand(new SHPServiceBasicRevDataResponseCommand(accept, serviceBasicRevDataRequestCommand.ContextId));
            var transferChannel = this.GetSendTransferChannel(serviceBasicRevDataRequestCommand.TransferProtocal);
            var transferBasicPolicy = new TransferBasicPolicy(preReceiveDataInfo.DataCode, null, 0, 2000)
            {
                RepeatCount = 3,
                Priority = short.MinValue
            };
            var transferPolicy = base.CreateTransferPolicy(receiveDataItem.SrcEndPoint, transferBasicPolicy);
            transferChannel.SendData(serviceBasicTransferCommandRes.ToBytes(), transferPolicy);
        }

        private void ProPostData(ReceiveDataItem receiveDataItem)
        {
            this._proRevDataFunc(new SHPServiceTransferData(receiveDataItem));
        }

        private void ProPostData(ReceiveDataItem receiveDataItem, SHPServiceBasicTransferCommand serviceBasicTransferCommand)
        {
            var serviceBasicPostDataCommand = new SHPServiceBasicPostDataCommand();
            serviceBasicPostDataCommand.Parse(serviceBasicTransferCommand.Data);
            this._proRevDataFunc(new SHPServiceTransferData(serviceBasicPostDataCommand.DataCode, serviceBasicPostDataCommand.Data, receiveDataItem.SrcEndPoint));
        }

        private void ProRequestData(ReceiveDataItem receiveDataItem, SHPServiceBasicTransferCommand serviceBasicTransferCommand)
        {
            var serviceBasicRequestDataCommand = new SHPServiceBasicRequestDataCommand();
            serviceBasicRequestDataCommand.Parse(serviceBasicTransferCommand.Data);

            SHPServiceResponseData responseData = this._proRequestFunc(new SHPServiceTransferData(serviceBasicRequestDataCommand.DataCode, serviceBasicRequestDataCommand.Data, receiveDataItem.SrcEndPoint));
            var serviceBasicTransferCommandRes = new SHPServiceBasicTransferCommand(new SHPServiceBasicResponseDataCommand(responseData, serviceBasicRequestDataCommand));

            var transferBasicPolicy = new TransferBasicPolicy(responseData.DataCode);
            var transferChannel = this.GetSendTransferChannel(serviceBasicRequestDataCommand.TransferProtocal);
            //注:此处没使用serviceRouteTransferPolicy.TransferPolicy属性值,而重新创建一个传输策略对象,是因为响应必须从那儿回那儿去,
            //如果使用serviceRouteTransferPolicy.TransferPolicy属性值不一定能回到源去
            var transferPolicy = base.CreateTransferPolicy(receiveDataItem.SrcEndPoint, transferBasicPolicy);
            transferChannel.SendData(serviceBasicTransferCommandRes.ToBytes(), transferPolicy);
        }
        #endregion

        protected override string GetRouteServiceBasicUrl()
        {
            return this._shpBasicServiceInfo.RouteServiceUrl;
        }

        protected override TransferProtocal GetReceiveDataTransferProtocal()
        {
            return this._shpBasicServiceInfo.TransferProtocal;
        }

        protected override IEnumerable<int> GetNotAllocatePortList()
        {
            return this._hostNotAllocatePortList;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                this._uploadPayloadThread.Stop();
                this._uploadPayloadThread.Dispose();
                this._restFullServiceLauncher.Stop();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose发生异常");
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// 获取服务实例名称
        /// </summary>
        public override string ServiceInsName
        {
            get
            {
                return this._shpBasicServiceInfo.ServiceInsName;
            }
        }
    }
}
