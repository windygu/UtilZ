using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace TestStandardLib
{
    public class Class1
    {
        public void Test()
        {
            TestSQLite();
            TestSQLServer();
            TestMySql();
            TestOracle();
        }

        public void TestSQLite()
        {
            try
            {
                var scsb = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                scsb.Pooling = true;
                scsb.DataSource = @"F:\Project\Git\UtilZ\UtilZ.DotnetCore\TestE\SQLiteDB\SQLite.db";
                using (var con = new System.Data.SQLite.SQLiteConnection(scsb.ConnectionString))
                {
                    con.Open();
                    Console.WriteLine("SQLite OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLite " + ex.Message);
            }
        }

        public void TestSQLServer()
        {
            try
            {
                string conStr = @"data source=192.168.0.102;initial catalog=ntestdb;user id=sa;password=qweQWE123";
                using (var con = new System.Data.SqlClient.SqlConnection(conStr))
                {
                    con.Open();
                    Console.WriteLine("SQLServer OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLServer " + ex.Message);
            }
        }

        public void TestMySql()
        {
            try
            {
                string conStr = @"database=test;data source=192.168.0.102;Port=3306;user id=yf;password=qweQWE123;SslMode=none";
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(conStr))
                {
                    con.Open();
                    Console.WriteLine("MySql OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MySql " + ex.Message);
            }
        }

        public void TestOracle()
        {
            try
            {
                string conStr = @"User Id=DBUSER;Password=qwe123;Data Source=192.168.0.102:1521/ntestdb.org";
                using (var con = new OracleConnection(conStr))
                {
                    con.Open();
                    Console.WriteLine("Oracle OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oracle " + ex.Message);
            }
        }
    }
}
