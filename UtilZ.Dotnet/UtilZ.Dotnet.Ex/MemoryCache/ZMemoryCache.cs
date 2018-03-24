using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.DataStruct.Threading;

namespace UtilZ.Dotnet.Ex.MemoryCache
{
    /// <summary>
    /// 内存缓存器类
    /// </summary>
    public sealed class ZMemoryCache
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ZMemoryCache()
        {
            _validThread = new ThreadEx(ZMemoryCache.ValueValidThreadMethod, "内存缓存器线程", true);
            _validThread.Start();
        }

        /// <summary>
        /// 胡交性验证线程
        /// </summary>
        private readonly static IThreadEx _validThread = null;

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly static object _dataMonitor = new object();

        /// <summary>
        /// 所有数据字典集合,用于检索数据
        /// </summary>
        private readonly static Dictionary<string, ZMemoryCacheItem> _dicDatas = new Dictionary<string, ZMemoryCacheItem>();

        /// <summary>
        /// 线程等待通知信号对象
        /// </summary>
        private readonly static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 有效性验证时间间隔
        /// </summary>
        private static int interval = 100;

        /// <summary>
        /// 获取或设置有效性验证时间间隔
        /// </summary>
        public static int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        /// <summary>
        /// 值有效性验证线程方法
        /// </summary>
        /// <param name="token">取消线程对象</param>
        private static void ValueValidThreadMethod(CancellationToken token)
        {
            //当前所有项集合
            List<ZMemoryCacheItem> items = null;
            //本次遍历超时移除项集合
            List<ZMemoryCacheItem> timeOutItems = new List<ZMemoryCacheItem>();

            while (true)
            {
                try
                {
                    //获取所有项
                    lock (ZMemoryCache._dataMonitor)
                    {
                        items = ZMemoryCache._dicDatas.Values.ToList();
                    }

                    //如果项数为0,则等待新项添加后的通知
                    if (items.Count == 0)
                    {
                        ZMemoryCache._autoResetEvent.WaitOne();
                        return;
                    }

                    //遍历项查找是否超时
                    foreach (var item in items)
                    {
                        switch (item.TimeOutType)
                        {
                            case ZMemoryTimeOutType.ExpiresAt:
                                if (item.ExpiresAt <= DateTime.Now)
                                {
                                    timeOutItems.Add(item);
                                }

                                break;
                            case ZMemoryTimeOutType.ValidFor:
                                if (DateTime.Now - item.UpdateTime > item.ValidFor)
                                {
                                    timeOutItems.Add(item);
                                }

                                break;
                            case ZMemoryTimeOutType.Never:
                                break;
                            default:
                                throw new Exception(string.Format("未处理的存储模式:{0}", item.TimeOutType));
                        }
                    }

                    //如果待移除的集合项数为0,则进入下次循环
                    if (timeOutItems.Count == 0)
                    {
                        continue;
                    }

                    //移除超时集合中的项
                    lock (ZMemoryCache._dataMonitor)
                    {
                        foreach (var item in timeOutItems)
                        {
                            if (ZMemoryCache._dicDatas.ContainsKey(item.Key))
                            {
                                ZMemoryCache._dicDatas.Remove(item.Key);
                            }
                        }

                        timeOutItems.Clear();
                    }
                }
                catch (Exception)
                { }
                finally
                {
                    Thread.Sleep(ZMemoryCache.interval);
                }
            }
        }

        /// <summary>
        /// 存储值
        /// </summary>
        /// <param name="item">要存放的缓存对象</param>
        private static void Store(ZMemoryCacheItem item)
        {

            lock (ZMemoryCache._dataMonitor)
            {
                switch (item.Mode)
                {
                    case ZMemoryStoreMode.Add:
                    case ZMemoryStoreMode.Replace:
                    case ZMemoryStoreMode.Set:
                        if (ZMemoryCache._dicDatas.ContainsKey(item.Key))
                        {
                            item.UpdateTime = DateTime.Now;
                            ZMemoryCache._dicDatas[item.Key] = item;
                        }
                        else
                        {
                            item.UpdateTime = DateTime.Now;
                            ZMemoryCache._dicDatas.Add(item.Key, item);
                        }

                        break;
                    default:
                        throw new Exception(string.Format("未处理的存储模式:{0}", item.Mode));
                }

                ZMemoryCache._autoResetEvent.Set();
            }
        }

        /// <summary>
        /// 存储值,永久有效[存储成功返回True,否则返回false]
        /// </summary>
        /// <param name="mode">存储模式</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        public static void Store(ZMemoryStoreMode mode, string key, object value)
        {
            ZMemoryCacheItem item = new ZMemoryCacheItem();
            item.Key = key;
            item.Mode = mode;
            item.TimeOutType = ZMemoryTimeOutType.Never;
            item.Value = value;
            ZMemoryCache.Store(item);
        }

        /// <summary>
        /// 存储值[存储成功返回True,否则返回false]
        /// </summary>
        /// <param name="mode">存储模式</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="expiresAt">有效值到指定时间点</param>
        public static void Store(ZMemoryStoreMode mode, string key, object value, DateTime expiresAt)
        {
            ZMemoryCacheItem item = new ZMemoryCacheItem();
            item.Key = key;
            item.Mode = mode;
            item.TimeOutType = ZMemoryTimeOutType.ExpiresAt;
            item.ExpiresAt = expiresAt;
            item.Value = value;
            ZMemoryCache.Store(item);
        }

        /// <summary>
        /// 存储值[存储成功返回True,否则返回false]
        /// </summary>
        /// <param name="mode">存储模式</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="validFor">有效时长</param>
        public static void Store(ZMemoryStoreMode mode, string key, object value, TimeSpan validFor)
        {
            ZMemoryCacheItem item = new ZMemoryCacheItem();
            item.Key = key;
            item.Mode = mode;
            item.TimeOutType = ZMemoryTimeOutType.ValidFor;
            item.ValidFor = validFor;
            item.Value = value;
            ZMemoryCache.Store(item);
        }

        /// <summary>
        /// 尝试获取值[获取成功返回True,否则返回false]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>获取成功返回True,否则返回false</returns>
        public static bool TryGet(string key, out object value)
        {
            lock (ZMemoryCache._dataMonitor)
            {
                if (ZMemoryCache._dicDatas.ContainsKey(key))
                {
                    value = ZMemoryCache._dicDatas[key].Value;
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>值</returns>
        public static T Get<T>(string key)
        {
            object value = null;
            if (ZMemoryCache.TryGet(key, out value))
            {
                if (value is T)
                {
                    return (T)value;
                }
                else
                {
                    throw new Exception(string.Format("关键字key:{0}对应的记录类型不能转换为:{1}", key, typeof(T).FullName));
                }
            }
            else
            {
                throw new Exception(string.Format("不存在关键字key:{0}的记录", key));
            }
        }

        /// <summary>
        /// 删除数据[删除成功返回True,否则返回false]
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>删除成功返回True,否则返回false</returns>
        public static bool Delete(string key)
        {
            lock (ZMemoryCache._dataMonitor)
            {
                if (ZMemoryCache._dicDatas.ContainsKey(key))
                {
                    return ZMemoryCache._dicDatas.Remove(key);
                }
                else
                {
                    return false;
                }
            }
        }

        //public ulong Increment(string key, ulong defaultValue, ulong delta);
        //public ulong Increment(string key, ulong defaultValue, ulong delta, DateTime expiresAt);
        //public ulong Increment(string key, ulong defaultValue, ulong delta, TimeSpan validFor);

        //public ulong Decrement(string key, ulong defaultValue, ulong delta);
        //public ulong Decrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt);
        //public ulong Decrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor);
    }
}
