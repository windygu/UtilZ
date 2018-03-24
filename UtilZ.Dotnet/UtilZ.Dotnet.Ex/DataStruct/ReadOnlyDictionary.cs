using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataStruct
{
    /// <summary>
    /// 只读字典集合
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    [Serializable]
    public class ReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 内部字典集合
        /// </summary>
        private readonly ConcurrentDictionary<TKey, TValue> _dic;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dic">初始化字典集合</param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dic)
        {
            _dic = new ConcurrentDictionary<TKey, TValue>(dic);
        }

        /// <summary>
        /// 集合中是否包含指定key[包含返回true;不包含返回false]
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>包含返回true;不包含返回false</returns>
        public bool ContainsKey(TKey key)
        {
            return this._dic.ContainsKey(key);
        }

        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>返回值</returns>
        public TValue this[TKey key]
        {
            get
            {
                return this._dic[key];
            }
        }

        /// <summary>
        /// 获取键集合
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return this._dic.Keys;
            }
        }

        /// <summary>
        /// 获取值集合
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return this._dic.Values;
            }
        }

        /// <summary>
        /// 集合项数
        /// </summary>
        public int Count
        {
            get { return this._dic.Count; }
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>一个循环访问集合的枚举数</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._dic.GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>一个循环访问集合的枚举数</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
