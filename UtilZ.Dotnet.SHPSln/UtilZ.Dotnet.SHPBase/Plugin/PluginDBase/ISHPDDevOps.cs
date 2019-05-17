using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Plugin.PluginDBase
{
    /// <summary>
    /// DevOps端运控接口
    /// </summary>
    public interface ISHPDDevOps : ISHPDevOpsBase
    {
        /// <summary>
        /// 获取运控控件
        /// </summary>
        /// <returns>运控控件</returns>
        Control GetDevOpsControl();
    }
}
