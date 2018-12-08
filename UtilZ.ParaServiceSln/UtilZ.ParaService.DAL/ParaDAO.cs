using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.DAL
{
    public class ParaDAO : BaseDAO
    {
        public ParaDAO() : base()
        {

        }

        public List<Para> QueryParas_bk(long projectId, long paraGroupId, int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        var queryGroupCmd = conInfo.Connection.CreateCommand();
                        queryGroupCmd.Transaction = transaction;
                        queryGroupCmd.CommandText = string.Format(@"SELECT ID,Name FROM ParaGroup WHERE ProjectID={0}ProjectID", dbAccess.ParaSign);
                        dbAccess.AddCommandParameter(queryGroupCmd, "ProjectID", projectId);
                        var groupReader = queryGroupCmd.ExecuteReader();
                        var groupDic = new Dictionary<long, ParaGroup>();
                        while (groupReader.Read())
                        {
                            var group = new ParaGroup();
                            group.ID = groupReader.GetInt64(0);
                            group.Name = groupReader.GetString(1);
                            groupDic.Add(group.ID, group);
                        }

                        var queryParaCmd = conInfo.Connection.CreateCommand();
                        queryParaCmd.Transaction = transaction;
                        string queryParaSql;
                        if (paraGroupId > 0)
                        {
                            queryParaSql = string.Format(@"SELECT ID,GroupID,Key,Name,Des FROM Para WHERE ProjectID={0}ProjectID AND GroupID={0}GroupID", paraSign);
                            dbAccess.AddCommandParameter(queryParaCmd, "GroupID", paraGroupId);
                        }
                        else
                        {
                            queryParaSql = string.Format(@"SELECT ID,GroupID,Key,Name,Des FROM Para WHERE ProjectID={0}ProjectID", paraSign);
                        }

                        if (pageIndex > 0)
                        {
                            queryParaSql = dbAccess.CreatePagingQuerySql(queryParaSql, "ID", pageSize, pageIndex, true, new string[] { "ID" });
                        }

                        queryParaCmd.CommandText = queryParaSql;
                        dbAccess.AddCommandParameter(queryParaCmd, "ProjectID", projectId);
                        var paraReader = queryParaCmd.ExecuteReader();
                        var paras = new List<Para>();
                        while (paraReader.Read())
                        {
                            var para = new Para();
                            para.ProjectID = projectId;
                            para.ID = paraReader.GetInt64(0);
                            para.GroupID = paraReader.GetInt64(1);
                            if (groupDic.ContainsKey(para.GroupID))
                            {
                                para.Group = groupDic[para.GroupID];
                            }

                            para.Key = paraReader.GetString(2);
                            para.Name = paraReader.GetString(3);
                            para.Des = paraReader.GetString(4);
                            paras.Add(para);
                        }

                        transaction.Commit();
                        return paras;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Para> QueryParas(long projectId, long paraGroupId, int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            var parameters = new NDbParameterCollection();
            string paraSign = dbAccess.ParaSign;
            string sqlStr;
            if (paraGroupId > 0)
            {
                sqlStr = string.Format(@"SELECT ID,GroupID,Key,Name,Des FROM Para WHERE ProjectID={0}ProjectID AND GroupID={0}GroupID", paraSign);
                parameters.Add("GroupID", paraGroupId);
            }
            else
            {
                sqlStr = string.Format(@"SELECT ID,GroupID,Key,Name,Des FROM Para WHERE ProjectID={0}ProjectID", paraSign);
            }

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

            var paras = new List<Para>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var para = new Para();
                    para.ProjectID = projectId;
                    para.ID = (long)(row[0]);
                    para.GroupID = (long)(row[1]);
                    para.Key = row[2].ToString();
                    para.Name = row[3].ToString();
                    para.Des = row[4].ToString();
                    paras.Add(para);
                }
            }

            return paras;
        }

        public Para QueryPara(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                var queryCmd = conInfo.Connection.CreateCommand();
                queryCmd.CommandText = string.Format(@"SELECT ProjectID,GroupID,Key,Name,Des FROM Para WHERE ID={0}ID", paraSign);
                dbAccess.AddCommandParameter(queryCmd, "ID", id);
                var reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    var para = new Para();
                    para.ID = id;
                    para.ProjectID = reader.GetInt64(0);
                    para.GroupID = reader.GetInt64(1);
                    para.Key = reader.GetString(2);
                    para.Name = reader.GetString(3);
                    para.Des = reader.GetString(4);
                    return para;
                }
                else
                {
                    return null;
                }
            }
        }

        public long AddPara(Para para)
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
                        existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM Para WHERE ProjectID={0}ProjectID AND Key={0}Key", paraSign);
                        dbAccess.AddCommandParameter(existCmd, "ProjectID", para.ProjectID);
                        dbAccess.AddCommandParameter(existCmd, "Key", para.Key);
                        long count = (long)existCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new DBException(ParaServiceConstant.DB_EIXST, $"项目中已存在Key为{para.Key}的参数");
                        }

                        //插入
                        var insertCmd = conInfo.Connection.CreateCommand();
                        insertCmd.Transaction = transaction;
                        insertCmd.CommandText = string.Format(@"INSERT INTO Para (ProjectID,GroupID,Key,Name,Des) VALUES ({0}ProjectID,{0}GroupID,{0}Key,{0}Name,{0}Des)", paraSign);
                        dbAccess.AddCommandParameter(insertCmd, "ProjectID", para.ProjectID);
                        dbAccess.AddCommandParameter(insertCmd, "GroupID", para.GroupID);
                        dbAccess.AddCommandParameter(insertCmd, "Key", para.Key);
                        dbAccess.AddCommandParameter(insertCmd, "Name", para.Name);
                        dbAccess.AddCommandParameter(insertCmd, "Des", para.Des);
                        int ret = insertCmd.ExecuteNonQuery();
                        if (ret != 1)
                        {
                            throw new DBException(ParaServiceConstant.DB_FAIL, "写入数据库失败，原因未知");
                        }

                        //查询刚添加记录的主键ID
                        var queryCmd = conInfo.Connection.CreateCommand();
                        queryCmd.Transaction = transaction;
                        queryCmd.CommandText = string.Format(@"SELECT ID FROM Para WHERE WHERE ProjectID={0}ProjectID AND Key={0}Key", paraSign);
                        dbAccess.AddCommandParameter(queryCmd, "ProjectID", para.ProjectID);
                        dbAccess.AddCommandParameter(queryCmd, "Key", para.Key);
                        object obj = queryCmd.ExecuteScalar();
                        if (obj == null)
                        {
                            throw new DBException(ParaServiceConstant.DB_NOT_EIXST, $"写入数据库成功，但未查询到Key为{para.Key}的参数记录");
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

        public int UpdatePara(Para para)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            var parameters = new NDbParameterCollection();
            string sqlStr = string.Format(@"UPDATE Para SET GroupID={0}GroupID,Key={0}Key,Name={0}Name,Des={0}Des WHERE ID={0}ID", dbAccess.ParaSign);
            parameters.Add("GroupID", para.GroupID);
            parameters.Add("Key", para.Key);
            parameters.Add("Name", para.Name);
            parameters.Add("Des", para.Des);
            parameters.Add("ID", para.ID);
            int updateRet = dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W, parameters);
            if (updateRet == 0)
            {
                throw new DBException(ParaServiceConstant.DB_NOT_EIXST, $"不存在{para.ID}为的记录");
            }

            return updateRet;
        }

        public int DeletePara(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            var parameters = new NDbParameterCollection();
            string sqlStr = string.Format(@"DELETE FROM Para WHERE ID={0}ID", dbAccess.ParaSign);
            parameters.Add("ID", id);
            return dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W, parameters);
        }
    }
}
