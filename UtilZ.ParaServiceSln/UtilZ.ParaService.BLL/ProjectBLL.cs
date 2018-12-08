using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
