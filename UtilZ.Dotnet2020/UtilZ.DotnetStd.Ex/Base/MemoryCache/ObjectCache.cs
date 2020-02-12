using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UtilZ.DotnetStd.Ex.Log;

namespace UtilZ.DotnetStd.Ex.Base.MemoryCache
{
    /// <summary>
    /// 原始缓存类
    /// </summary>
    public class ObjectCache : IDisposable
    {
        private readonly ConcurrentDictionary<object, object> _cacheDic = new ConcurrentDictionary<object, object>();
        public ObjectCache()
        {

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

            object value;
            this._cacheDic.TryGetValue(key, out value);
            return value;
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

            return this._cacheDic.ContainsKey(key);
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
            this._cacheDic.TryRemove(key, out value);
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

            this._cacheDic.AddOrUpdate(key, value, (o, n) =>
            {
                //todo..触发移除
                return value;
            });
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

            //using (ICacheEntry cacheEntry = this._memoryCache.CreateEntry(key))
            //{
            //    cacheEntry.Value = value;
            //    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expirationMilliseconds));
            //}
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expirationMilliseconds">缓存项有效时间,单位/毫秒</param>
        /// <param name="removedCallback">移除回调</param>
        public void Set(object key, object value, int expirationMilliseconds, CacheEntryRemovedCallback removedCallback)
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

            //using (ICacheEntry cacheEntry = this._memoryCache.CreateEntry(key))
            //{
            //    cacheEntry.Value = value;
            //    cacheEntry.RegisterPostEvictionCallback(removedCallback);
            //    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expirationMilliseconds));
            //}
        }



        public void Dispose()
        {
            try
            {
                //this._memoryCache.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
