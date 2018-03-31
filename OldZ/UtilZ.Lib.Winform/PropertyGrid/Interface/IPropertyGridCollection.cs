using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid.Interface
{
    /// <summary>
    /// 表格集合显示项
    /// </summary>
    public interface IPropertyGridCollection
    {
        /// <summary>
        /// 获取集合显示信息
        /// </summary>
        string GetCollectionDisplayInfo(string propertyName);
    }


    public interface IPropertyGridCollectionItem
    {
        string GetItemInfo();
    }
}
