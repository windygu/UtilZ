using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPDevOps.Base
{
    public partial class UCHostTreeControl : UserControl
    {
        private const int _IMAGE_GROUP_INDEX = 0;
        private const int _IMAGE_HOST_ON_LINE_INDEX = 1;
        private const int _IMAGE_HOST_OFF_LINE_INDEX = 2;

        private readonly TreeNode _rootNode;

        public TreeNode RootNode
        {
            get { return _rootNode; }
        }

        public UCHostTreeControl()
        {
            InitializeComponent();

            this._rootNode = this.CreateNode("主机", _IMAGE_GROUP_INDEX, null);
            tvHost.Nodes.Add(this._rootNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">true:在线;false:离线</param>
        /// <returns></returns>
        private int GetHostImgIndexByHostStatus(HostStatus status)
        {
            int imageIndex;
            if (status == HostStatus.OnLine)
            {
                imageIndex = _IMAGE_HOST_ON_LINE_INDEX;
            }
            else
            {
                imageIndex = _IMAGE_HOST_OFF_LINE_INDEX;
            }

            return imageIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostInfo"></param>
        /// <returns></returns>
        public TreeNode CreateHostNode(HostInfo hostInfo)
        {
            int imageIndex = this.GetHostImgIndexByHostStatus(hostInfo.Status);
            return this.CreateNode(hostInfo.Name, imageIndex, hostInfo);
        }

        public TreeNode CreateHostGroupNode(HostGroup hostGroup)
        {
            return this.CreateNode(hostGroup.Name, UCHostTreeControl._IMAGE_GROUP_INDEX, hostGroup);
        }

        private TreeNode CreateNode(string text, int imageIndex, object tag)
        {
            var node = new TreeNode(text);
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;
            node.Tag = tag;
            return node;
        }

        public void DeleteHostGroupNode(HostGroup hostGroup)
        {
            this.DeleteNode(hostGroup);
        }

        public void DeleteHostNode(HostInfo hostInfo)
        {
            this.DeleteNode(hostInfo);
        }

        private void DeleteNode(object tag)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object>(this.DeleteNode), tag);
            }
            else
            {
                this.PrimitiveDeleteNode(this.tvHost.Nodes, tag);
            }
        }

        private void PrimitiveDeleteNode(TreeNodeCollection nodes, object tag)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == tag)
                {
                    nodes.Remove(node);
                    return;
                }
            }
        }

        private void UCHostTreeControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                tvHost.SelectedNode = this._rootNode;
                this.tvHost.AfterCheck += this.tvHost_AfterCheck;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        #region AfterCheck
        private void tvHost_AfterCheck(object sender, TreeViewEventArgs e)
        {
            this.tvHost.AfterCheck -= this.tvHost_AfterCheck;
            var node = e.Node;
            try
            {
                if (node.Nodes.Count > 0)
                {
                    this.UpdateChildNodeCheckedStatus(node.Nodes, node.Checked);
                }

                this.PrimitiveChecked(node);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
            finally
            {
                this.tvHost.AfterCheck += this.tvHost_AfterCheck;
            }
        }

        private void Check(bool isChecked)
        {
            this.tvHost.AfterCheck -= this.tvHost_AfterCheck;
            try
            {
                this.UpdateChildNodeCheckedStatus(this.tvHost.Nodes, isChecked);
            }
            finally
            {
                this.tvHost.AfterCheck += this.tvHost_AfterCheck;
            }
        }

        private void PrimitiveChecked(TreeNode node)
        {
            if (node.Checked)
            {
                this.CheckAllChecked(node);
            }
            else
            {
                this.CheckUnAllChecked(node);
            }
        }

        /// <summary>
        /// 检查是否非全选
        /// </summary>
        /// <param name="node"></param>
        /// <param name="groupImgIndex"></param>
        private void CheckUnAllChecked(TreeNode node)
        {
            while (node.Parent != null)
            {
                if (node.Parent.Checked)
                {
                    node.Parent.Checked = false;
                }

                node = node.Parent;
            }
        }

        /// <summary>
        /// 检查是否全选
        /// </summary>
        /// <param name="node"></param>
        /// <param name="groupImgIndex"></param>
        /// <returns></returns>
        private bool CheckAllChecked(TreeNode node)
        {
            while (node.Parent != null)
            {
                foreach (TreeNode pnode in node.Parent.Nodes)
                {
                    if (pnode == node)
                    {
                        continue;
                    }

                    if (!pnode.Checked)
                    {
                        return false;
                    }

                    if (pnode.Nodes.Count > 0)
                    {
                        if (!CheckAllChild(pnode.Nodes))
                        {
                            return false;
                        }
                    }
                }

                node.Parent.Checked = true;
                node = node.Parent;
            }

            return true;
        }

        private bool CheckAllChild(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (!node.Checked)
                {
                    return false;
                }
                else
                {
                    if (node.Nodes.Count > 0)
                    {
                        return this.CheckAllChild(node.Nodes);
                    }
                }
            }

            return true;
        }

        private void UpdateChildNodeCheckedStatus(TreeNodeCollection nodes, bool isChecked)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = isChecked;
                this.UpdateChildNodeCheckedStatus(node.Nodes, isChecked);
            }
        }

        public void CheckAll()
        {
            this.Check(true);
        }

        public void UnCheck()
        {
            this.Check(false);
        }

        public List<HostInfo> GetCheckHostInfoList()
        {
            var checkHostInfoList = new List<HostInfo>();
            this.GetCheckHostInfoList(tvHost.Nodes, checkHostInfoList);
            return checkHostInfoList;
        }

        private void GetCheckHostInfoList(TreeNodeCollection nodes, List<HostInfo> checkHostInfoList)
        {
            foreach (TreeNode node in nodes)
            {
                if (node == this._rootNode || node.Tag is HostGroup)
                {
                    this.GetCheckHostInfoList(node.Nodes, checkHostInfoList);
                }
                else if (node.Tag is HostInfo)
                {
                    if (node.Checked)
                    {
                        checkHostInfoList.Add((HostInfo)node.Tag);
                    }
                }
                else
                {
                    throw new ApplicationException($"未知的节点数据类型[{node.Tag.GetType().FullName}]");
                }
            }
        }
        #endregion

        public void RefreshHostTree(List<HostGroup> hostGroupList, List<HostInfo> hostInfoList)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<List<HostGroup>, List<HostInfo>>(this.RefreshHostTree), hostGroupList, hostInfoList);
            }
            else
            {
                this._rootNode.Nodes.Clear();
                this.AddHostGroup(hostGroupList, DevOpsConstant.DEFAULT_HOST_GROUP_ID, this._rootNode, hostInfoList);

                foreach (var hostInfo in hostInfoList)
                {
                    this._rootNode.Nodes.Add(this.CreateNode(hostInfo.Name, _IMAGE_HOST_ON_LINE_INDEX, hostInfo));
                }

                tvHost.ExpandAll();
            }
        }

        private void AddHostGroup(List<HostGroup> hostGroupList, long parentId, TreeNode parentNode, List<HostInfo> hostInfoList)
        {
            var hostGroups = (from t in hostGroupList where t.ParentId == parentId select t).ToArray();
            foreach (var hostGroup in hostGroups)
            {
                hostGroupList.Remove(hostGroup);
                TreeNode hostGroupNode = this.CreateNode(hostGroup.Name, _IMAGE_GROUP_INDEX, hostGroup);
                parentNode.Nodes.Add(hostGroupNode);
                this.AddHostGroup(hostGroupList, hostGroup.Id, hostGroupNode, hostInfoList);

                var hostInfos = (from h in hostInfoList where h.HostGoupId == hostGroup.Id select h).ToArray();
                foreach (var hostInfo in hostInfos)
                {
                    hostInfoList.Remove(hostInfo);
                    hostGroupNode.Nodes.Add(this.CreateNode(hostInfo.Name, _IMAGE_HOST_ON_LINE_INDEX, hostInfo));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostInfo"></param>
        public void HostStatusChanged(HostInfo hostInfo)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<HostInfo>(this.HostStatusChanged), hostInfo);
                }
                else
                {
                    TreeNode hostNode = this.FindHostTreeNode(this.tvHost.Nodes, hostInfo.Id);
                    if (hostNode == null)
                    {
                        return;
                    }

                    hostNode.ImageIndex = this.GetHostImgIndexByHostStatus(hostInfo.Status);
                    hostNode.SelectedImageIndex = hostNode.ImageIndex;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private TreeNode FindHostTreeNode(TreeNodeCollection nodes, long hostId)
        {
            TreeNode targetNode = null;
            foreach (TreeNode node in nodes)
            {
                if (node == this._rootNode || node.Tag is HostGroup)
                {
                    targetNode = this.FindHostTreeNode(node.Nodes, hostId);
                    if (targetNode != null)
                    {
                        return targetNode;
                    }
                }
                else if (node.Tag is HostInfo)
                {
                    if (((HostInfo)node.Tag).Id == hostId)
                    {
                        return node;
                    }
                }
                else
                {
                    throw new ApplicationException("未知错误逻辑");
                }
            }

            return null;
        }
    }
}
