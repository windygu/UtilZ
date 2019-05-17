using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Net;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;
using UtilZ.Dotnet.SHPDevOpsBLL.CommandExcutors;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    public class DevOpsBLL : ICommandSender, IDisposable
    {
        private readonly SHPPluginManager<ISHPDDevOps, ISHPHardDisplay> _pluginManager = new SHPPluginManager<ISHPDDevOps, ISHPHardDisplay>();
        private readonly SHPCommandExcutorManager _commandExcutorManager = new SHPCommandExcutorManager();

        private readonly AsynQueue<ReceiveDataItem> _parseDataQueue;

        private readonly TransferChannel _transferChannel;
        private readonly ISHPNet _net;


        private readonly HostManager _hostManager;

        public HostManager HostManager
        {
            get { return _hostManager; }
        }

        private readonly ServiceManager _serviceManager;
        public ServiceManager ServiceManager
        {
            get { return _serviceManager; }
        }


        private readonly RouteManager _routeManager;
        public RouteManager RouteManager
        {
            get { return _routeManager; }
        }

        private readonly HostStatusInfoManager _hostStatusInfoManager;
        public HostStatusInfoManager HostStatusInfoManager
        {
            get { return _hostStatusInfoManager; }
        }

        public DevOpsBLL(ICollectionOwner owner)
        {
            Config.Load<Config>("config.xml");

            this._parseDataQueue = new AsynQueue<ReceiveDataItem>(this.ParseDataCallback, "解析数据线程", true, false);
            var transferConfig = new TransferConfig();
            transferConfig.NetConfig.ListenEP = new IPEndPoint(IPAddress.Any, Config.Instance.DevOpsPort);
            this._transferChannel = new TransferChannel(transferConfig, (t) => { this._parseDataQueue.Enqueue(t); });
            this._net = new SHPNet(this._transferChannel, Config.Instance);

            this._hostStatusInfoManager = new HostStatusInfoManager();
            this._routeManager = new RouteManager(this, this._hostStatusInfoManager, owner);
            this._hostManager = new HostManager(this, this._pluginManager, this._routeManager, this._hostStatusInfoManager, owner);
            this._serviceManager = new ServiceManager(this, this._routeManager, owner);
        }

        #region ICommandSender接口
        public byte[] SendInteractiveCommandHost<T>(T command, string ip, int millisecondsTimeout) where T : ICommand
        {
            var transferCommand = new SHPTransferCommand(SHPTransferCommand.CreateContextId(), SHPConstant.SHP_PLUGINID, command, millisecondsTimeout);
            return this._net.SendInteractiveCommandHost(transferCommand, ip);
        }

        public void SendCommandHost<T>(T command, string ip) where T : ICommand
        {
            var transferCommand = new SHPTransferCommand(SHPTransferCommand.DefaultContextId, SHPConstant.SHP_PLUGINID, command, Config.Instance.SendCommandDefaultTimeout);
            this._net.SendCommandHost(transferCommand, ip);
        }
        #endregion

        #region 处理接收的命令
        private void ParseDataCallback(ReceiveDataItem receiveDataItem)
        {
            try
            {
                using (var ms = new MemoryStream(receiveDataItem.Data))
                {
                    var br = new BinaryReader(ms);
                    var transferCommand = new SHPTransferCommand();
                    transferCommand.Parse(br, receiveDataItem.SrcEndPoint);
                    if (SHPConstant.SHP_PLUGINID == transferCommand.PluginId)
                    {
                        //平台内置命令
                        this.ProInnerCommand(receiveDataItem, transferCommand);
                    }
                    else
                    {
                        //插件命令
                        this.ProPluginCommand(receiveDataItem, transferCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "处理数据异常");
            }
        }

        private void ProPluginCommand(ReceiveDataItem receiveDataItem, SHPTransferCommand transferCommand)
        {
            var plugin = this._pluginManager.GetTDPluginById(transferCommand.PluginId);
            if (plugin != null)
            {
                plugin.Plugin.ExcuteCommand(transferCommand);
            }
            else
            {
                Loger.Warn($"命令[{transferCommand.Cmd}]对应的插件不存在...");
            }
        }

        /// <summary>
        /// 处理平台内部命令
        /// </summary>
        /// <param name="receiveDataItem"></param>
        /// <param name="transferCommand"></param>
        private void ProInnerCommand(ReceiveDataItem receiveDataItem, SHPTransferCommand transferCommand)
        {
            ICommandExcutor devOpsCommandExcutor = this._commandExcutorManager.GetExcutorByCommand(transferCommand.Cmd);
            devOpsCommandExcutor.ProCommand(transferCommand);
        }
        #endregion


        public Dictionary<int, PluginInfo<ISHPDDevOps>>.ValueCollection GetDDevOpsPlugins()
        {
            return this._pluginManager.GetTDPlugins();
        }

        public void Start()
        {
            Loger.Info("正在启动...");
            SHPCommandDefine.Init<SHPCommandDefine>();
            this.RegisterSyncResponseCommand();
            this._commandExcutorManager.Init(typeof(IDevOpsCommandExcutor).Assembly, this.CommandExcutorAdvanceSetting);

            Loger.Info("加载插件开始...");
            this._pluginManager.LoadPlugin("Plugins", this._net);
            Loger.Info("加载插件完成...");

            this._hostManager.Start();
            this._serviceManager.Start();
            this._routeManager.Start();

            this._parseDataQueue.Start();
            this._transferChannel.Start();

            //注册并启动RestFull服务
            var uri = new Uri(Config.Instance.ServiceRouteUrl);
            var serviceRouteServiceLauncher = new RestFullServiceLauncher<IServiceRouteService>(uri, new ServiceRouteService(this._routeManager.GetServiceRouteInfo));
            RestFullServiceLauncherManager.RegistRestFullServiceLauncher(serviceRouteServiceLauncher);
            RestFullServiceLauncherManager.Start();

            Loger.Info("启动完成...");
        }

        private void RegisterSyncResponseCommand()
        {
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.ADD_HOST_RES);
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.KILL_PROCESS_RES);
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.CONTROL_APP_RES);
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.PROCESS_ADD_TO_MONITOR_RES);
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.REMOVE_MONITOR_APP_RES);
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.EXCUTE_SCRPT_RES);
            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(SHPCommandDefine.SERVICE_DEPLOY_RES);
        }

        private void CommandExcutorAdvanceSetting(ICommandExcutor commandExcutor)
        {
            commandExcutor.Net = this._net;
            var devOpsExcutor = commandExcutor as IDevOpsCommandExcutor;
            if (devOpsExcutor == null)
            {
                return;
            }

            devOpsExcutor.BLL = this;
        }

        public void Dispose()
        {
            try
            {
                this._transferChannel.Dispose();
                this._hostManager.Dispose();
                this._parseDataQueue.Dispose();
                RestFullServiceLauncherManager.Stop();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
