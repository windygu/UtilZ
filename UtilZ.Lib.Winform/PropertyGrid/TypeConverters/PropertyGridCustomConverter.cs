using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.Winform.PropertyGrid.TypeConverters
{
    /// <summary>
    /// 属性表格自定义枚举转换器
    /// </summary>
    [Serializable]
    public class PropertyGridCustomConverter : TypeConverter
    {
        /// <summary>
        /// 返回此转换器是否可以使用指定的上下文将该对象转换为指定的类型[是否能转换为显示对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="destinationType">表示要转换到的类型</param>
        /// <returns>如果该转换器能够执行转换，则为 true；否则为 false</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (context.PropertyDescriptor.PropertyType.IsEnum)
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的值对象转换为指定的类型[转换为显示对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="culture">如果传递 null，则采用当前区域性</param>
        /// <param name="value">要转换的 Object</param>
        /// <param name="destinationType">value 参数要转换成的 Type</param>
        /// <returns>表示转换的 value 的 Object</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (context.PropertyDescriptor.PropertyType.IsEnum)
            {
                return EnumHelper.GetEnumItemDisplayName(value);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
