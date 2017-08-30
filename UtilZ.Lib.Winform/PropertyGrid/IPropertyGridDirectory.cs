using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// 表格文件选择接口
    /// </summary>
    public interface IPropertyGridDirectory
    {
        /// <summary>
        /// 获取初始目录
        /// </summary>
        /// <param name="fileFieldName">设置属性名称</param>
        string GetInitialSelectedPath(string fileFieldName);
    }
}
