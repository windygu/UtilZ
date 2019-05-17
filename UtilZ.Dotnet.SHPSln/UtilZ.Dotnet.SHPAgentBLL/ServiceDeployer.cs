using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Compress;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.FileTransfer;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    internal class ServiceDeployer : IDisposable
    {
        private readonly Stopwatch _watch;
        private readonly string _serviceDeployDir;
        private readonly string _serviceMirrorLocalFilePath;
        private readonly ServiceDeployPara _serviceDeployPara;
        private readonly IAppMonitorManagement _appMonitorItemManager;
        private readonly string _serviceInsName;

        private readonly IFileTransfer _fileTransfer;
        private long _position = 0;

        private string _appExeFilePath = string.Empty;
        private string _appProcessFilePath = string.Empty;
        private readonly AutoResetEvent _deployEventHandler = new AutoResetEvent(false);
        private ServiceInsListenInfo _serviceInsListenInfo = null;

        public ServiceDeployPara ServiceDeployPara
        {
            get { return _serviceDeployPara; }
        }

        public string ServiceInsName
        {
            get { return _serviceInsName; }
        }

        public ServiceDeployer(string serviceDeployBaseDir, ServiceDeployPara serviceDeployPara, IAppMonitorManagement appMonitorItemManager)
        {
            this._serviceDeployPara = serviceDeployPara;
            this._appMonitorItemManager = appMonitorItemManager;
            this._serviceInsName = serviceDeployPara.ServiceInsName;
            this._serviceDeployDir = Path.Combine(serviceDeployBaseDir, this._serviceInsName);

            string serviceMirrorLocalFileName = $"{TimeEx.GetTimestamp()}{Path.GetExtension(serviceDeployPara.MirrorFilePath)}";
            this._serviceMirrorLocalFilePath = Path.GetFullPath(Path.Combine(Config.Instance.TmpDir, serviceMirrorLocalFileName));
            DirectoryInfoEx.CheckFilePathDirectory(this._serviceMirrorLocalFilePath);
            this._fileTransfer = FileTransferFactory.Create(serviceDeployPara.BaseUrl, serviceDeployPara.FileServiceUsername, serviceDeployPara.FileServicePassword);
            this._watch = Stopwatch.StartNew();
        }

        internal ServiceListenInfo Deploy()
        {
            Loger.Info($"部署服务[{this._serviceInsName}]开始...");
            var millisecondsTimeout = this._serviceDeployPara.MillisecondsTimeout;
            while (true)
            {
                try
                {
                    if (millisecondsTimeout != Timeout.Infinite && millisecondsTimeout > 0)
                    {
                        if (this._watch.ElapsedMilliseconds >= millisecondsTimeout)
                        {
                            throw new TimeoutException($"下载服务[{this._serviceInsName}]镜像超时,超时时长[{millisecondsTimeout}]");
                        }
                    }

                    this._fileTransfer.Download(this._serviceDeployPara.MirrorFilePath, this._serviceMirrorLocalFilePath, 0, -1, this._position, this.DownloadScheduleNotify);
                    Loger.Info($"服务[{this._serviceInsName}]镜像下载完成...");
                    break;
                }
                catch (TimeoutException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    //断点续传,减少数据传输量
                    Loger.Error(ex, $"下载服务[{this._serviceInsName}]镜像异常");
                }
            }

            //部署服务
            this.DeployService();

            //启动服务
            ServiceListenInfo serviceListenInfo = this.StartService();
            Loger.Info($"[{this._serviceInsName}]部署服务完成...");
            return serviceListenInfo;
        }

        internal bool IsDeploying(string filePath)
        {
            return string.Equals(filePath, this._appExeFilePath, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(filePath, this._appProcessFilePath, StringComparison.OrdinalIgnoreCase);
        }

        private ServiceListenInfo StartService()
        {
            int surplusMillisecondsTimeout = this._serviceDeployPara.MillisecondsTimeout - (int)this._watch.ElapsedMilliseconds;
            if (surplusMillisecondsTimeout < 1)
            {
                throw new TimeoutException($"[{this._serviceInsName}]部署解压超时...");
            }

            ProcessEx.Start(this._appExeFilePath, this._serviceDeployPara.Arguments);
            if (this._deployEventHandler.WaitOne(surplusMillisecondsTimeout))
            {
                Loger.Info($"第一次启动服务[{this._serviceInsName}]完成...");

                var appMonitor = new AppMonitorItem(this._serviceInsName, this._appExeFilePath, this._appProcessFilePath, this._serviceDeployPara.Arguments);
                this._appMonitorItemManager.AddMonitorItem(appMonitor);
                Loger.Info($"服务[{this._serviceInsName}]添加到监视完成...");

                Loger.Info($"服务[{this._serviceInsName}]部署完成...");

                return new SHPBase.Model.ServiceListenInfo(this._serviceDeployPara.Id, this._serviceInsListenInfo.ListenPort);
            }
            else
            {
                throw new TimeoutException($"启动服务[{this._serviceInsName}]超时...");
            }
        }

        internal void UploadServiceInsListenInfo(ServiceInsListenInfo para)
        {
            this._serviceInsListenInfo = para;
            try
            {
                this._deployEventHandler.Set();
            }
            catch (ObjectDisposedException)
            { }
        }

        private void DeployService()
        {
            Loger.Info($"部署服务[{this._serviceInsName}]开始...");
            DirectoryInfoEx.CheckDirectory(this._serviceDeployDir);
            switch (this._serviceDeployPara.ServiceMirrorType)
            {
                case ServiceMirrorType.Zip:
                    CompressHelper.DeCompressZip(this._serviceMirrorLocalFilePath, this._serviceDeployDir);
                    break;
                case ServiceMirrorType.Rar:
                    CompressHelper.DecompressRar(this._serviceMirrorLocalFilePath, this._serviceDeployDir, true);
                    break;
                default:
                    throw new NotImplementedException($"未实现的服务镜像类型[{this._serviceDeployPara.ServiceMirrorType.ToString()}]");
            }

            string appExeFilePath = Path.Combine(this._serviceDeployDir, this._serviceDeployPara.AppExeFilePath);
            string appProcessFilePath = Path.Combine(this._serviceDeployDir, this._serviceDeployPara.AppProcessFilePath);

            if (!File.Exists(appExeFilePath))
            {
                throw new InvalidOperationException($"服务[{this._serviceInsName}]启动程序[{appExeFilePath}]不存在");
            }

            if (!File.Exists(appProcessFilePath))
            {
                throw new InvalidOperationException($"服务[{this._serviceInsName}]进程程序[{appProcessFilePath}]不存在");
            }

            this._appExeFilePath = appExeFilePath;
            this._appProcessFilePath = appProcessFilePath;

            Loger.Info($"部署服务[{this._serviceInsName}]完成...");
        }

        /// <summary>
        /// 进度回调
        /// </summary>
        /// <param name="">传输总大小</param>
        /// <param name="">已传输数据大小</param>
        private void DownloadScheduleNotify(long totalSize, long transferSize)
        {
            this._position = transferSize;
            Loger.Info($"正在下载服务[{this._serviceInsName}]镜像[{transferSize}/{totalSize}]...");
        }

        public void Dispose()
        {
            try
            {
                this._watch.Stop();
                FileEx.TryDeleFile(this._serviceMirrorLocalFilePath);
                this._deployEventHandler.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
