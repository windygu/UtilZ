using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.DAL
{
    public class ProjectModuleDAO : BaseDAO
    {
        public ProjectModuleDAO() : base()
        {

        }

        public List<ProjectModule> QueryProjectModules(long projectId, int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string sqlStr = string.Format(@"SELECT ID,Alias,Name,ParentID,Des FROM ProjectModule WHERE ProjectID={0}ProjectID", dbAccess.ParaSign);
            var parameters = new NDbParameterCollection();
            parameters.Add("ProjectID", projectId);

            DataTable dt;
            if (pageIndex > 0)
            {
                dt = dbAccess.QueryPagingData(sqlStr, "ID", pageSize, pageIndex, false, parameters);
            }
            else
            {
                dt = dbAccess.QueryData(sqlStr, parameters);
            }

            var projectModules = new List<ProjectModule>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var projectModule = new ProjectModule();
                    projectModule.ProjectID = projectId;
                    projectModule.ID = (long)(row[0]);
                    projectModule.Alias = row[1].ToString();
                    projectModule.Name = row[2].ToString();

                    if (DBNull.Value != row[3])
                    {
                        projectModule.ParentID = (long)(row[3]);
                    }

                    projectModule.Des = row[4].ToString();
                    projectModules.Add(projectModule);
                }
            }

            return projectModules;
        }

        public ProjectModule QueryProjectModule(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                var queryCmd = conInfo.Connection.CreateCommand();
                queryCmd.CommandText = string.Format(@"SELECT ProjectID,Alias,Name,ParentID,Des FROM ProjectModule WHERE ID={0}ID", paraSign);
                dbAccess.AddCommandParameter(queryCmd, "ID", id);
                var reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    var projectModule = new ProjectModule();
                    projectModule.ID = id;
                    projectModule.ProjectID = reader.GetInt64(0);
                    projectModule.Alias = reader.GetString(1);
                    projectModule.Name = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                    {
                        projectModule.ParentID = reader.GetInt64(3);
                    }

                    projectModule.Des = reader.GetString(4);
                    return projectModule;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 添加项目模块返回主键
        /// </summary>
        /// <param name="projectModule"></param>
        /// <returns></returns>
        public long AddProjectModule(ProjectModule projectModule)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        var existCmd = conInfo.Connection.CreateCommand();
                        existCmd.Transaction = transaction;
                        existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM ProjectModule WHERE Alias={0}Alias", paraSign);
                        dbAccess.AddCommandParameter(existCmd, "Alias", projectModule.Alias);
                        long count = (long)existCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            return -1;
                        }

                        var insertCmd = conInfo.Connection.CreateCommand();
                        insertCmd.Transaction = transaction;
                        if (projectModule.ParentID > 0)
                        {
                            insertCmd.CommandText = string.Format(@"INSERT INTO ProjectModule (ProjectID,Alias,Name,ParentID,Des) VALUES ({0}ProjectID,{0}Alias,{0}Name,{0}ParentID,{0}Des)", paraSign);
                            dbAccess.AddCommandParameter(insertCmd, "ParentID", projectModule.ParentID);
                        }
                        else
                        {
                            insertCmd.CommandText = string.Format(@"INSERT INTO ProjectModule (ProjectID,Alias,Name,Des) VALUES ({0}ProjectID,{0}Alias,{0}Name,{0}Des)", paraSign);
                        }

                        dbAccess.AddCommandParameter(insertCmd, "ProjectID", projectModule.ProjectID);
                        dbAccess.AddCommandParameter(insertCmd, "Alias", projectModule.Alias);
                        dbAccess.AddCommandParameter(insertCmd, "Name", projectModule.Name);
                        dbAccess.AddCommandParameter(insertCmd, "Des", projectModule.Des);
                        int ret = insertCmd.ExecuteNonQuery();
                        if (ret != 1)
                        {
                            return -2;
                        }

                        var queryCmd = conInfo.Connection.CreateCommand();
                        queryCmd.Transaction = transaction;
                        queryCmd.CommandText = string.Format(@"SELECT ID FROM ProjectModule WHERE Alias={0}Alias", paraSign);
                        dbAccess.AddCommandParameter(queryCmd, "Alias", projectModule.Alias);
                        object obj = queryCmd.ExecuteScalar();
                        if (obj == null)
                        {
                            return -3;
                        }

                        transaction.Commit();
                        return (long)obj;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int UpdateProjectModule(ProjectModule projectModule)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            var parameters = new NDbParameterCollection();
            string sqlStr = string.Format(@"UPDATE ProjectModule SET Alias={0}Alias,Name={0}Name,ParentID={0}ParentID,Des={0}Des WHERE ID={0}ID", paraSign);
            parameters.Add("Alias", projectModule.Alias);
            parameters.Add("Name", projectModule.Name);
            if (projectModule.ParentID > 0)
            {
                parameters.Add("ParentID", projectModule.ParentID);
            }
            else
            {
                parameters.Add("ParentID", DBNull.Value);
            }

            parameters.Add("Des", projectModule.Des);
            parameters.Add("ID", projectModule.ID);
            return dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W, parameters);
        }

        public int DeleteProjectModule(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        //删除项目模块参数
                        var delModuleParaCmd = conInfo.Connection.CreateCommand();
                        delModuleParaCmd.Transaction = transaction;
                        delModuleParaCmd.CommandText = string.Format(@"DELETE FROM ModulePara WHERE ModuleID={0}ModuleID", paraSign);
                        dbAccess.AddCommandParameter(delModuleParaCmd, "ModuleID", id);
                        delModuleParaCmd.ExecuteNonQuery();

                        //删除项目模块
                        var delProjectModuleCmd = conInfo.Connection.CreateCommand();
                        delProjectModuleCmd.Transaction = transaction;
                        delProjectModuleCmd.CommandText = string.Format(@"DELETE FROM ProjectModule WHERE ID={0}ID", paraSign);
                        dbAccess.AddCommandParameter(delProjectModuleCmd, "ID", id);
                        int delProjectModuleRet = delProjectModuleCmd.ExecuteNonQuery();

                        transaction.Commit();
                        return delProjectModuleRet;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
