using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.ParaService.DAL;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.BLL
{
    public class ProjectBLL
    {
        public ProjectBLL()
        {

        }

        #region Project
        private ProjectDAO _projectDAO = null;
        private ProjectDAO GetProjectDAO()
        {
            if (this._projectDAO == null)
            {
                this._projectDAO = new ProjectDAO();
            }

            return this._projectDAO;
        }

        public ApiData QueryProjects(int pageSize, int pageIndex)
        {
            try
            {
                if (pageIndex > 0)
                {
                    if (pageSize < 1)
                    {
                        pageSize = 100;
                    }
                }

                List<Project> prjs = this.GetProjectDAO().QueryProjects(pageSize, pageIndex);
                return new ApiData(ParaServiceConstant.DB_SUCESS, prjs);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData QueryProject(long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectDAO().QueryProject(id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData AddProject(Project project)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectDAO().AddProject(project));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData UpdateProject(Project project)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectDAO().UpdateProject(project));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData DeleteProject(long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectDAO().DeleteProject(id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }
        #endregion

        #region ProjectModule
        private ProjectModuleDAO _projectModuleDAO = null;
        private ProjectModuleDAO GetProjectModuleDAO()
        {
            if (this._projectModuleDAO == null)
            {
                this._projectModuleDAO = new ProjectModuleDAO();
            }

            return this._projectModuleDAO;
        }

        public ApiData QueryProjectModules(long projectId, int pageSize, int pageIndex)
        {
            if (pageIndex > 0)
            {
                if (pageSize < 1)
                {
                    pageSize = 100;
                }
            }

            try
            {
                var projectModules = this.GetProjectModuleDAO().QueryProjectModules(projectId, pageSize, pageIndex);
                return new ApiData(ParaServiceConstant.DB_SUCESS, projectModules);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData QueryProjectModule(long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectModuleDAO().QueryProjectModule(id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData AddProjectModule(ProjectModule project)
        {
            try
            {
                long id = this.GetProjectModuleDAO().AddProjectModule(project);
                return new ApiData(ParaServiceConstant.DB_SUCESS, id);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData UpdateProjectModule(ProjectModule project)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectModuleDAO().UpdateProjectModule(project));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData DeleteProjectModule(long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetProjectModuleDAO().DeleteProjectModule(id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }
        #endregion

        #region ParaGroup
        private ParaGroupDAO _paraGroupDAO = null;
        private ParaGroupDAO GetParaGroupDAO()
        {
            if (this._paraGroupDAO == null)
            {
                this._paraGroupDAO = new ParaGroupDAO();
            }

            return this._paraGroupDAO;
        }

        public ApiData QueryParaGroups(long projectID, int pageSize, int pageIndex)
        {
            if (pageIndex > 0)
            {
                if (pageSize < 1)
                {
                    pageSize = 100;
                }
            }

            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaGroupDAO().QueryParaGroups(projectID, pageSize, pageIndex));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData QueryParaGroup(long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaGroupDAO().QueryParaGroup(id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        /// <summary>
        /// 添加参数组返回主键
        /// </summary>
        /// <param name="paraGroup"></param>
        /// <returns></returns>
        public ApiData AddParaGroup(ParaGroup paraGroup)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaGroupDAO().AddParaGroup(paraGroup));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData UpdateParaGroup(ParaGroup paraGroup)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaGroupDAO().UpdateParaGroup(paraGroup));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData DeleteParaGroup(long projectId, long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaGroupDAO().DeleteParaGroup(projectId, id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }
        #endregion

        #region Para
        private ParaDAO _paraDAO = null;
        private ParaDAO GetParaDAO()
        {
            if (this._paraDAO == null)
            {
                this._paraDAO = new ParaDAO();
            }

            return this._paraDAO;
        }
        public ApiData QueryParas(long projectId, long paraGroupId, int pageSize, int pageIndex)
        {
            try
            {
                if (pageIndex > 0)
                {
                    if (pageSize < 1)
                    {
                        pageSize = 100;
                    }
                }

                var paras = this.GetParaDAO().QueryParas(projectId, paraGroupId, pageSize, pageIndex);
                var groups = this.GetParaGroupDAO().QueryParaGroups(projectId, -1, -1);
                var groupDic = groups.ToDictionary(t => { return t.ID; });
                foreach (var para in paras)
                {
                    if (groupDic.ContainsKey(para.GroupID))
                    {
                        para.Group = groupDic[para.GroupID];
                    }
                }

                return new ApiData(ParaServiceConstant.DB_SUCESS, paras);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData QueryPara(long id)
        {
            try
            {
                var para = this.GetParaDAO().QueryPara(id);
                para.Group = this.GetParaGroupDAO().QueryParaGroup(para.GroupID);
                return new ApiData(ParaServiceConstant.DB_SUCESS, para);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData AddPara(Para para)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaDAO().AddPara(para));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData UpdatePara(Para para)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaDAO().UpdatePara(para));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData DeletePara(long id)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaDAO().DeletePara(id));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }
        #endregion

        private string GetVerCacheKey(long projectId)
        {
            return $"prj_{projectId}_cache_key";
        }

        private string GetProjectModuleParaValueCacheKey(long projectId, long moduleId, long version)
        {
            return $"{projectId}_{moduleId}_{version}";
        }

        private const int _CACHE_EXPIRE_TIME = 3600000;

        #region ParaValue
        private ParaValueDAO _paraValueDAO = null;
        private ParaValueDAO GetParaValueDAO()
        {
            if (this._paraValueDAO == null)
            {
                this._paraValueDAO = new ParaValueDAO();
            }

            return this._paraValueDAO;
        }

        public ApiData SetParaValue(ParaValueSettingPost para)
        {
            try
            {
                //添加参数值
                long version = this.GetParaValueDAO().AddParaValue(para);

                //更新缓存
                this.UpdateCacheParaValue(para, version);

                return new ApiData(ParaServiceConstant.DB_SUCESS, version);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        private void UpdateCacheParaValue(ParaValueSettingPost para, long version)
        {
            this.AddProjectParaVerToCache(this.GetVerCacheKey(para.PID), version);

            try
            {
                var moduleParas = this.GetModuleParaDAO().QueryProjectAllModuleParas(para.PID);
                IEnumerable<IGrouping<long, ModulePara>> moduleParaGroups = moduleParas.GroupBy(t => { return t.ModuleID; });
                var paraValueDic = para.ParaValueSettings.ToDictionary(k => { return k.Id; }, v => { return v.Value; });
                List<Para> paras = this.GetParaDAO().QueryParas(para.PID, -1, -1, -1);
                var paraDic = paras.ToDictionary(k => { return k.ID; }, v => { return v.Key; });

                foreach (var moduleParaGroup in moduleParaGroups)
                {
                    var servicePara = new ServicePara();
                    servicePara.Version = version;
                    foreach (var modulePara in moduleParaGroup)
                    {
                        servicePara.Items.Add(new ServiceParaItem() { Key = paraDic[modulePara.ParaID], Value = paraValueDic[modulePara.ParaID] });
                    }

                    string cacheKey = this.GetProjectModuleParaValueCacheKey(para.PID, moduleParaGroup.Key, version);
                    this.AddProjectParaToCache(cacheKey, servicePara);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "更新参数缓存异常");
            }
        }

        private void AddProjectParaToCache(string cacheKey, ServicePara servicePara)
        {
            MemoryCacheEx.Set(cacheKey, servicePara, _CACHE_EXPIRE_TIME);
        }

        private void AddProjectParaVerToCache(string verCacheKey, long version)
        {
            MemoryCacheEx.Set(verCacheKey, version);
        }

        public ApiData QueryParaValues(long projectId, long moduleId, long version)
        {
            try
            {
                ParaValueDAO paraValueDAO = this.GetParaValueDAO();
                if (version <= 0)
                {
                    string verCacheKey = this.GetVerCacheKey(projectId);
                    object obj = MemoryCacheEx.Get(verCacheKey);
                    if (obj == null)
                    {
                        version = paraValueDAO.QueryBestNewVersion(projectId);
                        this.AddProjectParaVerToCache(verCacheKey, version);
                    }
                    else
                    {
                        version = (long)obj;
                    }
                }

                string cacheKey = this.GetProjectModuleParaValueCacheKey(projectId, moduleId, version);
                var servicePara = MemoryCacheEx.Get(cacheKey) as ServicePara;
                if (servicePara == null)
                {
                    servicePara = this.GetParaValueDAO().QueryParaValues(projectId, moduleId, version);
                }

                this.AddProjectParaToCache(cacheKey, servicePara);
                return new ApiData(ParaServiceConstant.DB_SUCESS, servicePara);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        /// <summary>
        /// 删除旧的参数值
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="beginVer">起始版本号</param>
        /// <param name="endVer">结束版本号</param>
        /// <returns></returns>
        public ApiData DeleteParaValue(long projectId, long beginVer, long endVer)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetParaValueDAO().DeleteParaValue(projectId, beginVer, endVer));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }
        #endregion

        #region ModulePara
        private ModuleParaDAO _moduleParaDAO = null;
        private ModuleParaDAO GetModuleParaDAO()
        {
            if (this._moduleParaDAO == null)
            {
                this._moduleParaDAO = new ModuleParaDAO();
            }

            return this._moduleParaDAO;
        }

        public ApiData QueryModuleParas(long projectId, long moduleId)
        {
            try
            {
                var mpg = new ModuleParaGet();
                mpg.AllParas = this.GetParaDAO().QueryParas(projectId, -1, -1, -1);
                mpg.Groups = this.GetParaGroupDAO().QueryParaGroups(projectId, -1, -1);
                mpg.ModuleParas = this.GetModuleParaDAO().QueryModuleParas(moduleId);
                return new ApiData(ParaServiceConstant.DB_SUCESS, mpg);
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }

        public ApiData UpdateModuleParas(ModuleParaPost moduleParaPost)
        {
            try
            {
                return new ApiData(ParaServiceConstant.DB_SUCESS, this.GetModuleParaDAO().UpdateModuleParas(moduleParaPost));
            }
            catch (DBException dbex)
            {
                return new ApiData(dbex.Status, dbex.Message);
            }
            catch (Exception ex)
            {
                return new ApiData(ParaServiceConstant.DB_FAIL_NONE, ex.Message);
            }
        }
        #endregion
    }
}
