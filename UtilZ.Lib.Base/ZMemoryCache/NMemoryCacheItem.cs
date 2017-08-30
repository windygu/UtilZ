using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.ZMemoryCache
{
    /// <summary>
    /// 内存缓存项
    /// </summary>
    [Serializable]
    internal class NMemoryCacheItem
    {
        /// <summary>
        /// 存储模式
        /// </summary>
        public NMemoryStoreMode Mode;

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 有效时间类型
        /// </summary>
        public NMemoryTimeOutType TimeOutType { get; set; }

        /// <summary>
        /// 有效值到指定时间点
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 有效时长
        /// </summary>
        public TimeSpan ValidFor { get; set; }
    }
}
