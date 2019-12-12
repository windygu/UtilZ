using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.Ex.LRPC;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.Commands.AppControl;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPBase.Commands.Host;
using UtilZ.Dotnet.Ex.AsynWait;
using UtilZ.Dotnet.WindowEx.Winform.Controls;
using System.Collections;
using UtilZ.Dotnet.SHPDevOps.Base;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class UCHostManagerControl : UserControl, ICollectionOwner
    {
        private RouteManager _routeManager;
        private HostManager _hostManager;
        private HostStatusInfoManager _hostStatusInfoManager;
        private readonly int _cpuUsageLineId = 1;
        private readonly int _memoryUsageLineId = 1;
        private readonly TreeView _tvHost;

        private readonly BindingCollection<HostProcessInfoItem> _hostProcessList;
        /// <summary>
        /// 监视应用列表
        /// </summary>
        private readonly BindingCollection<AppMonitorItem> _appMonitorList;
        private HostInfo _currentSelectedhostInfo = null;
        private bool _hasExtendStatusInfo = false;

        private readonly ToolStripItem[] _rootMenuItems;
        private readonly ToolStripItem[] _hostMenuItems;
        private readonly ToolStripItem[] _hostGroupMenuItems;

        public UCHostManagerControl()
        {
            InitializeComponent();

            this._tvHost = ucHostTreeControl.tvHost;
            this._tvHost.ContextMenuStrip = cmsHost;
            this._hostProcessList = new BindingCollection<HostProcessInfoItem>(this);
            this._appMonitorList = new BindingCollection<AppMonitorItem>(this);

            this._rootMenuItems = new ToolStripItem[] { tsmiAddHost, tsmiAddHostGroup, tsmiHostRefresh,
                tsmiHostCheck, tsmiHostCheckAll, tsmiHostUnCheck,tsmiHostScript };
            this._hostMenuItems = new ToolStripItem[] { tsmiModifyHost, tsmiDeleteHost, tsmiHostRefresh, tsmiHostShutdown, tsmiHostRestart, tsmiHostLogout,
                tsmiHostCheck, tsmiHostCheckAll, tsmiHostUnCheck,tsmiHostScript };
            this._hostGroupMenuItems = new ToolStripItem[] { tsmiAddHost, tsmiAddHostGroup, tsmiModifyHostGroup, tsmiDeleteHostGroup, tsmiHostRefresh,
                tsmiHostCheck, tsmiHostCheckAll, tsmiHostUnCheck ,tsmiHostScript};
        }

        public void Init(DevOpsBLL bll)
        {
            this._hostManager = bll.HostManager;
            this._routeManager = bll.RouteManager;
            this._hostStatusInfoManager = bll.HostStatusInfoManager;

            dgvHostType.ShowData(this._hostManager.HostTypeList.DataSource, $"{typeof(UCHostManagerControl).Name}_HostTypeList");
            dgvHostType.GridControl.ContextMenuStrip = cmsHostType;

            this.InitUsageControl();
            this.InitHardControl();

            Loger.Info("正在创建远程调用通道...");
            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_REV_HOST_HARD_INFO_CHANNEL, this.LRPCRevHostHardInfoCallback);
            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_REV_HOST_STATUS_INFO_CHANNEL, this.LRPCRevHostStatusInfoCallback);
            LRPCCore.CreateChannelF(SHPConstant.GET_HOST_IFNO_CHANNEL_NAME, this.LRPCGetHostInfoCallback);
            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_HOST_DELETE_CHANNEL, this.LRPCHostDeleteCallback);
            LRPCCore.CreateChannelF(DevOpsConstant.LRPC_HOST_HROUP_DELETE_CHANNEL, this.LRPCHostGroupDeleteCallback);

            this._hostStatusInfoManager.StartStatusAnalyze(this.HostStatusChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostInfo"></param>
        /// <param name="flag">bool:true:上线;false:离线</param>
        private void HostStatusChanged(HostInfo hostInfo)
        {
            ucHostTreeControl.HostStatusChanged(hostInfo);
        }

        private void InitUsageControl()
        {
            this.usageControlCpu.AddLine(new CharLine(this._cpuUsageLineId, null));
            this.usageControlCpu.ShowTitle = false;
            this.usageControlMemory.AddLine(new CharLine(this._memoryUsageLineId, null));
            this.usageControlMemory.ShowTitle = false;
        }

        private object LRPCHostGroupDeleteCallback(object obj)
        {
            ucHostTreeControl.DeleteHostGroupNode((HostGroup)obj);
            return null;
        }

        private object LRPCHostDeleteCallback(object obj)
        {
            ucHostTreeControl.DeleteHostNode((HostInfo)obj);
            return null;
        }

        private object LRPCGetHostInfoCallback(object obj)
        {
            return this._currentSelectedhostInfo;
        }

        private object LRPCRevHostStatusInfoCallback(object obj)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    return this.Invoke(new Func<object, object>(this.LRPCRevHostStatusInfoCallback), obj);
                }
                else
                {
                    var hostStatusInfo = (HostStatusInfo)obj;
                    var dHostLoadPlugins = this._hostManager.GetDHostLoadPlugins();
                    if (!this.ValidateSelectedNodeIsTargetHost(hostStatusInfo.Id, dHostLoadPlugins))
                    {
                        return null;
                    }

                    this.RefreshHostStatus(hostStatusInfo, dHostLoadPlugins);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            return null;
        }

        private object LRPCRevHostHardInfoCallback(object obj)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Func<object, object>(this.LRPCRevHostHardInfoCallback), obj);
                }
                else
                {
                    var hostHardInfo = (HostHardInfo)obj;
                    var dHostLoadPlugins = this._hostManager.GetDHostLoadPlugins();
                    if (!this.ValidateSelectedNodeIsTargetHost(hostHardInfo.Id, dHostLoadPlugins))
                    {
                        return null;
                    }

                    this.RefreshHostHardInfo(hostHardInfo, dHostLoadPlugins);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            return null;
        }

        private void InitHardControl()
        {
            Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins = this._hostManager.GetDHostLoadPlugins();

            //处理硬件信息展示控件以及负载状态展示控件
            foreach (var dHostHardPlugin in dHostLoadPlugins)
            {
                try
                {
                    var plugin = dHostHardPlugin.Plugin;
                    var pluginName = dHostHardPlugin.PluginAttribute.Name;

                    //硬件信息展示控件
                    var hardInfoShowControl = plugin.GetHardInfoShowControl();
                    if (hardInfoShowControl != null)
                    {
                        hardInfoShowControl.Dock = DockStyle.Fill;
                        listBoxHostHardInfo.Items.Add(new HardInfoShowItem(plugin.HardName, hardInfoShowControl));
                    }

                    //负载展示控件
                    var statusShowControl = plugin.GetStatusShowControl();
                    if (statusShowControl != null)
                    {
                        var tp = new TabPage();
                        statusShowControl.Dock = DockStyle.Fill;
                        tp.Controls.Add(statusShowControl);
                        tp.Text = pluginName;
                        tp.Name = $"{nameof(tabControlHostLoad)}_tp_{pluginName}";
                        tabControlHostLoad.TabPages.Add(tp);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, $"加载{dHostHardPlugin.GetType().FullName}控件发生异常");
                }
            }
        }

        public void RefreshRootGroupAndNoneHroupHost()
        {
            var para = new PartAsynWaitPara<HostManager, object>();
            para.Caption = "刷新数据";
            para.IsShowCancel = false;
            para.Para = this._hostManager;

            para.Function = (p) =>
            {
                this.PrimitiveRefreshRootGroupAndNoneHroupHost(p.AsynWait);
                return null;
            };

            para.Completed = (ret) =>
            {
                if (ret.Status == PartAsynExcuteStatus.Exception)
                {
                    Loger.Error(ret.Exception, "刷新数据异常");
                }
            };

            WinformPartAsynWaitHelper.Wait(para, this);
        }

        private void PrimitiveRefreshRootGroupAndNoneHroupHost(IPartAsynWait asynWait)
        {
            asynWait.Hint = "正在查询组";
            List<HostGroup> hostGroupList = this._hostManager.QueryHostGroupList();

            asynWait.Hint = "正在查询默认组主机";
            List<HostInfo> hostInfoList = this._hostStatusInfoManager.GetHostInfoList();

            asynWait.Hint = "正在刷新数据";

            ucHostTreeControl.RefreshHostTree(hostGroupList, hostInfoList);
        }

        private void UCHostManager_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                dgvProcessList.ShowData(this._hostProcessList.DataSource, $"{typeof(UCHostManagerControl).Name}_hostProcessList");
                dgvProcessList.GridControl.ContextMenuStrip = cmsProcessList;

                dgvHostServiceList.ShowData(this._appMonitorList.DataSource, "DevOps__appMonitorList");
                dgvHostServiceList.GridControl.ContextMenuStrip = cmsHostServiceList;

                this._tvHost.AfterSelect += this.tvHost_AfterSelect;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tvHost_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var node = e.Node;
                HostInfo currentSelectedhostInfo = null;
                if (node.Tag == null)
                {
                    //根节点
                    this.DisableHostDetail();
                    this.ClearHostStatus();
                }
                else if (node.Tag is HostGroup)
                {
                    //主机分组节点
                    this.DisableHostDetail();
                    this.ClearHostStatus();
                    //this.PrimitiveRefreshHostList(node, (HostGroup)node.Tag);
                }
                else if (node.Tag is HostInfo)
                {
                    //主机根节点
                    currentSelectedhostInfo = (HostInfo)node.Tag;
                    this._hasExtendStatusInfo = true;
                    this.ClearHostStatus();
                    this.RefreashHostInfo(currentSelectedhostInfo);
                }
                else
                {
                    throw new NotImplementedException($"不识别的类型[{node.Tag.GetType().FullName}]");
                }

                this._currentSelectedhostInfo = currentSelectedhostInfo;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void listBoxHostHardInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBoxHostHardInfo.SelectedItem == null)
                {
                    return;
                }

                var control = ((HardInfoShowItem)listBoxHostHardInfo.SelectedItem).Control;
                panelHostHardInfo.Controls.Clear();

                if (control != null)
                {
                    panelHostHardInfo.Controls.Add(control);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}