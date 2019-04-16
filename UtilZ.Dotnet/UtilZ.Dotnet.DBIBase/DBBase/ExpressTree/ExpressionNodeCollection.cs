using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBIBase.DBBase.ExpressTree
{
    [Serializable]
    public class ExpressionNodeCollection<T> : ICollection<T>
    {
        private readonly List<T> _nodeList = new List<T>();
        public ExpressionNodeCollection()
        {

        }

        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public LogicOperaters LogicOperaters { get; set; } = LogicOperaters.And;

        /// <summary>
        /// 获取集合中包含的元素数
        /// </summary>
        public int Count
        {
            get
            {
                return this._nodeList.Count;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示集合是否为只读。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 将某项添加到集合中
        /// </summary>
        /// <param name="item"> 要添加到 System.Collections.Generic.ICollection`1 的对象</param>
        public void Add(T item)
        {
            this._nodeList.Add(item);
        }

        /// <summary>
        /// 从 System.Collections.Generic.ICollection`1 中移除所有项
        /// </summary>
        public void Clear()
        {
            this._nodeList.Clear();
        }

        /// <summary>
        /// 确定 System.Collections.Generic.ICollection`1 是否包含特定值
        /// </summary>
        /// <param name="item">要在 System.Collections.Generic.ICollection`1 中定位的对象</param>
        /// <returns>如果在 System.Collections.Generic.ICollection`1 中找到 item，则为 true；否则为 false</returns>
        public bool Contains(T item)
        {
            return this._nodeList.Contains(item);
        }

        /// <summary>
        /// 从特定的 System.Array 索引开始，将 System.Collections.Generic.ICollection`1 的元素复制到一个 System.Array中
        /// </summary>
        /// <param name="array">作为从 System.Collections.Generic.ICollection`1 复制的元素的目标的一维 System.Array。System.Array必须具有从零开始的索引</param>
        /// <param name="arrayIndex">array 中从零开始的索引，从此索引处开始进行复制</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._nodeList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 从 System.Collections.Generic.ICollection`1 中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="item">要从 System.Collections.Generic.ICollection`1 中移除的对象</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return this._nodeList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._nodeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
