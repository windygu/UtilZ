using System;
using System.Collections.Generic;
using System.Data;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBFactory;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.DAL
{
    public class ProjectDAO : BaseDAO
    {
        public List<Project> QueryProjects(int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string sqlStr = @"SELECT ID,Alias,Name,Des FROM Project";
            DataTable dt;
            if (pageIndex > 0)
            {
                dt = dbAccess.QueryPagingData(sqlStr, "ID", pageSize, pageIndex, false);
            }
            else
            {
                dt = dbAccess.QueryData(sqlStr);
            }

            var projects = new List<Project>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var project = new Project();
                    project.ID = (long)(row[0]);
                    project.Alias = row[1].ToString();
                    project.Name = row[2].ToString();
                    project.Des = row[3].ToString();
                    projects.Add(project);
                }
            }

            return projects;
        }

        public Project QueryProject(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                var queryCmd = conInfo.Connection.CreateCommand();
                queryCmd.CommandText = string.Format(@"SELECT Alias,Name,Des FROM Project WHERE ID={0}ID", paraSign);
                base.AddParameter(queryCmd, "ID", id);
                var reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    var project = new Project();
                    project.ID = id;
                    project.Alias = reader.GetString(0);
                    project.Name = reader.GetString(1);
                    project.Des = reader.GetString(2);
                    return project;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 添加项目返回主键
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public long AddProject(Project project)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    var existCmd = conInfo.Connection.CreateCommand();
                    existCmd.Transaction = transaction;
                    existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM Project WHERE Alias={0}Alias", paraSign);
                    base.AddParameter(existCmd, "Alias", project.Alias);
                    long count = (long)existCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return -1;
                    }

                    var insertCmd = conInfo.Connection.CreateCommand();
                    insertCmd.Transaction = transaction;
                    insertCmd.CommandText = string.Format(@"INSERT INTO Project (Alias,Name,Des) VALUES ({0}Alias,{0}Name,{0}Des)", paraSign);
                    base.AddParameter(insertCmd, "Alias", project.Alias);
                    base.AddParameter(insertCmd, "Name", project.Name);
                    base.AddParameter(insertCmd, "Des", project.Des);
                    int ret = insertCmd.ExecuteNonQuery();
                    if (ret != 1)
                    {
                        return -2;
                    }

                    var queryCmd = conInfo.Connection.CreateCommand();
                    queryCmd.Transaction = transaction;
                    queryCmd.CommandText = string.Format(@"SELECT ID FROM Project WHERE Alias={0}Alias", paraSign);
                    base.AddParameter(queryCmd, "Alias", project.Alias);
                    object obj = queryCmd.ExecuteScalar();
                    if (obj == null)
                    {
                        return -3;
                    }

                    transaction.Commit();
                    return (long)obj;
                }
            }
        }

        public int UpdateProject(Project project)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            string sqlStr = string.Format(@"UPDATE Project SET Alias={0}Alias,Name={0}Name,Des={0}Des WHERE ID={0}ID", paraSign);
            var parameters = new NDbParameterCollection();
            parameters.Add("Alias", project.Alias);
            parameters.Add("Name", project.Name);
            parameters.Add("Des", project.Des);
            parameters.Add("ID", project.ID);
            return dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W, parameters);
        }

        public int DeleteProject(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string sqlStr = string.Format(@"DELETE FROM Project WHERE ID={0}ID", dbAccess.ParaSign);
            return dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W);
        }
    }
}
