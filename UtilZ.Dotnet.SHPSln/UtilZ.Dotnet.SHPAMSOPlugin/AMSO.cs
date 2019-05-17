using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Net;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;
using UtilZ.Dotnet.SHPBase.Plugin.PluginABase;
using UtilZ.Dotnet.SHPMSOPluginBase;

namespace UtilZ.Dotnet.SHPAMSOPlugin
{
    [SHPPluginAttribute(MSOCommandDefine.MSO_PLUGIN_ID, "测轨", "测轨A插件")]
    public class AMSO : SHPADevOpsBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AMSO()
        {

        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="transferCommand"></param>
        /// <returns></returns>
        protected override void PrimitiveExcuteCommand(SHPTransferCommand transferCommand)
        {
            Loger.Info($"收到数据[{Encoding.UTF8.GetString(transferCommand.Data)}]");
            if (transferCommand.Cmd == MSOCommandDefine.TEST_SEND_INTER)
            {
                var transferCommandRew = new SHPTransferCommand(transferCommand, MSOCommandDefine.TEST_SEND_INTER_RES, SHPCommandExcuteResult.Sucess, transferCommand.Data);
                this.Net.SendCommandTo(transferCommandRew, transferCommand.SrcEndPoint);
                Loger.Info($"响应数据完成...");
            }
        }
    }
}
