using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interfaces;
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

        public static void Test()
        {
            TestDBLib();

            //TestLoad();
        }

        private static void TestDBLib()
        {
            try
            {
                //var scsb = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                //scsb.Pooling = true;
                //scsb.DataSource = @"F:\Project\Git\UtilZ\UtilZ.DotnetCore\TestE\SQLiteDB\SQLite.db";
                //using (var con = new System.Data.SQLite.SQLiteConnection(scsb.ConnectionString))
                //{
                //    con.Open();

                //}

                //string xx = AssemblyEx.GetAssemblyName(@"System.Text.Encoding.CodePages.dll");

                //string conStr = @"data source=192.168.0.102;initial catalog=ntestdb;user id=sa;password=qweQWE123";
                //using (var con = new System.Data.SqlClient.SqlConnection(conStr))
                //{
                //    con.Open();
                //    Console.WriteLine("Open OK");
                //}

                //System.Data.IDbDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open faile " + ex.Message);
            }
        }

        private readonly static int _sqliteDbid = 1;
        private readonly static int _sqlServerDbid = 2;
        public static void TestLoad()
        {
            try
            {
                //SqlConnection
                //int dbid;
                //dbid = _sqliteDbid;
                ////dbid = _sqlServerDbid;
                //IDBAccess dbAccess = DBAccessManager.GetDBAccessInstance(dbid);
                //DateTime sysTime = dbAccess.GetDataBaseSysTime();
                //Console.WriteLine(sysTime.ToString());
                //TestLoad1(_sqliteDbid);
                //TestLoad1(_sqliteDbid);

                TestLoad1(_sqlServerDbid);
                //TestLoad1(_sqlServerDbid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void TestLoad1(int dbid)
        {
            try
            {
                IDBAccess dbAccess = DBAccessManager.GetDBAccessInstance(dbid);
                DateTime sysTime = dbAccess.GetDataBaseSysTime();
                Console.WriteLine(sysTime.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
