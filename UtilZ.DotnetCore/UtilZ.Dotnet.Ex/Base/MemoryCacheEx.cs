using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// .NET系统缓存类简单封装
    /// </summary>
    public class MemoryCacheEx
    {
        /*
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
            if (Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            Set(key, value);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Add(string key, object value, int expiration)
        {
            if (Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            Set(key, value, expiration);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Set(string key, object value)
        {
            //缓存数据有效项为1000年,当作永久有效
            System.Runtime.Caching.MemoryCache.Default.Set(key, value, new DateTimeOffset(DateTime.Now.AddDays(Util.GetForeverTimeSpan().Days)));
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
            if (Exist(key))
            {
                throw new ArgumentException(string.Format("key:{0}已存在", key));
            }

            MemoryCacheEx.SetWeakReference(key, value, expiration);
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
                Remove(key);
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
            return Get(key) != null;
        }*/

        private readonly static Hashtable _htCacheItems = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 有效性验证线程
        /// </summary>
        private readonly static IThreadEx _validThread = null;

        /// <summary>
        /// 有效性验证时间间隔
        /// </summary>
        private static int _interval = 20;

        /// <summary>
        /// 获取或设置有效性验证时间间隔
        /// </summary>
        public static int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        ///// <summary>
        ///// 线程等待通知信号对象
        ///// </summary>
        //private readonly static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MemoryCacheEx()
        {
            _validThread = new ThreadEx(ValueValidThreadMethod, "内存缓存有效性验证线程", true);
            _validThread.Start();
        }

        /// <summary>
        /// 值有效性验证线程方法
        /// </summary>
        /// <param name="token">取消线程对象</param>
        private static void ValueValidThreadMethod(CancellationToken token)
        {
            //当前所有项集合
            var items = new List<MemoryCacheItem>();
            DateTime currentTime;
            while (true)
            {
                try
                {
                    items.Clear();
                    //获取所有项
                    lock (_htCacheItems.SyncRoot)
                    {
                        foreach (MemoryCacheItem item in _htCacheItems.Values)
                        {
                            items.Add(item);
                        }
                    }

                    //如果项数为0,则等待新项添加后的通知
                    if (items.Count == 0)
                    {
                        //_autoResetEvent.WaitOne(waitMs);
                        continue;
                    }

                    currentTime = DateTime.Now;
                    //遍历项查找是否超时
                    foreach (var item in items)
                    {
                        if (item.Validate(currentTime))
                        {
                            _htCacheItems.Remove(item.Key);
                            item.OnRaiseCacheEntryRemovedCallback();
                        }
                    }
                }
                catch (Exception)
                { }
                finally
                {
                    Thread.Sleep(_interval);
                }
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>缓存项,获取成功返回缓存项,值过期或key不存在返回null</returns>
        public static object Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var item = _htCacheItems[key] as MemoryCacheItem;
            return item == null ? null : item.GetValue();
        }

        /// <summary>
        /// 移除一个缓存项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>移除的缓存项</returns>
        public static object Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var item = _htCacheItems[key] as MemoryCacheItem;
            _htCacheItems.Remove(key);
            return item == null ? null : item.GetValue();
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_htCacheItems.SyncRoot)
            {
                if (_htCacheItems.ContainsKey(key))
                {
                    throw new ArgumentException(string.Format("key:{0}已存在", key));
                }

                _htCacheItems[key] = new MemoryCacheItem(key, value);
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Add(string key, object value, int expiration)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_htCacheItems.SyncRoot)
            {
                if (_htCacheItems.ContainsKey(key))
                {
                    throw new ArgumentException(string.Format("key:{0}已存在", key));
                }

                _htCacheItems[key] = new MemoryCacheItem(key, value, expiration);
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        public static void Set(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_htCacheItems.SyncRoot)
            {
                var oldItem = _htCacheItems[key] as MemoryCacheItem;
                if (oldItem != null)
                {
                    oldItem.UpdateValue(value);
                }
                else
                {
                    _htCacheItems[key] = new MemoryCacheItem(key, value);
                }
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        public static void Set(string key, object value, int expiration)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_htCacheItems.SyncRoot)
            {
                var oldItem = _htCacheItems[key] as MemoryCacheItem;
                if (oldItem != null)
                {
                    oldItem.UpdateValue(value, expiration);
                }
                else
                {
                    _htCacheItems[key] = new MemoryCacheItem(key, value, expiration);
                }
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">缓存项</param>
        /// <param name="expiration">缓存项有效时间,单位/毫秒</param>
        /// <param name="removedCallback">移除回调[key,value]</param>
        public static void Set(string key, object value, int expiration, Action<string, object> removedCallback)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_htCacheItems.SyncRoot)
            {
                var oldItem = _htCacheItems[key] as MemoryCacheItem;
                if (oldItem != null)
                {
                    oldItem.UpdateValue(value, expiration);
                }
                else
                {
                    _htCacheItems[key] = new MemoryCacheItem(key, value, expiration, removedCallback);
                }
            }
        }

        /// <summary>
        /// 是否存在key值的数据[存在返回true;不存在返回false]
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在返回true;不存在返回false</returns>
        public static bool Exist(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _htCacheItems.ContainsKey(key);
        }
    }

    /// <summary>
    /// 内存缓存项
    /// </summary>
    [Serializable]
    internal class MemoryCacheItem
    {
        /// <summary>
        /// key
        /// </summary>
        private string _key;

        public string Key { get { return _key; } }

        /// <summary>
        /// 缓存值
        /// </summary>
        private object _value = null;

        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime _updateTime;

        /// <summary>
        /// 有效时间类型
        /// </summary>
        private MemoryTimeOutType _timeOutType;

        /// <summary>
        /// 有效值到指定时间点
        /// </summary>
        private DateTime _expiresAt;

        private Action<string, object> _cacheEntryRemovedCallback;

        internal object GetValue()
        {
            lock (this)
            {
                return _value;
            }
        }

        internal void UpdateValue(object value)
        {
            lock (this)
            {
                this._value = value;
                this._timeOutType = MemoryTimeOutType.Never;
                this._updateTime = DateTime.Now;
            }
        }

        internal void UpdateValue(object value, int expiration)
        {
            lock (this)
            {
                this._value = value;
                this._timeOutType = MemoryTimeOutType.ExpiresAt;
                this._updateTime = DateTime.Now;
                this._expiresAt = this._updateTime.AddMilliseconds(expiration);
            }
        }

        internal bool Validate(DateTime currentTime)
        {
            lock (this)
            {
                switch (_timeOutType)
                {
                    case MemoryTimeOutType.ExpiresAt:
                        return currentTime >= this._expiresAt;
                    case MemoryTimeOutType.Never:
                    default:
                        return false;
                }
            }
        }

        protected MemoryCacheItem(string key, object value, MemoryTimeOutType timeOutType, DateTime expiresAt, Action<string, object> removedCallback)
        {
            this._key = key;
            this._value = value;
            this._timeOutType = timeOutType;
            this._expiresAt = expiresAt;
            this._updateTime = DateTime.Now;
        }

        public MemoryCacheItem(string key, object value) :
            this(key, value, MemoryTimeOutType.Never, DateTime.Now, null)
        {

        }

        public MemoryCacheItem(string key, object value, int expiration, Action<string, object> removedCallback = null) :
            this(key, value, MemoryTimeOutType.ExpiresAt, DateTime.Now.AddMilliseconds(expiration), removedCallback)
        {

        }

        public MemoryCacheItem(string key, object value, DateTime expiresAt, Action<string, object> removedCallback = null) :
           this(key, value, MemoryTimeOutType.ExpiresAt, expiresAt, removedCallback)
        {

        }

        public void OnRaiseCacheEntryRemovedCallback()
        {
            var handler = this._cacheEntryRemovedCallback;
            if (handler == null)
            {
                return;
            }

            //Task.Factory.StartNew(() =>
            //{
            try
            {
                handler(this._key, this._value);
            }
            catch
            { }
            //});
        }
    }

    /// <summary>
    /// 超时时间类型
    /// </summary>
    internal enum MemoryTimeOutType
    {
        /// <summary>
        /// 有效值到达指定时刻
        /// </summary>
        ExpiresAt = 1,

        /// <summary>
        /// 从不
        /// </summary>
        Never = 2
    }
}