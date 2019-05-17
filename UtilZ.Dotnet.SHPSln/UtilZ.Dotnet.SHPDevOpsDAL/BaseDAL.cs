using NDatabase;
using NDatabase.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
//using UtilZ.Dotnet.DBIBase.DBBase.Core;
//using UtilZ.Dotnet.DBIBase.DBBase.Interface;

namespace UtilZ.Dotnet.SHPDevOpsDAL
{
    public abstract class BaseDAL
    {
        //private readonly int _dbid;
        //public BaseDAL(int dbid)
        //{
        //    this._dbid = dbid;
        //}

        //protected IDBAccess GetDBAccess()
        //{
        //    return DBAccessManager.GetDBAccessInstance(this._dbid);
        //}

        private readonly string _dataBaseFilePath;
        public BaseDAL()
        {
            this._dataBaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"DB\shp.db");
            UtilZ.Dotnet.Ex.Base.DirectoryInfoEx.CheckFilePathDirectory(this._dataBaseFilePath);
        }

        protected IOdb OpenDB()
        {
            return OdbFactory.Open(this._dataBaseFilePath);
        }

        protected long CreateId()
        {
            return TimeEx.GetTimestamp();
        }
    }
}
