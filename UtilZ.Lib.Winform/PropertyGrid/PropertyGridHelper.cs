using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// 属性表格辅助类
    /// </summary>
    public class PropertyGridHelper
    {
        /// <summary>
        /// 更新通过属性表格修改值的对象中的值到目标对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="valueObj">值对象</param>
        /// <param name="targetObj">目标对象</param>
        public static void UpdateValue<T>(T valueObj, T targetObj) where T : class
        {
            UtilZ.Lib.Base.Extend.NAttributeHelper.UpdateValue<T, PropertyGridAttribute>(valueObj, targetObj);
        }
    }
}
