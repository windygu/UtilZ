using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Exceptions;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Command;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Baisc
{
    public abstract class SHPServiceBasicBase : ISHPServiceBasic
    {
        private readonly AsynQueue<ReceiveDataItem> _parseDataQueue = null;

        protected const int _QUERY_INTERVAL = 200;

        public SHPServiceBasicBase()
        {
            this._parseDataQueue = new AsynQueue<ReceiveDataItem>(this.ParseDataCallback, "接收数据解析线程", true, false);
            this._parseDataQueue.Start();
        }

        protected abstract void ParseDataCallback(ReceiveDataItem receiveDataItem);

        protected void PrimitiveProResponseData(int contextId, object tag)
        {
            try
            {
                var eventWaitHandleInfo = AutoEventWaitHandleManager.GetEventWaitHandleInfo(contextId);
                if (eventWaitHandleInfo != null)
                {
                    eventWaitHandleInfo.Tag = tag;
                    eventWaitHandleInfo.EventWaitHandle.Set();
                }
            }
            catch (ObjectDisposedException)
            { }
        }

        protected void ProResponseData(SHPServiceBasicTransferCommand serviceBasicTransferCommand)
        {
            var serviceBasicResponseDataCommandRes = new SHPServiceBasicResponseDataCommand();
            serviceBasicResponseDataCommandRes.Parse(serviceBasicTransferCommand.Data);
            this.PrimitiveProResponseData(serviceBasicResponseDataCommandRes.ContextId, serviceBasicResponseDataCommandRes);
        }

        protected abstract string GetRouteServiceBasicUrl();

        protected abstract TransferProtocal GetReceiveDataTransferProtocal();

        private readonly ConcurrentDictionary<TransferProtocal, TransferChannel> _transferProtocalTransferChannelDic = new ConcurrentDictionary<TransferProtocal, TransferChannel>();
        private readonly object _transferProtocalTransferChannelDicLock = new object();
        protected TransferChannel GetSendTransferChannel(TransferProtocal transferProtocal)
        {
            TransferChannel transferChannel;
            if (!this._transferProtocalTransferChannelDic.TryGetValue(transferProtocal, out transferChannel))
            {
                lock (this._transferProtocalTransferChannelDicLock)
                {
                    if (!this._transferProtocalTransferChannelDic.TryGetValue(transferProtocal, out transferChannel))
                    {
                        int serviceListenPort;
                        transferChannel = this.CreateTransferChannel(transferProtocal, this.GetNotAllocatePortList(), out serviceListenPort);
                        this.AddTransferChannel(transferProtocal, transferChannel);
                    }
                }
            }

            return transferChannel;
        }

        protected void AddTransferChannel(TransferProtocal transferProtocal, TransferChannel transferChannel)
        {
            this._transferProtocalTransferChannelDic.Add(transferProtocal, transferChannel);
        }

        protected abstract IEnumerable<int> GetNotAllocatePortList();

        protected TransferChannel CreateTransferChannel(TransferProtocal transferProtocal, IEnumerable<int> notAllocatePortList, out int serviceListenPort)
        {
            var transferConfig = this.CreateDefaultTransferConfig(transferProtocal);

            const int portBeginIndex = 16000;

            serviceListenPort = portBeginIndex;
            TransferChannel transferChannel;
            while (true)
            {
                try
                {
                    serviceListenPort++;
                    if (notAllocatePortList.Contains(serviceListenPort))
                    {
                        continue;
                    }

                    transferConfig.NetConfig.ListenEP = new IPEndPoint(IPAddress.Any, serviceListenPort);
                    transferChannel = new TransferChannel(transferConfig, this.RevData);
                    transferChannel.Start();
                    break;
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

            return transferChannel;
        }

        protected void RevData(ReceiveDataItem receiveDataItem)
        {
            this._parseDataQueue.Enqueue(receiveDataItem);
        }

        protected TransferConfig CreateDefaultTransferConfig(TransferProtocal transferProtocal)
        {
            var transferConfig = new TransferConfig();
            transferConfig.NetConfig.Protocal = transferProtocal;
            return transferConfig;
        }

        #region 传输策略
        private readonly ConcurrentDictionary<int, ServiceRouteTransferPolicy> _dataCodeTransferPolicyDic = new ConcurrentDictionary<int, ServiceRouteTransferPolicy>();
        private readonly object _dataCodeTransferPolicyDicLock = new object();
        protected ServiceRouteTransferPolicy GetServiceRouteTransferPolicy(TransferBasicPolicy transferBasicPolicy, long size)
        {
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                var sendFailServiceInsIdList = new List<long>();
                ServiceRouteTransferPolicy serviceRouteTransferPolicy;

                if (!this._dataCodeTransferPolicyDic.TryGetValue(transferBasicPolicy.DataCode, out serviceRouteTransferPolicy))
                {
                    lock (this._dataCodeTransferPolicyDicLock)
                    {
                        if (!this._dataCodeTransferPolicyDic.TryGetValue(transferBasicPolicy.DataCode, out serviceRouteTransferPolicy))
                        {
                            serviceRouteTransferPolicy = this.GetTransferPolicyByTransferBasicPolicy(watch, transferBasicPolicy, sendFailServiceInsIdList);
                            this._dataCodeTransferPolicyDic.Add(transferBasicPolicy.DataCode, serviceRouteTransferPolicy);
                        }
                    }
                }

                return this.FindServiceRouteTransferPolicy(watch, transferBasicPolicy, size, sendFailServiceInsIdList, serviceRouteTransferPolicy);
            }
            finally
            {
                watch.Stop();
            }
        }

        private ServiceRouteTransferPolicy FindServiceRouteTransferPolicy(Stopwatch watch, TransferBasicPolicy transferBasicPolicy, long size,
            List<long> sendFailServiceInsIdList, ServiceRouteTransferPolicy serviceRouteTransferPolicy)
        {
            while (true)
            {
                try
                {
                    if (this.CheckAcceptSendData(transferBasicPolicy, size, serviceRouteTransferPolicy))
                    {
                        this._dataCodeTransferPolicyDic.AddOrUpdate(transferBasicPolicy.DataCode, serviceRouteTransferPolicy, (i, s) => { return serviceRouteTransferPolicy; });
                        return serviceRouteTransferPolicy;
                    }
                    else
                    {
                        if (sendFailServiceInsIdList.Contains(serviceRouteTransferPolicy.ServiceRouteInfo.Id))
                        {
                            throw new SHPRouteException("没有可用的服务实例");
                        }
                        else
                        {
                            sendFailServiceInsIdList.Add(serviceRouteTransferPolicy.ServiceRouteInfo.Id);
                            serviceRouteTransferPolicy = this.GetTransferPolicyByTransferBasicPolicy(watch, transferBasicPolicy, sendFailServiceInsIdList);
                        }
                    }
                }
                catch (SHPRouteException)
                {
                    throw;
                }
                catch (TimeoutException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        private ServiceRouteTransferPolicy GetTransferPolicyByTransferBasicPolicy(Stopwatch watch, TransferBasicPolicy transferBasicPolicy, List<long> sendFailServiceInsIdList)
        {
            string queryServiceRouteUrl = this.GetRouteServiceBasicUrl() + ServiceRouteServiceMethodNameConstant.QUERY_SERVICE_ROUTE;
            ApiResult apiResult;
            while (true)
            {
                if (watch.ElapsedMilliseconds >= transferBasicPolicy.TotalMillisecondsTimeout)
                {
                    throw new TimeoutException("查询服务路由超时");
                }

                try
                {
                    var json = RestFullClientHelper.Post(queryServiceRouteUrl, new GetServiceRoutePara(sendFailServiceInsIdList, transferBasicPolicy));
                    if (watch.ElapsedMilliseconds >= transferBasicPolicy.TotalMillisecondsTimeout)
                    {
                        throw new TimeoutException("查询服务路由超时");
                    }

                    apiResult = SerializeEx.WebScriptJsonDeserializeObject<ApiResult>(json);
                    switch (apiResult.Status)
                    {
                        case ApiResultStatus.Succes:
                            return this.CreateTransferPolicyByServiceRoute(transferBasicPolicy, apiResult);
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
                    Loger.Error(ex, "查询服务路由异常");
                }

                Thread.Sleep(_QUERY_INTERVAL);
            }
        }

        private bool CheckAcceptSendData(TransferBasicPolicy transferBasicPolicy, long size, ServiceRouteTransferPolicy serviceRouteTransferPolicy)
        {
            if (size <= TransferConstant.MESSAGE_MAX_SIZE)
            {
                return true;
            }

            var contextId = Guid.NewGuid().GetHashCode();
            var acceptBuffer = new SHPServiceBasicTransferCommand(new SHPServiceBasicRevDataRequestCommand(transferBasicPolicy.DataCode, size, GetReceiveDataTransferProtocal(), contextId)).ToBytes();
            var transferChannel = this.GetSendTransferChannel(serviceRouteTransferPolicy.ServiceRouteInfo.TransferProtocal);

            var eventWaitHandleInfo = AutoEventWaitHandleManager.CreateEventWaitHandle(contextId, transferBasicPolicy.MillisecondsTimeout * 2);
            try
            {
                transferChannel.SendData(acceptBuffer, serviceRouteTransferPolicy.TransferPolicy);
                if (eventWaitHandleInfo.EventWaitHandle.WaitOne(transferBasicPolicy.MillisecondsTimeout))
                {
                    var serviceBasicRevDataResponseCommand = (SHPServiceBasicRevDataResponseCommand)eventWaitHandleInfo.Tag;
                    if (serviceBasicRevDataResponseCommand.Accept)
                    {
                        //对方同意接收数据
                        return true;
                    }
                    else
                    {
                        //对方因为一些原因,不同意接收数据,重新找一个目标服务实例
                        return false;
                    }
                }
                else
                {
                    //发送超时
                    return false;
                }
            }
            catch (Exception)
            {
                //对方不在线等场景
                return false;
            }
            finally
            {
                AutoEventWaitHandleManager.RemoveEventWaitHandle(contextId);
                eventWaitHandleInfo.EventWaitHandle.Dispose();
            }
        }

        private ServiceRouteTransferPolicy CreateTransferPolicyByServiceRoute(TransferBasicPolicy transferBasicPolicy, ApiResult apiResult)
        {
            ServiceRouteTransferPolicy serviceRouteTransferPolicy;
            try
            {
                var serviceRoute = SerializeEx.WebScriptJsonDeserializeObject<ServiceRouteInfo>(apiResult.Data);
                if (transferBasicPolicy == null)
                {
                    transferBasicPolicy = new TransferBasicPolicy();
                }

                TransferPolicy transferPolicy = this.CreateTransferPolicy(new IPEndPoint(IPAddress.Parse(serviceRoute.Ip), serviceRoute.Port), transferBasicPolicy);
                serviceRouteTransferPolicy = new ServiceRouteTransferPolicy(serviceRoute, transferPolicy);
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据传输策略异常", ex);
            }

            return serviceRouteTransferPolicy;
        }

        protected TransferPolicy CreateTransferPolicy(IPEndPoint ipEndPoint, TransferBasicPolicy transferBasicPolicy)
        {
            return new TransferPolicy(ipEndPoint, transferBasicPolicy.Priority, transferBasicPolicy.MillisecondsTimeout, 0);
        }
        #endregion

        #region ISHPServiceBasic接口       
        /// <summary>
        /// 获取服务实例名称
        /// </summary>
        public abstract string ServiceInsName { get; }

        private void SendData(byte[] data, TransferBasicPolicy transferBasicPolicy, TransferChannel transferChannel, ServiceRouteTransferPolicy serviceRouteTransferPolicy)
        {
            int repeatCount = 0;
            Stopwatch watch = Stopwatch.StartNew();
            List<long> sendFailServiceInsIdList = new List<long>();
            try
            {
                while (true)
                {
                    try
                    {
                        transferChannel.SendData(data, serviceRouteTransferPolicy.TransferPolicy);
                        break;
                    }
                    catch (TimeoutException ex)
                    {
                        repeatCount++;
                        if (repeatCount >= transferBasicPolicy.RepeatCount)
                        {
                            throw ex;
                        }

                        sendFailServiceInsIdList.Add(serviceRouteTransferPolicy.ServiceRouteInfo.Id);
                        serviceRouteTransferPolicy = this.GetTransferPolicyByTransferBasicPolicy(watch, transferBasicPolicy, sendFailServiceInsIdList);
                        this._dataCodeTransferPolicyDic.AddOrUpdate(transferBasicPolicy.DataCode, serviceRouteTransferPolicy, (k, v) => { return serviceRouteTransferPolicy; });
                    }
                }
            }
            finally
            {
                watch.Stop();
            }
        }

        private void PrimitiveSendFile(string filePath, TransferBasicPolicy transferBasicPolicy, TransferChannel transferChannel, ServiceRouteTransferPolicy serviceRouteTransferPolicy)
        {
            int repeatCount = 0;
            Stopwatch watch = Stopwatch.StartNew();
            List<long> sendFailServiceInsIdList = new List<long>();
            try
            {
                while (true)
                {
                    try
                    {
                        transferChannel.SendFile(filePath, serviceRouteTransferPolicy.TransferPolicy);
                        break;
                    }
                    catch (TimeoutException ex)
                    {
                        repeatCount++;
                        if (repeatCount >= transferBasicPolicy.RepeatCount)
                        {
                            throw ex;
                        }

                        sendFailServiceInsIdList.Add(serviceRouteTransferPolicy.ServiceRouteInfo.Id);
                        serviceRouteTransferPolicy = this.GetTransferPolicyByTransferBasicPolicy(watch, transferBasicPolicy, sendFailServiceInsIdList);
                        this._dataCodeTransferPolicyDic.AddOrUpdate(transferBasicPolicy.DataCode, serviceRouteTransferPolicy, (k, v) => { return serviceRouteTransferPolicy; });
                    }
                }
            }
            finally
            {
                watch.Stop();
            }
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="transferBasicPolicy">传输基础策略,为null使用默认策略</param>
        /// <returns></returns>
        public SHPServiceTransferData Request(byte[] data, TransferBasicPolicy transferBasicPolicy)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (transferBasicPolicy == null)
            {
                throw new ArgumentNullException(nameof(transferBasicPolicy));
            }

            ServiceRouteTransferPolicy serviceRouteTransferPolicy = this.GetServiceRouteTransferPolicy(transferBasicPolicy, data.Length);
            var transferChannel = this.GetSendTransferChannel(serviceRouteTransferPolicy.ServiceRouteInfo.TransferProtocal);

            var eventWaitHandleId = Guid.NewGuid().GetHashCode();
            var serviceBasicTransferCommand = new SHPServiceBasicTransferCommand(new SHPServiceBasicRequestDataCommand(transferBasicPolicy.DataCode, eventWaitHandleId, data, transferChannel.Config.NetConfig.Protocal));
            var eventWaitHandleInfo = AutoEventWaitHandleManager.CreateEventWaitHandle(eventWaitHandleId);

            try
            {
                //为请求响应模式重新构造数据
                //transferChannel.SendData(serviceBasicTransferCommand.ToBytes(), serviceRouteTransferPolicy.TransferPolicy);
                this.SendData(serviceBasicTransferCommand.ToBytes(), transferBasicPolicy, transferChannel, serviceRouteTransferPolicy);

                int millisecondsTimeout = Timeout.Infinite;
                if (transferBasicPolicy != null)
                {
                    millisecondsTimeout = transferBasicPolicy.MillisecondsTimeout;
                }

                if (eventWaitHandleInfo.EventWaitHandle.WaitOne(millisecondsTimeout))
                {
                    var serviceBasicResponseDataCommand = (SHPServiceBasicResponseDataCommand)eventWaitHandleInfo.Tag;
                    return new SHPServiceTransferData(serviceBasicResponseDataCommand.DataCode, serviceBasicResponseDataCommand.Data, null);
                }
                else
                {
                    throw new TimeoutException("请求超时");
                }
            }
            finally
            {
                try
                {
                    AutoEventWaitHandleManager.RemoveEventWaitHandle(eventWaitHandleId);
                    eventWaitHandleInfo.EventWaitHandle.Dispose();
                }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="dataCode">数据路由编码</param>
        /// <param name="data">要发送的数据</param>
        /// <param name="destinationConsistentKey">数据目的地一致性Key</param>
        /// <param name="transferBasicPolicy">传输基础策略,为null使用默认策略</param>
        public void Post(byte[] data, TransferBasicPolicy transferBasicPolicy)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (transferBasicPolicy == null)
            {
                throw new ArgumentNullException(nameof(transferBasicPolicy));
            }

            ServiceRouteTransferPolicy serviceRouteTransferPolicy = this.GetServiceRouteTransferPolicy(transferBasicPolicy, data.Length);
            var transferChannel = this.GetSendTransferChannel(serviceRouteTransferPolicy.ServiceRouteInfo.TransferProtocal);

            var serviceBasicTransferCommand = new SHPServiceBasicTransferCommand(new SHPServiceBasicPostDataCommand(transferBasicPolicy.DataCode, data));
            //transferChannel.SendData(serviceBasicTransferCommand.ToBytes(), serviceRouteTransferPolicy.TransferPolicy);
            this.SendData(serviceBasicTransferCommand.ToBytes(), transferBasicPolicy, transferChannel, serviceRouteTransferPolicy);
        }

        /// <summary>
        /// 提交文件
        /// </summary>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="transferBasicPolicy">传输基础策略,为null使用默认策略</param>
        public void PostFile(string filePath, TransferBasicPolicy transferBasicPolicy)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("要发送的文件不存在", filePath);
            }

            if (transferBasicPolicy == null)
            {
                throw new ArgumentNullException(nameof(transferBasicPolicy));
            }

            var size = new FileInfo(filePath).Length;
            ServiceRouteTransferPolicy serviceRouteTransferPolicy = this.GetServiceRouteTransferPolicy(transferBasicPolicy, size);
            var transferChannel = this.GetSendTransferChannel(serviceRouteTransferPolicy.ServiceRouteInfo.TransferProtocal);
            //transferChannel.SendFile(filePath, serviceRouteTransferPolicy.TransferPolicy);
            this.PrimitiveSendFile(filePath, transferBasicPolicy, transferChannel, serviceRouteTransferPolicy);
        }
        #endregion

        #region IDispose 接口
        /// <summary>
        /// IDispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">是否正在释放</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                this._parseDataQueue.Stop();
                this._parseDataQueue.Dispose();

                foreach (var transferChannel in this._transferProtocalTransferChannelDic.Values)
                {
                    transferChannel.Dispose();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose发生异常");
            }
        }
        #endregion
    }
}
