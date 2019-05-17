using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.Plugin.PluginABase;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    internal class HostLoadsUploader : IDisposable
    {
        private readonly DevOpsInfoCollection _devOpsInfoCollection;
        private readonly TransferChannel _net;
        private readonly Dictionary<int, PluginInfo<ISHPAHardCollect>>.ValueCollection _plugins;
        private readonly IAppMonitorManagement _appMonitorItemManager;
        private readonly ServiceInstanceManager _serviceInstanceManager;

        private readonly ThreadEx _uploadLoadsThread;
        private readonly AutoResetEvent _uploadInterEventHandle = new AutoResetEvent(false);

        //负载
        private readonly ComputerInfo _computerInfo = new ComputerInfo();
        private readonly PerformanceCounter _cpuUsePerformanceCounter;
        private readonly List<NetworkInterfacePerformanceCounter> _networkInterfacePerformanceCounters = new List<NetworkInterfacePerformanceCounter>();

        internal HostLoadsUploader(DevOpsInfoCollection devOpsInfoCollection, TransferChannel net,
            Dictionary<int, PluginInfo<ISHPAHardCollect>>.ValueCollection plugins,
            IAppMonitorManagement appMonitorItemManager, ServiceInstanceManager serviceInstanceManager)
        {
            this._devOpsInfoCollection = devOpsInfoCollection;
            this._net = net;
            this._plugins = plugins;
            this._appMonitorItemManager = appMonitorItemManager;
            this._serviceInstanceManager = serviceInstanceManager;

            PerformanceCounterCategory cpuCategory = new PerformanceCounterCategory("Processor");
            this._cpuUsePerformanceCounter = cpuCategory.GetCounters("_Total")[0];

            PerformanceCounterCategory netCategory = new PerformanceCounterCategory("Network Interface");
            var instanceNames = netCategory.GetInstanceNames();
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var instanceName in instanceNames)
            {
                var networkInterface = this.GetNetworkInterfaceBytePerformanceCounterInstanceName(instanceName, networkInterfaces);
                if (networkInterface == null)
                {
                    Loger.Warn($"GetNetworkInterfaceBytePerformanceCounterInstanceName获取失败,[instanceName:{instanceName}],忽略");
                    continue;
                }

                this._networkInterfacePerformanceCounters.Add(new NetworkInterfacePerformanceCounter(instanceName, networkInterface));
            }


            this._uploadLoadsThread = new ThreadEx(this.UploadLoadsThreadMethod, "上报负载线程", true);
        }

        public void Start()
        {
            this._uploadLoadsThread.Start();
        }

        private void UploadLoadsThreadMethod(CancellationToken token)
        {
            int statusUploadIntervalMilliseconds;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    statusUploadIntervalMilliseconds = this._devOpsInfoCollection.GetStatusUploadIntervalMilliseconds();
                    if (this._uploadInterEventHandle.WaitOne(statusUploadIntervalMilliseconds))
                    {
                        continue;
                    }

                    DevOpsInfo[] devOpsHostInfos = this._devOpsInfoCollection.ToArray();
                    if (devOpsHostInfos.Length == 0)
                    {
                        continue;
                    }

                    var hostStatusInfo = this.GetHostLoads();

                    foreach (var devOpsHostInfo in devOpsHostInfos)
                    {
                        try
                        {
                            hostStatusInfo.Id = devOpsHostInfo.HostId;
                            hostStatusInfo.ServiceStatusInfoList = this._serviceInstanceManager.GetServiceStatusInfoListByDOID(devOpsHostInfo.Id);
                            var hostStatusInfoUploadCommand = new HostStatusInfoUploadCommand(hostStatusInfo);
                            var resTransferCommand = new SHPTransferCommand(SHPTransferCommand.DefaultContextId, SHPConstant.SHP_PLUGINID, hostStatusInfoUploadCommand, Config.Instance.HostStatusUploadTimeout);
                            var buffer = resTransferCommand.GenerateBuffer();
                            var policy = TransferPolicyManager.Instance.GetTransferPolicy(devOpsHostInfo.DevOpsIPEndPoint);
                            this._net.SendData(buffer, policy);
                        }
                        catch (TimeoutException)
                        {
                            //上报超时
                        }
                        catch (Exception ex)
                        {
                            Loger.Error(ex, $"上报负载状态到[{devOpsHostInfo.ToString()}]异常");
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        private HostStatusInfo GetHostLoads()
        {
            var hostStatusInfo = new HostStatusInfo();
            hostStatusInfo.CPU = this._cpuUsePerformanceCounter.NextValue();
            hostStatusInfo.UseMemory = this._computerInfo.TotalPhysicalMemory - this._computerInfo.AvailablePhysicalMemory;
            hostStatusInfo.TotalMemory = this._computerInfo.TotalPhysicalMemory;

            if (Config.Instance.UploadExtendStatus)
            {
                hostStatusInfo.Nets = this.GetNetLoads();
                hostStatusInfo.HostDiskInfos = this.GetDiskLoads();
                hostStatusInfo.ExtendStatusInfos = this.GetExtendStatusInfos();
                hostStatusInfo.ProcessInfoList = this.GetProcessInfoList();
                lock (this._appMonitorItemManager.AppMonitorList.SyncRoot)
                {
                    hostStatusInfo.AppMonitorItemList = this._appMonitorItemManager.AppMonitorList.ToList();
                }
            }

            return hostStatusInfo;
        }

        private List<HostProcessInfoItem> GetProcessInfoList()
        {
            var processInfoList = new List<HostProcessInfoItem>();
            var processArra = Process.GetProcesses();
            foreach (var process in processArra)
            {
                try
                {
                    var hostProcessInfoItem = new HostProcessInfoItem();
                    hostProcessInfoItem.Id = process.Id;
                    hostProcessInfoItem.Name = process.ProcessName;
                    hostProcessInfoItem.Cpu = 0;//todo..此值以后再说
                    hostProcessInfoItem.Memory = process.WorkingSet64 / 1048576;//MB
                    hostProcessInfoItem.ThreadCount = process.Threads.Count;
                    hostProcessInfoItem.HandleCount = process.HandleCount;
                    processInfoList.Add(hostProcessInfoItem);
                }
                catch
                { }
            }

            return processInfoList;
        }

        private List<ExtendInfo> GetExtendStatusInfos()
        {
            var extendStatusInfos = new List<ExtendInfo>();
            foreach (var plugin in this._plugins)
            {
                try
                {
                    extendStatusInfos.Add(new ExtendInfo(plugin.PluginAttribute.Id, plugin.Plugin.GetLoad()));
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, $"获取插件[{plugin.Plugin.GetType().FullName}]负载信息发生异常");
                }
            }

            return extendStatusInfos;
        }

        private List<HostDiskLoadItem> GetDiskLoads()
        {
            var items = new List<HostDiskLoadItem>();
            var drivers = DriveInfo.GetDrives();
            foreach (var driveInfo in drivers)
            {
                try
                {
                    var item = new HostDiskLoadItem();
                    item.DriveFormat = driveInfo.DriveFormat;
                    item.DriveType = driveInfo.DriveType;
                    item.Name = driveInfo.Name;

                    if (driveInfo.IsReady)
                    {
                        item.AvailableFreeSpace = driveInfo.AvailableFreeSpace;
                        item.TotalSize = driveInfo.TotalSize;
                    }

                    items.Add(item);
                }
                catch (IOException)
                { }
                catch (Exception ex)
                {
                    Loger.Error(ex, "获取存储状态异常");
                }
            }

            return items;
        }

        private List<NetInfoLoadItem> GetNetLoads()
        {
            var items = new List<NetInfoLoadItem>();
            foreach (var networkInterfacePerformanceCounter in this._networkInterfacePerformanceCounters)
            {
                items.Add(networkInterfacePerformanceCounter.Next());
            }

            return items;
        }

        private NetworkInterface GetNetworkInterfaceBytePerformanceCounterInstanceName(string instanceName, NetworkInterface[] networkInterfaces)
        {
            foreach (var networkInterface in networkInterfaces)
            {
                if (string.Equals(instanceName, networkInterface.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return networkInterface;
                }

                if (string.Equals(instanceName, networkInterface.Description.Replace('#', '_'), StringComparison.OrdinalIgnoreCase))
                {
                    return networkInterface;
                }
            }

            return null;
        }

        public void Dispose()
        {
            try
            {
                this._uploadLoadsThread.Stop();
                this._uploadInterEventHandle.Set();
                this._uploadLoadsThread.Dispose();
                this._uploadInterEventHandle.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }

    internal class NetworkInterfacePerformanceCounter
    {
        private readonly NetworkInterface _networkInterface;
        private readonly PerformanceCounter _receivePerformanceCounter;
        private readonly PerformanceCounter _sendPerformanceCounter;
        //private readonly PerformanceCounter _totalPerformanceCounter;
        public NetworkInterfacePerformanceCounter(string instanceName, NetworkInterface networkInterface)
        {
            _networkInterface = networkInterface;
            this._receivePerformanceCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instanceName, ".");
            var rev = this._receivePerformanceCounter.NextValue();
            this._sendPerformanceCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instanceName, ".");
            //this._totalPerformanceCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instanceName, ".");
        }

        public NetInfoLoadItem Next()
        {
            var item = new NetInfoLoadItem();

            try
            {
                item.Name = this._networkInterface.Name;
                item.Description = this._networkInterface.Description;
                item.Speed = this._networkInterface.Speed;

                IPInterfaceProperties ipp = this._networkInterface.GetIPProperties();
                UnicastIPAddressInformation xx = ipp.UnicastAddresses.Where(t => { return t.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork; }).FirstOrDefault();
                if (xx != null)
                {
                    item.IP = xx.Address.ToString();
                }

                item.Receive = this._receivePerformanceCounter.NextValue();
                item.Send = this._sendPerformanceCounter.NextValue();
                //item.Total = this._totalPerformanceCounter.NextValue();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            return item;
        }
    }
}
