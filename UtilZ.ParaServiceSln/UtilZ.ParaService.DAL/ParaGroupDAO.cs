using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.DAL
{
    public class ParaGroupDAO : BaseDAO
    {
        public ParaGroupDAO() : base()
        {

        }

        public List<ParaGroup> QueryParaGroups(long projectID, int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string sqlStr = string.Format(@"SELECT ID,ProjectID,Name,Des FROM ParaGroup WHERE ProjectID={0}ProjectID", dbAccess.ParaSign);
            var parameters = new NDbParameterCollection();
            parameters.Add("ProjectID", projectID);

            DataTable dt;
            if (pageIndex > 0)
            {
                dt = dbAccess.QueryPagingData(sqlStr, "ID", pageSize, pageIndex, false, parameters);
            }
            else
            {
                dt = dbAccess.QueryData(sqlStr, parameters);
            }

            var paraGroups = new List<ParaGroup>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var paraGroup = new ParaGroup();
                    paraGroup.ID = (long)(row[0]);
                    paraGroup.ProjectID = (long)(row[1]);
                    paraGroup.Name = row[2].ToString();
                    paraGroup.Des = row[3].ToString();
                    paraGroups.Add(paraGroup);
                }
            }

            return paraGroups;
        }

        public ParaGroup QueryParaGroup(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                var queryCmd = conInfo.Connection.CreateCommand();
                queryCmd.CommandText = string.Format(@"SELECT ProjectID,Name,Des FROM ParaGroup WHERE ID={0}ID", paraSign);
                dbAccess.AddCommandParameter(queryCmd, "ID", id);
                var reader = queryCmd.ExecuteReader();
                if (reader.Read())
                {
                    var paraGroup = new ParaGroup();
                    paraGroup.ID = id;
                    paraGroup.ProjectID = reader.GetInt64(0);
                    paraGroup.Name = reader.GetString(1);
                    paraGroup.Des = reader.GetString(2);
                    return paraGroup;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 添加参数组返回主键
        /// </summary>
        /// <param name="paraGroup"></param>
        /// <returns></returns>
        public long AddParaGroup(ParaGroup paraGroup)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    var existCmd = conInfo.Connection.CreateCommand();
                    existCmd.Transaction = transaction;
                    existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM ParaGroup WHERE Name={0}Name", paraSign);
                    dbAccess.AddCommandParameter(existCmd, "Name", paraGroup.Name);
                    long count = (long)existCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return -1;
                    }

                    var insertCmd = conInfo.Connection.CreateCommand();
                    insertCmd.Transaction = transaction;
                    insertCmd.CommandText = string.Format(@"INSERT INTO ParaGroup (ProjectID,Name,Des) VALUES ({0}ProjectID,{0}Name,{0}Des)", paraSign);
                    dbAccess.AddCommandParameter(insertCmd, "ProjectID", paraGroup.ProjectID);
                    dbAccess.AddCommandParameter(insertCmd, "Name", paraGroup.Name);
                    dbAccess.AddCommandParameter(insertCmd, "Des", paraGroup.Des);
                    int ret = insertCmd.ExecuteNonQuery();
                    if (ret != 1)
                    {
                        return -2;
                    }

                    var queryCmd = conInfo.Connection.CreateCommand();
                    queryCmd.Transaction = transaction;
                    queryCmd.CommandText = string.Format(@"SELECT ID FROM ParaGroup WHERE Name={0}Name", paraSign);
                    dbAccess.AddCommandParameter(queryCmd, "Name", paraGroup.Name);
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

        public int UpdateParaGroup(ParaGroup paraGroup)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            string sqlStr = string.Format(@"UPDATE ParaGroup SET Name={0}Name,Des={0}Des WHERE ID={0}ID", paraSign);
            var parameters = new NDbParameterCollection();
            parameters.Add("Name", paraGroup.Name);
            parameters.Add("Des", paraGroup.Des);
            parameters.Add("ID", paraGroup.ID);
            return dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W, parameters);
        }

        public int DeleteParaGroup(long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string sqlStr = string.Format(@"DELETE FROM ParaGroup WHERE ID={0}ID", dbAccess.ParaSign);
            var parameters = new NDbParameterCollection();
            parameters.Add("ID", id);
            return dbAccess.ExecuteNonQuery(sqlStr, Dotnet.DBBase.Model.DBVisitType.W, parameters);
        }
    }
}
