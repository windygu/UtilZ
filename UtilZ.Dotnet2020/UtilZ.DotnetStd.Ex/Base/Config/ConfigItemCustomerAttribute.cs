using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base
{
    /// <summary>
    /// 标记配置项自定义读写
    /// </summary>
    public class ConfigItemCustomerAttribute : ConfigAttribute
    {
        /// <summary>
        /// 自定义配置项读写
        /// </summary>
        public ICustomerConfig CustomerConfig { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="des">描述</param>
        /// <param name="customerConfigType">自定义配置项读写类型</param>
        /// <param name="allowNullValueElement">值为null时节点是否存在[true:存在;false:不存在],默认值false</param>
        public ConfigItemCustomerAttribute(string name, string des, Type customerConfigType, bool allowNullValueElement = false)
            : base(name, des, allowNullValueElement)
        {
            this.CustomerConfig = ActivatorEx.CreateInstance<ICustomerConfig>(customerConfigType);
        }
    }
}
