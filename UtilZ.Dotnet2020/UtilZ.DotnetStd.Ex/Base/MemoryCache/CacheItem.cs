using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base.MemoryCache
{
    /// <summary>
    /// 张位存实体
    /// </summary>
    public class CacheItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">CacheItem 项的唯一标识符</param>
        /// <param name="value">CacheItem 项的数据</param>
        public CacheItem(object key, object value)
        {

        }


        /// <summary>
        /// 获取CacheItem 实例的唯一标识符
        /// </summary>
        public object Key { get; private set; }

        /// <summary>
        /// 获取或设置 System.Runtime.Caching.CacheItem 实例的数据
        /// </summary>
        public object Value { get; private set; }

        ///// <summary>
        ///// 存储模式
        ///// </summary>
        //public ZMemoryStoreMode Mode { get; set; }

        ///// <summary>
        ///// key
        ///// </summary>
        //public string Key { get; set; }

        ///// <summary>
        ///// 值
        ///// </summary>
        //public object Value { get; set; }

        ///// <summary>
        ///// 修改时间
        ///// </summary>
        //public DateTime UpdateTime { get; set; }

        ///// <summary>
        ///// 有效时间类型
        ///// </summary>
        //public ZMemoryTimeOutType TimeOutType { get; set; }

        ///// <summary>
        ///// 有效值到指定时间点
        ///// </summary>
        //public DateTime ExpiresAt { get; set; }

        ///// <summary>
        ///// 有效时长
        ///// </summary>
        //public TimeSpan ValidFor { get; set; }
    }


    /// <summary>
    /// 超时时间类型
    /// </summary>
    public enum ZMemoryTimeOutType
    {
        /// <summary>
        /// 有效值到达指定时刻
        /// </summary>
        [DisplayNameExAttribute("有效值到达指定时刻")]
        ExpiresAt = 1,

        /// <summary>
        /// 有效时长
        /// </summary>
        [DisplayNameExAttribute("有效时长")]
        ValidFor = 2,

        /// <summary>
        /// 从不
        /// </summary>
        [DisplayNameExAttribute("从不")]
        Never = 3
    }


    /// <summary>
    /// 存储模式
    /// </summary>
    public enum ZMemoryStoreMode
    {
        /// <summary>
        /// 添加
        /// </summary>
        [DisplayNameExAttribute("添加")]
        Add = 1,

        /// <summary>
        /// 替换
        /// </summary>
        [DisplayNameExAttribute("替换")]
        Replace = 2,

        /// <summary>
        /// 设置[不存在该项则添加,存在则替换旧的值]
        /// </summary>
        [DisplayNameExAttribute("设置")]
        Set = 3,
    }
}
