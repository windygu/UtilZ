using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base.Config
{
    /// <summary>
    /// 配置Attribute基类
    /// </summary>
    public abstract class ConfigAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// 描述
        /// </summary>
        public string Des { get; private set; }

        /// <summary>
        /// 值为null时节点是否存在[true:存在;false:不存在]
        /// </summary>
        public bool AllowNullValueElement { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="des">描述</param>
        /// <param name="allowNullValueElement">值为null时节点是否存在[true:存在;false:不存在]</param>
        public ConfigAttribute(string name, string des, bool allowNullValueElement)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this._name = name;
            this.Des = des;
            this.AllowNullValueElement = allowNullValueElement;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigAttribute()
        {
            
        }


        internal string GetName(Type type)
        {
            if (string.IsNullOrWhiteSpace(this._name))
            {
                return type.Name;
            }

            return this._name;
        }

        internal string GetName(PropertyInfo propertyInfo)
        {
            if (string.IsNullOrWhiteSpace(this._name))
            {
                return propertyInfo.Name;
            }

            return this._name;
        }
    }
}
