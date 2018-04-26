using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.DBIBase.DBBase.Core
{
    /// <summary>
    /// EF配置类
    /// </summary>
    public class EFDbConfiguration : DbConfiguration
    {
        /// <summary>
        /// [key:providerInvariantName;value:DbProviderServices]
        /// </summary>
        private readonly static Hashtable _htProviderServices = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// [key:providerInvariantName;value:DbProviderFactory]
        /// </summary>
        private readonly static Hashtable _htProviderFactory = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 添加Call this method from the constructor of a class derived from System.Data.Entity.DbConfiguration to register an ADO.NET provider.
        /// </summary>
        /// <param name="providerInvariantName">The ADO.NET provider invariant name indicating the type of ADO.NET connection for which this provider will be used.</param>
        /// <param name="providerFactory">The provider instance</param>
        public static void AddProviderFactory(string providerInvariantName, DbProviderFactory providerFactory)
        {
            _htProviderFactory[providerInvariantName] = providerFactory;
        }

        /// <summary>
        /// Call this method from the constructor of a class derived from System.Data.Entity.DbConfiguration to register an Entity Framework provider.
        /// </summary>
        /// <param name="providerInvariantName">The ADO.NET provider invariant name indicating the type of ADO.NET connection for which this provider will be used</param>
        /// <param name="provider">The provider instance.</param>
        public static void AddProviderServices(string providerInvariantName, DbProviderServices provider)
        {
            _htProviderServices[providerInvariantName] = provider;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EFDbConfiguration()
        {
            SetDefaultConnectionFactory(new EFConnectionFactory());
            foreach (string providerInvariantName in _htProviderFactory.Keys)
            {
                SetProviderFactory(providerInvariantName, (DbProviderFactory)_htProviderFactory[providerInvariantName]);
            }

            foreach (string providerInvariantName in _htProviderServices.Keys)
            {
                SetProviderServices(providerInvariantName, (DbProviderServices)_htProviderServices[providerInvariantName]);
            }


            //SetDefaultConnectionFactory(new CustomConnectionFactory());

            //SetProviderServices("MySql.Data.MySqlClient", new MySqlProviderServices());
            //SetProviderFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);

            //SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            //SetProviderFactory("System.Data.SqlClient", SqlClientFactory.Instance);

            //SetProviderServices("Oracle.ManagedDataAccess.Client", EFOracleProviderServices.Instance);
            //SetProviderFactory("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance);

            //SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            //SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            //var s = SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices));
            //SetProviderServices("System.Data.SQLite", (DbProviderServices)s);
        }
    }

    internal class EFConnectionFactory : IDbConnectionFactory
    {
        public System.Data.Common.DbConnection CreateConnection(string nameOrConnectionString)
        {
            return null;
        }
    }
}
