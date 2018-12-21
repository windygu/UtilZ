using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.DAL
{
    public class ParaValueDAO : BaseDAO
    {
        public ParaValueDAO() : base()
        {

        }

        /// <summary>
        /// 添加参数值，成功返回参数值版本号
        /// </summary>
        /// <param name="paraValues"></param>
        /// <returns></returns>
        public long AddParaValue(ParaValueSettingPost para)
        {
            var projectId = para.PID;
            var paraValues = para.ToParaValues();
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        //查询最大版本号
                        var queryParaVersionCmd = conInfo.Connection.CreateCommand();
                        queryParaVersionCmd.Transaction = transaction;
                        queryParaVersionCmd.CommandText = string.Format(@"SELECT MAX(Version) FROM ParaVersion WHERE ProjectID={0}ProjectID", paraSign);
                        dbAccess.AddCommandParameter(queryParaVersionCmd, "ProjectID", projectId);
                        object obj = queryParaVersionCmd.ExecuteScalar();

                        bool hasParaValue;
                        long paraVersion;
                        if (obj == null || obj == DBNull.Value)
                        {
                            paraVersion = 1;
                            hasParaValue = false;
                        }
                        else
                        {
                            paraVersion = (long)obj + 1;
                            hasParaValue = true;
                        }

                        if (hasParaValue)
                        {
                            //有设置过值则更新版本号
                            var updateParaVersionCmd = conInfo.Connection.CreateCommand();
                            updateParaVersionCmd.Transaction = transaction;
                            updateParaVersionCmd.CommandText = string.Format(@"UPDATE ParaVersion SET Version={0}Version WHERE ProjectID={0}ProjectID", paraSign);
                            dbAccess.AddCommandParameter(updateParaVersionCmd, "Version", paraVersion);
                            dbAccess.AddCommandParameter(updateParaVersionCmd, "ProjectID", projectId);
                            if (updateParaVersionCmd.ExecuteNonQuery() != 1)
                            {
                                throw new DBException(ParaServiceConstant.DB_FAIL, "修改参数值版本号失败，原因未知");
                            }
                        }
                        else
                        {
                            //没有设置过值则插入版本号
                            var insertParaVersionCmd = conInfo.Connection.CreateCommand();
                            insertParaVersionCmd.Transaction = transaction;
                            insertParaVersionCmd.CommandText = string.Format(@"INSERT INTO ParaVersion(ProjectID,Version) VALUES ({0}ProjectID,{0}Version)", paraSign);
                            dbAccess.AddCommandParameter(insertParaVersionCmd, "ProjectID", projectId);
                            dbAccess.AddCommandParameter(insertParaVersionCmd, "Version", paraVersion);
                            if (insertParaVersionCmd.ExecuteNonQuery() != 1)
                            {
                                throw new DBException(ParaServiceConstant.DB_FAIL, "插入参数值版本号失败，原因未知");
                            }
                        }

                        //插入参数值
                        var insertCmd = conInfo.Connection.CreateCommand();
                        insertCmd.Transaction = transaction;
                        insertCmd.CommandText = string.Format(@"INSERT INTO ParaValue (ParaID,ProjectID,Version,Value) VALUES ({0}ParaID,{0}ProjectID,{0}Version,{0}Value)", paraSign);

                        foreach (var paraValue in paraValues)
                        {
                            dbAccess.AddCommandParameter(insertCmd, "ParaID", paraValue.ParaID);
                            dbAccess.AddCommandParameter(insertCmd, "ProjectID", projectId);
                            dbAccess.AddCommandParameter(insertCmd, "Version", paraVersion);
                            dbAccess.AddCommandParameter(insertCmd, "Value", paraValue.Value);
                            if (insertCmd.ExecuteNonQuery() != 1)
                            {
                                throw new DBException(ParaServiceConstant.DB_FAIL, "写入数据库失败，原因未知");
                            }
                        }

                        transaction.Commit();
                        return paraVersion;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public long QueryBestNewVersion(long projectId)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                var queryParaVersionCmd = conInfo.Connection.CreateCommand();
                queryParaVersionCmd.CommandText = string.Format(@"SELECT MAX(Version) FROM ParaVersion WHERE ProjectID={0}ProjectID", paraSign);
                dbAccess.AddCommandParameter(queryParaVersionCmd, "ProjectID", projectId);
                object obj = queryParaVersionCmd.ExecuteScalar();
                if (obj == null || obj == DBNull.Value)
                {
                    throw new DBException(ParaServiceConstant.DB_FAIL, "参数值未设置");
                }
                else
                {
                    return (long)obj;
                }
            }
        }

        public ServicePara QueryParaValues(long projectId, long moduleId, long version)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            var servicePara = new ServicePara();
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                servicePara.Version = version;

                var queryParaValueCmd = conInfo.Connection.CreateCommand();
                //queryParaValueCmd.CommandText = string.Format(@"SELECT ParaID,Value FROM ParaValue WHERE ProjectID={0}ProjectID AND Version={0}Version", paraSign);
                //SELECT Key,Value FROM (SELECT ParaValue.ProjectID,ParaValue.Version,Key,Value FROM ParaValue INNER JOIN Para ON ParaValue.ParaID=Para.ID) WHERE ProjectID=8 AND Version=1
                //queryParaValueCmd.CommandText = string.Format(@"SELECT Key,Value FROM (SELECT ParaValue.ProjectID,ParaValue.Version,Key,Value FROM ParaValue INNER JOIN Para ON ParaValue.ParaID=Para.ID) WHERE ProjectID={0}ProjectID AND Version={0}Version", paraSign);
                queryParaValueCmd.CommandText = string.Format(@"SELECT Key,Value FROM 
(SELECT ParaID,Key,Value FROM (SELECT ParaValue.ParaID,ParaValue.ProjectID,ParaValue.Version,Key,Value FROM ParaValue INNER JOIN Para ON ParaValue.ParaID=Para.ID) WHERE ProjectID={0}ProjectID AND Version={0}Version) t 
INNER JOIN ModulePara ON ModulePara.ParaID=t.ParaID WHERE ModuleID={0}ModuleID", paraSign);
                dbAccess.AddCommandParameter(queryParaValueCmd, "ProjectID", projectId);
                dbAccess.AddCommandParameter(queryParaValueCmd, "Version", version);
                dbAccess.AddCommandParameter(queryParaValueCmd, "ModuleID", moduleId);
                var paraValueReader = queryParaValueCmd.ExecuteReader();
                while (paraValueReader.Read())
                {
                    var serviceParaItem = new ServiceParaItem();
                    serviceParaItem.Key = paraValueReader.GetString(0);
                    serviceParaItem.Value = paraValueReader.GetString(1);
                    servicePara.Items.Add(serviceParaItem);
                }
            }

            return servicePara;
        }

        public List<ParaValue> QueryParaValues(long projectId, long version)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                var queryParaValueCmd = conInfo.Connection.CreateCommand();
                queryParaValueCmd.CommandText = string.Format(@"SELECT ParaID,Value FROM ParaValue WHERE ProjectID={0}ProjectID AND Version={0}Version", paraSign);
                dbAccess.AddCommandParameter(queryParaValueCmd, "ProjectID", projectId);
                dbAccess.AddCommandParameter(queryParaValueCmd, "Version", version);
                var paraValueReader = queryParaValueCmd.ExecuteReader();

                var serviceParas = new List<ParaValue>();
                while (paraValueReader.Read())
                {
                    var serviceParaItem = new ParaValue();
                    serviceParaItem.ParaID = paraValueReader.GetInt64(0);
                    serviceParaItem.Value = paraValueReader.GetString(1);
                    serviceParas.Add(serviceParaItem);
                }

                return serviceParas;
            }
        }

        public int DeleteParaValue(long projectId, long beginVer, long endVer)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            string deleteSqlStr = string.Format(@"DELETE FROM ParaValue WHERE ProjectID={0}ProjectID AND Version>={0}BeginVer AND Version<={0}EndVer", paraSign);
            var paras = new NDbParameterCollection();
            paras.Add("ProjectID", projectId);
            paras.Add("BeginVer", beginVer);
            paras.Add("EndVer", endVer);
            return dbAccess.ExecuteNonQuery(deleteSqlStr, Dotnet.DBBase.Model.DBVisitType.W, paras);
        }
    }
}
