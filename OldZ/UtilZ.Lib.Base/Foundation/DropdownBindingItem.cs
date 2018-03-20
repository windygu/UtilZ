using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Foundation
{
    /// <summary>
    /// UI显示基类
    /// </summary>
    [Serializable]
    public class DropdownBindingItem
    {
        /// <summary>
        /// 获取显示字段名称
        /// </summary>
        public const string DisplayNameFieldName = "DisplayName";

        /// <summary>
        /// 获取或设置显示名称
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 数据标识
        /// </summary>
        public object Tag { get; private set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DropdownBindingItem()
            : this(string.Empty, null, string.Empty, null)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="value">值</param>
        public DropdownBindingItem(string displayName, object value)
            : this(displayName, value, string.Empty, null)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="value">值</param>
        /// <param name="description">项描述</param>
        public DropdownBindingItem(string displayName, object value, string description)
            : this(displayName, value, description, null)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="value">值</param>
        /// <param name="description">项描述</param>
        /// <param name="tag">数据标识</param>
        public DropdownBindingItem(string displayName, object value, string description, object tag)
        {
            this.DisplayName = displayName;
            this.Value = value;
            this.Description = description;
            this.Tag = tag;
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>下拉框显示文本</returns>
        public override string ToString()
        {
            return this.DisplayName;
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
