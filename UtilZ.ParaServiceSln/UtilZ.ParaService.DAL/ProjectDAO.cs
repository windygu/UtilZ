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
        public ProjectDAO() : base()
        {

        }

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
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                var queryCmd = conInfo.Connection.CreateCommand();
                queryCmd.CommandText = string.Format(@"SELECT Alias,Name,Des FROM Project WHERE ID={0}ID", paraSign);
                dbAccess.AddCommandParameter(queryCmd, "ID", id);
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
                    try
                    {
                        //查找是否存在同别名的项
                        var existCmd = conInfo.Connection.CreateCommand();
                        existCmd.Transaction = transaction;
                        existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM Project WHERE Alias={0}Alias", paraSign);
                        dbAccess.AddCommandParameter(existCmd, "Alias", project.Alias);
                        long count = (long)existCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            return -1;
                        }

                        //插入
                        var insertCmd = conInfo.Connection.CreateCommand();
                        insertCmd.Transaction = transaction;
                        insertCmd.CommandText = string.Format(@"INSERT INTO Project (Alias,Name,Des) VALUES ({0}Alias,{0}Name,{0}Des)", paraSign);
                        dbAccess.AddCommandParameter(insertCmd, "Alias", project.Alias);
                        dbAccess.AddCommandParameter(insertCmd, "Name", project.Name);
                        dbAccess.AddCommandParameter(insertCmd, "Des", project.Des);
                        int ret = insertCmd.ExecuteNonQuery();
                        if (ret != 1)
                        {
                            return -2;
                        }

                        //查询刚添加记录的主键ID
                        var queryCmd = conInfo.Connection.CreateCommand();
                        queryCmd.Transaction = transaction;
                        queryCmd.CommandText = string.Format(@"SELECT ID FROM Project WHERE Alias={0}Alias", paraSign);
                        dbAccess.AddCommandParameter(queryCmd, "Alias", project.Alias);
                        object obj = queryCmd.ExecuteScalar();
                        if (obj == null)
                        {
                            return -3;
                        }

                        long prjId = (long)obj;

                        //插入默认分组
                        var insertDefaultParaGroupCmd = conInfo.Connection.CreateCommand();
                        insertDefaultParaGroupCmd.Transaction = transaction;
                        insertDefaultParaGroupCmd.CommandText = string.Format(@"INSERT INTO ParaGroup (ProjectID,Name,Des) VALUES ({0}ProjectID,{0}Name,{0}Des)", paraSign);
                        dbAccess.AddCommandParameter(insertDefaultParaGroupCmd, "ProjectID", prjId);
                        dbAccess.AddCommandParameter(insertDefaultParaGroupCmd, "Name", "默认分组");
                        dbAccess.AddCommandParameter(insertDefaultParaGroupCmd, "Des", "默认分组由系统自动创建，不可删除及修改");
                        ret = insertDefaultParaGroupCmd.ExecuteNonQuery();
                        if (ret != 1)
                        {
                            return -2;
                        }

                        transaction.Commit();
                        return prjId;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
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
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        //删除参数
                        var delParaCmd = conInfo.Connection.CreateCommand();
                        delParaCmd.Transaction = transaction;
                        delParaCmd.CommandText = string.Format(@"DELETE FROM Para WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(delParaCmd, "ProjectID", id);
                        delParaCmd.ExecuteNonQuery();

                        //删除参数分组
                        var delParaGroupCmd = conInfo.Connection.CreateCommand();
                        delParaGroupCmd.Transaction = transaction;
                        delParaGroupCmd.CommandText = string.Format(@"DELETE FROM ParaGroup WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(delParaGroupCmd, "ProjectID", id);
                        delParaGroupCmd.ExecuteNonQuery();

                        //删除参数版本号
                        var delParaVerionCmd = conInfo.Connection.CreateCommand();
                        delParaVerionCmd.Transaction = transaction;
                        delParaVerionCmd.CommandText = string.Format(@"DELETE FROM ParaVersion WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(delParaVerionCmd, "ProjectID", id);
                        delParaVerionCmd.ExecuteNonQuery();

                        //删除参数值
                        var delParaValueCmd = conInfo.Connection.CreateCommand();
                        delParaValueCmd.Transaction = transaction;
                        delParaValueCmd.CommandText = string.Format(@"DELETE FROM ParaValue WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(delParaValueCmd, "ProjectID", id);
                        delParaValueCmd.ExecuteNonQuery();

                        //删除项目模块参数
                        var delModuleParaCmd = conInfo.Connection.CreateCommand();
                        delModuleParaCmd.Transaction = transaction;
                        delModuleParaCmd.CommandText = string.Format(@"DELETE FROM ModulePara WHERE ModuleID in (SELECT ID FROM ProjectModule WHERE ProjectID={0}ProjectID)", paraSign);
                        dbAccess.AddCommandParameter(delModuleParaCmd, "ProjectID", id);
                        delModuleParaCmd.ExecuteNonQuery();

                        //删除项目模块
                        var delProjectModuleCmd = conInfo.Connection.CreateCommand();
                        delProjectModuleCmd.Transaction = transaction;
                        delProjectModuleCmd.CommandText = string.Format(@"DELETE FROM ProjectModule WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(delProjectModuleCmd, "ProjectID", id);
                        delProjectModuleCmd.ExecuteNonQuery();

                        //删除项目
                        var delProjectCmd = conInfo.Connection.CreateCommand();
                        delProjectCmd.Transaction = transaction;
                        delProjectCmd.CommandText = string.Format(@"DELETE FROM Project WHERE ID={0}ID", paraSign);
                        dbAccess.AddCommandParameter(delProjectCmd, "ID", id);
                        int delProjectRet = delProjectCmd.ExecuteNonQuery();

                        transaction.Commit();
                        return delProjectRet;
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
