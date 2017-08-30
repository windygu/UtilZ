using UtilZ.Lib.Base.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Lib.Winform
{
    /// <summary>
    /// 扩展TreeView类
    /// </summary>
    public static class NExtendTreeView
    {
        #region TreeView 展开深度扩展方法
        /// <summary>
        /// 递归展开结点
        /// </summary>
        /// <param name="treeNodes">TreeView.TreeNodeCollection</param>        
        /// <param name="currentDepth">当前已展开的深度</param>
        /// <param name="depth">要展开的深度</param>
        private static void ExpandDepth(TreeNodeCollection treeNodes, int currentDepth, int depth)
        {
            if (currentDepth > depth)
            {
                return;
            }

            foreach (TreeNode node in treeNodes)
            {
                node.Expand();
                NExtendTreeView.ExpandDepth(node.Nodes, ++currentDepth, depth);
                currentDepth--;
            }
        }

        /// <summary>
        /// 展开树深度
        /// </summary>
        /// <param name="tree">当前的树</param>
        /// <param name="depth">要展开的深度</param>
        public static void ExpandDepth(this TreeView tree, int depth)
        {
            NExtendTreeView.ExpandDepth(tree.Nodes, 0, depth - 1);
        }
        #endregion

        /// <summary>
        /// 设置选中的结点的背景颜色
        /// </summary>
        /// <param name="treeView">当前的树</param>
        /// <param name="lastSelectedNode">上一次选中的结点</param>
        /// <param name="backColor">选中节点背景色</param>
        /// <param name="foreColor">选中节点前景色</param>
        public static void SetSelectedTreeNodeBackcolor(this TreeView treeView, ref TreeNode lastSelectedNode, System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            if (treeView == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => treeView), string.Empty);
            }

            if (treeView.SelectedNode == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(p => treeView.SelectedNode), string.Empty);
            }

            if (treeView.SelectedNode == lastSelectedNode)
            {
                return;
            }

            if (lastSelectedNode != null)
            {
                lastSelectedNode.BackColor = treeView.BackColor;
                lastSelectedNode.ForeColor = treeView.ForeColor;
            }

            treeView.SelectedNode.BackColor = backColor;
            treeView.SelectedNode.ForeColor = foreColor;
            lastSelectedNode = treeView.SelectedNode;
        }

        #region 根据TreeView选中结点的全路径设置选中结点方法
        /// <summary>
        /// 根据TreeView选中结点的全路径设置选中结点
        /// </summary>
        /// <param name="tree">当前的树</param>
        /// <param name="fullPath">全路径[@"Earth/Assi/Yindu/jiazi2"]</param>
        /// <returns>设置成功返回true,失败返回false</returns>
        public static bool SetSelectedNodeByFullPath(this TreeView tree, string fullPath)
        {
            string[] paths = fullPath.Split('/');
            TreeNode selectedNode = null;

            if (paths.Length == 0)
            {
                throw new Exception(string.Format("全路径{0}不是合法的TreeView选中结点全路径", fullPath));
            }

            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                if (!tree.Nodes[i].Text.Equals(paths[0]))
                {
                    continue;
                }

                //此处第二个参数传入固定值1，是因为这儿的for循环对应的路径索引为0,所以下一层的值为从1开始                
                selectedNode = NExtendTreeView.FindSelectedNode(paths, 1, tree.Nodes[i]);
                if (selectedNode != null)
                {
                    tree.SelectedNode = selectedNode;
                    return true;
                }
                else
                {
                    continue;
                }
            }

            return false;
        }

        /// <summary>
        /// 递归查找选中项结点
        /// </summary>
        /// <param name="paths">路径集合</param>
        /// <param name="index">当前路径索引</param>
        /// <param name="parentNode">父级节点</param>
        /// <returns>找到的选中节点</returns>
        private static TreeNode FindSelectedNode(string[] paths, int index, TreeNode parentNode)
        {
            for (int i = 0; i < parentNode.Nodes.Count; i++)
            {
                if (parentNode.Nodes[i].Text == paths[index])
                {
                    index++;
                    if (parentNode.Nodes[i].Nodes.Count > 0 && paths.Length > index)
                    {
                        return NExtendTreeView.FindSelectedNode(paths, index, parentNode.Nodes[i]);
                    }
                    else if (parentNode.Nodes[i].Nodes.Count == 0 && paths.Length == index)
                    {
                        return parentNode.Nodes[i];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }
        #endregion
    }
}
