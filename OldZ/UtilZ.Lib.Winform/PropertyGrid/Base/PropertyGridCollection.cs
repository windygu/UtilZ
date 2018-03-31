using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid.Base
{
    /// <summary>
    /// PropertyGrid集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TypeConverter(typeof(TypeConverters.PropertyGridCollectionConverter))]
    public class PropertyGridCollection<T> : ICollection<T>, ICustomTypeDescriptor
    {
        /// <summary>
        /// 内部集合列表
        /// </summary>
        private readonly List<T> _items = new List<T>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public PropertyGridCollection()
        {

        }

        /// <summary>
        /// 集合项数
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        /// <summary>
        /// 集合是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="item">要添加的项</param>
        public void Add(T item)
        {
            this._items.Add(item);
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

        /// <summary>
        /// 是否包含指定项
        /// </summary>
        /// <param name="item">指定项</param>
        /// <returns>返回包含结果</returns>
        public bool Contains(T item)
        {
            return this._items.Contains(item);
        }

        /// <summary>
        /// 从目标数组的指定索引处开始，将整个 System.Collections.Generic.List`1 复制到兼容的一维数组
        /// </summary>
        /// <param name="array">一维 System.Array，它是从 System.Collections.Generic.List`1 复制的元素的目标。 System.Array必须具有从零开始的索引。</param>
        /// <param name="arrayIndex">中从零开始的索引，从此处开始复制</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        #region ICustomTypeDescriptor接口
        /// <summary>
        /// 返回此组件实例的自定义属性的集合
        /// </summary>
        /// <returns>包含此对象的属性的 System.ComponentModel.AttributeCollection。</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// 返回此组件实例的类名称
        /// </summary>
        /// <returns>对象的类名称，如果该类没有名称，则为 null</returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// 返回某个组件的此实例的名称
        /// </summary>
        /// <returns>对象的名称，如果该对象不具有名称，则为 null</returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// 返回此组件实例的类型转换器
        /// </summary>
        /// <returns>作为此对象转换器的 System.ComponentModel.TypeConverter，或为 null（如果此对象不存在任何 System.ComponentModel.TypeConverter）。</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// 返回某个组件的此实例的默认事件
        /// </summary>
        /// <returns>表示此对象的默认事件的 System.ComponentModel.EventDescriptor；如果该对象没有事件，则为 null。</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// 返回此组件实例的默认属性
        /// </summary>
        /// <returns>表示此对象的默认属性的 System.ComponentModel.PropertyDescriptor；如果该对象没有属性，则为 null。</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// 返回指定类型的此实例的一个组件的编辑器
        /// </summary>
        /// <param name="editorBaseType">一个 System.Type ，它表示此对象的编辑器</param>
        /// <returns>System.Object 是此对象的编辑器的指定类型或 null 如果找不到编辑器中。</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// 返回此组件实例的事件
        /// </summary>
        /// <returns>一个 System.ComponentModel.EventDescriptorCollection，表示此组件实例的事件。</returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// 使用指定的属性数组作为筛选器，返回此组件实例的事件。
        /// </summary>
        /// <param name="attributes">用作筛选器的 System.Attribute 类型数组</param>
        /// <returns>表示此组件实例的已筛选事件的 System.ComponentModel.EventDescriptorCollection。</returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        /// <summary>
        /// 返回此组件实例的属性
        /// </summary>
        /// <returns>表示此组件实例的属性的 System.ComponentModel.PropertyDescriptorCollection。</returns>
        public PropertyDescriptorCollection GetProperties()
        {
            var pds = new PropertyDescriptorCollection(null);
            for (int i = 0; i < this.Count; i++)
            {
                var pd = new PropertyGridCollectionPropertyDescriptor<T>(this, i);
                pds.Add(pd);
            }

            return pds;
        }

        /// <summary>
        /// 使用特性数组作为筛选器，返回此组件实例的属性
        /// </summary>
        /// <param name="attributes">用作筛选器的 System.Attribute 类型数组</param>
        /// <returns>一个 System.ComponentModel.PropertyDescriptorCollection，它表示此组件实例的已筛选属性</returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        /// <summary>
        /// 返回一个对象，该对象包含指定的属性描述符所描述的属性
        /// </summary>
        /// <param name="pd">一个 System.ComponentModel.PropertyDescriptor，表示要查找其所有者的属性</param>
        /// <returns>一个 System.Object，表示指定属性的所有者</returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public bool Remove(T item)
        {
            return this._items.Remove(item);
        }

        /// <summary>
        /// 返回循环访问 System.Collections.Generic.List`1 的枚举数
        /// </summary>
        /// <returns>用于 System.Collections.Generic.List`1.Enumerator 的 System.Collections.Generic.List`1</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// 返回循环访问 System.Collections.Generic.List`1 的枚举数
        /// </summary>
        /// <returns>用于 System.Collections.Generic.List`1.Enumerator 的 System.Collections.Generic.List`1</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }

    internal class PropertyGridCollectionPropertyDescriptor<T> : PropertyDescriptor
    {
        private PropertyGridCollection<T> _collection;
        private int _index;

        public PropertyGridCollectionPropertyDescriptor(PropertyGridCollection<T> coll, int index) :
            base(index.ToString(), null)
        {
            this._collection = coll;
            this._index = index;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection(null);
            }
        }


        public override Type ComponentType
        {
            get
            {
                return this._collection.GetType();
            }
        }

        public override string DisplayName
        {
            get
            {
                return this._collection.ElementAt(_index).ToString();
            }
        }

        public override string Description
        {
            get
            {
                //Employee emp = this.collection[index];
                //StringBuilder sb = new StringBuilder();
                //sb.Append(emp.FirstName);
                //sb.Append(",");
                //sb.Append(emp.Age);
                //sb.Append(" years old, working for ");
                //return sb.ToString();
                //return this._collection.ElementAt(_index).ToString();

                return "Employy.Description";
            }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(T); }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return this._collection.ElementAt(_index);
        }

        public override void ResetValue(object component)
        {

        }

        public override void SetValue(object component, object value)
        {

        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}
