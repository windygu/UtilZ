using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using UtilZ.DotnetStd.Ex.Log;

namespace UtilZ.DotnetStd.Ex.Base
{
    /// <summary>
    /// .NET系统缓存类简单封装
    /// </summary>
    public class MemoryCacheEx
    {
        private readonly static MemoryCacheExCore _default;
        static MemoryCacheEx()
        {
            _default = new MemoryCacheExCore();
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
        public static void Set(object key, object value, int expiration, PostEvictionDelegate removedCallback)
        {
            _default.Set(key, value, expiration, removedCallback);
        }

        /// <summary>
        /// 创建缓存实体
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存实体</returns>
        public static ICacheEntry CreateEntry(object key)
        {
            return _default.CreateEntry(key);
        }

        /// <summary>
        /// 提交缓存实体
        /// </summary>
        /// <param name="cacheEntry">缓存实体</param>
        public static void CommitEntry(ICacheEntry cacheEntry)
        {
            _default.CommitEntry(cacheEntry);
        }

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

    public class MemoryCacheExCore : IDisposable
    {
        private readonly MemoryCache _memoryCache;
        public MemoryCacheExCore()
        {
            this._memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存项,获取成功返回缓存项,值过期或key不存在返回null</returns>
        public object Get(object key)
        {
            if (key == null)
            {
                return null;
            }

            //return System.Runtime.Caching.MemoryCache.Default.Get(key);
            return this._memoryCache.Get(key);
            //object value;
            //this._memoryCache.TryGetValue(key, out value);
            //return value;
        }

        /// <summary>
        /// 是否存在key值的数据[存在返回true;不存在返回false]
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在返回true;不存在返回false</returns>
        public bool Exist(object key)
        {
            if (key == null)
            {
                return false;
            }

            return this.Get(key) != null;
        }

        /// <summary>
        /// 移除一个缓存项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>移除的缓存项</returns>
        public object Remove(object key)
        {
            //return System.Runtime.Caching.MemoryCache.Default.Remove(key);
            if (key == null)
            {
                return null;
            }

            object value;
            if (this._memoryCache.TryGetValue(key, out value))
            {
                this._memoryCache.Remove(key);
            }
            return value;
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public void Add(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            this.Set(key, value);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public void Add(object key, object value, int expiration)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            this.Set(key, value, expiration);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public void Set(object key, object value)
        {
            //缓存数据有效项为1000年,当作永久有效
            //System.Runtime.Caching.MemoryCache.Default.Set(key, value, new DateTimeOffset(DateTime.Now.AddDays(Util.GetForeverTimeSpan().Days)));

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            using (ICacheEntry cacheEntry = this._memoryCache.CreateEntry(key))
            {
                cacheEntry.Value = value;
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expirationMilliseconds">缓存项有效时间,单位/毫秒</param>
        public void Set(object key, object value, int expirationMilliseconds)
        {
            //System.Runtime.Caching.MemoryCache.Default.Set(key, value, new DateTimeOffset(DateTime.Now.AddMilliseconds(expiration)));

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            using (ICacheEntry cacheEntry = this._memoryCache.CreateEntry(key))
            {
                cacheEntry.Value = value;
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expirationMilliseconds));
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expirationMilliseconds">缓存项有效时间,单位/毫秒</param>
        /// <param name="removedCallback">移除回调</param>
        public void Set(object key, object value, int expirationMilliseconds, PostEvictionDelegate removedCallback)
        {
            //CacheItem item = new CacheItem(key, value);
            //CacheItemPolicy policy = new CacheItemPolicy();
            //policy.RemovedCallback = removedCallback;
            //policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMilliseconds(expiration));
            //System.Runtime.Caching.MemoryCache.Default.Set(item, policy);

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            using (ICacheEntry cacheEntry = this._memoryCache.CreateEntry(key))
            {
                cacheEntry.Value = value;
                cacheEntry.RegisterPostEvictionCallback(removedCallback);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expirationMilliseconds));
            }
        }

        /// <summary>
        /// 创建缓存实体
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存实体</returns>
        public ICacheEntry CreateEntry(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return this._memoryCache.CreateEntry(key);
        }

        /// <summary>
        /// 提交缓存实体
        /// </summary>
        /// <param name="cacheEntry">缓存实体</param>
        public void CommitEntry(ICacheEntry cacheEntry)
        {
            if (cacheEntry == null)
            {
                return;
            }

            cacheEntry.Dispose();
        }


        public void Dispose()
        {
            try
            {
                this._memoryCache.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}