using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Base;
using UtilZ.Dotnet.DBIBase.DBBase.Core;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBBase.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBSqlServer.Core
{
    /// <summary>
    /// SQLServer数据访问工厂类
    /// </summary>
    public class SQLServerDBFactory : DBFactoryBase
    {
        /// <summary>
        /// 数据库交互实例 数据库访问实例字典[key:dbid;value:数据库访问实例]
        /// </summary>
        private readonly DBInteractioBase _dbAccess = new SQLServerInteraction();

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLServerDBFactory() : base()
        {

        }

        /// <summary>
        /// 获取数据库交互实例
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <returns>数据库交互实例</returns>
        public override DBInteractioBase GetDBInteraction(DBConfigElement config)
        {
            return this._dbAccess;
        }

        /// <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public override IDBAccess GetDBAccess(int dbid)
        {
            return new SQLServerDBAccess(dbid);
        }

        /// <summary>
        /// 附加EF配置
        /// </summary>
        public override void AttatchEFConfig()
        {
            //EFDbConfiguration.AddProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            //EFDbConfiguration.AddProviderFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            EFDbConfiguration.AddProviderServices(typeof(System.Data.SqlClient.SqlConnection).Namespace, SqlProviderServices.Instance);
            EFDbConfiguration.AddProviderFactory(typeof(System.Data.SqlClient.SqlConnection).Namespace, SqlClientFactory.Instance);
        }
    }
}
