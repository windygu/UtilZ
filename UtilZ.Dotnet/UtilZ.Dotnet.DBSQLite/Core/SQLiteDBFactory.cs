using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Base;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Factory;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Core;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Interface;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Config;

namespace UtilZ.Dotnet.DBSQLite.Core
{
    /// <summary>
    /// SQLite数据访问工厂类
    /// </summary>
    public class SQLiteDBFactory : DBFactoryBase
    {
        /// <summary>
        /// 数据库交互实例字典集合[key:数据库编号ID;value:数据库交互实例]
        /// </summary>
        private readonly ConcurrentDictionary<int, DBInteractioBase> _dicWriteConsDBInteractions = new ConcurrentDictionary<int, DBInteractioBase>();

        /// <summary>
        /// 数据库交互实例字典集合锁
        /// </summary>
        private readonly object _dicWriteConsDBInteractionsMonitor = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLiteDBFactory() : base()
        {

        }

        /// <summary>
        /// 获取数据库交互实例
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <returns>数据库交互实例</returns>
        public override DBInteractioBase GetDBInteraction(DBConfigElement config)
        {
            DBInteractioBase dbInteraction;
            int dbid = config.DBID;
            if (this._dicWriteConsDBInteractions.ContainsKey(dbid))
            {
                dbInteraction = this._dicWriteConsDBInteractions[dbid];
            }
            else
            {
                lock (this._dicWriteConsDBInteractionsMonitor)
                {
                    if (this._dicWriteConsDBInteractions.ContainsKey(dbid))
                    {
                        dbInteraction = this._dicWriteConsDBInteractions[dbid];
                    }
                    else
                    {
                        dbInteraction = new SQLiteInteraction(config);
                        this._dicWriteConsDBInteractions.TryAdd(dbid, dbInteraction);
                    }
                }
            }

            return dbInteraction;
        }

        /// <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public override IDBAccess GetDBAccess(int dbid)
        {
            string databaseName = ((SQLiteInteraction)this.GetDBInteraction(ConfigManager.GetConfigItem(dbid))).DatabaseTypeName;
            return new SQLiteDBAccess(dbid, databaseName);
        }
    }
}
