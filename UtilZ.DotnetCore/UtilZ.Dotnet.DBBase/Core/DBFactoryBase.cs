using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;

namespace UtilZ.Dotnet.DBBase.Core
{
    /// <summary>
    /// 工厂基类
    /// </summary>
    public abstract class DBFactoryBase : IDBFactory
    {
        /// <summary>
        /// 数据库访问对象字典集合
        /// </summary>
        private readonly ConcurrentDictionary<int, IDBAccess> _dbAccessDic = new ConcurrentDictionary<int, IDBAccess>();

        /// <summary>
        /// 数据库访问对象字典集合锁
        /// </summary>
        private readonly object _dbAccessDicLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBFactoryBase()
        {

        }

        // <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public IDBAccess GetDBAccess(int dbid)
        {
            IDBAccess dbAccess;
            if (!this._dbAccessDic.TryGetValue(dbid, out dbAccess))
            {
                lock (this._dbAccessDicLock)
                {
                    if (!this._dbAccessDic.TryGetValue(dbid, out dbAccess))
                    {
                        dbAccess = this.CreateDBAccess(dbid);
                        if (!this._dbAccessDic.TryAdd(dbid, dbAccess))
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
        protected abstract IDBAccess CreateDBAccess(int dbid);
    }
}
