using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace UtilZ.Dotnet.IWindowEx.Winform.PageGrid
{
    /// <summary>
    /// WEiFenLuoDockPanel辅助类
    /// </summary>
    public class WFLHelper
    {
        /// <summary>
        /// 添加停靠内容
        /// </summary>
        /// <typeparam name="T">停靠类型</typeparam>
        /// <param name="dockPanel">停靠容器</param>
        /// <param name="text">显示标题</param>
        /// <param name="style">停靠样式</param>
        /// <param name="visibleState">停靠窗口初始状态</param>
        /// <returns>新添加的停靠窗口</returns>
        public static T AddDock<T>(DockPanel dockPanel, string text, DockStyle style, DockState visibleState) where T : DockContent, new()
        {
            T item = new T();
            WFLHelper.AddDock<T>(item, dockPanel, text, style, visibleState);
            return item;
        }

        /// <summary>
        /// 添加停靠内容
        /// </summary>
        /// <typeparam name="T">停靠类型</typeparam>
        /// <param name="item">目标停靠窗口</param>
        /// <param name="dockPanel">停靠容器</param>
        /// <param name="text">显示标题</param>
        /// <param name="style">停靠样式</param>
        /// <param name="visibleState">停靠窗口初始状态</param>
        public static void AddDock<T>(T item, DockPanel dockPanel, string text, DockStyle style, DockState visibleState) where T : DockContent
        {
            if (item == null)
            {
                throw new ArgumentNullException("停靠项不能为null", "item");
            }

            if (dockPanel == null)
            {
                throw new ArgumentNullException("停靠容器控件不能为null", "dockPanel");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("标题文本不能为空", "text");
            }

            item.Text = text;
            item.Show(dockPanel);
            item.CloseButtonVisible = false;
            item.HideOnClose = true;//指你关闭窗口时，窗体只是隐藏而不是真的关闭
            item.StartPosition = FormStartPosition.CenterParent;
            item.DockTo(dockPanel, style);
            item.VisibleState = visibleState;
        }

        /// <summary>
        /// 获取停靠控件可见
        /// </summary>
        /// <param name="targetContent">目标停靠窗口</param>
        /// <returns>true:显示;false:隐藏</returns>
        public static bool GetDockContentVisible(IDockContent targetContent)
        {
            if (targetContent == null)
            {
                throw new ArgumentNullException("目标停靠控件不能为null", "targetContent");
            }

            return !targetContent.DockHandler.IsHidden;
        }

        /// <summary>
        /// 设置停靠控件可见
        /// </summary>
        /// <param name="targetContent">目标停靠窗口</param>
        /// <param name="isDisplay">停靠控件可见[true:显示;false:隐藏;默认为隐藏]</param>
        public static void SetDockContentVisible(IDockContent targetContent, bool isDisplay = false)
        {
            if (targetContent == null)
            {
                throw new ArgumentNullException("目标停靠控件不能为null", "targetContent");
            }

            bool isHiden = !isDisplay;
            if (targetContent.DockHandler.IsHidden != isHiden)
            {
                targetContent.DockHandler.IsHidden = isHiden;
            }

            //if (isDisplay && targetContent.DockHandler.IsHidden)
            //{
            //    targetContent.DockHandler.Show();
            //}
            //else if (!isDisplay && !targetContent.DockHandler.IsHidden)
            //{
            //    targetContent.DockHandler.Hide();
            //}
        }

        /// <summary>
        /// 关闭还是隐藏窗体
        /// </summary>
        /// <param name="handler">目标停靠窗口句柄</param>
        /// <param name="dockPanel">停靠窗口控件</param>
        /// <param name="isDisplay">是关闭还是隐藏指定的停靠窗口[true:显示;false:隐藏;默认为隐藏]</param>
        public static void SetDockContentVisible(IntPtr handler, DockPanel dockPanel, bool isDisplay = false)
        {
            if (handler == IntPtr.Zero)
            {
                throw new ArgumentException(string.Format("目标停靠窗口句柄不能为{0}", IntPtr.Zero), "handler");
            }

            if (dockPanel == null)
            {
                throw new ArgumentNullException("停靠容器控件不能为null", "dockPanel");
            }

            foreach (IDockContent dockContent in dockPanel.Contents)
            {
                if (handler == dockContent.DockHandler.Form.Handle)
                {
                    WFLHelper.SetDockContentVisible(dockContent, isDisplay);
                    break;
                }
            }
        }
    }
}
