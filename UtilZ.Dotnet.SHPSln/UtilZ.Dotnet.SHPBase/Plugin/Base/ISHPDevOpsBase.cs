using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    public interface ISHPDevOpsBase : ISHPPluginBase
    {
        void RegisterSHPResultCommand(IEnumerable<int> cmds);

        void RegisterSHPResultCommand(int cmd);

        /// <summary>
        /// 网络传输对象
        /// </summary>
        ISHPNet Net { get; set; }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="transferCommand"></param>
        void ExcuteCommand(SHPTransferCommand transferCommand);
    }
}
