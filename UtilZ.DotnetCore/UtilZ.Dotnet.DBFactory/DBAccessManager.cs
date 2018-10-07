using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBFactory
{
    /// <summary>
    /// 数据库访问工厂管理类
    /// </summary>
    public class DBAccessManager
    {
        /// <summary>
        /// 数据库访问对象字典集合
        /// </summary>
        private static readonly ConcurrentDictionary<int, IDBAccess> _dbAccessDic = new ConcurrentDictionary<int, IDBAccess>();

        /// <summary>
        /// 数据库访问对象字典集合锁
        /// </summary>
        private static readonly object _dbAccessDicLock = new object();

        // <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public static IDBAccess GetDBAccess(int dbid)
        {
            IDBAccess dbAccess;
            if (!_dbAccessDic.TryGetValue(dbid, out dbAccess))
            {
                lock (_dbAccessDicLock)
                {
                    if (!_dbAccessDic.TryGetValue(dbid, out dbAccess))
                    {
                        dbAccess = CreateDBAccess(dbid);
                        if (!_dbAccessDic.TryAdd(dbid, dbAccess))
                        {
                            DBLog.OutLog("IDBAccess对象添加到_dbAccessDic字典集合中失败,原因未知");
                        }
                    }
                }
            }

            return dbAccess;
        }

        // <summary>
        /// 创建数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        private static IDBAccess CreateDBAccess(int dbid)
        {
            IDBAccess dbAccess;
            var dbConfig = ConfigManager.GetConfigByDBID(dbid);
            switch (dbConfig.DBType)
            {
                case DataBaseType.MySql:
                    dbAccess = new DBMySql.Core.MySqlDBAccess(dbid, new DBMySql.Core.MySqlInteractiveBuilder());
                    break;
                case DataBaseType.Oracle:
                    dbAccess = new DBOracle.Core.OracleDBAccess(dbid, new DBOracle.Core.OracleInteractiveBuilder());
                    break;
                case DataBaseType.SQLite:
                    dbAccess = new DBSQLite.Core.SQLiteDBAccess(dbid, new DBSQLite.Core.SQLiteInteractiveBuilder());
                    break;
                case DataBaseType.SQLServer:
                    dbAccess = new DBSQLServer.Core.SQLServerDBAccess(dbid, new DBSQLServer.Core.SQLServerInteractiveBuilder());
                    break;
                default:
                    throw new NotImplementedException(string.Format("数据库类型:{0}未实现访问对象创建", dbConfig.DBType.ToString()));
            }

            return dbAccess;
        }
    }
}
