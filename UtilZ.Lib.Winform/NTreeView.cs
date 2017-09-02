using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Lib.Winform
{
    /// <summary>
    /// 自定义树控件
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.TreeView))]//定义工具栏中的图标
    public class NTreeView : System.Windows.Forms.TreeView
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NTreeView()
            : base()
        {
            this.EnabledDoubleClick = false;
            this.AutoExpandedCollapse = true;
            this.AutoChecked = true;
        }

        /// <summary>
        /// 是否响应鼠标双击事件[true:响应;false:不响应;默认false]
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("是否响应鼠标双击事件")]
        [DefaultValueAttribute(false)]
        [CategoryAttribute("扩展")]
        public bool EnabledDoubleClick { get; set; }

        /// <summary>
        /// 是否双击自动展开或折叠树节点[true:自动展开或折叠;false:不自动展开或折叠;默认true]
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("是否双击自动展开或折叠树节点[true:自动展开或折叠;false:不自动展开或折叠;默认true]")]
        [DefaultValueAttribute(true)]
        [CategoryAttribute("扩展")]
        public bool AutoExpandedCollapse { get; set; }

        /// <summary>
        /// 显示复选框时,当一外复选框勾选状态改变后,是否自动勾选其低级及子节点[true:自动勾选;false:不自动勾选;默认true]
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("显示复选框时,当一外复选框勾选状态改变后,是否自动勾选其低级及子节点[true:自动勾选;false:不自动勾选;默认true]")]
        [DefaultValueAttribute(true)]
        [CategoryAttribute("扩展")]
        public bool AutoChecked { get; set; }

        /// <summary>
        /// 重写WndProc
        /// </summary>
        /// <param name="m">消息</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x203 && !this.EnabledDoubleClick)
            {
                //屏蔽WM_LBUTTONDBLCLK=0x0203消息
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 重写OnNodeMouseDoubleClick
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseDoubleClick(e);

            if (this.AutoExpandedCollapse)
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
                else
                {
                    e.Node.Expand();
                }
            }
        }

        /// <summary>
        /// 重写OnAfterCheck
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);

            if (e.Action != TreeViewAction.Unknown && this.AutoChecked)
            {
                this.SetChildNodes(e.Node, e.Node.Checked);
                this.SetParentNodes(e.Node, e.Node.Checked);
            }
        }

        /// <summary>
        /// 设置父节点选中
        /// </summary>
        /// <param name="curNode">当前节点</param>
        /// <param name="isChecked">选中状态</param>
        private void SetParentNodes(TreeNode curNode, bool isChecked)
        {
            if (curNode.Parent != null)
            {
                if (isChecked)
                {
                    curNode.Parent.Checked = isChecked;
                    SetParentNodes(curNode.Parent, isChecked);
                }
                else
                {
                    bool ParFlag = false;
                    foreach (TreeNode tmp in curNode.Parent.Nodes)
                    {
                        if (tmp.Checked)
                        {
                            ParFlag = true;
                            break;
                        }
                    }
                    curNode.Parent.Checked = ParFlag;
                    SetParentNodes(curNode.Parent, ParFlag);
                }
            }
        }

        /// <summary>
        /// 设置子节点选中状态
        /// </summary>
        /// <param name="curNode">当前节点</param>
        /// <param name="isChecked">选中状态</param>
        private void SetChildNodes(TreeNode curNode, bool isChecked)
        {
            if (curNode.Nodes != null)
            {
                foreach (TreeNode tmpNode in curNode.Nodes)
                {
                    tmpNode.Checked = isChecked;
                    SetChildNodes(tmpNode, isChecked);
                }
            }
        }
    }
}
