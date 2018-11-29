using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBFactory;
using UtilZ.ParaService.Model;

namespace UtilZ.ParaService.DAL
{
    public class BaseDAO
    {
        protected IDBAccess GetDBAccess()
        {
            IDBAccess dbAccess = DBAccessManager.GetDBAccess(WebAppConfig.Instance.DBID);
            return dbAccess;
        }

        protected int GeneratePRiKey()
        {
            return 1;
        }
    }
}
