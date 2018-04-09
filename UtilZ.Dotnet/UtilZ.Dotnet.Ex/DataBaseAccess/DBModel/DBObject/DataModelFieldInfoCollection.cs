using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBObject
{
    /// <summary>
    /// 数据模型字段信息集合
    /// </summary>
    [Serializable]
    public class DataModelFieldInfoCollection : ICollection<DataModelFieldInfo>
    {
        /// <summary>
        /// 集合
        /// </summary>
        private readonly List<DataModelFieldInfo> _items = new List<DataModelFieldInfo>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DataModelFieldInfoCollection()
        {

        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="item">数据模型字段信息</param>
        public DataModelFieldInfoCollection(DataModelFieldInfo item)
        {
            this.Add(item);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据模型字段信息集合</param>
        public DataModelFieldInfoCollection(IEnumerable<DataModelFieldInfo> items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 将对象添加到集合中
        /// </summary>
        /// <param name="item">要添加到集合的对象</param>
        public void Add(DataModelFieldInfo item)
        {
            if (this.Contains(item))
            {
                throw new ArgumentException("项已存在,不能添加重复项", "item");
            }

            this._items.Add(item);
        }

        /// <summary>
        /// 从集合中移除所有项
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

        /// <summary>
        /// 确定集合是否包含特定值
        /// </summary>
        /// <param name="item">要在集合中定位的对象</param>
        /// <returns>如果在集合中找到item，则为 true；否则为 false</returns>
        public bool Contains(DataModelFieldInfo item)
        {
            return this._items.Contains(item);
        }

        /// <summary>
        /// 从特定的Array索引开始，将集合的元素复制到一个 Array 中
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="arrayIndex">作为从集合复制的元素的目标的一维 Array。 Array 必须具有从零开始的索引</param>
        public void CopyTo(DataModelFieldInfo[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获取集合中包含的元素数
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        /// <summary>
        /// 获取一个值，该值指示集合是否为只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 从集合中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(DataModelFieldInfo item)
        {
            return this._items.Remove(item);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>一个循环访问集合的枚举数</returns>
        public IEnumerator<DataModelFieldInfo> GetEnumerator()
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
