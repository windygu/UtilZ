using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
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
        public long AddParaValue(List<ParaValue> paraValues)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        var projectId = paraValues[0].ProjectID;

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

        public List<ServicePara> QueryParaValues(long projectId, long moduleId, long version)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;
            var serviceParas = new List<ServicePara>();
            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                if (version <= 0)
                {
                    //查找是否存在同别名的项
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
                        version = (long)obj;
                    }
                }

                //查找是否存在同别名的项
                var queryParaValueCmd = conInfo.Connection.CreateCommand();
                queryParaValueCmd.CommandText = string.Format(@"SELECT ParaID,Value FROM ParaValue WHERE ProjectID={0}ProjectID AND Version={0}Version", paraSign);
                dbAccess.AddCommandParameter(queryParaValueCmd, "ProjectID", projectId);
                dbAccess.AddCommandParameter(queryParaValueCmd, "Version", version);
                var paraValueReader = queryParaValueCmd.ExecuteReader();
                while (paraValueReader.Read())
                {
                    var servicePara = new ServicePara();
                    servicePara.Key = paraValueReader.GetInt64(0).ToString();
                    servicePara.Value = paraValueReader.GetString(1);
                    serviceParas.Add(servicePara);
                }
            }

            return serviceParas;
        }
    }
}
