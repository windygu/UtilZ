using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base.Config
{
    /// <summary>
    /// 标记配置项集合
    /// </summary>
    public class ConfigItemCollectionAttribute : ConfigAttribute
    {
        /// <summary>
        /// 子项名称
        /// </summary>
        public string ChildName { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="des">描述</param>
        /// <param name="childName">子项名称</param>
        /// <param name="allowNullValueElement">值为null时节点是否存在[true:存在;false:不存在],默认值false</param>
        public ConfigItemCollectionAttribute(string name = null, string des = null, string childName = null, bool allowNullValueElement = false)
            : base(name, des, allowNullValueElement)
        {
            if (string.IsNullOrWhiteSpace(childName))
            {
                throw new ArgumentNullException(nameof(childName));
            }

            this.ChildName = childName;
        }
    }


    /// <summary>
    /// 标记配置项集合
    /// </summary>
    public class ConfigItemPrimitiveCollectionAttribute : ConfigItemCollectionAttribute
    {
        /// <summary>
        /// 配置值转换对象
        /// </summary>
        public IConfigValueConverter Converter { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="des">描述</param>
        /// <param name="childName">子项名称</param>
        /// <param name="converterType">基元类型转换接口类型,该类型必需实现IConfigValueConverter接口</param>
        /// <param name="allowNullValueElement">值为null时节点是否存在[true:存在;false:不存在],默认值false</param>
        public ConfigItemPrimitiveCollectionAttribute(string name = null, string des = null, string childName = null, Type converterType = null, bool allowNullValueElement = false)
            : base(name, des, childName, allowNullValueElement)
        {
            if (converterType != null)
            {
                this.Converter = ActivatorEx.CreateInstance<IConfigValueConverter>(converterType);
            }
        }
    }
}
