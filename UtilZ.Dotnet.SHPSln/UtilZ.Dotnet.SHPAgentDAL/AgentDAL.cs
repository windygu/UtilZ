using NDatabase;
using NDatabase.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPAgentDAL
{
    public class AgentDAL
    {
        private readonly string _dataBaseFilePath;
        public AgentDAL()
        {
            this._dataBaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"DB\agent.db");
            DirectoryInfoEx.CheckFilePathDirectory(this._dataBaseFilePath);
        }

        private IOdb OpenDB()
        {
            return OdbFactory.Open(this._dataBaseFilePath);
        }

        public void AddDevOps(DevOpsInfo devOpsInfo)
        {
            using (var odb = this.OpenDB())
            {
                var existDevOpsInfos = (from t in odb.AsQueryable<DevOpsInfo>() where t.Id == devOpsInfo.Id select t).ToArray();
                foreach (var existDevOpsInfo in existDevOpsInfos)
                {
                    odb.Delete(existDevOpsInfo);
                }

                odb.Store(devOpsInfo);
            }
        }

        public DevOpsInfo[] QueryAllDevOps()
        {
            using (var odb = this.OpenDB())
            {
                return (from t in odb.AsQueryable<DevOpsInfo>() select t).ToArray();
            }
        }

        public void DeleteDevOps(List<DevOpsInfo> deleDevOpsList)
        {
            using (var odb = this.OpenDB())
            {
                foreach (var deleDevOps in deleDevOpsList)
                {
                    var preDelItems = (from t in odb.AsQueryable<DevOpsInfo>() where t.Id == deleDevOps.Id select t).ToArray();
                    foreach (var preDelItem in preDelItems)
                    {
                        odb.Delete(preDelItem);
                    }
                }
            }
        }

        public SHPAgentServiceInsInfo[] QueryAllServiceInsInfo()
        {
            using (var odb = this.OpenDB())
            {
                return (from t in odb.AsQueryable<SHPAgentServiceInsInfo>() select t).ToArray();
            }
        }

        public void AddServiceInsInfo(SHPAgentServiceInsInfo serviceInsInfo)
        {
            using (var odb = this.OpenDB())
            {
                odb.Store(serviceInsInfo);
            }
        }

        public void DeleteServiceInsInfo(long doId, List<long> idList)
        {
            using (var odb = this.OpenDB())
            {
                var preDelItems = (from t in odb.AsQueryable<SHPAgentServiceInsInfo>() where t.DOID == doId && idList.Contains(t.Id) select t).ToArray();
                foreach (var preDelItem in preDelItems)
                {
                    odb.Delete(preDelItem);
                }
            }
        }

        public void UpdateServiceInsInfoListenPort(SHPAgentServiceInsInfo serviceInsInfo)
        {
            using (var odb = this.OpenDB())
            {
                var srcServiceInsInfo = (from t in odb.AsQueryable<SHPAgentServiceInsInfo>() where t.DOID == serviceInsInfo.DOID && t.Id == serviceInsInfo.Id select t).FirstOrDefault();
                if (srcServiceInsInfo != null)
                {
                    srcServiceInsInfo.Update(serviceInsInfo);
                }
                else
                {
                    throw new ApplicationException($"服务实例[{serviceInsInfo.FilePath}]不存在");
                }

                odb.Store(srcServiceInsInfo);
            }
        }
    }
}
