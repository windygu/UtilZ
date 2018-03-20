using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Foundation
{
    /// <summary>
    /// 元素集合基础配置类
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class BaseConfigurationElementCollection<T> : System.Configuration.ConfigurationElementCollection where T : BaseConfig, new()
    {
        /// <summary>
        /// 创建一个新的 ConfigurationElement
        /// </summary>
        /// <returns>一个新的 ConfigurationElement</returns>
        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// 获取指定配置元素的元素键
        /// </summary>
        /// <param name="element">要为其返回键的 ConfigurationElement</param>
        /// <returns>一个 Object，用作指定 ConfigurationElement 的键</returns>
        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            return ((T)element).Key;
        }

        #region 扩展方法属性
        /// <summary>
        /// 获取指定索引位置的值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>指定索引引位的值</returns>
        public T this[int index]
        {
            get
            {
                return (T)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }

                base.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 确定项的索引
        /// </summary>
        /// <param name="config">项</param>
        /// <returns>项的索引</returns>
        public int IndexOf(T config)
        {
            return base.BaseIndexOf(config);
        }

        /// <summary>
        /// 是否包含某个配置项
        /// </summary>
        /// <param name="item">要判断的项</param>
        /// <returns>包含返回true;不包含返回false</returns>
        public bool Contains(T item)
        {
            int index = base.BaseIndexOf(item);
            return index >= 0;
        }

        /// <summary>
        /// 添加配置项
        /// </summary>
        /// <param name="item">要添加的配置项</param>
        public void Add(BaseConfig item)
        {
            int index = base.BaseIndexOf(item);
            if (index >= 0)
            {
                base.BaseRemoveAt(index);
            }

            base.BaseAdd(item);
        }

        /// <summary>
        /// 移除配置项
        /// </summary>
        /// <param name="item">要移除的配置项</param>
        public void Remove(BaseConfig item)
        {
            int index = base.BaseIndexOf(item);
            if (index >= 0)
            {
                base.BaseRemoveAt(index);
            }
        }

        /// <summary>
        /// 清空配置集合
        /// </summary>
        public void Clear()
        {
            base.BaseClear();
        }
        #endregion

        ///// <summary>
        ///// 获取指定索引位置的值
        ///// </summary>
        ///// <param name="Name">索引</param>
        ///// <returns>指定索引引位的值</returns>
        //public FileLogConfigElement this[int index]
        //{
        //    get
        //    {
        //        return (FileLogConfigElement)base.BaseGet(index);
        //    }
        //    set
        //    {
        //        if (base.BaseGet(index) != null)
        //        {
        //            base.BaseRemoveAt(index);
        //        }

        //        base.BaseAdd(index, value);
        //    }
        //}

        ///// <summary>
        ///// 获取指定索引名称的值
        ///// </summary>
        ///// <param name="Name">索引名称</param>
        ///// <returns>指定索引名称的值</returns>
        //public new FileLogConfigElement this[string Name]
        //{
        //    get
        //    {
        //        return (FileLogConfigElement)base.BaseGet(Name);
        //    }
        //}

        /*
        public FileLogConfigCollection()
        {
            // Add one config to the collection.  This is
            // not necessary; could leave the collection 
            // empty until items are added to it outside
            // the constructor.
            FileLogConfigElement config = (FileLogConfigElement)this.CreateNewElement();
            this.Add(config);
        }




        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new FileLogConfigElement(elementName);
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FileLogConfigElement)element).Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileLogConfigElement();
        }

        public new string RemoveElementName
        {
            get { return base.RemoveElementName; }
        }

        public FileLogConfigElement this[int index]
        {
            get
            {
                return (FileLogConfigElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public new FileLogConfigElement this[string Name]
        {
            get
            {
                return (FileLogConfigElement)base.BaseGet(Name);
            }
        }

        public int IndexOf(FileLogConfigElement config)
        {
            return base.BaseIndexOf(config);
        }

        public void Add(FileLogConfigElement config)
        {
            base.BaseAdd(config);
            // Add custom code here.
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            base.BaseAdd(element, false);
            // Add custom code here.
        }

        public void Remove(FileLogConfigElement config)
        {
            if (base.BaseIndexOf(config) >= 0)
            {
                base.BaseRemove(config.Key);
            }
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void Clear()
        {
            base.BaseClear();
        }*/
    }
}
