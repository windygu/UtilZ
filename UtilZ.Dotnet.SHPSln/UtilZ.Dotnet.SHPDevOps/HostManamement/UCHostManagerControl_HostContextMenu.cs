using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class UCHostManagerControl
    {
        #region 主机右键菜单
        private void tsmiAddHost_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null || _tvHost.SelectedNode.Tag is HostInfo)
                {
                    return;
                }

                long hostGroupId;
                if (_tvHost.SelectedNode == this.ucHostTreeControl.RootNode)
                {
                    hostGroupId = DevOpsConstant.DEFAULT_HOST_GROUP_ID;
                }
                else
                {
                    hostGroupId = ((HostGroup)_tvHost.SelectedNode.Tag).Id;
                }

                var frm = new FEditHost(this._hostManager, hostGroupId, null);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var hostInfo = frm.GetHostInfo();
                _tvHost.SelectedNode.Nodes.Add(this.ucHostTreeControl.CreateHostNode(hostInfo));
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiModifyHost_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null || !(_tvHost.SelectedNode.Tag is HostInfo))
                {
                    return;
                }

                var hostInfo = (HostInfo)_tvHost.SelectedNode.Tag;
                var frm = new FEditHost(this._hostManager, hostInfo.HostGoupId, hostInfo);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                hostInfo = frm.GetHostInfo();
                _tvHost.SelectedNode.Text = hostInfo.Name;
                _tvHost.SelectedNode.Tag = hostInfo;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDeleteHost_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null || !(_tvHost.SelectedNode.Tag is HostInfo))
                {
                    return;
                }

                var hostInfo = (HostInfo)_tvHost.SelectedNode.Tag;
                if (MessageBox.Show($"确定删除主机[{hostInfo.Name}]?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    return;
                }

                this._hostManager.DeleteHost(hostInfo);
                _tvHost.SelectedNode.Remove();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiAddHostGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null || _tvHost.SelectedNode.Tag is HostInfo)
                {
                    return;
                }

                long parentHostGroupId;
                if (_tvHost.SelectedNode.Tag is HostGroup)
                {
                    parentHostGroupId = ((HostGroup)_tvHost.SelectedNode.Tag).Id;
                }
                else
                {
                    parentHostGroupId = DevOpsConstant.DEFAULT_HOST_GROUP_ID;
                }

                var hostGroup = (HostGroup)_tvHost.SelectedNode.Tag;
                var frm = new FEditHostGroup(this._hostManager, parentHostGroupId, null);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                hostGroup = frm.GetHostGroup();
                _tvHost.SelectedNode.Nodes.Add(this.ucHostTreeControl.CreateHostGroupNode(hostGroup));
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiModifyHostGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null || !(_tvHost.SelectedNode.Tag is HostGroup))
                {
                    return;
                }

                var hostGroup = (HostGroup)_tvHost.SelectedNode.Tag;
                var frm = new FEditHostGroup(this._hostManager, hostGroup.ParentId, hostGroup);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                hostGroup = frm.GetHostGroup();
                _tvHost.SelectedNode.Text = hostGroup.Name;
                _tvHost.SelectedNode.Tag = hostGroup;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiDeleteHostGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null || !(_tvHost.SelectedNode.Tag is HostGroup))
                {
                    return;
                }

                var hostGroup = (HostGroup)_tvHost.SelectedNode.Tag;
                string text;
                if (_tvHost.SelectedNode.Nodes.Count > 0)
                {
                    text = $"分组[{hostGroup.Name}]下有其它分组或主机,确定删除?";
                }
                else
                {
                    text = $"确定删除分组[{hostGroup.Name}]?";
                }

                if (MessageBox.Show(text, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    return;
                }

                this._hostManager.DeleteHostGroup(hostGroup);
                _tvHost.SelectedNode.Remove();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null)
                {
                    return;
                }

                if (_tvHost.SelectedNode == this.ucHostTreeControl.RootNode)
                {
                    this.RefreshRootGroupAndNoneHroupHost();
                }
                else if (_tvHost.SelectedNode.Tag is HostInfo)
                {
                    this.RefreashHostInfo((HostInfo)_tvHost.SelectedNode.Tag);
                }
                else if (_tvHost.SelectedNode.Tag is HostGroup)
                {
                    this.PrimitiveRefreshHostList(_tvHost.SelectedNode, (HostGroup)_tvHost.SelectedNode.Tag);
                }
                else
                {
                    Loger.Error($"不支持的类型[{_tvHost.SelectedNode.Tag.GetType().FullName}]");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostShutdown_Click(object sender, EventArgs e)
        {
            try
            {
                this._hostManager.ControlHost<ShutdownCommand>(this._currentSelectedhostInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostRestart_Click(object sender, EventArgs e)
        {
            try
            {
                this._hostManager.ControlHost<RestartCommand>(this._currentSelectedhostInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostLogout_Click(object sender, EventArgs e)
        {
            try
            {
                this._hostManager.ControlHost<LogoutCommand>(this._currentSelectedhostInfo);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }


        private void tsmiHostCheck_Click(object sender, EventArgs e)
        {
            _tvHost.CheckBoxes = !_tvHost.CheckBoxes;
        }

        private void tsmiHostCheckAll_Click(object sender, EventArgs e)
        {
            this.ucHostTreeControl.CheckAll();
        }

        private void tsmiHostUnCheck_Click(object sender, EventArgs e)
        {
            this.ucHostTreeControl.UnCheck();
        }

        private void tsmiHostScript_Click(object sender, EventArgs e)
        {
            try
            {
                List<HostInfo> hostInfoList = this.ucHostTreeControl.GetCheckHostInfoList();
                if (hostInfoList == null || hostInfoList.Count == 0)
                {
                    Loger.Warn("未选择执行脚本的目标服务器");
                    return;
                }

                var frm = new FScriptEdit();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var para = new PartAsynWaitPara<Tuple<ScriptType, string, int, List<HostInfo>, HostManager>, object>();
                para.Caption = "正在执行脚本";
                para.IsShowCancel = false;
                para.Para = new Tuple<ScriptType, string, int, List<HostInfo>, HostManager>(frm.ScriptType, frm.ScriptContent, frm.MillisecondsTimeout, hostInfoList, this._hostManager);
                para.Function = (p) =>
                {
                    for (int i = 0; i < p.Para.Item4.Count; i++)
                    {
                        try
                        {
                            p.AsynWait.Hint = $"第{i + 1}/{p.Para.Item4.Count}主机[{p.Para.Item4[i].Name}]正在执行脚本...";
                            p.Para.Item5.ExcuteScript(p.Para.Item1, p.Para.Item2, p.Para.Item3, p.Para.Item4[i]);
                        }
                        catch (Exception ex)
                        {
                            Loger.Info(ex, $"主机[{p.Para.Item4[i].Name}]执行脚本异常");
                        }
                    }

                    return null;
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception, "启动应用异常");
                    }
                    else if (ret.Status == PartAsynExcuteStatus.Completed)
                    {
                        Loger.Info($"执行脚本完成");
                    }
                };

                PartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void cmsHost_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (_tvHost.SelectedNode == null)
                {
                    e.Cancel = true;
                    return;
                }

                bool checkEnable = _tvHost.CheckBoxes;
                tsmiHostCheckAll.Enabled = checkEnable;
                tsmiHostUnCheck.Enabled = checkEnable;

                cmsHost.Items.Clear();
                if (_tvHost.SelectedNode == this.ucHostTreeControl.RootNode)
                {
                    cmsHost.Items.AddRange(this._rootMenuItems);
                }
                else if (_tvHost.SelectedNode.Tag is HostInfo)
                {
                    cmsHost.Items.AddRange(this._hostMenuItems);
                }
                else if (_tvHost.SelectedNode.Tag is HostGroup)
                {
                    cmsHost.Items.AddRange(this._hostGroupMenuItems);
                }
                else
                {
                    Loger.Error($"不支持的类型[{_tvHost.SelectedNode.Tag.GetType().FullName}]");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
