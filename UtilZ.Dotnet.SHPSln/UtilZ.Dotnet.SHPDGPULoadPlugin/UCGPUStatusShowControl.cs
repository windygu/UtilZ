using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPAGPULoadPlugin;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPDGPULoadPlugin
{
    public partial class UCGPUStatusShowControl : UserControl
    {
        private readonly List<GPUInfoItem> _gpuLoadInfoItems = new List<GPUInfoItem>();


        public UCGPUStatusShowControl()
        {
            InitializeComponent();
        }

        private void UCGPUStatusShowControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }


        }

        private void AddStatus(GPUInfoItem item)
        {
            var lineId = item.DeviceIndex;
            if (!this.lineChartControl.ExistLine(lineId))
            {
                this.lineChartControl.AddLine(new CharLine(lineId, $"设备{lineId}", ColorHelper.Colors[(int)lineId], 1));
            }

            if (item.Status)
            {
                this.lineChartControl.AddValueBegin(lineId, item.Use);
            }
        }

        internal void RefreshLoadStatus(List<GPUInfoItem> list)
        {
            foreach (var item in list)
            {
                this.AddStatus(item);
            }

            this.lineChartControl.AddValueEnd(1);

            //var gpuLoadInfoItemDicNew = list.ToDictionary(t => { return t.DeviceIndex; });
            //var gpuLoadInfoItemDicOld = this._gpuLoadInfoItems.ToDictionary(t => { return t.DeviceIndex; });

            ////遍历旧的,如果新没有则从旧的集合中移除
            //foreach (var key in gpuLoadInfoItemDicOld.Keys)
            //{
            //    if (!gpuLoadInfoItemDicNew.ContainsKey(key))
            //    {
            //        this._gpuLoadInfoItems.Remove(gpuLoadInfoItemDicOld[key]);
            //        this.lineChartControl.RemoveLineById(key);
            //    }
            //}

            ////遍历新的,旧集合中存在则更新,不存在则添加
            //foreach (var kv in gpuLoadInfoItemDicNew)
            //{
            //    if (gpuLoadInfoItemDicOld.ContainsKey(kv.Key))
            //    {
            //        //更新
            //        gpuLoadInfoItemDicOld[kv.Key].Update(kv.Value);
            //    }
            //    else
            //    {
            //        //添加
            //        this._gpuLoadInfoItems.Add(kv.Value);
            //    }
            //}
        }

        internal void RefreshLoadStatus(List<GPUInfoItem>[] listArr)
        {
            foreach (var list in listArr)
            {
                foreach (var item in list)
                {
                    this.AddStatus(item);
                }
            }

            this.lineChartControl.AddValueEnd(listArr.Length);
        }

        internal void ClearData()
        {
            this.lineChartControl.ClearLine();
        }
    }
}
