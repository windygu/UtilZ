using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Plugin.PluginDBase
{
    public abstract class SHPHardDisplayBase : SHPHardBase, ISHPHardDisplay
    {
        public SHPHardDisplayBase()
            : base()
        {

        }

        /// <summary>
        /// 硬件名称
        /// </summary>
        public abstract string HardName { get; }

        /// <summary>
        /// 获取硬件信息展示控件
        /// </summary>
        /// <returns>硬件信息展示控件</returns>
        public abstract Control GetHardInfoShowControl();

        /// <summary>
        /// 刷新硬件信息
        /// </summary>
        /// <param name="hardInfo">硬件信息</param>
        public abstract void RefreshHardInfo(string hardInfo);

        /// <summary>
        /// 获取状态展示控件
        /// </summary>
        /// <returns>负载展示控件</returns>
        public abstract Control GetStatusShowControl();

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="status">状态字符串</param>
        public abstract void RefreshStatus(string status);

        /// <summary>
        /// 显示连续状态
        /// </summary>
        /// <param name="durationStatusStrs">连续状态字符串数组</param>
        public abstract void ShowDurationStatus(List<string> durationStatusStrs);

        /// <summary>
        /// 清空状态
        /// </summary>
        public abstract void ClearStatus();
    }
}
