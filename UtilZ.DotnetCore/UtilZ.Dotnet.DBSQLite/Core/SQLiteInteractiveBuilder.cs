using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBSQLite.Core
{
    /// <summary>
    /// SQLite数据库交互类
    /// </summary>
    public class SQLiteInteractiveBuilder : IDBInteractiveBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLiteInteractiveBuilder()
        {

        }

        /// <summary>
        /// 创建数据库读连接对象
        /// </summary>
        /// /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接对象</returns>
        public DbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>连接对象</returns>
        public DbConnection CreateConnection(DBConfig config, DBVisitType visitType)
        {
            SQLiteConnectionStringBuilder scsb;
            if (config.ConnectionType == 0)
            {
                scsb = new SQLiteConnectionStringBuilder(config.ConnectionString);
            }
            else
            {
                scsb = new SQLiteConnectionStringBuilder();
                scsb.Pooling = true;
                scsb.DataSource = config.DatabaseName;
                if (!string.IsNullOrEmpty(config.Password))
                {
                    scsb.Password = config.Password;
                }
            }

            string dbDir = Path.GetDirectoryName(scsb.DataSource);
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }

            if (visitType == DBVisitType.R)
            {
                scsb.ReadOnly = true;
            }
            else
            {
                scsb.ReadOnly = false;
            }

            return new SQLiteConnection(scsb.ConnectionString);
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return new SQLiteDataAdapter();
        }
    }
}
