using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsDAL
{
    public class RouteDAL : BaseDAL
    {
        public RouteDAL()
            : base()
        {

        }

        #region 数据路由
        public void AddDataRoute(DataRouteInfo dataRouteItem)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<DataRouteInfo>() where t.DataCode == dataRouteItem.DataCode select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"数据[{dataRouteItem.DataCode}]路由已存在...");
                }

                dataRouteItem.Id = base.CreateId();
                odb.Store(dataRouteItem);
            }
        }

        public void RemoveDataRoute(DataRouteInfo dataRouteItem)
        {
            using (var odb = base.OpenDB())
            {
                //删除服务实例中包含的数据路由
                var serviceInsArr = (from t in odb.AsQueryable<SHPServiceInsInfo>() where t.DataRouteIdList.Contains(dataRouteItem.Id) select t).ToArray();
                foreach (var serviceIns in serviceInsArr)
                {
                    serviceIns.DataRouteIdList.Remove(dataRouteItem.Id);
                    odb.Store(serviceIns);
                }

                //删除数据路由记录
                var items = (from t in odb.AsQueryable<DataRouteInfo>() where t.Id == dataRouteItem.Id select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }
            }
        }

        public void ModifyDataRoute(DataRouteInfo dataRouteItem)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<DataRouteInfo>() where t.Id != dataRouteItem.Id && t.DataCode == dataRouteItem.DataCode select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"数据[{dataRouteItem.DataCode}]路由已存在...");
                }

                var oldItem = (from t in odb.AsQueryable<DataRouteInfo>() where t.Id == dataRouteItem.Id select t).FirstOrDefault();
                if (oldItem == null)
                {
                    dataRouteItem.Id = base.CreateId();
                    oldItem = dataRouteItem;
                }
                else
                {
                    oldItem.Update(dataRouteItem);
                }

                odb.Store(oldItem);
            }
        }

        public void ClearDataRoute()
        {
            using (var odb = base.OpenDB())
            {
                //删除服务实例中包含的数据路由
                var serviceInsArr = (from t in odb.AsQueryable<SHPServiceInsInfo>() select t).ToArray();
                foreach (var serviceIns in serviceInsArr)
                {
                    serviceIns.DataRouteIdList.Clear();
                    odb.Store(serviceIns);
                }

                //删除数据路由记录
                var items = (from t in odb.AsQueryable<DataRouteInfo>() select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }
            }
        }

        public DataRouteInfo[] QueryAllDataRoute()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<DataRouteInfo>() select t).ToArray();
            }
        }
        #endregion

        #region 服务实例
        public void AddServiceIns(SHPServiceInsInfo shpServiceInsInfo)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<SHPServiceInsInfo>()
                             where t.DataRouteIdList == shpServiceInsInfo.DataRouteIdList
                             && t.HostId == shpServiceInsInfo.HostId
                             && t.EndPointPort == shpServiceInsInfo.EndPointPort
                             select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"服务实例[{shpServiceInsInfo.ToString()}]已存在...");
                }

                odb.Store(shpServiceInsInfo);
            }
        }

        public void UpdateServiceIns(SHPServiceInsInfo shpServiceInsInfo)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<SHPServiceInsInfo>()
                             where t.Id != shpServiceInsInfo.Id && t.DataRouteIdList == shpServiceInsInfo.DataRouteIdList
                             select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"服务实例[{shpServiceInsInfo.ToString()}]已存在...");
                }

                var oldItem = (from t in odb.AsQueryable<SHPServiceInsInfo>() where t.Id == shpServiceInsInfo.Id select t).FirstOrDefault();
                if (oldItem == null)
                {
                    throw new ApplicationException($"服务[{shpServiceInsInfo.Name}]的Id为[{shpServiceInsInfo.Id}]的服务实例不存在");
                }
                else
                {
                    oldItem.Update(shpServiceInsInfo);
                }

                odb.Store(oldItem);
            }
        }

        public SHPServiceInsInfo[] QueryAllServiceInsInfo()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<SHPServiceInsInfo>() where t.Status != SHPBase.Model.ServiceInsStatus.Delete select t).ToArray();
            }
        }

        public void AddDeleteServiceInsInfo(IEnumerable<SHPServiceInsInfo> shpServiceInsInfoArr)
        {
            var serviceRouteInfoDic = shpServiceInsInfoArr.ToDictionary(t =>
            {
                t.Status = SHPBase.Model.ServiceInsStatus.Delete;
                return t.Id;
            });

            using (var odb = base.OpenDB())
            {
                var items = (from t in odb.AsQueryable<SHPServiceInsInfo>() where serviceRouteInfoDic.ContainsKey(t.Id) select t).ToArray();
                foreach (var item in items)
                {
                    item.Status = SHPBase.Model.ServiceInsStatus.Delete;
                    odb.Store(item);
                }
            }
        }

        public IEnumerable<SHPServiceInsInfo> QueryAllDeleteServiceInsInfo()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<SHPServiceInsInfo>() where t.Status == SHPBase.Model.ServiceInsStatus.Delete select t).ToArray();
            }
        }

        public void RemoveDeleteServiceIns(List<long> idList)
        {
            using (var odb = base.OpenDB())
            {
                var items = (from t in odb.AsQueryable<SHPServiceInsInfo>() where idList.Contains(t.Id) select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }
            }
        }
        #endregion
    }
}
