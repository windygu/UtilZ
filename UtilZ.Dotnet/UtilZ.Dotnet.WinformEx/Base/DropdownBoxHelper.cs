using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using UtilZ.Lib.Base;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.WinformEx.Base
{
    /// <summary>
    /// 下拉框控件数据绑定及获取辅助类[分类三类:枚举，泛型集合，字符串集合]
    /// </summary>
    public partial class DropdownBoxHelper
    {
        /// <summary>
        /// 创建绑定集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="items">泛型集合项</param>
        /// <param name="displayMember">显示的成员,属性名或字段名,当为null时调用成员的ToString方法的值作为显示值[默认值为null]</param>
        /// <returns>绑定集合</returns>
        protected static List<DropdownBindingItem> CreateBindingList<T>(IEnumerable<T> items, string displayMember = null) where T : class
        {
            if (items == null)
            {
                throw new Exception("集合不能为null");
            }

            List<DropdownBindingItem> dbiItems = new List<DropdownBindingItem>();
            if (string.IsNullOrEmpty(displayMember))
            {
                //如果显示的成员名称为空或null,则直接对象的ToString方法为显示文本
                foreach (var item in items)
                {
                    if (item == null)
                    {
                        throw new ArgumentException("泛型集合中不能有为null的项");
                    }

                    dbiItems.Add(new DropdownBindingItem(item.ToString(), item, string.Empty, item));
                }
            }
            else
            {
                Type tType = typeof(T);

                object displayObj = null;//显示成员的值
                string displayText = string.Empty;//显示成员文本
                //显示的字段属性
                PropertyInfo displayProperty = tType.GetProperty(displayMember);
                if (displayProperty == null)
                {
                    FieldInfo displayFieldInfo = tType.GetField(displayMember);
                    if (displayFieldInfo == null)
                    {
                        //如果显示的成员名称为空或null,则直接对象的ToString方法为显示文本
                        throw new ArgumentException(string.Format("类型:{0}中不存在名称为:{1}的属性或字段", tType.FullName, displayMember));
                    }
                    else
                    {
                        //根据字段值创建绑定项
                        foreach (var item in items)
                        {
                            if (item == null)
                            {
                                throw new ArgumentException("泛型集合中不能有为null的项");
                            }

                            displayObj = displayFieldInfo.GetValue(item);
                            if (displayObj == null)
                            {
                                displayText = string.Empty;
                            }
                            else
                            {
                                displayText = displayObj.ToString();
                            }

                            dbiItems.Add(new DropdownBindingItem(displayText, item, string.Empty, item));
                        }
                    }
                }
                else
                {
                    //根据属性值创建绑定项
                    foreach (var item in items)
                    {
                        if (item == null)
                        {
                            throw new ArgumentException("泛型集合中不能有为null的项");
                        }

                        displayObj = displayProperty.GetValue(item, null);
                        if (displayObj == null)
                        {
                            displayText = string.Empty;
                        }
                        else
                        {
                            displayText = displayObj.ToString();
                        }

                        dbiItems.Add(new DropdownBindingItem(displayText, item, string.Empty, item));
                    }
                }
            }

            return dbiItems;
        }
    }
}
