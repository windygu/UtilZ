using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;

namespace UtilZ.Lib.Winform.PropertyGrid
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

    /// <summary>
    /// 属性表格枚举自定义编辑器
    /// </summary>
    public class PropertyGridCustomEnumEditor : UITypeEditor
    {
        /// <summary>
        /// 获取由 EditValue 方法使用的编辑器样式
        /// </summary>
        /// <param name="context">可用于获取附加上下文信息的 ITypeDescriptorContext</param>
        /// <returns>UITypeEditorEditStyle 值，指示 EditValue 方法使用的编辑器样式。 如果 UITypeEditor 不支持该方法，则 GetEditStyle 将返回 None</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //None 不提供任何交互用户界面 (UI) 组件。 
            //Modal 显示一个省略号 (...) 按钮，该按钮可启动模式对话框，对于这种对话框，用户必须输入数据才能继续程序；该按钮也可以启动非模式对话框，这种对话框停留在屏幕上，可供用户随时使用，但它允许用户执行其他活动。 
            //DropDown 显示一个下拉箭头按钮，并在下拉对话框中承载用户界面 (UI)。 
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            return base.GetEditStyle(context);
        }

        /// <summary>
        /// 使用 GetEditStyle 方法所指示的编辑器样式编辑指定对象的值
        /// </summary>
        /// <param name="context">可用于获取附加上下文信息的 ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider，此编辑器可用其来获取服务</param>
        /// <param name="value">要编辑的对象</param>
        /// <returns>新的对象值。 如果对象的值尚未更改，则它返回的对象应与传递给它的对象相同</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;

            if (context != null && context.Instance != null && provider != null)
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    PropertyGridEnumControl propertyGridEnumControl = new PropertyGridEnumControl(value);
                    editorService.DropDownControl(propertyGridEnumControl);
                    return propertyGridEnumControl.EnumValue;
                }
            }

            return value;
        }
    }
}
