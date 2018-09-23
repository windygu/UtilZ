using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBSQLServer.Core
{
    /// <summary>
    /// SQLite数据库交互类
    /// </summary>
    public class SQLServerInteractiveBuilder : IDBInteractiveBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLServerInteractiveBuilder()
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
            return new SqlCommand();
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
                throw new ArgumentNullException("config");
            }

            if (config.Port == 0)
            {
                config.Port = 1433;
            }

            string sqlConStr = string.Format(@"data source={0},{1};initial catalog={2};user id={3};password={4}", config.Host, config.Port, config.DatabaseName, config.Account, config.Password);
            return new SqlConnection(sqlConStr);
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
