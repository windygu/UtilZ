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
        #region System.Windows.Forms.ComboBox
        #region 枚举
        /// <summary>
        /// 绑定枚举值到ComboBox
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <param name="ignoreList">忽略项列表</param>
        public static void BindingEnumToComboBox<T>(System.Windows.Forms.ComboBox combox, IEnumerable<T> ignoreList = null) where T : struct
        {
            Type enumType = typeof(T);
            EnumHelper.AssertEnum(enumType);
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (ignoreList == null)
            {
                ignoreList = new List<T>();
            }

            try
            {
                combox.Items.Clear();
                List<DropdownBindingItem> items = EnumHelper.GetNDisplayNameAttributeDisplayNameBindingItems(enumType);
                if (items.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    bool isIgnore = (from ignoreItem in ignoreList where item.Value.Equals(ignoreItem) select ignoreItem).Count() > 0;
                    if (isIgnore)
                    {
                        continue;
                    }

                    combox.Items.Add(item);
                }

                combox.DisplayMember = DropdownBindingItem.DisplayNameFieldName;
                if (combox.Items.Count > 0)
                {
                    combox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 绑定枚举值到ComboBox
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <param name="defaultSelectedValue">默认选中项值</param>
        /// <param name="ignoreList">忽略项列表</param>
        public static void BindingEnumToComboBox<T>(System.Windows.Forms.ComboBox combox, T defaultSelectedValue, IEnumerable<T> ignoreList = null) where T : struct
        {
            Type enumType = typeof(T);
            EnumHelper.AssertEnum(enumType);
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (ignoreList == null)
            {
                ignoreList = new List<T>();
            }

            try
            {
                combox.Items.Clear();

                List<DropdownBindingItem> items = EnumHelper.GetNDisplayNameAttributeDisplayNameBindingItems(enumType);
                if (items.Count == 0)
                {
                    return;
                }

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

                    combox.Items.Add(item);
                }

                combox.DisplayMember = DropdownBindingItem.DisplayNameFieldName;
                combox.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ComboBox枚举选中项
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <param name="enumValue">选中项值</param>
        public static void SetEnumToComboBox<T>(System.Windows.Forms.ComboBox combox, T enumValue) where T : struct
        {
            Type enumType = typeof(T);
            EnumHelper.AssertEnum(enumType);
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (combox.Items.Count == 0)
            {
                throw new Exception("ToolStripComboBox的中还没有绑定数据项");
            }

            try
            {
                for (int i = 0; i < combox.Items.Count; i++)
                {
                    if (enumValue.Equals(((DropdownBindingItem)combox.Items[i]).Value))
                    {
                        combox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("枚举类型:{0}与绑定到ComboBox的枚举类型不匹配", enumType.FullName));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ComboBox枚举选中项
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <returns>选中项枚举值</returns>
        public static T GetEnumFromComboBox<T>(System.Windows.Forms.ComboBox combox) where T : struct
        {
            EnumHelper.AssertEnum<T>();
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            try
            {
                DropdownBindingItem selectedItem = (DropdownBindingItem)(combox.Items[combox.SelectedIndex]);
                return (T)selectedItem.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("获取值失败", ex);
            }
        }
        #endregion

        #region 泛型集合
        /*****备份,有更简介的方案
        /// <summary>
        /// 绑定泛型集合到ComboBox
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <param name="items">要绑定的集合</param>
        /// <param name="displayMember">显示的成员,属性名或字段名,当为null时调用成员的ToString方法的值作为显示值[默认值为null]</param>
        /// <param name="selectedItem">默认选中项,不设置默认选中时该值为null[默认值为null]</param>        
        public static void BindingIEnumerableGenericToComboBox<T>(System.Windows.Forms.ComboBox combox, IEnumerable<T> items, string displayMember = null, T selectedItem = null) where T : class
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (items == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items), "集合不能为null");
            }

            try
            {
                combox.Items.Clear();
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

                    combox.Items.Add(item);
                }

                combox.DisplayMember = NDisplayNameBase.DisplayNameFieldName;
                combox.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ComboBox泛型选中项
        /// </summary>
        /// <param name="combox">ComboBox</param>
        /// <param name="selectedItem">选中项值</param>
        public static void SetGenericToComboBox(System.Windows.Forms.ComboBox combox, object selectedItem)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (selectedItem == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => selectedItem), "选中项不能为null");
            }

            if (combox.Items.Count == 0)
            {
                throw new Exception("ToolStripComboBox的中还没有绑定数据项");
            }

            try
            {
                object value = null;
                for (int i = 0; i < combox.Items.Count; i++)
                {
                    value = ((DropdownBindingItem)combox.Items[i]).Value;
                    if (value != null && value.Equals(selectedItem))
                    {
                        combox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("ComboBox集合项中不包含类型:{0}的项:{1}", selectedItem.GetType().Name, selectedItem.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ComboBox泛型选中项值
        /// </summary>
        /// <typeparam name="T">绑定时的集合类型</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <returns>选中项值</returns>
        public static T GetGenericFromComboBox<T>(System.Windows.Forms.ComboBox combox)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (combox.SelectedIndex == -1)
            {
                throw new Exception("ComboBox的选中项索引为-1,没有选中项");
            }

            try
            {
                return (T)((DropdownBindingItem)combox.Items[combox.SelectedIndex]).Value;
            }
            catch (Exception ex)
            {
                throw new Exception("获取值失败", ex);
            }
        }
        */

        /// <summary>
        /// 绑定泛型集合到ComboBox
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <param name="items">要绑定的集合</param>
        /// <param name="displayMember">显示的成员,属性名或字段名,当为null时调用成员的ToString方法的值作为显示值[默认值为null]</param>
        /// <param name="selectedItem">默认选中项,不设置默认选中时该值为null[默认值为null]</param>        
        public static void BindingIEnumerableGenericToComboBox<T>(System.Windows.Forms.ComboBox combox, IEnumerable<T> items, string displayMember = null, T selectedItem = null) where T : class
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (items == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items), "集合不能为null");
            }

            try
            {
                combox.DataSource = items;
                combox.DisplayMember = displayMember;
                if (selectedItem != null)
                {
                    for (int i = 0; i < items.Count(); i++)
                    {
                        if (selectedItem == items.ElementAt(i) || object.Equals(selectedItem, items.ElementAt(i)))
                        {
                            combox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ComboBox泛型选中项
        /// </summary>
        /// <param name="combox">ComboBox</param>
        /// <param name="selectedItem">选中项值</param>
        public static void SetGenericToComboBox(System.Windows.Forms.ComboBox combox, object selectedItem)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (selectedItem == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => selectedItem), "选中项不能为null");
            }

            if (combox.Items.Count == 0)
            {
                throw new Exception("ToolStripComboBox的中还没有绑定数据项");
            }

            try
            {
                for (int i = 0; i < combox.Items.Count; i++)
                {
                    if (selectedItem == combox.Items[i] || object.Equals(selectedItem, combox.Items[i]))
                    {
                        combox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("ComboBox集合项中不包含类型:{0}的项:{1}", selectedItem.GetType().Name, selectedItem.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ComboBox泛型选中项值
        /// </summary>
        /// <typeparam name="T">绑定时的集合类型</typeparam>
        /// <param name="combox">ComboBox</param>
        /// <returns>选中项值</returns>
        public static T GetGenericFromComboBox<T>(System.Windows.Forms.ComboBox combox)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (combox.SelectedIndex == -1)
            {
                throw new Exception("ComboBox的选中项索引为-1,没有选中项");
            }

            try
            {
                //return (T)((DropdownBindingItem)combox.SelectedItem).Value;
                return (T)combox.SelectedItem;
            }
            catch (Exception ex)
            {
                throw new Exception("获取值失败", ex);
            }
        }
        #endregion

        #region 字符串集合
        /// <summary>
        /// 绑定字符串集合到ComboBox
        /// </summary>
        /// <param name="combox">ComboBox</param>
        /// <param name="items">集合项</param>
        /// <param name="selectedItem">默认选中项,不设置默认选中时该值为null[默认值为null]</param>
        public static void BindingIEnumerableStringToComboBox(System.Windows.Forms.ComboBox combox, IEnumerable<string> items, string selectedItem = null)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (items == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => items), "集合不能为null");
            }

            try
            {
                combox.Items.Clear();
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
                        combox.Items.Clear();
                        throw new ArgumentException("字符串集合中不能有为空或null的项");
                    }

                    if (item == selectedItem || object.Equals(item, selectedItem))
                    {
                        selectedIndex = i;
                    }

                    combox.Items.Add(new DropdownBindingItem(item.ToString(), item, string.Empty, item));
                }

                combox.DisplayMember = DropdownBindingItem.DisplayNameFieldName;
                combox.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("绑定值失败", ex);
            }
        }

        /// <summary>
        /// 设置ComboBox字符串选中项
        /// </summary>
        /// <param name="combox">ComboBox</param>
        /// <param name="selectedItem">选中项值</param>
        /// <param name="ignoreCase">是否区分大小写[true:区分大小写,false:不区分,默认值为false]</param>
        public static void SetStringToComboBox(System.Windows.Forms.ComboBox combox, string selectedItem, bool ignoreCase = false)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (string.IsNullOrEmpty(selectedItem))
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => selectedItem), "选中项不能为空或null");
            }

            if (combox.Items.Count == 0)
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
                for (int i = 0; i < combox.Items.Count; i++)
                {
                    value = ((DropdownBindingItem)combox.Items[i]).Value;
                    if (value == null)
                    {
                        continue;
                    }

                    if (!ignoreCase && selectedItem.Equals(value.ToString().ToUpper()))
                    {
                        combox.SelectedIndex = i;
                        return;
                    }
                    else if (ignoreCase && selectedItem.Equals(value.ToString()))
                    {
                        combox.SelectedIndex = i;
                        return;
                    }
                }

                throw new Exception(string.Format("ComboBox集合项中不包含类型:{0}的项:{1}", selectedItem.GetType().Name, selectedItem.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("设定值失败", ex);
            }
        }

        /// <summary>
        /// 获取ComboBox字符串选中项值
        /// </summary>
        /// <param name="combox">ComboBox</param>
        /// <returns>选中项值</returns>
        public static string GetStringFromComboBox(System.Windows.Forms.ComboBox combox)
        {
            if (combox == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => combox), "目标控件不能为null");
            }

            if (combox.SelectedIndex == -1)
            {
                throw new Exception("ComboBox的选中项索引为-1,没有选中项");
            }

            try
            {
                object value = ((DropdownBindingItem)combox.Items[combox.SelectedIndex]).Value;
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
