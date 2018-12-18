using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.ParaService.DBModel;

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
    }
}
