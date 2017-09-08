using UtilZ.Lib.Base;
using UtilZ.Lib.Base.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UtilZ.Lib.Winform.DropdownBox
{
    /// <summary>
    /// 下拉框控件数据绑定及获取辅助类
    /// </summary>
    public partial class DropdownBoxHelper
    {
        #region System.Windows.Forms.ToolStripComboBox
        #region 枚举
        /// <summary>
        /// 绑定枚举值到ToolStripComboBox
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <param name="defaultSelectedValue">默认选中项值</param>
        /// <param name="ignoreList">忽略项列表</param>
        public static void BindingEnumToToolStripComboBox<T>(System.Windows.Forms.ToolStripComboBox toolStripComboBox, T defaultSelectedValue, IEnumerable<T> ignoreList = null) where T : struct
        {
            Type enumType = typeof(T);
            EnumHelper.AssertEnum(enumType);
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (ignoreList == null)
            {
                ignoreList = new List<T>();
            }

            try
            {
                toolStripComboBox.Items.Clear();

                List<DropdownBindingItem> items = EnumHelper.GetNDisplayNameAttributeDisplayNameBindingItems(enumType);
                int selectedIndex = -1;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    bool isIgnore = (from ignoreItem in ignoreList where item.Value.Equals(ignoreItem) select ignoreItem).Count() > 0;
                    if (isIgnore)
                    {
                        continue;
                    }

                    if (item.Value.Equals(defaultSelectedValue))
                    {
                        selectedIndex = i;
                    }

                    toolStripComboBox.Items.Add(item);
                }

                toolStripComboBox.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ToolStripComboBox枚举选中项
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <param name="enumValue">选中项值</param>
        public static void SetEnumToToolStripComboBox<T>(System.Windows.Forms.ToolStripComboBox toolStripComboBox, T enumValue) where T : struct
        {
            Type enumType = typeof(T);
            EnumHelper.AssertEnum(enumType);
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (toolStripComboBox.Items.Count == 0)
            {
                throw new Exception("ToolStripComboBox的中还没有绑定数据项");
            }

            try
            {
                for (int i = 0; i < toolStripComboBox.Items.Count; i++)
                {
                    if (enumValue.Equals(((DropdownBindingItem)toolStripComboBox.Items[i]).Value))
                    {
                        toolStripComboBox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("枚举类型:{0}与绑定到ToolStripComboBox的枚举类型不匹配", enumType.FullName));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ToolStripComboBox枚举选中项
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <returns>选中项枚举值</returns>
        public static T GetEnumFromToolStripComboBox<T>(System.Windows.Forms.ToolStripComboBox toolStripComboBox) where T : struct
        {
            EnumHelper.AssertEnum<T>();
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            try
            {
                DropdownBindingItem selectedItem = (DropdownBindingItem)(toolStripComboBox.Items[toolStripComboBox.SelectedIndex]);
                return (T)selectedItem.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("获取值失败", ex);
            }
        }
        #endregion

        #region 泛型集合
        /// <summary>
        /// 绑定集合到ToolStripComboBox
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <param name="items">要绑定的集合</param>
        /// <param name="displayMember">显示的成员,属性名或字段名,当为null时调用成员的ToString方法的值作为显示值[默认值为null]</param>
        /// <param name="selectedItem">默认选中项,不设置默认选中时该值为null[默认值为null]</param>  
        public static void BindingIEnumerableGenericToToolStripComboBox<T>(System.Windows.Forms.ToolStripComboBox toolStripComboBox, IEnumerable<T> items, string displayMember = null, T selectedItem = null) where T : class
        {
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (items == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items), "集合不能为null");
            }

            try
            {
                toolStripComboBox.Items.Clear();
                if (items.Count() == 0)
                {
                    return;
                }

                List<DropdownBindingItem> dbiItems = DropdownBoxHelper.CreateBindingList<T>(items, displayMember);
                int selectedIndex = -1;
                DropdownBindingItem item = null;

                for (int i = 0; i < dbiItems.Count; i++)
                {
                    item = dbiItems[i];
                    if (item.Value == selectedItem)
                    {
                        selectedIndex = i;
                    }

                    toolStripComboBox.Items.Add(item);
                }

                toolStripComboBox.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ToolStripComboBox泛型选中项
        /// </summary>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <param name="selectedItem">选中项值</param>
        public static void SetGenericToToolStripComboBox<T>(System.Windows.Forms.ToolStripComboBox toolStripComboBox, T selectedItem)
        {
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (selectedItem == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => selectedItem), "选中项不能为null");
            }

            if (toolStripComboBox.Items.Count == 0)
            {
                throw new Exception("ToolStripComboBox的中还没有绑定数据项");
            }

            try
            {
                object value = null;
                for (int i = 0; i < toolStripComboBox.Items.Count; i++)
                {
                    value = ((DropdownBindingItem)toolStripComboBox.Items[i]).Value;
                    if (value != null && value.Equals(selectedItem))
                    {
                        toolStripComboBox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("ToolStripComboBox集合项中不包含类型:{0}的项:{1}", selectedItem.GetType().Name, selectedItem.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ToolStripComboBox泛型选中项值
        /// </summary>
        /// <typeparam name="T">绑定时的集合类型</typeparam>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <returns>选中项值</returns>
        public static T GetGenericFromToolStripComboBox<T>(System.Windows.Forms.ToolStripComboBox toolStripComboBox)
        {
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (toolStripComboBox.SelectedIndex == -1)
            {
                throw new Exception("ToolStripComboBox的选中项索引为-1,没有选中项");
            }

            try
            {
                return (T)((DropdownBindingItem)toolStripComboBox.Items[toolStripComboBox.SelectedIndex]).Value;
            }
            catch (Exception ex)
            {
                throw new Exception("获取值失败", ex);
            }
        }
        #endregion

        #region 字符串集合
        /// <summary>
        /// 绑定字符串集合到ToolStripComboBox
        /// </summary>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <param name="items">集合项</param>
        /// <param name="selectedItem">默认选中项,不设置默认选中时该值为null[默认值为null]</param>
        public static void BindingIEnumerableStringToolStripComboBox(System.Windows.Forms.ToolStripComboBox toolStripComboBox, IEnumerable<string> items, string selectedItem = null)
        {
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (items == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items), "集合不能为null");
            }

            try
            {
                toolStripComboBox.Items.Clear();
                if (items.Count() == 0)
                {
                    return;
                }

                int selectedIndex = -1;
                string item = null;

                for (int i = 0; i < items.Count(); i++)
                {
                    item = items.ElementAt(i);
                    if (string.IsNullOrEmpty(item))
                    {
                        toolStripComboBox.Items.Clear();
                        throw new ArgumentException("字符串集合中不能有为空或null的项");
                    }

                    if (item.Equals(selectedItem))
                    {
                        selectedIndex = i;
                    }

                    toolStripComboBox.Items.Add(new DropdownBindingItem(item.ToString(), item, string.Empty, item));
                }

                toolStripComboBox.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ToolStripComboBox字符串选中项
        /// </summary>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <param name="selectedItem">选中项值</param>
        /// <param name="ignoreCase">是否区分大小写[true:区分大小写,false:不区分,默认值为false]</param>
        public static void SetStringToToolStripComboBox(System.Windows.Forms.ToolStripComboBox toolStripComboBox, string selectedItem, bool ignoreCase = false)
        {
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (string.IsNullOrEmpty(selectedItem))
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => selectedItem), "选中项不能为空或null");
            }

            if (toolStripComboBox.Items.Count == 0)
            {
                throw new Exception("ToolStripComboBox的中还没有绑定数据项");
            }

            try
            {
                if (!ignoreCase)
                {
                    selectedItem = selectedItem.ToUpper();
                }

                object value = null;
                for (int i = 0; i < toolStripComboBox.Items.Count; i++)
                {
                    value = ((DropdownBindingItem)toolStripComboBox.Items[i]).Value;
                    if (value == null)
                    {
                        continue;
                    }

                    if (!ignoreCase && selectedItem.Equals(value.ToString().ToUpper()))
                    {
                        toolStripComboBox.SelectedIndex = i;
                        return;
                    }
                    else if (ignoreCase && selectedItem.Equals(value.ToString()))
                    {
                        toolStripComboBox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("ToolStripComboBox集合项中不包含类型:{0}的项:{1}", selectedItem.GetType().Name, selectedItem.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ToolStripComboBox字符串选中项值
        /// </summary>
        /// <param name="toolStripComboBox">ToolStripComboBox</param>
        /// <returns>选中项值</returns>
        public static string GetStringFromToolStripComboBox(System.Windows.Forms.ToolStripComboBox toolStripComboBox)
        {
            if (toolStripComboBox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => toolStripComboBox), "目标控件不能为null");
            }

            if (toolStripComboBox.SelectedIndex == -1)
            {
                throw new Exception("ToolStripComboBox的选中项索引为-1,没有选中项");
            }

            try
            {
                object value = ((DropdownBindingItem)toolStripComboBox.Items[toolStripComboBox.SelectedIndex]).Value;
                return value == null ? null : value.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("获取值失败", ex);
            }
        }
        #endregion
        #endregion
    }
}
