using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Base;
using UtilZ.Dotnet.DBIBase.DBBase.Core;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBBase.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Config;

namespace UtilZ.Dotnet.DBMySql.Core
{
    /// <summary>
    /// Mysql数据访问工厂类
    /// </summary>
    public class MySqlDBFactory : DBFactoryBase
    {
        /// <summary>
        /// 数据库交互实例 数据库访问实例字典[key:dbid;value:数据库访问实例]
        /// </summary>
        private readonly DBInteractioBase _dbAccess = new MySqlInteraction();

        /// <summary>
        /// 构造函数
        /// </summary>
        public MySqlDBFactory() : base()
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
            return new MySqlDBAccess(dbid);
        }

        /// <summary>
        /// 附加EF配置
        /// </summary>
        public override void AttatchEFConfig()
        {
            //EFDbConfiguration.AddProviderServices("MySql.Data.MySqlClient", new MySqlProviderServices());
            //EFDbConfiguration.AddProviderFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);

            //EFDbConfiguration.AddProviderServices(typeof(MySqlProviderServices).Namespace, new MySqlProviderServices());
            //EFDbConfiguration.AddProviderFactory(typeof(MySqlProviderServices).Namespace, MySqlClientFactory.Instance);

            EFDbConfiguration.AddProviderServices("MySql.Data.MySqlClient", new MySqlProviderServices());
            EFDbConfiguration.AddProviderFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        }
    }
}
