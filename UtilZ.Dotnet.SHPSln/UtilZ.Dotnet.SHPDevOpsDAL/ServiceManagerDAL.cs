using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.AsynWait;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.FileTransfer;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsDAL
{
    public class ServiceManagerDAL : BaseDAL
    {
        private readonly IFileTransfer _fileTransfer;
        private const string _SERVICE_DIR = "SHPServiceMirror";

        public ServiceManagerDAL()
            : base()
        {
            var config = Config.Instance;
            this._fileTransfer = FileTransferFactory.Create(config.ServiceMirrorFtpUrl, config.FileServiceUserName, config.FileServicePassword);
        }

        private string GetServiceMirrorRemoteFileName(ServiceMirrorInfo serviceMirrorInfo)
        {
            return Path.Combine(_SERVICE_DIR, serviceMirrorInfo.ServiceInfoId.ToString(), serviceMirrorInfo.Version.ToString(), Path.GetFileName(serviceMirrorInfo.MirrorFilePath));
        }

        private string GetServiceMirrorRootDir()
        {
            return _SERVICE_DIR;
        }

        private string GetServiceMirrorDir(long serviceInfoId)
        {
            return Path.Combine(_SERVICE_DIR, serviceInfoId.ToString());
        }

        public void AddService(ServiceInfo serviceInfoItem)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<ServiceInfo>() where t.Name == serviceInfoItem.Name select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"已存在名称为[{ serviceInfoItem.Name}]的服务...");
                }

                serviceInfoItem.Id = base.CreateId();
                odb.Store(serviceInfoItem);
            }
        }

        private string GenerateScheduleNotifyHint(long total, long size)
        {
            return $"已传输{size * 100 / total}%,请稍候...";
        }

        public void RemoveService(ServiceInfo serviceTypeItem)
        {
            using (var odb = base.OpenDB())
            {
                var dataRouteCount = (from t in odb.AsQueryable<DataRouteInfo>() where t.ServiceInfoId == serviceTypeItem.Id select t).Count();
                if (dataRouteCount > 0)
                {
                    throw new InvalidOperationException($"存在{dataRouteCount}条数据路由,要清空数据路由请先删除相应的数据路由");
                }

                this._fileTransfer.DeleteDirectory(this.GetServiceMirrorDir(serviceTypeItem.Id), true);

                var serviceMirrorInfos = (from t in odb.AsQueryable<ServiceMirrorInfo>() where t.ServiceInfoId == serviceTypeItem.Id select t).ToArray();
                foreach (var serviceMirrorInfo in serviceMirrorInfos)
                {
                    //this._fileTransfer.DeleteFile(serviceMirrorInfo.MirrorFilePath);
                    odb.Delete(serviceMirrorInfo);
                }

                var serviceInfoItems = (from t in odb.AsQueryable<ServiceInfo>() where t.Id == serviceTypeItem.Id select t).ToArray();
                foreach (var serviceInfoItem in serviceInfoItems)
                {
                    odb.Delete(serviceInfoItem);
                }
            }
        }

        public void ModifyService(ServiceInfo serviceInfoItem)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<ServiceInfo>() where t.Id != serviceInfoItem.Id && t.Name == serviceInfoItem.Name select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"已存在名称为[{ serviceInfoItem.Name}]的服务...");
                }

                var oldServiceInfoItem = (from t in odb.AsQueryable<ServiceInfo>() where t.Id == serviceInfoItem.Id select t).FirstOrDefault();
                if (oldServiceInfoItem == null)
                {
                    throw new ApplicationException($"服务[{serviceInfoItem.Name}]记录不存在");
                }
                else
                {
                    oldServiceInfoItem.Update(serviceInfoItem);
                }

                odb.Store(oldServiceInfoItem);
            }
        }

        public void ClearService()
        {
            using (var odb = base.OpenDB())
            {
                var dataRouteCount = (from t in odb.AsQueryable<DataRouteInfo>() select t).Count();
                if (dataRouteCount > 0)
                {
                    throw new InvalidOperationException($"存在{dataRouteCount}条数据路由,要清空数据路由请先清空数据路由");
                }

                //删除服务镜像
                this._fileTransfer.DeleteDirectory(this.GetServiceMirrorRootDir(), true);

                var items = (from t in odb.AsQueryable<ServiceInfo>() select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }

                var serviceMirrorInfos = (from t in odb.AsQueryable<ServiceMirrorInfo>() select t).ToArray();
                foreach (var serviceMirrorInfo in serviceMirrorInfos)
                {
                    //this._fileTransfer.DeleteFile(serviceMirrorInfo.MirrorFilePath);
                    odb.Delete(serviceMirrorInfo);
                }
            }
        }

        public ServiceInfo[] QueryAllServiceInfoItem()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<ServiceInfo>() select t).ToArray();
            }
        }

        public ServiceMirrorInfo[] QueryAllServiceInfoMirror()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<ServiceMirrorInfo>() select t).ToArray();
            }
        }

        public void AddServiceMirror(ServiceMirrorInfo serviceMirrorInfo, IPartAsynWait asynWait)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<ServiceMirrorInfo>()
                             where t.ServiceInfoId == serviceMirrorInfo.ServiceInfoId
                             && t.Version == serviceMirrorInfo.Version
                             select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"已存在版本为[{ serviceMirrorInfo.Version}]的镜像...");
                }

                serviceMirrorInfo.Id = base.CreateId();
                var remoteName = this.GetServiceMirrorRemoteFileName(serviceMirrorInfo);
                this._fileTransfer.Upload(remoteName, serviceMirrorInfo.MirrorFilePath, 0, -1, UpdateMode.Create, 0, (t, s) =>
                {
                    asynWait.Hint = this.GenerateScheduleNotifyHint(t, s);
                });
                serviceMirrorInfo.MirrorFilePath = remoteName;
                serviceMirrorInfo.Id = base.CreateId();
                odb.Store(serviceMirrorInfo);
            }
        }

        public void DeleteServiceMirrorById(long serviceMirrorInfoId)
        {
            using (var odb = base.OpenDB())
            {
                var serviceMirrorInfos = (from t in odb.AsQueryable<ServiceMirrorInfo>() where t.Id == serviceMirrorInfoId select t).ToArray();
                foreach (var serviceMirrorInfo in serviceMirrorInfos)
                {
                    this._fileTransfer.DeleteFile(serviceMirrorInfo.MirrorFilePath);
                    odb.Delete(serviceMirrorInfo);
                }
            }
        }

        public void UsageServiceMirror(ServiceInfo serviceInfoItem, ServiceMirrorInfo serviceMirrorInfo)
        {
            using (var odb = base.OpenDB())
            {
                var oldServiceInfoItem = (from t in odb.AsQueryable<ServiceInfo>() where t.Id == serviceInfoItem.Id select t).FirstOrDefault();
                if (oldServiceInfoItem == null)
                {
                    throw new ApplicationException($"服务[{serviceInfoItem.Name}]记录不存在");
                }

                oldServiceInfoItem.ServiceMirrorId = serviceMirrorInfo.Id;
                odb.Store(oldServiceInfoItem);
                //serviceInfoItem.Update(oldServiceInfoItem);
                serviceInfoItem.ServiceMirrorId = serviceMirrorInfo.Id;
            }
        }
    }
}
