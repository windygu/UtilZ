using NDatabase;
using NDatabase.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsModel;

namespace UtilZ.Dotnet.SHPDevOpsDAL
{
    public class HostManagerDAL : BaseDAL
    {
        public HostManagerDAL()
            : base()
        {

        }

        #region 主机分组
        public void AddHostGroup(HostGroup hostGroup)
        {
            hostGroup.Id = base.CreateId();
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<HostGroup>() where t.ParentId == hostGroup.ParentId && t.Name == hostGroup.Name select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"主机分组组名[{hostGroup.Name}]已存在...");
                }

                odb.Store(hostGroup);
            }
        }

        public void ModifyHostGroup(HostGroup hostGroup)
        {
            using (var odb = base.OpenDB())
            {
                var oldItem = (from t in odb.AsQueryable<HostGroup>() where t.Id == hostGroup.Id select t).FirstOrDefault();
                if (oldItem == null)
                {
                    hostGroup.Id = base.CreateId();
                    oldItem = hostGroup;
                }
                else
                {
                    var count = (from t in odb.AsQueryable<HostGroup>() where t.ParentId == hostGroup.ParentId && t.Name == hostGroup.Name select t).Count();
                    if (count > 0)
                    {
                        throw new ArgumentException($"主机分组组名[{hostGroup.Name}]已存在...");
                    }

                    oldItem.Update(hostGroup);
                }

                odb.Store(oldItem);
            }
        }

        public void DeleteHostGroupById(long hostGroupId)
        {
            using (var odb = base.OpenDB())
            {
                var hostGroups = (from t in odb.AsQueryable<HostGroup>() where t.Id == hostGroupId select t).ToArray();
                foreach (var hostGroup in hostGroups)
                {
                    this.PrimitiveDeleteHostGroup(odb, hostGroup);
                }
            }
        }

        private void PrimitiveDeleteHostGroup(IOdb odb, HostGroup hostGroup)
        {
            var hostInfos = (from h in odb.AsQueryable<HostInfo>() where h.HostGoupId == hostGroup.Id select h).ToArray();
            foreach (var hostInfo in hostInfos)
            {
                odb.Delete(hostInfo);
            }

            odb.Delete(hostGroup);

            var childGroups = (from g in odb.AsQueryable<HostGroup>() where g.ParentId == hostGroup.Id select g).ToArray();
            foreach (var childGroup in childGroups)
            {
                this.PrimitiveDeleteHostGroup(odb, childGroup);
            }
        }

        public List<HostGroup> QueryHostGroupList()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<HostGroup>() select t).ToList();
            }
        }
        #endregion

        #region 主机
        public void AddHost(HostInfo hostInfo)
        {
            hostInfo.Id = base.CreateId();
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<HostInfo>() where t.Ip == hostInfo.Ip select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"IP为[{ hostInfo.Ip}]的主机已存在...");
                }

                odb.Store(hostInfo);
            }
        }

        public void ModifyHost(HostInfo hostInfo)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<HostInfo>() where t.Id != hostInfo.Id && t.Ip == hostInfo.Ip select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"IP为[{ hostInfo.Ip}]的主机已存在...");
                }

                var oldItem = (from t in odb.AsQueryable<HostInfo>() where t.Id == hostInfo.Id select t).FirstOrDefault();
                if (oldItem == null)
                {
                    hostInfo.Id = base.CreateId();
                    oldItem = hostInfo;
                }
                else
                {
                    oldItem.Update(hostInfo);
                }

                odb.Store(oldItem);
            }
        }

        public void DeleteHost(HostInfo hostInfo)
        {
            using (var odb = base.OpenDB())
            {
                var serviceRouteCount = (from t in odb.AsQueryable<SHPServiceInsInfo>() where t.HostId == hostInfo.Id select t.HostId).Count();
                if (serviceRouteCount > 0)
                {
                    throw new InvalidOperationException($"主机上存在{serviceRouteCount}条手动部署服务的服务实例,要删除主机请先删除对应的服务实例");
                }

                var items = (from t in odb.AsQueryable<HostInfo>() where t.Id == hostInfo.Id select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }
            }
        }

        public List<HostInfo> QueryHostListByHostGroupID(long hostGroupId)
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<HostInfo>() where t.HostGoupId == hostGroupId select t).ToList();
            }
        }

        public HostInfo[] QueryAllHostinfo()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<HostInfo>() select t).ToArray();
            }
        }
        #endregion

        #region 主机HostHardInfo
        public void UpdateHostHardInfo(HostHardInfo hostHardInfo)
        {
            using (var odb = base.OpenDB())
            {
                var items = (from t in odb.AsQueryable<HostHardInfo>() where t.Id == hostHardInfo.Id select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }

                odb.Store(hostHardInfo);
            }
        }

        public HostHardInfo GetHostHardInfoByHostId(long hostId)
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<HostHardInfo>() where t.Id == hostId select t).FirstOrDefault();
            }
        }

        public HostHardInfo[] QueryAllHostHardInfo()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<HostHardInfo>() select t).ToArray();
            }
        }

        public void DeleteHostHardInfo(IEnumerable<HostHardInfo> delHostHardInfoList)
        {
            if (delHostHardInfoList == null || delHostHardInfoList.Count() == 0)
            {
                return;
            }

            using (var odb = base.OpenDB())
            {
                foreach (var delHostHardInfo in delHostHardInfoList)
                {
                    var items = (from t in odb.AsQueryable<HostHardInfo>() where t.Id == delHostHardInfo.Id select t).ToArray();
                    foreach (var item in items)
                    {
                        odb.Delete(item);
                    }
                }
            }
        }
        #endregion

        #region 主机类型
        public void AddHostType(HostTypeItem hostTypeItem)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<HostTypeItem>() where t.Name == hostTypeItem.Name select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"主机类型[{ hostTypeItem.Name}]已存在...");
                }

                hostTypeItem.Id = base.CreateId();
                odb.Store(hostTypeItem);
            }
        }

        public void DeleteHostType(HostTypeItem hostTypeItem)
        {
            using (var odb = base.OpenDB())
            {
                var hostCount = (from t in odb.AsQueryable<HostInfo>() where t.HostTypeId == hostTypeItem.Id select t).Count();
                if (hostCount > 0)
                {
                    throw new InvalidOperationException($"存在[{hostCount}]台主机属于主机类型[{hostTypeItem.Name}],必须先删除主机才能删除主机类型");
                }

                var serviceCount = (from t in odb.AsQueryable<ServiceInfo>() where t.HostTypeId == hostTypeItem.Id select t).Count();
                if (serviceCount > 0)
                {
                    throw new InvalidOperationException($"存在[{serviceCount}]个服务属于主机类型[{hostTypeItem.Name}],必须先删除服务才能删除主机类型");
                }


                var items = (from t in odb.AsQueryable<HostTypeItem>() where t.Id == hostTypeItem.Id select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }
            }
        }

        public void ModifyHostType(HostTypeItem hostTypeItem)
        {
            using (var odb = base.OpenDB())
            {
                var count = (from t in odb.AsQueryable<HostTypeItem>() where t.Id != hostTypeItem.Id && t.Name == hostTypeItem.Name select t).Count();
                if (count > 0)
                {
                    throw new ArgumentException($"主机类型[{ hostTypeItem.Name}]已存在...");
                }

                var oldItem = (from t in odb.AsQueryable<HostTypeItem>() where t.Id == hostTypeItem.Id select t).FirstOrDefault();
                if (oldItem == null)
                {
                    hostTypeItem.Id = base.CreateId();
                    oldItem = hostTypeItem;
                }
                else
                {
                    oldItem.Update(hostTypeItem);
                }

                odb.Store(oldItem);
            }
        }

        public void DeleteAllHostType()
        {
            using (var odb = base.OpenDB())
            {
                var hostCount = (from t in odb.AsQueryable<HostInfo>() select t).Count();
                if (hostCount > 0)
                {
                    throw new InvalidOperationException($"存在[{hostCount}]台主机,必须先删除主机才能清空主机类型");
                }

                var serviceCount = (from t in odb.AsQueryable<ServiceInfo>() select t).Count();
                if (serviceCount > 0)
                {
                    throw new InvalidOperationException($"存在[{serviceCount}]个服务,必须先删除服务才能清空主机类型");
                }

                var items = (from t in odb.AsQueryable<HostTypeItem>() select t).ToArray();
                foreach (var item in items)
                {
                    odb.Delete(item);
                }
            }
        }

        public HostTypeItem[] QueryAllHostTypeItem()
        {
            using (var odb = base.OpenDB())
            {
                return (from t in odb.AsQueryable<HostTypeItem>() select t).ToArray();
            }
        }
        #endregion
    }
}
