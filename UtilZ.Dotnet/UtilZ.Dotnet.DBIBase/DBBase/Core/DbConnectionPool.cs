using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.Common;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Constant;
using UtilZ.Dotnet.DBIBase.DBModel.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBIBase.DBBase.Core
{
    /// <summary>
    /// DbConnection连接池
    /// </summary>
    public class DbConnectionPool : IDisposable
    {
        /// <summary>
        /// 读连接对象集合池
        /// </summary>
        private readonly BlockingCollection<DbConnection> _readConPool = new BlockingCollection<DbConnection>();

        /// <summary>
        /// 写连接对象集合池
        /// </summary>
        private readonly BlockingCollection<DbConnection> _writeConPool = new BlockingCollection<DbConnection>();

        /// <summary>
        /// 数据库配置
        /// </summary>
        private DBConfigElement _config;

        /// <summary>
        /// 数据库交互实例
        /// </summary>
        private IDBInteraction _interaction;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="interaction">数据库交互实例</param>
        public DbConnectionPool(DBConfigElement config, IDBInteraction interaction)
        {
            this._config = config;
            this._interaction = interaction;
        }

        /// <summary>
        /// 初始化连接池
        /// </summary>
        private void InitPool()
        {
            for (int i = 0; i < this._config.ReadConCount; i++)
            {
                this._readConPool.Add(this._interaction.CreateConnection(this._config, DBVisitType.R));
            }

            for (int i = 0; i < this._config.WriteConCount; i++)
            {
                this._writeConPool.Add(this._interaction.CreateConnection(this._config, DBVisitType.W));
            }
        }

        /// <summary>
        /// 获取数据库访问连接对象
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>数据库访问连接对象</returns>
        private DbConnection GetDbConnection(DBVisitType visitType)
        {
            DbConnection con = null;
            if (visitType == DBVisitType.R)
            {
                if (this._config.ReadConCount < 1)
                {
                    con = this._interaction.CreateConnection(this._config, visitType);
                }
                else
                {
                    if (!this._readConPool.TryTake(out con, this._config.GetConTimeout))
                    {
                        throw new ApplicationException("从连接池获取读连接超时");
                    }
                }
            }
            else if (visitType == DBVisitType.W)
            {
                if (this._config.WriteConCount < 1)
                {
                    con = this._interaction.CreateConnection(this._config, visitType);
                }
                else
                {
                    if (!this._writeConPool.TryTake(out con, this._config.GetConTimeout))
                    {
                        throw new ApplicationException("从连接池获取写连接超时");
                    }
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("不支持的访问类型:{0}", visitType.ToString()));
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            return con;
        }

        /// <summary>
        /// 释放数据库访问连接对象
        /// </summary>
        /// <param name="con">数据库访问连接对象</param>
        /// <param name="visitType">数据库访问类型</param>
        private void ReleaseDbConnection(DbConnection con, DBVisitType visitType)
        {
            if (visitType == DBVisitType.R)
            {
                if (this._config.ReadConCount < 1)
                {
                    con.Close();
                }
                else
                {
                    this._readConPool.Add(con);
                }
            }
            else if (visitType == DBVisitType.W)
            {
                if (this._config.WriteConCount < 1)
                {
                    con.Close();
                }
                else
                {
                    this._writeConPool.Add(con);
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("不支持的访问类型:{0}", visitType.ToString()));
            }
        }

        #region IDispose接口实现
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDisposing">是否释放资源标识</param>
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                foreach (var readCon in this._readConPool)
                {
                    try
                    {
                        readCon.Close();
                    }
                    catch (Exception exi)
                    {
                        DBLog.OutLog(exi);
                    }
                }

                foreach (var writeCon in this._writeConPool)
                {
                    try
                    {
                        writeCon.Close();
                    }
                    catch (Exception exi)
                    {
                        DBLog.OutLog(exi);
                    }
                }

                this._readConPool.Dispose();
                this._writeConPool.Dispose();
            }
            catch (Exception ex)
            {
                DBLog.OutLog(ex);
            }
        }
        #endregion

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
        public static void AddDbConnectionPool(DBConfigElement config, IDBInteraction interaction)
        {
            int dbid = config.DBID;
            lock (_dbConnectionPoolDicMonitor)
            {
                DbConnectionPool dbConnectionPool;
                if (_dbConnectionPoolDic.ContainsKey(dbid))
                {
                    if (_dbConnectionPoolDic.TryRemove(dbid, out dbConnectionPool))
                    {
                        dbConnectionPool.Dispose();
                    }
                }

                dbConnectionPool = new DbConnectionPool(config, interaction);
                dbConnectionPool.InitPool();
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
                if (_dbConnectionPoolDic.ContainsKey(dbid))
                {
                    if (_dbConnectionPoolDic.TryRemove(dbid, out dbConnectionPool))
                    {
                        dbConnectionPool.Dispose();
                    }
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
            if (_dbConnectionPoolDic.ContainsKey(dbid))
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
            else
            {
                throw new ApplicationException(string.Format("连接池中不包含数据库编号ID为:{0}的连接信息", dbid));
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
