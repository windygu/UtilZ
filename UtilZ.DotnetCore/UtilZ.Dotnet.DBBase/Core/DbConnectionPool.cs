using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;

namespace UtilZ.Dotnet.DBBase.Core
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
        private DBConfig _config;

        /// <summary>
        /// 数据库交互创建对象
        /// </summary>
        private readonly IDBInteractiveBuilder _dbInteractiveBase;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="dbInteractive">数据库交互创建对象</param>
        public DbConnectionPool(DBConfig config, IDBInteractiveBuilder dbInteractive)
        {
            this._config = config;
            this._dbInteractiveBase = dbInteractive;

            //初始化连接池
            for (int i = 0; i < this._config.ReadConnectionCount; i++)
            {
                this._readConPool.Add(this._dbInteractiveBase.CreateConnection(this._config, DBVisitType.R));
            }

            for (int i = 0; i < this._config.WriteConnectionCount; i++)
            {
                this._writeConPool.Add(this._dbInteractiveBase.CreateConnection(this._config, DBVisitType.W));
            }
        }

        /// <summary>
        /// 获取数据库访问连接对象
        /// </summary>
        /// <param name="visitType">数据库访问类型</param>
        /// <returns>数据库访问连接对象</returns>
        public DbConnection GetDbConnection(DBVisitType visitType)
        {
            DbConnection con = null;
            switch (visitType)
            {
                case DBVisitType.R:
                    if (this._config.ReadConnectionCount < 1)
                    {
                        con = this._dbInteractiveBase.CreateConnection(this._config, DBVisitType.R);
                    }
                    else
                    {
                        if (!this._readConPool.TryTake(out con, this._config.GetConnectionObjectTimeout))
                        {
                            throw new ApplicationException("从连接池获取读连接超时");
                        }
                    }
                    break;
                case DBVisitType.W:
                    if (this._config.WriteConnectionCount < 1)
                    {
                        con = this._dbInteractiveBase.CreateConnection(this._config, DBVisitType.W);
                    }
                    else
                    {
                        if (!this._writeConPool.TryTake(out con, this._config.GetConnectionObjectTimeout))
                        {
                            throw new ApplicationException("从连接池获取写连接超时");
                        }
                    }
                    break;
                default:
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
        internal void ReleaseDbConnection(DbConnection con, DBVisitType visitType)
        {
            if (visitType == DBVisitType.R)
            {
                if (this._config.ReadConnectionCount < 1)
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
                if (this._config.WriteConnectionCount < 1)
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
    }
}
