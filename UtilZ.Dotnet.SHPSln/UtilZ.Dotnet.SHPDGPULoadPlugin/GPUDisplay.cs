using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPAGPULoadPlugin;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;

namespace UtilZ.Dotnet.SHPDGPULoadPlugin
{
    /// <summary>
    /// GPU负载展示
    /// </summary>
    [SHPPluginAttribute(10001, "GPU", "GPU硬件监视D端")]
    public class GPUDisplay : SHPHardDisplayBase
    {
        private readonly UCGPUHardInfoShowControl _hardInfoShowControl;
        private readonly UCGPUStatusShowControl _statusShowControl;

        public GPUDisplay()
        {
            this._hardInfoShowControl = new UCGPUHardInfoShowControl();
            this._statusShowControl = new UCGPUStatusShowControl();
            this._enable = true;
        }

        private bool _enable = false;
        /// <summary>
        /// 插件是否可用
        /// </summary>
        public override bool Enable
        {
            get
            {
                return this._enable;
            }
        }

        /// <summary>
        /// 硬件名称
        /// </summary>
        public override string HardName
        {
            get { return "GPU"; }
        }

        /// <summary>
        /// 获取硬件信息展示控件
        /// </summary>
        /// <returns>硬件信息展示控件</returns>
        public override Control GetHardInfoShowControl()
        {
            return this._hardInfoShowControl;
        }

        /// <summary>
        /// 刷新硬件信息
        /// </summary>
        /// <param name="hardInfo">硬件信息</param>
        public override void RefreshHardInfo(string hardInfo)
        {
            this._hardInfoShowControl.RefreshHardInfo(hardInfo);
        }

        /// <summary>
        /// 获取状态展示控件
        /// </summary>
        /// <returns>负载展示控件</returns>
        public override Control GetStatusShowControl()
        {
            return this._statusShowControl;
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="status">状态字符串</param>
        public override void RefreshStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return;
                }

                var list = SerializeEx.WebScriptJsonDeserializeObject<List<GPUInfoItem>>(status);
                this._statusShowControl.RefreshLoadStatus(list);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "刷新GPU负载异常");
            }
        }

        /// <summary>
        /// 显示连续状态
        /// </summary>
        /// <param name="durationStatusStrs">连续状态字符串数组</param>
        public override void ShowDurationStatus(List<string> durationStatusStrs)
        {
            try
            {
                if (durationStatusStrs == null || durationStatusStrs.Count == 0)
                {
                    return;
                }

                var listArr = new List<GPUInfoItem>[durationStatusStrs.Count];
                for (int i = 0; i < durationStatusStrs.Count; i++)
                {
                    listArr[i] = SerializeEx.WebScriptJsonDeserializeObject<List<GPUInfoItem>>(durationStatusStrs[i]);
                }

                this._statusShowControl.RefreshLoadStatus(listArr);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "刷新GPU负载异常");
            }
        }

        /// <summary>
        /// 清空状态
        /// </summary>
        public override void ClearStatus()
        {
            this._statusShowControl.ClearData();
        }
    }
}
