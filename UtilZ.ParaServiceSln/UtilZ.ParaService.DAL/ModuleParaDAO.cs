using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.DAL
{
    public class ModuleParaDAO : BaseDAO
    {
        public ModuleParaDAO() : base()
        {

        }

        public List<ModulePara> QueryModuleParas(long moduleId)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.R))
            {
                var queryCmd = conInfo.Connection.CreateCommand();
                queryCmd.CommandText = $@"SELECT ParaID FROM ModulePara WHERE ModuleID={paraSign}ModuleID";
                dbAccess.AddCommandParameter(queryCmd, "ModuleID", moduleId);
                var moduleParas = new List<ModulePara>();

                var reader = queryCmd.ExecuteReader();
                while (reader.Read())
                {
                    var modulePara = new ModulePara();
                    modulePara.ModuleID = moduleId;
                    modulePara.ParaID = reader.GetInt64(0);
                    moduleParas.Add(modulePara);
                }

                return moduleParas;
            }
        }

        public int UpdateModuleParas(ModuleParaPost moduleParaPost)
        {
            IDBAccess dbAccess = base.GetDBAccess();
            string paraSign = dbAccess.ParaSign;

            using (var conInfo = dbAccess.CreateConnection(Dotnet.DBBase.Model.DBVisitType.W))
            {
                using (var transaction = conInfo.Connection.BeginTransaction())
                {
                    try
                    {
                        //删除旧的模块参数
                        var deleteOldModuleParaCmd = conInfo.Connection.CreateCommand();
                        deleteOldModuleParaCmd.Transaction = transaction;
                        deleteOldModuleParaCmd.CommandText = $@"DELETE FROM ModulePara WHERE ModuleID={paraSign}ModuleID";
                        dbAccess.AddCommandParameter(deleteOldModuleParaCmd, "ModuleID", moduleParaPost.ModuleId);
                        deleteOldModuleParaCmd.ExecuteNonQuery();

                        //插入新的模块参数
                        var insertCmd = conInfo.Connection.CreateCommand();
                        insertCmd.Transaction = transaction;
                        insertCmd.CommandText = string.Format(@"INSERT INTO ModulePara (ModuleID,ParaID) VALUES ({0}ModuleID,{0}ParaID)", paraSign);

                        foreach (var paraId in moduleParaPost.ParaIds)
                        {
                            dbAccess.AddCommandParameter(insertCmd, "ModuleID", moduleParaPost.ModuleId);
                            dbAccess.AddCommandParameter(insertCmd, "ParaID", paraId);
                            if (insertCmd.ExecuteNonQuery() != 1)
                            {
                                throw new DBException(ParaServiceConstant.DB_FAIL, "写入数据库失败，原因未知");
                            }
                        }

                        transaction.Commit();
                        return moduleParaPost.ParaIds.Count;
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
