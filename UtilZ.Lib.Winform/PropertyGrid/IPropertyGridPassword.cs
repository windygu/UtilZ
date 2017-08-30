using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// 表格密码设置接口
    /// </summary>
    public interface IPropertyGridPassword
    {
        /// <summary>
        /// 获取密码显示字符
        /// </summary>
        /// <param name="fileFieldName">设置属性名称</param>
        /// <returns>密码显示字符</returns>
        char GetPasswordChar(string fileFieldName);
    }
}
