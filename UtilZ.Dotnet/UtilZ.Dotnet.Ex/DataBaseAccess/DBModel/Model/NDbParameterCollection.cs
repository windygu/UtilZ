using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model
{
    /// <summary>
    /// 命令的参数集合
    /// </summary>
    [Serializable]
    public class NDbParameterCollection : ICollection<NDbParameter>
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        private readonly Dictionary<string, NDbParameter> _dicNDbParameters;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NDbParameterCollection()
        {
            this._dicNDbParameters = new Dictionary<string, NDbParameter>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection">参数集合</param>
        public NDbParameterCollection(NDbParameterCollection collection)
            : this()
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }

            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        public void Add(string parameterName, object value)
        {
            this.Add(new NDbParameter(parameterName, value));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        /// <param name="direction">参数方向</param>
        public void Add(string parameterName, object value, ParameterDirection direction)
        {
            this.Add(new NDbParameter(parameterName, value, direction));
        }

        /// <summary>
        /// 将某项添加到集合中
        /// </summary>
        /// <param name="item">要添加到集合的对象</param>
        public virtual void Add(NDbParameter item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("要添加到集合的对象不能为null", "item");
            }

            if (this.Contains(item))
            {
                throw new ArgumentException(string.Format("已包含参数名为:{0}的参数项", item.ParameterName));
            }

            this._dicNDbParameters.Add(item.ParameterName, item);
        }

        /// <summary>
        /// 从集合中移除所有项
        /// </summary>
        public void Clear()
        {
            this._dicNDbParameters.Clear();
        }

        /// <summary>
        /// 确定集合是否包含特定值
        /// </summary>
        /// <param name="item">要在集合中定位的对象</param>
        /// <returns>如果在集合中找到item，则为 true；否则为 false</returns>
        public bool Contains(NDbParameter item)
        {
            return this._dicNDbParameters.Values.Contains(item);
        }

        /// <summary>
        /// 从特定的 Array 索引开始，将集合的元素复制到一个 Array 中
        /// </summary>
        /// <param name="array">作为从集合复制的元素的目标的一维 Array。 Array 必须具有从零开始的索引</param>
        /// <param name="arrayIndex">array 中从零开始的索引，从此索引处开始进行复制</param>
        public void CopyTo(NDbParameter[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            this._dicNDbParameters.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获取集合中包含的元素数
        /// </summary>
        public int Count
        {
            get { return this._dicNDbParameters.Count; }
        }

        /// <summary>
        /// 获取一个值，该值指示集合是否为只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>参数对象</returns>
        public NDbParameter this[string parameterName]
        {
            get { return this._dicNDbParameters[parameterName]; }
        }

        /// <summary>
        /// 获取指定索引处的元素
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>索引处的元素</returns>
        public NDbParameter this[int index]
        {
            get
            {
                if (this._dicNDbParameters.Count == 0 ||
                  index < 0 ||
                  index >= this._dicNDbParameters.Count)
                {
                    throw new ArgumentOutOfRangeException("索引超出范围。必须为非负值并小于集合大小", "index");
                }

                return this._dicNDbParameters.ElementAt(index).Value;
            }
        }

        /// <summary>
        /// 获取所有参数名称集合
        /// </summary>
        public List<string> ParameterNames
        {
            get { return (from key in this._dicNDbParameters.Keys select key.ToUpper()).ToList(); }
        }

        /// <summary>
        /// 从集合中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="item">要移除的项</param>
        /// <returns>移除结果[true:成功;false:失败]</returns>
        public bool Remove(NDbParameter item)
        {
            if (this.Contains(item))
            {
                return this._dicNDbParameters.Remove(item.ParameterName);
            }

            return false;
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        public IEnumerator<NDbParameter> GetEnumerator()
        {
            return this._dicNDbParameters.Values.GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数
        /// </summary>
        /// <returns>循环访问集合的枚举数</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in this._dicNDbParameters)
            {
                sb.AppendLine(string.Format(@"ParaName:{0};Value:{1}", kv.Key, kv.Value.Value));
            }

            return sb.ToString();
        }
    }
}
