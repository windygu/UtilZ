using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms.Design;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// 属性表格枚举编辑下拉框转换器
    /// </summary>
    [Serializable]
    public class PropertyGridEnumConverter : TypeConverter
    {
        /// <summary>
        /// 使用指定的上下文返回此对象是否支持可以从列表中选取的标准值集
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <returns>如果应调用 GetStandardValues 来查找对象支持的一组公共值，则为 true；否则，为 false</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.PropertyType.IsEnum)
            {
                return true;
            }
            else
            {
                return base.GetStandardValuesSupported(context);
            }
        }

        /// <summary>
        /// 获取下拉框的显示枚举集合
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <returns>包含标准有效值集的 TypeConverter.StandardValuesCollection；如果数据类型不支持标准值集，则为null</returns>
        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.PropertyType.IsEnum)
            {
                List<DropdownBindingItem> dbiItems = EnumHelper.GetNDisplayNameAttributeDisplayNameBindingItems(context.PropertyDescriptor.PropertyType);
                var enumItems = (from item in dbiItems select item.Value).ToArray();
                return new StandardValuesCollection(enumItems);
            }
            else
            {
                return base.GetStandardValues(context);
            }
        }

        /// <summary>
        /// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型[是否从显示文本转换为真实对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="sourceType">表示要转换的类型</param>
        /// <returns>如果该转换器能够执行转换，则为 true；否则为 false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (context.PropertyDescriptor.PropertyType.IsEnum)
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的对象转换为此转换器的类型[从显示文本转换为真实对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="culture">用作当前区域性的 CultureInfo</param>
        /// <param name="value">要转换的 Object</param>
        /// <returns>表示转换的 value 的 Object</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (context.PropertyDescriptor.PropertyType.IsEnum)
            {
                string valueStr = value.ToString();
                if (string.IsNullOrEmpty(valueStr))
                {
                    return base.ConvertFrom(context, culture, value);
                }
                else
                {
                    return EnumHelper.GetEnumByNDisplayNameAttributeDisplayName(context.PropertyDescriptor.PropertyType, valueStr);
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

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
