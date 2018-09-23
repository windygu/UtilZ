using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.LRPC;

namespace UtilZ.Dotnet.DBBase.Core
{
    /// <summary>
    /// 数据库访问类管理器
    /// </summary>
    public class DBAccessManager
    {
        /// <summary>
        /// 数据库访问对象实例字典[key:数据库编号ID;value:数据库访问实例]
        /// </summary>
        private static readonly ConcurrentDictionary<int, IDBAccess> _dicDBAccess = new ConcurrentDictionary<int, IDBAccess>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object _monitor = new object();

        /// <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public static IDBAccess GetDBAccessInstance(int dbid)
        {
            IDBAccess dbAccess;
            if (!_dicDBAccess.TryGetValue(dbid, out dbAccess))
            {
                lock (_monitor)
                {
                    if (!_dicDBAccess.TryGetValue(dbid, out dbAccess))
                    {
                        dbAccess = DBFactoryManager.GetDBAccessByDBID(dbid);
                        if (!_dicDBAccess.TryAdd(dbid, dbAccess))
                        {
                            Console.WriteLine(string.Format("添加数据库编号ID为{0}数据库访问实例失败", dbid));
                        }
                    }
                }
            }

            return dbAccess;
        }
    }
}
