using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base.Config
{
    /// <summary>
    /// 标记基元配置项
    /// </summary>
    public class ConfigItemAttribute : ConfigAttribute
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
        /// <param name="converterType">基元类型转换接口类型,该类型必需实现IConfigValueConverter接口</param>
        /// <param name="allowNullValueElement">值为null时节点是否存在[true:存在;false:不存在],默认值false</param>
        public ConfigItemAttribute(string name = null, string des = null, Type converterType = null, bool allowNullValueElement = false)
            : base(name, des, allowNullValueElement)
        {
            if (converterType != null)
            {
                this.Converter = ActivatorEx.CreateInstance<IConfigValueConverter>(converterType);
            }
        }
    }
}
