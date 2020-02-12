using System;
using System.Collections.Generic;
using UtilZ.DotnetStd.Ex.Log;

namespace UtilZ.DotnetStd.Ex.Base.MemoryCache
{
    /// <summary>
    /// 内存缓存类
    /// </summary>
    public class MemoryCacheEx
    {
        private readonly static ObjectCache _default;

        static MemoryCacheEx()
        {
            _default = new ObjectCache();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存项,获取成功返回缓存项,值过期或key不存在返回null</returns>
        public static object Get(object key)
        {
            return _default.Get(key);
        }

        /// <summary>
        /// 是否存在key值的数据[存在返回true;不存在返回false]
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在返回true;不存在返回false</returns>
        public static bool Exist(object key)
        {
            return _default.Exist(key);
        }

        /// <summary>
        /// 移除一个缓存项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>移除的缓存项</returns>
        public static object Remove(object key)
        {
            return _default.Remove(key);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Add(object key, object value)
        {
            _default.Add(key, value);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Add(object key, object value, int expiration)
        {
            _default.Add(key, value, expiration);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Set(object key, object value)
        {
            _default.Set(key, value);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Set(object key, object value, int expiration)
        {
            _default.Set(key, value, expiration);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        /// <param name="removedCallback">移除回调</param>
        public static void Set(object key, object value, int expiration, CacheEntryRemovedCallback removedCallback)
        {
            _default.Set(key, value, expiration, removedCallback);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void Dispose()
        {
            try
            {
                _default.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}