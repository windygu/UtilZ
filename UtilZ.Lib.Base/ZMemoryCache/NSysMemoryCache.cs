using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace UtilZ.Lib.Base.ZMemoryCache
{
    /// <summary>
    /// .NET系统缓存类简单封装
    /// </summary>
    public class NSysMemoryCache
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存项,获取成功返回缓存项,值过期或key不存在返回null</returns>
        public static object Get(string key)
        {
            return System.Runtime.Caching.MemoryCache.Default.Get(key);
        }

        /// <summary>
        /// 移除一个缓存项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>移除的缓存项</returns>
        public static object Remove(string key)
        {
            return System.Runtime.Caching.MemoryCache.Default.Remove(key);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Add(string key, object value)
        {
            if (NSysMemoryCache.Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            NSysMemoryCache.Set(key, value);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Add(string key, object value, int expiration)
        {
            if (NSysMemoryCache.Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            NSysMemoryCache.Set(key, value, expiration);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Set(string key, object value)
        {
            //缓存数据有效项为1000年,当作永久有效
            System.Runtime.Caching.MemoryCache.Default.Set(key, value, new DateTimeOffset(DateTime.Now.AddDays(NBaeHepler.GetForeverTimeSpan().Days)));
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Set(string key, object value, int expiration)
        {
            System.Runtime.Caching.MemoryCache.Default.Set(key, value, new DateTimeOffset(DateTime.Now.AddMilliseconds(expiration)));
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        /// <param name="removedCallback">移除回调</param>
        public static void Set(string key, object value, int expiration, CacheEntryRemovedCallback removedCallback)
        {
            CacheItem item = new CacheItem(key, value);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.RemovedCallback = removedCallback;
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMilliseconds(expiration));
            System.Runtime.Caching.MemoryCache.Default.Set(item, policy);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="policy">缓存策略</param>
        public static void Set(string key, object value, CacheItemPolicy policy)
        {
            CacheItem item = new CacheItem(key, value);
            System.Runtime.Caching.MemoryCache.Default.Set(item, policy);
        }

        /// <summary>
        /// 存储弱引用数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void SetWeakReference(string key, object value, int expiration)
        {
            WeakReference item = new WeakReference(value);
            System.Runtime.Caching.MemoryCache.Default.Set(key, item, new DateTimeOffset(DateTime.Now.AddMilliseconds(expiration)));
        }

        /// <summary>
        /// 存储弱引用数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void AddWeakReference(string key, object value, int expiration)
        {
            if (NSysMemoryCache.Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            NSysMemoryCache.SetWeakReference(key, value, expiration);
        }

        /// <summary>
        /// 获取弱引用数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存项,获取成功返回缓存项,值过期或key不存在返回null</returns>
        public static object GetWeakReference(string key)
        {
            WeakReference item = System.Runtime.Caching.MemoryCache.Default.Get(key) as WeakReference;
            if (item == null)
            {
                return null;
            }

            if (item.IsAlive)
            {
                return item.Target;
            }
            else
            {
                //对象已消亡,从缓存中将弱引用对象移除
                NSysMemoryCache.Remove(key);
                return null;
            }
        }

        /// <summary>
        /// 是否存在key值的数据[存在返回true;不存在返回false]
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在返回true;不存在返回false</returns>
        public static bool Exist(string key)
        {
            return NSysMemoryCache.Get(key) != null;
        }
    }
}