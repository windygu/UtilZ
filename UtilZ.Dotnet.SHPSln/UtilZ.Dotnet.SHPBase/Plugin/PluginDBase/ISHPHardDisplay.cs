using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Plugin.PluginDBase
{
    /// <summary>
    /// DevOps端硬件数据展示接口
    /// </summary>
    public interface ISHPHardDisplay : ISHPHardBase
    {
        /// <summary>
        /// 硬件名称
        /// </summary>
        string HardName { get; }

        /// <summary>
        /// 获取硬件信息展示控件
        /// </summary>
        /// <returns>硬件信息展示控件</returns>
        Control GetHardInfoShowControl();

        /// <summary>
        /// 刷新硬件信息
        /// </summary>
        /// <param name="hardInfo">硬件信息</param>
        void RefreshHardInfo(string hardInfo);

        /// <summary>
        /// 获取状态展示控件
        /// </summary>
        /// <returns>负载展示控件</returns>
        Control GetStatusShowControl();

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="status">状态字符串</param>
        void RefreshStatus(string status);

        /// <summary>
        /// 显示连续状态
        /// </summary>
        /// <param name="durationStatusStrs">连续状态字符串数组</param>
        void ShowDurationStatus(List<string> durationStatusStrs);

        /// <summary>
        /// 清空状态
        /// </summary>
        void ClearStatus();
    }
}
