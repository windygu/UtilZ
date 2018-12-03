using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
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
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            string conStr;

            SQLiteConnectionStringBuilder scsb;
            if (config.ConnectionType == DBConstant.ConnectionTypeStr)
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

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                scsb.DataSource = scsb.DataSource.Replace('\\', Path.DirectorySeparatorChar);
            }
            else
            {
                scsb.DataSource = scsb.DataSource.Replace('/', Path.DirectorySeparatorChar);
            }

            string dbDir = Path.GetDirectoryName(scsb.DataSource);
            if (!string.IsNullOrWhiteSpace(dbDir) && !Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }

            //启用只读或写模式特么会死锁，没搞懂原因，很诡异
            //if (visitType == DBVisitType.R)
            //{
            //    scsb.ReadOnly = true;
            //}
            //else
            //{
            //    scsb.ReadOnly = false;
            //}

            conStr = scsb.ConnectionString;

            return new SQLiteConnection(conStr);
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
