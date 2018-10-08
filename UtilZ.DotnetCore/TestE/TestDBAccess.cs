using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBFactory;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace TestE
{
    public class TestDBAccess
    {
        static TestDBAccess()
        {
            DBLog.Log = (str, ex) =>
            {
                Loger.Error(str, ex);
            };
        }

        private readonly static int _sqliteDbid = 1;
        private readonly static int _sqlServerDbid = 2;
        private readonly static int _mySqlDbid = 3;
        private readonly static int _oracleDbid = 4;

        public static void Test()
        {
            //TestLoad();
            TestQuery();
        }

        private static void TestQuery()
        {
            PrimitiveTestQuery(_sqliteDbid);
            PrimitiveTestQuery(_sqlServerDbid);
            PrimitiveTestQuery(_mySqlDbid);
            PrimitiveTestQuery(_oracleDbid);
        }

        private static void PrimitiveTestQuery(int dbid)
        {
            try
            {
                string sqlStr = "SELECT * FROM Stu";
                IDBAccess dbAccess = DBAccessManager.GetDBAccess(dbid);
                //DataTable dt = dbAccess.QueryData(sqlStr);
                DataTable dt = dbAccess.QueryPagingData(sqlStr, "Age", 2, 1, true);
                Console.WriteLine(string.Format("Count:{0}    {1}", dt.Rows.Count, dbAccess.DatabaseTypeName));
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private static void TestLoad()
        {
            PrimitiveTestLoad(_sqliteDbid);
            PrimitiveTestLoad(_sqlServerDbid);
            PrimitiveTestLoad(_mySqlDbid);
            PrimitiveTestLoad(_oracleDbid);
        }

        private static void PrimitiveTestLoad(int dbid)
        {
            try
            {
                IDBAccess dbAccess = DBAccessManager.GetDBAccess(dbid);
                DateTime sysTime = dbAccess.GetDataBaseSysTime();
                Console.WriteLine(string.Format("{0}    {1}", sysTime.ToString(), dbAccess.DatabaseTypeName));
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
