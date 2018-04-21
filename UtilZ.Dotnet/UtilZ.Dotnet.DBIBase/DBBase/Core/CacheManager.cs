using System;
using System.Data;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBIBase.DBBase.Core
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// 获取缓存Key
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="tableName">列表</param>
        /// <param name="cacheType">缓存类型</param>
        /// <returns>缓存Key</returns>
        public static string GetCacheKey(int dbid, string tableName, CacheType cacheType)
        {
            string type = null;
            switch (cacheType)
            {
                case CacheType.PriKeyCols:
                    type = "PriKeyCols";
                    break;
                case CacheType.TableInfo:
                    type = "TableInfo";
                    break;
                case CacheType.FieldInfo:
                    type = "FieldInfo";
                    break;
                default:
                    throw new NotSupportedException(string.Format("未知的缓存类型{0}", cacheType));
            }

            return dbid + tableName + type;
        }

        #region 缓存信息
        /// <summary>
        /// 数据缓存时间默认10分钟
        /// </summary>
        private static int _cacheTime = 10 * 60 * 1000;

        /// <summary>
        /// 获取或设置数据缓存时间默认10分钟
        /// </summary>
        public static int CacheTime
        {
            get { return _cacheTime; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("缓存时间不能为负数", "value");
                }

                _cacheTime = value;
            }
        }

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        private static bool _enableCache = true;

        /// <summary>
        /// 获取或设置是否启用缓存[默认启用]
        /// </summary>
        public static bool EnableCache
        {
            get { return _enableCache; }
            set { _enableCache = value; }
        }

        /// <summary>
        /// 存储缓存数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void StoreCacheData(string key, object value)
        {
            if (_enableCache)
            {
                MemoryCacheEx.Set(key, value);
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void StoreCacheData(string key, object value, int expiration)
        {
            if (_enableCache)
            {
                MemoryCacheEx.Set(key, value, expiration);
            }
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存项,获取成功返回缓存项,值过期或key不存在返回null</returns>
        public static object GetCacheData(string key)
        {
            if (_enableCache)
            {
                return MemoryCacheEx.Get(key);
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 获取数据表信息
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="dbAccess">数据库访问对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>数据表信息</returns>
        public static DBTableInfo GetDBTableInfo(IDbConnection con, DBAccessBase dbAccess, string tableName)
        {
            string cacheKey = CacheManager.GetCacheKey(dbAccess.DBID, tableName, CacheType.TableInfo);//字段缓存key
            DBTableInfo dbTableInfo = CacheManager.GetCacheData(cacheKey) as DBTableInfo;//从缓存中获取表信息
            if (dbTableInfo == null)
            {
                //如果缓存中获取到手表信息为null,则查询该表的表信息
                dbTableInfo = dbAccess.InnerGetTableInfo(con, tableName, true);
                if (dbTableInfo == null)
                {
                    throw new ApplicationException(string.Format("表{0}不存在", tableName));
                }

                //添加表信息到缓存中
                CacheManager.StoreCacheData(cacheKey, dbTableInfo, CacheManager.CacheTime);
            }

            return dbTableInfo;
        }
    }

    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 主键列集合
        /// </summary>
        PriKeyCols,

        /// <summary>
        /// 表信息
        /// </summary>
        TableInfo,

        /// <summary>
        /// 字段信息
        /// </summary>
        FieldInfo
    }
}
