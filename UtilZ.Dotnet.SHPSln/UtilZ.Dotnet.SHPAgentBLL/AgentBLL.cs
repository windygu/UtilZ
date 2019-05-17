using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilZ.Dotnet.Compress;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.DataStruct;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPAgentBLL.CommandExcutors;
using UtilZ.Dotnet.SHPAgentDAL;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPAutoPatchBase;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.Net;
using UtilZ.Dotnet.SHPBase.Plugin.PluginABase;
using UtilZ.Dotnet.Ex.RestFullBase;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    public class AgentBLL : IDisposable
    {
        private readonly AgentProtectMonitor _agentProtectMonitor;
        private readonly AppMonitorManagement _appMonitorManagement;
        private readonly SHPPluginManager<ISHPADevOps, ISHPAHardCollect> _pluginManager = new SHPPluginManager<ISHPADevOps, ISHPAHardCollect>();
        private readonly SHPCommandExcutorManager _commandExcutorManager = new SHPCommandExcutorManager();
        private readonly AsynQueue<ReceiveDataItem> _parseDataQueue;
        private readonly TransferChannel _transferChannel;
        private readonly ISHPNet _net;
        public readonly AgentDAL _dal;
        public readonly DevOpsInfoCollection _devOpsInfoCollection = new DevOpsInfoCollection();
        private HostLoadsUploader _hostLoadsUploader;
        private readonly ServiceInstanceManager _serviceInstanceManager;

        public IAppMonitorManagement AppMonitorItemManager
        {
            get { return _appMonitorManagement; }
        }

        internal ServiceInstanceManager ServiceInstanceManager
        {
            get { return _serviceInstanceManager; }
        }

        public AgentBLL(string protectAppPath, int agentProtectProcessId, ICollectionOwner owner)
        {
            Config.Load<Config>("config.xml");

            this._agentProtectMonitor = new AgentProtectMonitor(protectAppPath, agentProtectProcessId);
            this._appMonitorManagement = new AppMonitorManagement(owner);

            this._parseDataQueue = new AsynQueue<ReceiveDataItem>(this.ParseDataCallback, "解析数据线程", true, false);
            var transferConfig = new TransferConfig();
            transferConfig.NetConfig.ListenEP = new IPEndPoint(IPAddress.Any, Config.Instance.AgentPort);
            this._transferChannel = new TransferChannel(transferConfig, (t) => { this._parseDataQueue.Enqueue(t); });
            this._net = new SHPNet(this._transferChannel, Config.Instance);
            this._dal = new AgentDAL();
            this._serviceInstanceManager = new ServiceInstanceManager(this._dal, this._appMonitorManagement, this._devOpsInfoCollection, this._transferChannel);
        }

        public void AssertRemoveMonitor(AppMonitorItem appMonitorItem)
        {
            if (this._serviceInstanceManager.MonitorIsServiceIns(appMonitorItem))
            {
                throw new InvalidOperationException($"监视项[{appMonitorItem.AppName}]是服务,拒绝操作");
            }
        }

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

        /// <summary>
        /// 处理插件命令
        /// </summary>
        /// <param name="receiveDataItem"></param>
        /// <param name="transferCommand"></param>
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
            ICommandExcutor agentCommandExcutor = this._commandExcutorManager.GetExcutorByCommand(transferCommand.Cmd);
            agentCommandExcutor.ProCommand(transferCommand);
        }
        #endregion

        #region 升级
        public void TestUpgrade()
        {
            string upgradePackge = @"D:\Demo\UtilZ.Dotnet\UtilZ.Dotnet.Agent\bin\Patch.zip";
            this.Upgrade(upgradePackge);
        }

        private void Upgrade(string upgradePackge)
        {
            try
            {
                string currenAppFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                string currentDir = Path.GetDirectoryName(currenAppFilePath);
                string autoPatchAppExeFilePath = Path.Combine(currentDir, @"AutoPatch\UtilZ.Dotnet.AutoPatch.exe");
                //if (!File.Exists(autoPatchAppExeFilePath))
                //{
                string autoPatchZipFilePath = Path.Combine(currentDir, "AutoPatch.zip");
                if (!File.Exists(autoPatchZipFilePath))
                {
                    Loger.Warn($"{autoPatchZipFilePath}文件不存在");
                    return;
                }

                CompressHelper.DeCompressZip(autoPatchZipFilePath, new FileInfo(autoPatchAppExeFilePath).Directory.FullName);
                //CompressEx.UnCompressZip(autoPatchZipFilePath, new FileInfo(autoPatchAppExeFilePath).Directory.FullName);
                if (!File.Exists(autoPatchAppExeFilePath))
                {
                    Loger.Warn($"{autoPatchAppExeFilePath}文件不存在");
                    return;
                }
                //}

                //杀死保护程序
                this._agentProtectMonitor.Stop();

                //启动升级程序
                var startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = autoPatchAppExeFilePath;

                StringBuilder sbArgs = new StringBuilder();
                sbArgs.Append($"{AutoPatchOptions.PROCESS_ID} {Process.GetCurrentProcess().Id} ");
                sbArgs.Append($"{AutoPatchOptions.UPGRADE_PACKGE_FILE_PATH} {upgradePackge} ");
                sbArgs.Append($"{AutoPatchOptions.UPGRADE_PACKGE_TYPE} {UpgradePackgeTypes.ZIP} ");
                sbArgs.Append($"{AutoPatchOptions.DIRECTORY} {currentDir} ");
                sbArgs.Append($"{AutoPatchOptions.APP_EXE_FILE_PATH} {currenAppFilePath}");
                startInfo.Arguments = sbArgs.ToString();
                startInfo.WorkingDirectory = currentDir;
                var agentProtectPro = Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "执行升级异常");
            }
        }
        #endregion

        public void Start()
        {
            Loger.Info("正在启动...");
            try
            {
                Directory.Delete(Config.Instance.TmpDir, true);
            }
            catch
            { }

            SHPCommandDefine.Init<SHPCommandDefine>();
            this._commandExcutorManager.Init(typeof(IAgentCommandExcutor).Assembly, this.CommandExcutorAdvanceSetting);

            Loger.Info("加载插件开始...");
            this._pluginManager.LoadPlugin("Plugins", this._net);
            Loger.Info("加载插件完成...");

            this._hostLoadsUploader = new HostLoadsUploader(this._devOpsInfoCollection, this._transferChannel, this._pluginManager.GetTHPlugins(), this._appMonitorManagement, this._serviceInstanceManager);
            this._hostLoadsUploader.Start();
            this._agentProtectMonitor.Start();
            this._appMonitorManagement.Start();
            this._parseDataQueue.Start();
            this._devOpsInfoCollection.AddRange(this._dal.QueryAllDevOps());
            this._transferChannel.Start();

            //注册并启动RestFull服务
            var uri = new Uri(AgentServiceMethodNameConstant.AGENT_SERVICE_BASE_URL);
            var serviceLauncher = new RestFullServiceLauncher<IAgentService>(uri, new AgentService(this._serviceInstanceManager));
            RestFullServiceLauncherManager.RegistRestFullServiceLauncher(serviceLauncher);
            RestFullServiceLauncherManager.Start();

            Loger.Info("启动完成...");
        }

        private void CommandExcutorAdvanceSetting(ICommandExcutor commandExcutor)
        {
            var agentCommandExcutor = commandExcutor as IAgentCommandExcutor;
            if (agentCommandExcutor == null)
            {
                return;
            }

            agentCommandExcutor.BLL = this;
            agentCommandExcutor.Net = this._net;
        }

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {
            try
            {
                this._hostLoadsUploader.Dispose();
                this._transferChannel.Dispose();
                this._agentProtectMonitor.Stop();
                this._appMonitorManagement.Dispose();
                this._parseDataQueue.Dispose();
                RestFullServiceLauncherManager.Stop();
                this._serviceInstanceManager.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
