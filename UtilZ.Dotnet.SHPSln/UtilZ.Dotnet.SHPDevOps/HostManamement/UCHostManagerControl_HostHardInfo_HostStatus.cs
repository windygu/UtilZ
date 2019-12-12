using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPDevOpsBLL;
using UtilZ.Dotnet.SHPDevOpsModel;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class UCHostManagerControl
    {
        #region tvHost_AfterSelect
        private void DisableHostDetail()
        {
            if (tabControlHostDetail.Enabled)
            {
                tabControlHostDetail.Enabled = false;
            }
        }

        private void PrimitiveRefreshHostList(TreeNode groupNode, HostGroup group)
        {
            var para = new PartAsynWaitPara<Tuple<TreeNode, HostManager, HostGroup>, List<HostInfo>>();
            para.Caption = "刷新主机列表";
            para.IsShowCancel = false;
            para.Para = new Tuple<TreeNode, HostManager, HostGroup>(groupNode, this._hostManager, group);
            para.Function = (p) =>
            {
                List<HostInfo> hostInfos = p.Para.Item2.QueryHostList(p.Para.Item3);
                this.Invoke(new Action(() =>
                {
                    p.Para.Item1.Nodes.Clear();
                    //}));

                    //this.Invoke(new Action(() =>
                    //{
                    foreach (var hostInfo in hostInfos)
                    {
                        // p.AsynWait.Hint = string.Format("正在停止ID:{0},类型:{1},名称:{2}的任务", taskInfo.TaskId, taskInfo.TaskTypeName, taskInfo.TaskName);
                        p.Para.Item1.Nodes.Add(ucHostTreeControl.CreateHostNode(hostInfo));
                    }
                }));

                return null;
            };

            para.Completed = (ret) =>
            {
                if (ret.Status == PartAsynExcuteStatus.Exception)
                {
                    Loger.Error(ret.Exception, "刷新主机列表异常");
                }
            };

            WinformPartAsynWaitHelper.Wait(para, this);
        }

        private void RefreashHostInfo(HostInfo hostInfo)
        {
            if (!tabControlHostDetail.Enabled)
            {
                tabControlHostDetail.Enabled = true;
            }

            var para = new PartAsynWaitPara<Tuple<HostManager, HostInfo>, object>();
            para.Caption = "刷新主机信息";
            para.IsShowCancel = false;
            para.Para = new Tuple<HostManager, HostInfo>(this._hostManager, hostInfo);
            para.Function = (p) =>
            {
                HostHardInfo hostHardInfo = p.Para.Item1.GetHostHardInfoByHostId(p.Para.Item2.Id);
                HostStatusInfo[] hostStatusInfos = p.Para.Item1.GetHostStatusInfo(hostInfo);
                this.Invoke(new Action<HostInfo, HostHardInfo, HostStatusInfo[]>(this.PrimitiveRefreashHostInfo), hostInfo, hostHardInfo, hostStatusInfos);
                return null;
            };

            para.Completed = (ret) =>
            {
                if (ret.Status == PartAsynExcuteStatus.Exception)
                {
                    Loger.Error(ret.Exception, "查询主机相关数据异常");
                }
            };

            WinformPartAsynWaitHelper.Wait(para, this);
        }

        private void PrimitiveRefreashHostInfo(HostInfo hostInfo, HostHardInfo hostHardInfo, HostStatusInfo[] hostStatusInfos)
        {
            try
            {
                var dHostLoadPlugins = this._hostManager.GetDHostLoadPlugins();
                this.RefreshHostHardInfo(hostHardInfo, dHostLoadPlugins);
                this.RefreshHostStatus(hostStatusInfos, dHostLoadPlugins);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "显示硬件信息或状态信息异常");
            }
        }

        private void RefreshHostStatus(HostStatusInfo hostStatusInfo, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (hostStatusInfo == null)
            {
                this.ClearHostStatus();
                return;
            }

            this.RefreshHostBasicStatusInfo(hostStatusInfo);
            this.RefreshHostExtendStatusInfo(hostStatusInfo, dHostLoadPlugins);
        }

        private void RefreshHostStatus(HostStatusInfo[] hostStatusInfos, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (hostStatusInfos == null || hostStatusInfos.Length == 0)
            {
                this.ClearHostStatus();
                return;
            }

            this.RefreshHostBasicStatusInfo(hostStatusInfos);
            this.RefreshHostExtendStatusInfo(hostStatusInfos, dHostLoadPlugins);
        }

        private bool ValidateSelectedNodeIsTargetHost(long hostId, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (_tvHost.SelectedNode == null ||
                _tvHost.SelectedNode == this.ucHostTreeControl.RootNode ||
                _tvHost.SelectedNode.Tag is HostGroup)
            {
                this.ClearHostExtendStatusInfo(dHostLoadPlugins);
                this.ClearHostBasicStatusInfo();
                return false;
            }

            if (((HostInfo)_tvHost.SelectedNode.Tag).Id != hostId)
            {
                return false;
            }

            return true;
        }

        private void RefreshHostExtendStatusInfo(HostStatusInfo[] hostStatusInfos, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (dHostLoadPlugins.Count == 0)
            {
                return;
            }

            if (hostStatusInfos == null || hostStatusInfos.Length == 0)
            {
                this.ClearHostExtendStatusInfo(dHostLoadPlugins);
            }
            else
            {
                var plugins = dHostLoadPlugins.ToList();
                var extendStatusInfoDic = new Dictionary<int, List<string>>();
                List<string> statusList;
                foreach (var hostStatusInfo in hostStatusInfos)
                {
                    foreach (var extendStatusInfo in hostStatusInfo.ExtendStatusInfos)
                    {
                        if (extendStatusInfoDic.ContainsKey(extendStatusInfo.Id))
                        {
                            statusList = extendStatusInfoDic[extendStatusInfo.Id];
                        }
                        else
                        {
                            statusList = new List<string>();
                            extendStatusInfoDic.Add(extendStatusInfo.Id, statusList);
                        }

                        statusList.Add(extendStatusInfo.Info);
                    }
                }

                var dHostLoadPluginList = dHostLoadPlugins.ToList();

                bool flag;
                foreach (var kv in extendStatusInfoDic)
                {
                    flag = true;
                    foreach (var dHostLoadPlugin in dHostLoadPlugins)
                    {
                        try
                        {
                            if (kv.Key == dHostLoadPlugin.PluginAttribute.Id)
                            {
                                flag = false;
                                var control = dHostLoadPlugin.Plugin.GetStatusShowControl();
                                if (!control.Enabled)
                                {
                                    control.Enabled = true;
                                }

                                dHostLoadPluginList.Remove(dHostLoadPlugin);
                                dHostLoadPlugin.Plugin.ShowDurationStatus(kv.Value);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Loger.Error(ex, $"刷新[{dHostLoadPlugin.GetType().FullName}]状态信息发生异常");
                        }
                    }

                    if (flag)
                    {
                        Loger.Warn($"负载插件ID[{kv.Key}]没有对应的状态信息展示控件,忽略...");
                    }
                }

                foreach (var disablePlugin in plugins)
                {
                    disablePlugin.Plugin.GetStatusShowControl().Enabled = false;
                }
            }
        }

        private void RefreshHostExtendStatusInfo(HostStatusInfo hostStatusInfo, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (dHostLoadPlugins.Count == 0)
            {
                return;
            }

            var extendStatusInfos = hostStatusInfo.ExtendStatusInfos;
            if (extendStatusInfos == null || extendStatusInfos.Count == 0)
            {
                if (this._hasExtendStatusInfo)
                {
                    this.ClearHostExtendStatusInfo(dHostLoadPlugins);
                    this._hasExtendStatusInfo = false;
                }
            }
            else
            {
                var plugins = dHostLoadPlugins.ToList();
                bool flag;
                foreach (var extendStatusInfo in extendStatusInfos)
                {
                    flag = true;
                    foreach (var dHostLoadPlugin in dHostLoadPlugins)
                    {
                        try
                        {
                            if (extendStatusInfo.Id == dHostLoadPlugin.PluginAttribute.Id)
                            {
                                flag = false;
                                var control = dHostLoadPlugin.Plugin.GetStatusShowControl();
                                if (!control.Enabled)
                                {
                                    control.Enabled = true;
                                }

                                plugins.Remove(dHostLoadPlugin);
                                dHostLoadPlugin.Plugin.RefreshStatus(extendStatusInfo.Info);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Loger.Error(ex, $"刷新[{dHostLoadPlugin.GetType().FullName}]状态信息发生异常");
                        }
                    }

                    if (flag)
                    {
                        Loger.Warn($"负载插件ID[{extendStatusInfo.Id}]没有对应的状态信息展示控件,忽略...");
                    }
                }

                foreach (var disablePlugin in plugins)
                {
                    disablePlugin.Plugin.GetStatusShowControl().Enabled = false;
                }
            }
        }

        private void RefreshHostBasicStatusInfo(HostStatusInfo hostStatusInfo)
        {
            //进程
            this.RefreshProcessInfoList(hostStatusInfo.ProcessInfoList);

            //监视应用
            this.RefreshAppMonitorList(hostStatusInfo.AppMonitorItemList);

            //cpu
            this.usageControlCpu.AddValueBegin(this._cpuUsageLineId, hostStatusInfo.CPU);
            this.usageControlCpu.AddValueEnd(1);

            //内存
            this.usageControlMemory.AddValueBegin(this._memoryUsageLineId, (float)hostStatusInfo.UseMemory * 100 / hostStatusInfo.TotalMemory);
            this.usageControlMemory.AddValueEnd(1);

            //网卡
            this.InitNetworkUsageControlChannel(hostStatusInfo.Nets);
            foreach (var net in hostStatusInfo.Nets)
            {
                usageControlNetwork.AddValueBegin(net.Name, net.GetUsage());
            }
            usageControlNetwork.AddValueEnd(1);

            //硬盘
            ucDiskInfo.ShowDiskInfo(hostStatusInfo.HostDiskInfos);
        }

        private void RefreshHostBasicStatusInfo(HostStatusInfo[] hostStatusInfos)
        {
            var lastHostStatusInfo = hostStatusInfos.Last();

            //进程
            this.RefreshProcessInfoList(lastHostStatusInfo.ProcessInfoList);

            //监视应用
            this.RefreshAppMonitorList(lastHostStatusInfo.AppMonitorItemList);

            //cpu
            var cpuUseages = hostStatusInfos.Select(t => { return t.CPU; }).ToArray();
            this.usageControlCpu.ClearDataNoRefresh();
            this.usageControlCpu.AddValueBegin(this._cpuUsageLineId, cpuUseages);
            this.usageControlCpu.AddValueEnd(cpuUseages.Length);

            //内存
            var memoryUseages = hostStatusInfos.Select(t => { return (float)t.UseMemory * 100 / t.TotalMemory; }).ToArray();
            this.usageControlMemory.ClearDataNoRefresh();
            this.usageControlMemory.AddValueBegin(this._memoryUsageLineId, memoryUseages);
            this.usageControlMemory.AddValueEnd(memoryUseages.Length);

            //网卡
            this.InitNetworkUsageControlChannel(hostStatusInfos[0].Nets);
            foreach (var hostStatusInfo in hostStatusInfos)
            {
                foreach (var net in hostStatusInfo.Nets)
                {
                    usageControlNetwork.AddValueBegin(net.Name, net.GetUsage());
                }
            }
            usageControlNetwork.AddValueEnd(hostStatusInfos.Length);

            //硬盘
            ucDiskInfo.ShowDiskInfo(lastHostStatusInfo.HostDiskInfos);
        }

        private void InitNetworkUsageControlChannel(List<NetInfoLoadItem> nets)
        {
            bool validResult = true;
            Hashtable ht = new Hashtable();
            foreach (var net in nets)
            {
                ht.Add(net.Name, net);
                if (!usageControlNetwork.ExistLine(net.Name))
                {
                    usageControlNetwork.ClearLine();
                    validResult = false;
                    break;
                }
            }

            if (validResult)
            {
                CharLine[] lines = usageControlNetwork.GetAllLine();
                foreach (var chartLine in lines)
                {
                    if (!ht.ContainsKey(chartLine.Id))
                    {
                        usageControlNetwork.RemoveLine(chartLine);
                        break;
                    }
                }
            }

            if (usageControlNetwork.LineCount > 0)
            {
                //已初始化过
                return;
            }

            List<Color> networkUsageLineColorList = Config.Instance.NetworkUsageLineColorList.ToList();
            for (int i = 0; i < nets.Count; i++)
            {
                if (networkUsageLineColorList.Count >= nets.Count)
                {
                    this.usageControlNetwork.AddLine(new CharLine(nets[i].Name, nets[i].Name, networkUsageLineColorList[i], 1));
                    networkUsageLineColorList.RemoveAt(i);
                }
                else
                {
                    this.usageControlNetwork.AddLine(new CharLine(nets[i].Name, nets[i].Name, ColorHelper.Colors[i], 1));
                }
            }
        }

        /// <summary>
        /// 合并集合,新集合中没有的项,旧集合中移除,集合中不存在的项添加,旧集合中存在的项更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="oldItems"></param>
        /// <param name="newItems"></param>
        /// <param name="keySelector"></param>
        /// <param name="update">第一个参数旧项,第二个参数为新项</param>
        private void MergeCollection<T, TKey>(ICollection<T> oldItems, ICollection<T> newItems, Func<T, TKey> keySelector, Action<T, T> update)
        {
            //进程列表
            var oldDic = oldItems.ToDictionary(keySelector);
            var newDic = newItems.ToDictionary(keySelector);

            //遍历旧集合,如果新的没有则移除
            foreach (var kv in oldDic)
            {
                if (!newDic.ContainsKey(kv.Key))
                {
                    oldItems.Remove(kv.Value);
                }
            }

            //遍历新集合,如果旧集合中存在则更新,不存在则添加
            foreach (var kv in newDic)
            {
                if (oldDic.ContainsKey(kv.Key))
                {
                    update(oldDic[kv.Key], kv.Value);
                }
                else
                {
                    oldItems.Add(kv.Value);
                }
            }
        }

        private void RefreshAppMonitorList(List<AppMonitorItem> appMonitorItemList)
        {
            this.MergeCollection<AppMonitorItem, string>(this._appMonitorList, appMonitorItemList, t => { return t.AppName; }, (o, n) => { o.Update(n); });

            ////进程列表
            //var oldAppMonitorDic = this._appMonitorList.ToDictionary(t => { return t.AppName; });
            //var newAppMonitorDic = appMonitorItemList.ToDictionary(t => { return t.AppName; });

            ////遍历旧集合,如果新的没有则移除
            //foreach (var kv in oldAppMonitorDic)
            //{
            //    if (!newAppMonitorDic.ContainsKey(kv.Key))
            //    {
            //        this._appMonitorList.Remove(kv.Value);
            //    }
            //}

            ////遍历新集合,如果旧集合中存在则更新,不存在则添加
            //foreach (var kv in newAppMonitorDic)
            //{
            //    if (oldAppMonitorDic.ContainsKey(kv.Key))
            //    {
            //        oldAppMonitorDic[kv.Key].Update(kv.Value);
            //    }
            //    else
            //    {
            //        this._appMonitorList.Add(kv.Value);
            //    }
            //}
        }

        private void RefreshProcessInfoList(List<HostProcessInfoItem> processInfoList)
        {
            this.MergeCollection<HostProcessInfoItem, int>(this._hostProcessList, processInfoList, t => { return t.Id; }, (o, n) => { o.Update(n); });

            ////进程列表
            //var oldHostProcessInfoDic = this._hostProcessList.ToDictionary(t => { return t.Id; });
            //var newHostProcessInfoDic = processInfoList.ToDictionary(t => { return t.Id; });

            ////遍历旧集合,如果新的没有则移除
            //foreach (var kv in oldHostProcessInfoDic)
            //{
            //    if (!newHostProcessInfoDic.ContainsKey(kv.Key))
            //    {
            //        this._hostProcessList.Remove(kv.Value);
            //    }
            //}

            ////遍历新集合,如果旧集合中存在则更新,不存在则添加
            //foreach (var kv in newHostProcessInfoDic)
            //{
            //    if (oldHostProcessInfoDic.ContainsKey(kv.Key))
            //    {
            //        oldHostProcessInfoDic[kv.Key].Update(kv.Value);
            //    }
            //    else
            //    {
            //        this._hostProcessList.Add(kv.Value);
            //    }
            //}
        }

        private void ClearHostStatus()
        {
            var dHostLoadPlugins = this._hostManager.GetDHostLoadPlugins();
            this.ClearHostExtendStatusInfo(dHostLoadPlugins);
            this.ClearHostBasicStatusInfo();
        }

        private void ClearHostBasicStatusInfo()
        {
            this._hostProcessList.Clear();//进程
            this._appMonitorList.Clear();//监视应用
            this.usageControlCpu.ClearData();//CPU
            this.usageControlMemory.ClearData();//内存
            this.usageControlNetwork.ClearLine();//网卡
                                                 //this.usageControlNetwork.ClearData();
            this.ucDiskInfo.Clear();//硬盘
        }

        private void ClearHostExtendStatusInfo(IEnumerable<PluginInfo<ISHPHardDisplay>> dHostLoadPlugins)
        {
            if (dHostLoadPlugins == null || dHostLoadPlugins.Count() == 0)
            {
                return;
            }

            foreach (var dHostLoadPlugin in dHostLoadPlugins)
            {
                try
                {
                    dHostLoadPlugin.Plugin.ClearStatus();
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, $"清空[{dHostLoadPlugin.GetType().FullName}]状态信息发生异常");
                }
            }
        }

        private void ClearHardInfo()
        {
            this.ClearHostBasicHardInfo();
            this.ClearHostExtendHardInfo();
        }

        private void ClearHostBasicHardInfo()
        {

        }

        private void ClearHostExtendHardInfo()
        {

        }

        private void RefreshHostHardInfo(HostHardInfo hostHardInfo, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (hostHardInfo == null)
            {
                this.ClearHardInfo();
                return;
            }

            this.RefreshHostBasicHardInfo(hostHardInfo);
            this.RefreshHostExtendHardInfo(hostHardInfo, dHostLoadPlugins);
        }

        private void RefreshHostBasicHardInfo(HostHardInfo hostHardInfo)
        {

        }

        private void RefreshHostExtendHardInfo(HostHardInfo hostHardInfo, Dictionary<int, PluginInfo<ISHPHardDisplay>>.ValueCollection dHostLoadPlugins)
        {
            if (dHostLoadPlugins == null || dHostLoadPlugins.Count == 0)
            {
                return;
            }

            var extendHardInfos = hostHardInfo.ExtendHardInfos;
            if (extendHardInfos == null || extendHardInfos.Count == 0)
            {
                //清空
                foreach (var dHostLoadPlugin in dHostLoadPlugins)
                {
                    try
                    {
                        dHostLoadPlugin.Plugin.RefreshHardInfo(null);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error(ex, $"清空[{dHostLoadPlugin.GetType().FullName}]硬件信息发生异常");
                    }
                }
            }
            else
            {
                //刷新
                var dHostLoadPluginList = dHostLoadPlugins.ToList();
                bool flag;
                foreach (var extendHardInfo in extendHardInfos)
                {
                    flag = true;
                    foreach (var dHostLoadPlugin in dHostLoadPlugins)
                    {
                        try
                        {
                            if (extendHardInfo.Id == dHostLoadPlugin.PluginAttribute.Id)
                            {
                                flag = false;
                                var control = dHostLoadPlugin.Plugin.GetStatusShowControl();
                                if (!control.Enabled)
                                {
                                    control.Enabled = true;
                                }

                                dHostLoadPluginList.Remove(dHostLoadPlugin);
                                dHostLoadPlugin.Plugin.RefreshHardInfo(extendHardInfo.Info);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Loger.Error(ex, $"刷新[{dHostLoadPlugin.GetType().FullName}]硬件信息发生异常");
                        }
                    }

                    if (flag)
                    {
                        Loger.Warn($"负载插件ID[{extendHardInfo.Id}]没有对应的硬件信息展示控件,忽略...");
                    }
                }

                foreach (var dHostLoadPlugin in dHostLoadPluginList)
                {
                    dHostLoadPlugin.Plugin.GetStatusShowControl().Enabled = false;
                }
            }
        }
        #endregion
    }
}
