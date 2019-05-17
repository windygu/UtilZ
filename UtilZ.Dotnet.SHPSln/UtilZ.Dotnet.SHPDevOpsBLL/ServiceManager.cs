using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.AsynWait;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsDAL;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    public class ServiceManager
    {
        private readonly ICommandSender _commandSender;
        private readonly RouteManager _routeManager;

        private readonly ServiceManagerDAL _dal = new ServiceManagerDAL();
        private readonly BindingCollection<ServiceInfo> _serviceInfoList;
        public BindingCollection<ServiceInfo> ServiceInfoList
        {
            get { return _serviceInfoList; }
        }

        private readonly Dictionary<long, ServiceMirrorInfo> _serviceMirrorInfoDic = new Dictionary<long, ServiceMirrorInfo>();

        public ServiceManager(ICommandSender sender, RouteManager routeManager, ICollectionOwner owner)
        {
            this._commandSender = sender;
            this._routeManager = routeManager;
            this._serviceInfoList = new BindingCollection<ServiceInfo>(owner);

            DataRouteInfo.GetServiceInfoByIdFunc = this.GetServiceInfoById;
            SHPServiceInsInfo.GetServiceInfoByIdFunc = this.GetServiceInfoById;
            ServiceInfo.GetServiceMirrorInfoByIdFunc = this.GetServiceMirrorInfoById;

            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_GET_SERVICE_INFO_CHANNEL, this.GetServiceInfo);
        }

        private object GetServiceInfo(object obj)
        {
            return this.GetServiceInfoById((long)obj);
        }

        private ServiceMirrorInfo GetServiceMirrorInfoById(long serviceMirrorId)
        {
            if (this._serviceMirrorInfoDic.ContainsKey(serviceMirrorId))
            {
                return this._serviceMirrorInfoDic[serviceMirrorId];
            }

            return null;
        }

        private ServiceInfo GetServiceInfoById(long serviceId)
        {
            lock (this._serviceInfoList.SyncRoot)
            {
                return this._serviceInfoList.Where(t => t.Id == serviceId).FirstOrDefault();
            }
        }

        public void Start()
        {
            ServiceInfo[] serviceInfoItems = this._dal.QueryAllServiceInfoItem();
            this._serviceInfoList.AddRange(serviceInfoItems);

            var serviceMirrorArr = this._dal.QueryAllServiceInfoMirror();
            foreach (var serviceMirror in serviceMirrorArr)
            {
                this._serviceMirrorInfoDic.Add(serviceMirror.Id, serviceMirror);
            }
        }

        public void AddServiceInfo(ServiceInfo serviceInfo)
        {
            this._dal.AddService(serviceInfo);
            this._serviceInfoList.Add(serviceInfo);
        }

        public void RemoveServiceInfo(ServiceInfo serviceInfo)
        {
            this._routeManager.DeleteServiceInsByServiceInfoId(serviceInfo.Id);

            this._dal.RemoveService(serviceInfo);
            this._serviceInfoList.Remove(serviceInfo);
            var preRemoveMirrorArr = this._serviceMirrorInfoDic.Values.Where(t => { return t.ServiceInfoId == serviceInfo.Id; }).ToArray();
            foreach (var serviceMirror in preRemoveMirrorArr)
            {
                this._serviceMirrorInfoDic.Remove(serviceMirror.Id);
            }
        }

        public ServiceMirrorInfo[] GetServiceInfoMirror(long serviceInfoId)
        {
            return this._serviceMirrorInfoDic.Values.Where(t => { return t.ServiceInfoId == serviceInfoId; }).ToArray();
        }

        public void ModifyServiceInfo(ServiceInfo serviceInfo)
        {
            this._dal.ModifyService(serviceInfo);
        }

        public void ClearServiceInfo()
        {
            this._dal.ClearService();
            this._serviceInfoList.Clear();
            this._serviceMirrorInfoDic.Clear();
        }

        public void AddServiceMirror(ServiceMirrorInfo serviceMirrorInfo, IPartAsynWait asynWait)
        {
            this._dal.AddServiceMirror(serviceMirrorInfo, asynWait);
            this._serviceMirrorInfoDic[serviceMirrorInfo.Id] = serviceMirrorInfo;
        }

        public void DeleteServiceMirrorById(long serviceMirrorInfoId)
        {
            this._dal.DeleteServiceMirrorById(serviceMirrorInfoId);
            if (this._serviceMirrorInfoDic.ContainsKey(serviceMirrorInfoId))
            {
                this._serviceMirrorInfoDic.Remove(serviceMirrorInfoId);
            }
        }

        public int QueryServiceInfoMirrorMaxVersion(long serviceInfoId)
        {
            if (this._serviceMirrorInfoDic.Count < 1)
            {
                return 0;
            }

            return this._serviceMirrorInfoDic.Values.Where(t => { return t.ServiceInfoId == serviceInfoId; }).Max(v => { return v.Version; });
        }

        public void UsageServiceMirror(ServiceInfo serviceInfoItem, ServiceMirrorInfo serviceMirrorInfo)
        {
            this._dal.UsageServiceMirror(serviceInfoItem, serviceMirrorInfo);
        }
    }
}
