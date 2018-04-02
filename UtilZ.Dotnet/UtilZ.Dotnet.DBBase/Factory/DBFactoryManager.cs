using System;
using System.Collections.Concurrent;
using UtilZ.Dotnet.DBModel.Config;
using UtilZ.Dotnet.DBModel.Constant;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBBase.Factory
{
    /// <summary>
    /// 数据库访问工厂管理类
    /// </summary>
    public class DBFactoryManager
    {
        /// <summary>
        /// 数据库连接信息集合[key:数据库访问工厂名称,value:数据库访问工厂实例]
        /// </summary>
        private static readonly ConcurrentDictionary<string, DBFactoryBase> _dbFactorys = new ConcurrentDictionary<string, DBFactoryBase>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object _monitor = new object();

        ///// <summary>
        ///// 获取数据库访问工厂实例
        ///// </summary>
        ///// <param name="dbid">数据库编号ID</param>
        ///// <returns>数据库访问工厂实例</returns>
        //public static DBFactoryBase GetDBFactory(int dbid)
        //{
        //    var dbItem = ConfigManager.GetConfigItem(dbid);
        //    string dbFactoryName = dbItem.DBFactory;
        //    DBFactoryBase dbFactory;
        //    if (_dbFactorys.ContainsKey(dbFactoryName))
        //    {
        //        dbFactory = GetDBFactory(dbFactoryName);
        //    }
        //    else
        //    {
        //        lock (_monitor)
        //        {
        //            if (_dbFactorys.ContainsKey(dbFactoryName))
        //            {
        //                dbFactory = GetDBFactory(dbFactoryName);
        //            }
        //            else
        //            {
        //                dbFactory = CreateDBFactory(dbItem);
        //            }
        //        }
        //    }

        //    return dbFactory;
        //}

        /// <summary>
        /// 获取数据库访问工厂实例
        /// </summary>
        /// <param name="dbItem">配置</param>
        /// <returns>数据库访问工厂实例</returns>
        private static DBFactoryBase CreateDBFactory(DBConfigElement dbItem)
        {
            string dbFactoryName = dbItem.DBFactory;
            var dbFactory = ActivatorEx.CreateInstance(dbFactoryName, dbItem) as DBFactoryBase;
            if (dbFactory != null)
            {
                bool ret = Util.Add<string, DBFactoryBase>(_dbFactorys, dbFactoryName, dbFactory, DBConstant.AddConcurrentDictionaryRepatCount);
                if (!ret)
                {
                    throw new ApplicationException(string.Format("添加名称为{0}数据库访问工厂实例失败", dbFactoryName));
                }
            }
            else
            {
                throw new ApplicationException(string.Format("创建名称为{0}数据库访问工厂实例失败", dbFactoryName));
            }

            return dbFactory;
        }

        /// <summary>
        /// 获取数据库访问工厂实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问工厂实例</returns>
        public static DBFactoryBase GetDBFactory(int dbid)
        {
            DBFactoryBase dbFactory;
            var dbItem = ConfigManager.GetConfigItem(dbid);
            string dbFactoryName = dbItem.DBFactory;
            if (_dbFactorys.ContainsKey(dbFactoryName))
            {
                if (!_dbFactorys.TryGetValue(dbFactoryName, out dbFactory))
                {
                    throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
                }
            }
            else
            {
                lock (_monitor)
                {
                    if (_dbFactorys.ContainsKey(dbFactoryName))
                    {
                        if (!_dbFactorys.TryGetValue(dbFactoryName, out dbFactory))
                        {
                            throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
                        }
                    }
                    else
                    {
                        dbFactory = ActivatorEx.CreateInstance(dbFactoryName) as DBFactoryBase;
                        if (dbFactory != null)
                        {
                            bool ret = Util.Add<string, DBFactoryBase>(_dbFactorys, dbFactoryName, dbFactory, DBConstant.AddConcurrentDictionaryRepatCount);
                            if (!ret)
                            {
                                throw new ApplicationException(string.Format("添加名称为{0}数据库访问工厂实例失败", dbFactoryName));
                            }
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("创建名称为{0}数据库访问工厂实例失败", dbFactoryName));
                        }
                    }
                }
            }

            if (dbFactory == null)
            {
                throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
            }

            return dbFactory;
        }

        ///// <summary>
        ///// 获取数据库访问实例
        ///// </summary>
        ///// <param name="dbid">数据库编号ID</param>
        ///// <returns>数据库访问实例</returns>
        //public static DBInteractioBase GetDBInteractionObj(int dbid)
        //{
        //    return GetDBFactory(dbid).GetDBInteraction();
        //}

        /*
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="conType">连接类型</param>
        /// <returns>数据库连接对象</returns>
        public static IDbConnection CreateDbConnection(int dbid, DBVisitType conType)
        {
            DBConfigElement dbItem = ConfigManager.GetConfigItem(dbid);
            DBFactoryBase dbFactory = null;
            string dbFactoryName = dbItem.DBFactory;
            if (_dbFactorys.ContainsKey(dbFactoryName))
            {
                dbFactory = GetDBFactory(dbFactoryName);
            }
            else
            {
                lock (_monitor)
                {
                    if (_dbFactorys.ContainsKey(dbFactoryName))
                    {
                        dbFactory = GetDBFactory(dbFactoryName);
                    }
                    else
                    {
                        dbFactory = CreateDBFactory(dbItem);
                    }
                }
            }

            if (dbFactory != null)
            {
                DBInteractioBase dbInteraction = dbFactory.GetDBInteraction();
                if (dbInteraction != null)
                {
                    IDbConnection con = null;
                    switch (conType)
                    {
                        case DBVisitType.R:
                            con = dbInteraction.CreateReadConnection(dbItem);
                            break;
                        case DBVisitType.W:
                            con = dbInteraction.CreateWriteConnection(dbItem);
                            break;
                    }

                    if (con == null)
                    {
                        throw new ApplicationException(string.Format("类型{0}中创建连接返回值为null", dbInteraction.GetType().FullName));
                    }

                    if (string.IsNullOrEmpty(con.ConnectionString))
                    {
                        con.ConnectionString = dbInteraction.GetDBConStr(dbItem);
                    }

                    return con;
                }
            }

            throw new ApplicationException(string.Format("创建数据库编号为{0}数据库连接实例失败", dbid));
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>创建好的DbDataAdapter</returns>
        public static IDbDataAdapter CreateDbDataAdapter(int dbid)
        {
            return GetDBInteractionObj(dbid).CreateDbDataAdapter();
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="dbDataParameter">命令参数</param>
        /// <param name="parameter">参数</param>
        /// <returns>创建好的命令参数</returns>
        public static void SetParameter(int dbid, IDbDataParameter cmdParameter, NDbParameter parameter)
        {
            GetDBInteractionObj(dbid).SetParameter(cmdParameter, parameter);
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="cmd">命令</param>
        /// <param name="collection">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public static void SetParameter(int dbid, IDbCommand cmd, NDbParameterCollection collection)
        {
            GetDBInteractionObj(dbid).SetParameter(cmd, collection);
        }*/
    }
}
