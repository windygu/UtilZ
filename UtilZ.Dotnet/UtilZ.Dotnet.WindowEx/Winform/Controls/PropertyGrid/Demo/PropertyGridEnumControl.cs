using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Model;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls.PropertyGrid.Demo
{
    /// <summary>
    /// 属性表格枚举编辑控件
    /// </summary>
    public partial class PropertyGridEnumControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PropertyGridEnumControl(object value)
        {
            InitializeComponent();

            if (value == null)
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => value));
            }

            Type valueType = value.GetType();
            if (!valueType.IsEnum)
            {
                throw new ArgumentException(string.Format("类型:{0}不是枚举类型", valueType.FullName));
            }

            List<DropdownBindingItem> dbiItems = EnumEx.GetNDisplayNameAttributeDisplayNameBindingItems(valueType);
            DropdownBindingItem selectedItem = (from item in dbiItems where value.Equals(item.Value) select item).FirstOrDefault();
            DropdownBoxHelper.BindingIEnumerableGenericToComboBox<DropdownBindingItem>(comboBoxEnum, dbiItems, "Text", selectedItem);
        }

        /// <summary>
        /// 获取编辑的枚举值
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object EnumValue
        {
            get
            {
                return DropdownBoxHelper.GetGenericFromComboBox<DropdownBindingItem>(comboBoxEnum).Value;
            }
        }
    }
}
