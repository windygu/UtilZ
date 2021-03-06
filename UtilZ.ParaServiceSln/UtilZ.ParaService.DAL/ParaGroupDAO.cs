﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.DAL
{
    public class ParaGroupDAO : BaseDAO
    {
        public ParaGroupDAO() : base()
        {

        }

        public List<ParaGroup> QueryParaGroups(long projectId, int pageSize, int pageIndex)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string sqlStr = string.Format(@"SELECT ID,ProjectID,Name,Des FROM ParaGroup WHERE ProjectID={0}ProjectID", dbAccess.ParaSign);
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
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
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
                    throw new DBException(ParaServiceConstant.DB_NOT_EIXST, $"不存在{id}为的记录");
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
                    try
                    {
                        var existCmd = conInfo.Connection.CreateCommand();
                        existCmd.Transaction = transaction;
                        existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM ParaGroup WHERE ProjectID={0}ProjectID AND Name={0}Name", paraSign);
                        dbAccess.AddCommandParameter(existCmd, "ProjectID", paraGroup.ProjectID);
                        dbAccess.AddCommandParameter(existCmd, "Name", paraGroup.Name);
                        long count = (long)existCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new DBException(ParaServiceConstant.DB_EIXST, $"项目中已存在名称为{paraGroup.Name}的参数分组");
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
                            throw new DBException(ParaServiceConstant.DB_FAIL, "写入数据库失败，原因未知");
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
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int UpdateParaGroup(ParaGroup paraGroup)
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
                        existCmd.CommandText = string.Format(@"SELECT COUNT(0) FROM ParaGroup WHERE ProjectID={0}ProjectID AND Name={0}Name", paraSign);
                        dbAccess.AddCommandParameter(existCmd, "ProjectID", paraGroup.ProjectID);
                        dbAccess.AddCommandParameter(existCmd, "Name", paraGroup.Name);
                        long count = (long)existCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new DBException(ParaServiceConstant.DB_EIXST, $"项目中已存在名称为{paraGroup.Name}的参数分组");
                        }

                        //查找默认组ID
                        var findMinParaGroupIdCmd = conInfo.Connection.CreateCommand();
                        findMinParaGroupIdCmd.Transaction = transaction;
                        findMinParaGroupIdCmd.CommandText = string.Format(@"SELECT min(ID) FROM ParaGroup WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(findMinParaGroupIdCmd, "ProjectID", paraGroup.ProjectID);
                        long defaultParaGroupId = (long)findMinParaGroupIdCmd.ExecuteScalar();
                        if (paraGroup.ID == defaultParaGroupId)
                        {
                            throw new DBException(ParaServiceConstant.DB_FAIL, "默认组不允修改");
                        }

                        //修改组
                        var updateCmd = conInfo.Connection.CreateCommand();
                        updateCmd.Transaction = transaction;
                        updateCmd.CommandText = string.Format(@"UPDATE ParaGroup SET Name={0}Name,Des={0}Des WHERE ID={0}ID", paraSign);
                        dbAccess.AddCommandParameter(updateCmd, "Name", paraGroup.Name);
                        dbAccess.AddCommandParameter(updateCmd, "Des", paraGroup.Des);
                        dbAccess.AddCommandParameter(updateCmd, "ID", paraGroup.ID);
                        int updateRet = updateCmd.ExecuteNonQuery();
                        transaction.Commit();
                        return updateRet;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int DeleteParaGroup(long projectId, long id)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        //查找默认组ID
                        var findMinParaGroupIdCmd = conInfo.Connection.CreateCommand();
                        findMinParaGroupIdCmd.Transaction = transaction;
                        findMinParaGroupIdCmd.CommandText = string.Format(@"SELECT min(ID) FROM ParaGroup WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(findMinParaGroupIdCmd, "ProjectID", projectId);
                        long defaultParaGroupId = (long)findMinParaGroupIdCmd.ExecuteScalar();
                        if (id == defaultParaGroupId)
                        {
                            throw new DBException(ParaServiceConstant.DB_FAIL, "默认组不允许删除");
                        }

                        //修改属于被删除组的参数到默认组
                        var updateParaOwnerGroupCmd = conInfo.Connection.CreateCommand();
                        updateParaOwnerGroupCmd.Transaction = transaction;
                        updateParaOwnerGroupCmd.CommandText = string.Format(@"UPDATE Para SET GroupID={0}DefaultParaGroupId WHERE GroupID={0}GroupID", dbAccess.ParaSign);
                        dbAccess.AddCommandParameter(updateParaOwnerGroupCmd, "DefaultParaGroupId", defaultParaGroupId);
                        dbAccess.AddCommandParameter(updateParaOwnerGroupCmd, "GroupID", id);
                        updateParaOwnerGroupCmd.ExecuteNonQuery();

                        //删除组
                        var deleteCmd = conInfo.Connection.CreateCommand();
                        deleteCmd.Transaction = transaction;
                        deleteCmd.CommandText = string.Format(@"DELETE FROM ParaGroup WHERE ID={0}ID", dbAccess.ParaSign);
                        dbAccess.AddCommandParameter(deleteCmd, "ID", id);
                        int deleteRet = deleteCmd.ExecuteNonQuery();
                        transaction.Commit();

                        return deleteRet;
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
