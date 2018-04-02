using System;
using System.Collections.Concurrent;
using UtilZ.Dotnet.DBBase.Factory;
using UtilZ.Dotnet.DBBase.Interface;
using UtilZ.Dotnet.DBModel.Constant;
using UtilZ.Dotnet.Ex.Base;

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
            if (_dicDBAccess.ContainsKey(dbid))
            {
                if (!_dicDBAccess.TryGetValue(dbid, out dbAccess))
                {
                    throw new ApplicationException("获取数据库访问实例失败,原因未知");
                }
            }

            lock (_monitor)
            {
                if (_dicDBAccess.ContainsKey(dbid))
                {
                    if (!_dicDBAccess.TryGetValue(dbid, out dbAccess))
                    {
                        throw new ApplicationException(string.Format("获取数据库编号ID为{0}数据库访问实例失败", dbid));
                    }
                }
                else
                {
                    dbAccess = DBFactoryManager.GetDBFactory(dbid).GetDBAccess(dbid);
                    dbAccess.Init();
                    bool ret = Util.Add<int, IDBAccess>(_dicDBAccess, dbid, dbAccess, DBConstant.AddConcurrentDictionaryRepatCount);
                    if (!ret)
                    {
                        Console.WriteLine(string.Format("添加数据库编号ID为{0}数据库访问实例失败", dbid));
                    }
                }
            }

            return dbAccess;
        }
    }
}
