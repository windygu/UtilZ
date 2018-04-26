using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Model.Attributes
{
    /// <summary>
    /// 显示名称特性
    /// </summary>
    public class DisplayNameExAttribute : Attribute
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DisplayNameExAttribute()
            : this(string.Empty)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        public DisplayNameExAttribute(string displayName)
            : this(displayName, 0)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="tag">标识</param>
        public DisplayNameExAttribute(string displayName, object tag)
            : this(displayName, null, tag)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="description">描述</param>
        public DisplayNameExAttribute(string displayName, string description)
            : this(displayName, 0, description)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="description">描述</param>
        /// <param name="tag">标识</param>
        public DisplayNameExAttribute(string displayName, string description, object tag)
            : this(displayName, 0, description, tag)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="index">项显示顺序</param>
        public DisplayNameExAttribute(string displayName, int index)
            : this(displayName, index, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="index">项显示顺序</param>
        /// <param name="description">描述</param>
        public DisplayNameExAttribute(string displayName, int index, string description)
            : this(displayName, index, description, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示文本</param>
        /// <param name="index">项显示顺序</param>
        /// <param name="description">描述</param>
        /// <param name="tag">标识</param>
        public DisplayNameExAttribute(string displayName, int index, string description, object tag)
        {
            this.DisplayName = displayName;
            this.Index = index;
            this.Description = description;
            this.Tag = tag;
        }

        /// <summary>
        /// 显示的文本
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 项显示顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置标识
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>返回特性文本</returns>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
