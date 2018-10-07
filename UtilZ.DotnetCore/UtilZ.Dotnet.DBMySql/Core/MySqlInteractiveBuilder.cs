using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBMySql.Core
{
    /// <summary>
    /// MySql数据库交互类
    /// </summary>
    public class MySqlInteractiveBuilder : IDBInteractiveBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MySqlInteractiveBuilder()
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
            return new MySqlCommand();
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
            if (config.ConnectionType == DBConstant.ConnectionTypeStr)
            {
                conStr = config.ConnectionString;
            }
            else
            {
                if (config.Port == 0)
                {
                    config.Port = 3306;
                }

                conStr = string.Format(@"database={0};data source={1};Port={2};user id={3};password={4};SslMode=none", config.DatabaseName, config.Host, config.Port, config.Account, config.Password);
            }

            return new MySqlConnection(conStr);
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return new MySqlDataAdapter();
        }
    }
}
