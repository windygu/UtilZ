using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// 属性表格下拉框选择列表接口
    /// </summary>
    public interface IPropertyGridDropDown
    {
        /// <summary>
        /// 获取表格下拉框选择列表集合
        /// </summary>
        /// <param name="fileFieldName">设置属性名称</param>
        /// <returns>表格下拉框选择列表集合</returns>
        System.Collections.ICollection GetPropertyGridDropDownItems(string fileFieldName);

        /// <summary>
        /// 获取下拉列表项对象显示项属性名称
        /// </summary>
        /// <param name="fileFieldName">设置属性名称</param>
        /// <returns>下拉列表项对象显示项属性名称</returns>
        string GetPropertyGridDisplayName(string fileFieldName);
    }
}
