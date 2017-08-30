using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.Extend;
using UtilZ.Lib.Winform.DropdownBox;

namespace UtilZ.Lib.Winform.PropertyGrid
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
                throw new ArgumentNullException(NExtendObject.GetVarName(p => value));
            }

            Type valueType = value.GetType();
            if (!valueType.IsEnum)
            {
                throw new ArgumentException(string.Format("类型:{0}不是枚举类型", valueType.FullName));
            }

            List<DropdownBindingItem> dbiItems = EnumHelper.GetNDisplayNameAttributeDisplayNameBindingItems(valueType);
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
