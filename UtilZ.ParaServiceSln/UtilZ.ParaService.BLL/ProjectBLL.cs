using System;
using System.Collections.Generic;
using UtilZ.ParaService.DAL;
using UtilZ.ParaService.DBModel;

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

        public List<Project> QueryProjects(int pageSize, int pageIndex)
        {
            if (pageIndex > 0)
            {
                if (pageSize < 1)
                {
                    pageSize = 100;
                }
            }

            return this.GetProjectDAO().QueryProjects(pageSize, pageIndex);
        }

        public Project QueryProject(long id)
        {
            return this.GetProjectDAO().QueryProject(id);
        }

        public long AddProject(Project project)
        {
            return this.GetProjectDAO().AddProject(project);
        }

        public int UpdateProject(Project project)
        {
            return this.GetProjectDAO().UpdateProject(project);
        }

        public int DeleteProject(long id)
        {
            return this._projectDAO.DeleteProject(id);
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

        public List<ProjectModule> QueryProjectModules(long projectID, int pageSize, int pageIndex)
        {
            if (pageIndex > 0)
            {
                if (pageSize < 1)
                {
                    pageSize = 100;
                }
            }

            return GetProjectModuleDAO().QueryProjectModules(projectID, pageSize, pageIndex);
        }

        public ProjectModule QueryProjectModule(long id)
        {
            return GetProjectModuleDAO().QueryProjectModule(id);
        }

        public long AddProjectModule(ProjectModule project)
        {
            return GetProjectModuleDAO().AddProjectModule(project);
        }

        public int UpdateProjectModule(ProjectModule project)
        {
            return GetProjectModuleDAO().UpdateProjectModule(project);
        }

        public int DeleteProjectModule(long id)
        {
            return GetProjectModuleDAO().DeleteProjectModule(id);
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

        public List<ParaGroup> QueryParaGroups(long projectID, int pageSize, int pageIndex)
        {
            if (pageIndex > 0)
            {
                if (pageSize < 1)
                {
                    pageSize = 100;
                }
            }

            return this.GetParaGroupDAO().QueryParaGroups(projectID, pageSize, pageIndex);
        }

        public ParaGroup QueryParaGroup(long id)
        {
            return this.GetParaGroupDAO().QueryParaGroup(id);
        }

        /// <summary>
        /// 添加参数组返回主键
        /// </summary>
        /// <param name="paraGroup"></param>
        /// <returns></returns>
        public long AddParaGroup(ParaGroup paraGroup)
        {
            return this.GetParaGroupDAO().AddParaGroup(paraGroup);
        }

        public int UpdateParaGroup(ParaGroup paraGroup)
        {
            return this.GetParaGroupDAO().UpdateParaGroup(paraGroup);
        }

        public int DeleteParaGroup(long id)
        {
            return this.GetParaGroupDAO().DeleteParaGroup(id);
        }
        #endregion
    }
}
