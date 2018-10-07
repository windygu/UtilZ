using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBOracle.Core
{
    /// <summary>
    /// SQLServer数据库交互类
    /// </summary>
    public class OracleInteractiveBuilder : IDBInteractiveBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OracleInteractiveBuilder()
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
            return new OracleCommand();
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
                    config.Port = 1521;
                }

                conStr = string.Format(@"User Id={0};Password={1};Data Source={2}:{3}/{4}", config.Account, config.Password, config.Host, config.Port, config.DatabaseName);
                //conStr = string.Format(@"User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})))",
                //config.Account, config.Password, config.Host, config.Port, config.DatabaseName);
            }

            return new OracleConnection(conStr);
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public IDbDataAdapter CreateDbDataAdapter()
        {
            return new OracleDataAdapter();
        }
    }
}
