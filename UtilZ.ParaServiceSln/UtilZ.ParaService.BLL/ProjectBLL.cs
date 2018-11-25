using System;
using System.Collections.Generic;
using UtilZ.ParaService.DAL;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.BLL
{
    public class ProjectBLL
    {
        private ProjectDAO _dal;

        public ProjectBLL()
        {
            this._dal = new ProjectDAO();
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

            return this._dal.QueryProjects(pageSize, pageIndex);
        }

        public Project QueryProject(int id)
        {
            return this._dal.QueryProject(id);
        }

        public long AddProject(Project project)
        {
            return this._dal.AddProject(project);
        }

        public int UpdateProject(Project project)
        {
            return this._dal.UpdateProject(project);
        }

        public int DeleteProject(int id)
        {
            return this._dal.DeleteProject(id);
        }
    }
}
