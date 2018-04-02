using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace UtilZ.Dotnet.WindowEx.WPF
{
    /// <summary>
    /// WPF UI扩展类
    /// </summary>
    public static class ElementExtend
    {
        /// <summary>
        /// 判断UI是否处于设计模式[处理设计模式返回true;否则返回false]
        /// </summary>
        /// <param name="ele">要判断的UI元素</param>
        /// <returns>处理设计模式返回true;否则返回false</returns>
        public static bool IsInDesignMode(this UIElement ele)
        {
            if (ele == null)
            {
                return false;
            }

            //非UI对象，要判断是否处于设计器模式
            //bool isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());

            //UI对象，要判断是否处于设计器模式
            //bool isInDesignMode = DesignerProperties.GetIsInDesignMode(ele);

            //这两种方式有时会失效（具体什么情况下会失效不明）
            return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
        }

    }
}
