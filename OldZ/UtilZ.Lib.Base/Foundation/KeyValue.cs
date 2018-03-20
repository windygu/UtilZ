using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Foundation
{
    /// <summary>
    /// 键值类
    /// </summary>
    [Serializable]
    public class KeyValue<K, V>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KeyValue()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// 获取或设置键
        /// </summary>
        public K Key { get; set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public V Value { get; set; }
    }
}
