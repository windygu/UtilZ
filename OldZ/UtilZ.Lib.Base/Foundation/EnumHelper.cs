using UtilZ.Lib.Base.Ex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Foundation
{
    /// <summary>
    /// 枚举辅助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 断言类型T为枚举类型
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        public static void AssertEnum<T>()
        {
            EnumHelper.AssertEnum(typeof(T));
        }

        /// <summary>
        /// 断言类型T为枚举类型
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public static void AssertEnum(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(string.Format("类型:{0}不是枚举类型", enumType.FullName));
            }
        }

        #region 获取枚举项上的特性显示文本
        /// <summary>
        /// 获取枚举项上的特性显示文本
        /// </summary>
        /// <param name="enumItem">枚举值</param>
        /// <returns>特性显示文本</returns>
        public static string GetEnumItemDisplayName(object enumItem)
        {
            if (enumItem == null)
            {
                return string.Empty;
            }

            return EnumHelper.GetEnumItemText(enumItem.GetType(), enumItem);
        }

        /// <summary>
        /// 获取枚举项上的特性显示文本
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumItem">枚举值</param>
        /// <returns>特性显示文本</returns>
        public static string GetEnumItemDisplayName<T>(T enumItem) where T : struct, IComparable, IFormattable, IConvertible
        {
            Type enumType = typeof(T);
            return EnumHelper.GetEnumItemText(enumType, enumItem);
        }

        /// <summary>
        /// 获取枚举项上的显示文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumItem">枚举项</param>
        /// <returns>显示文本</returns>
        private static string GetEnumItemText(Type enumType, object enumItem)
        {
            EnumHelper.AssertEnum(enumType);
            var fields = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Type enumAttriType = typeof(DisplayNameExAttribute);
            object[] csAttris = null;
            object enumValue = null;

            foreach (var field in fields)
            {
                enumValue = field.GetValue(null);
                if (enumItem.Equals(enumValue))
                {
                    csAttris = field.GetCustomAttributes(enumAttriType, false);
                    if (csAttris.Length == 0)
                    {
                        return enumValue.ToString();
                    }
                    else
                    {
                        return ((DisplayNameExAttribute)csAttris[0]).DisplayName;
                    }
                }
            }

            return string.Empty;
        }
        #endregion

        /// <summary>
        /// 根据枚举NEnumAttribute特性文本获取对应的枚举项
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="displayName">显示文本</param>
        /// <returns>枚举项</returns>
        public static object GetEnumByNDisplayNameAttributeDisplayName(Type enumType, string displayName)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => enumType));
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => displayName), "NEnumAttribute显示文本值不能为空或null");
            }

            EnumHelper.AssertEnum(enumType);
            var fields = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Type enumAttriType = typeof(DisplayNameExAttribute);
            object[] csAttris = null;
            object enumValue = null;
            List<System.Reflection.FieldInfo> noNEnumAttributeFields = new List<System.Reflection.FieldInfo>();

            //有特性返回特性文本对应的值
            foreach (var field in fields)
            {
                enumValue = field.GetValue(null);
                csAttris = field.GetCustomAttributes(enumAttriType, false);
                if (csAttris.Length == 0)
                {
                    noNEnumAttributeFields.Add(field);
                    continue;
                }

                if (displayName.Equals(((DisplayNameExAttribute)csAttris[0]).DisplayName))
                {
                    return enumValue;
                }
            }

            //无特性标注的返回枚举字符串对应的值
            foreach (var field in noNEnumAttributeFields)
            {
                enumValue = field.GetValue(null);
                if (displayName.Equals(enumValue.ToString()))
                {
                    return enumValue;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取枚举特性转换成的DropdownBindingItem列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>绑定列表集合</returns>
        public static List<DropdownBindingItem> GetNDisplayNameAttributeDisplayNameBindingItems(Type enumType)
        {
            EnumHelper.AssertEnum(enumType);

            var fields = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Type enumAttriType = typeof(DisplayNameExAttribute);
            object[] csAttris = null;

            List<Tuple<DisplayNameExAttribute, object>> enumAttriItems = new List<Tuple<DisplayNameExAttribute, object>>();
            object value = null;

            foreach (var field in fields)
            {
                DisplayNameExAttribute item = null;
                csAttris = field.GetCustomAttributes(enumAttriType, false);
                value = field.GetValue(null);

                if (csAttris.Length == 0)
                {
                    item = new DisplayNameExAttribute();
                    item.DisplayName = value.ToString();
                    item.Description = item.DisplayName;
                }
                else
                {
                    item = (DisplayNameExAttribute)csAttris[0];
                }

                enumAttriItems.Add(Tuple.Create<DisplayNameExAttribute, object>(item, value));
            }

            enumAttriItems = enumAttriItems.OrderBy(new Func<Tuple<DisplayNameExAttribute, object>, int>((item) => { return item.Item1.Index; })).ToList();

            List<DropdownBindingItem> dbiItems = new List<DropdownBindingItem>();
            foreach (var enumAttriItem in enumAttriItems)
            {
                dbiItems.Add(new DropdownBindingItem(enumAttriItem.Item1.DisplayName, enumAttriItem.Item2, enumAttriItem.Item1.Description, enumAttriItem.Item1));
            }

            return dbiItems;
        }

        /// <summary>
        /// 获取枚举特性转换成的字典集合[key:枚举值;value:枚举项上标记的特性(多项取第一项)]
        /// </summary>
        /// <typeparam name="ET">枚举类型</typeparam>
        /// <typeparam name="AT">枚举上对应的特性类型</typeparam>
        /// <returns>枚举特性转换成的字典集合</returns>
        public static Dictionary<ET, AT> GetEnumItemAttribute<ET, AT>() where AT : Attribute
        {
            Type enumType = typeof(ET);
            AssertEnum(enumType);

            var fields = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Type attriType = typeof(AT);
            object[] csAttris = null;
            ET value;
            AT attri;
            var itemAttriDic = new Dictionary<ET, AT>();
            foreach (var field in fields)
            {
                csAttris = field.GetCustomAttributes(attriType, false);
                value = (ET)field.GetValue(null);
                if (csAttris.Length == 0)
                {
                    attri = null;
                }
                else
                {
                    attri = (AT)csAttris[0];
                }

                itemAttriDic.Add(value, attri);
            }

            return itemAttriDic;
        }

        #region 获取枚举项上的标识
        /// <summary>
        /// 获取枚举项上的标识
        /// </summary>
        /// <param name="enumItem">枚举值</param>
        /// <returns>枚举项上的标识</returns>
        public static object GetEnumItemTag(object enumItem)
        {
            Type enumType = enumItem.GetType();
            return EnumHelper.GetEnumItemTag(enumType, enumItem);
        }

        /// <summary>
        /// 获取枚举项上的标识
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumItem">枚举值</param>
        /// <returns>枚举项上的标识</returns>
        public static object GetEnumItemTag<T>(T enumItem) where T : struct, IComparable, IFormattable, IConvertible
        {
            Type enumType = typeof(T);
            return EnumHelper.GetEnumItemTag(enumType, enumItem);
        }

        /// <summary>
        /// 获取枚举项上的标识
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumItem">枚举值</param>
        /// <returns>枚举项上的标识</returns>
        private static object GetEnumItemTag(Type enumType, object enumItem)
        {
            EnumHelper.AssertEnum(enumType);
            var fields = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Type enumAttriType = typeof(DisplayNameExAttribute);
            object[] csAttris = null;
            object enumValue = null;

            foreach (var field in fields)
            {
                enumValue = field.GetValue(null);
                if (enumItem.Equals(enumValue))
                {
                    csAttris = field.GetCustomAttributes(enumAttriType, false);
                    if (csAttris.Length == 0)
                    {
                        return enumValue.ToString();
                    }
                    else
                    {
                        return ((DisplayNameExAttribute)csAttris[0]).Tag;
                    }
                }
            }

            return string.Empty;
        }
        #endregion
    }
}
