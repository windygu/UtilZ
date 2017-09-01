using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// 属性表格编辑下拉框转换器
    /// </summary>
    [Serializable]
    public class PropertyGridDropDownListConverter : TypeConverter
    {
        //ExpandableObjectConverter:属性信息可展开编辑
        //TypeConverter:下拉选择

        /// <summary>
        /// IPropertyGridDropDownList类型
        /// </summary>
        private Type _ipropertyGridDropDownListType = typeof(IPropertyGridDropDown);

        /// <summary>
        /// 使用指定的上下文返回此对象是否支持可以从列表中选取的标准值集
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <returns>如果应调用 GetStandardValues 来查找对象支持的一组公共值，则为 true；否则，为 false</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance.GetType().GetInterface(this._ipropertyGridDropDownListType.FullName) != null)
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
            if (context.Instance.GetType().GetInterface(this._ipropertyGridDropDownListType.FullName) != null && context.Instance != null)
            {
                System.Collections.ICollection collection = ((IPropertyGridDropDown)context.Instance).GetPropertyGridDropDownItems(context.PropertyDescriptor.Name);
                if (collection != null)
                {
                    return new StandardValuesCollection(collection);
                }
            }

            return base.GetStandardValues(context);
        }

        /// <summary>
        /// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型[是否从显示文本转换为真实对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="sourceType">表示要转换的类型</param>
        /// <returns>如果该转换器能够执行转换，则为 true；否则为 false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (context.Instance.GetType().GetInterface(this._ipropertyGridDropDownListType.FullName) != null)
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
            if (context.Instance.GetType().GetInterface(this._ipropertyGridDropDownListType.FullName) != null && value != null)
            {
                string valueStr = value.ToString();
                if (string.IsNullOrEmpty(valueStr))
                {
                    return base.ConvertFrom(context, culture, value);
                }
                else
                {
                    IPropertyGridDropDown ipropertyGridDropDownList = (IPropertyGridDropDown)context.Instance;
                    System.Collections.ICollection collection = ipropertyGridDropDownList.GetPropertyGridDropDownItems(context.PropertyDescriptor.Name);
                    if (collection.Count > 0)
                    {
                        Type instanceType = null;
                        foreach (var item in collection)
                        {
                            instanceType = item.GetType();
                            break;
                        }

                        string displayPropertyName = ipropertyGridDropDownList.GetPropertyGridDisplayName(context.PropertyDescriptor.Name);
                        if (string.IsNullOrEmpty(displayPropertyName) || TypeCode.String == Type.GetTypeCode(instanceType))
                        {
                            //如果显示属性名称为空或null则直接用原始数据作比较,比如:字符串集合
                            foreach (var item in collection)
                            {
                                if (item.Equals(value))
                                {
                                    return item;
                                }
                            }
                        }
                        else
                        {
                            System.Reflection.PropertyInfo propertyInfo = instanceType.GetProperty(displayPropertyName);
                            if (propertyInfo == null)
                            {
                                System.Reflection.FieldInfo fieldInfo = instanceType.GetField(displayPropertyName);
                                if (fieldInfo == null)
                                {
                                    //如果属性名或字段名不正确,则也调用父类方法
                                    return base.ConvertFrom(context, culture, value);
                                }
                                else
                                {
                                    //通过字段反射获取值
                                    foreach (var item in collection)
                                    {
                                        if (fieldInfo.GetValue(item).Equals(value))
                                        {
                                            return item;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //通过属性反射获取值
                                foreach (var item in collection)
                                {
                                    if (propertyInfo.GetValue(item, null).Equals(value))
                                    {
                                        return item;
                                    }
                                }
                            }
                        }
                    }

                    return base.ConvertFrom(context, culture, value);
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
            if (context.Instance.GetType().GetInterface(this._ipropertyGridDropDownListType.FullName) != null)
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
            if (context.Instance.GetType().GetInterface(this._ipropertyGridDropDownListType.FullName) != null && value != null)
            {
                IPropertyGridDropDown ipropertyGridDropDownList = (IPropertyGridDropDown)context.Instance;
                string propertyGridDropDownDisplayName = ipropertyGridDropDownList.GetPropertyGridDisplayName(context.PropertyDescriptor.Name);
                //如果显示属性名称为空或null则调用父类方法
                if (string.IsNullOrEmpty(propertyGridDropDownDisplayName))
                {
                    if (value == null)
                    {
                        return base.ConvertTo(context, culture, value, destinationType);
                    }
                    else
                    {
                        return value.ToString();
                    }
                }

                object retValue = null;
                Type valueType = value.GetType();
                System.Reflection.PropertyInfo propertyInfo = valueType.GetProperty(propertyGridDropDownDisplayName);
                if (propertyInfo == null)
                {
                    System.Reflection.FieldInfo fieldInfo = valueType.GetField(propertyGridDropDownDisplayName);
                    if (fieldInfo == null)
                    {
                        //如果属性名或字段名不正确,则也调用父类方法
                        return base.ConvertTo(context, culture, value, destinationType);
                    }
                    else
                    {
                        //通过字段反射获取值
                        retValue = fieldInfo.GetValue(value);
                    }
                }
                else
                {
                    //通过属性反射获取值
                    retValue = propertyInfo.GetValue(value, null);
                }

                return retValue;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
