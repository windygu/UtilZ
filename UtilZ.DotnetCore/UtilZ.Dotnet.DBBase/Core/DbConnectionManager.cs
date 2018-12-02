using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBBase.Core
{
    /// <summary>
    /// 数据库连接管理类
    /// </summary>
    public class DbConnectionManager
    {
        #region 静态实例方法
        /// <summary>
        /// 连接池实例字典集合
        /// </summary>
        private readonly static ConcurrentDictionary<int, DbConnectionPool> _dbConnectionPoolDic = new ConcurrentDictionary<int, DbConnectionPool>();

        /// <summary>
        /// 连接池实例字典集合线程锁
        /// </summary>
        private readonly static object _dbConnectionPoolDicMonitor = new object();

        /// <summary>
        /// 添加数据库连接池
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="interaction">数据库交互实例</param>
        public static void AddDbConnectionPool(int dbid, DbConnectionPool dbConnectionPool)
        {
            if (dbConnectionPool == null)
            {
                throw new ArgumentNullException(nameof(dbConnectionPool));
            }

            lock (_dbConnectionPoolDicMonitor)
            {
                DbConnectionPool oldDBConnectionPool;
                if (_dbConnectionPoolDic.TryRemove(dbid, out oldDBConnectionPool))
                {
                    oldDBConnectionPool.Dispose();
                }

                _dbConnectionPoolDic.TryAdd(dbid, dbConnectionPool);
            }
        }

        /// <summary>
        /// 从数据库连接池中移除指定编号的数据库连接池实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        public static void RemoveDbConnectionPool(int dbid)
        {
            lock (_dbConnectionPoolDicMonitor)
            {
                DbConnectionPool dbConnectionPool;
                if (_dbConnectionPoolDic.TryRemove(dbid, out dbConnectionPool))
                {
                    dbConnectionPool.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取数据库访问连接对象
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>数据库访问连接对象</returns>
        public static DbConnection GetDbConnection(int dbid, DBVisitType visitType)
        {
            DbConnectionPool dbConnectionPool;
            if (_dbConnectionPoolDic.TryGetValue(dbid, out dbConnectionPool))
            {
                return dbConnectionPool.GetDbConnection(visitType);
            }
            else
            {
                throw new ApplicationException(string.Format("连接池中获取数据库编号ID为:{0}的连接池对象失败,原因未知", dbid));
            }
        }

        /// <summary>
        /// 释放数据库访问连接对象
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="con">数据库访问连接对象</param>
        /// <param name="visitType">数据库访问类型</param>
        public static void ReleaseDbConnection(int dbid, DbConnection con, DBVisitType visitType)
        {
            if (_dbConnectionPoolDic.ContainsKey(dbid))
            {
                DbConnectionPool dbConnectionPool;
                if (_dbConnectionPoolDic.TryGetValue(dbid, out dbConnectionPool))
                {
                    dbConnectionPool.ReleaseDbConnection(con, visitType);
                }
                else
                {
                    throw new ApplicationException(string.Format("连接池中获取数据库编号ID为:{0}的连接池对象失败,原因未知", dbid));
                }
            }
            else
            {
                throw new ApplicationException(string.Format("连接池中不包含数据库编号ID为:{0}的连接信息", dbid));
            }
        }
        #endregion
    }
}
