using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.OriginalDataShow
{
    /// <summary>
    /// 原始数据显示类型集合
    /// </summary>
    public class OriginalDataShowTypeCollection : IEnumerable<OriginalDataShowType>
    {
        /// <summary>
        /// 数据项集合
        /// </summary>
        private readonly List<OriginalDataShowType> _items = new List<OriginalDataShowType>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据项集合</param>
        public OriginalDataShowTypeCollection(IEnumerable<OriginalDataShowType> items)
        {
            this._items.AddRange(items);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>一个循环访问集合的枚举数</returns>
        public IEnumerator<OriginalDataShowType> GetEnumerator()
        {
            return this._items.GetEnumerator();
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
