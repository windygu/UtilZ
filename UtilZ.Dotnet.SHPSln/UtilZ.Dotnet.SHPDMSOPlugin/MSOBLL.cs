using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPMSOPluginBase;

namespace UtilZ.Dotnet.SHPDMSOPlugin
{
    internal class MSOBLL
    {
        private readonly ISHPDDevOps _shpDevOps;
        private readonly SHPCommandExcutorManager _commandExcutorManager = new SHPCommandExcutorManager();

        public MSOBLL(ISHPDDevOps shpDevOps)
        {
            this._shpDevOps = shpDevOps;
            this._commandExcutorManager.Init(this.GetType().Assembly, null);
        }

        internal object Test(HostInfo hostInfo, string text, bool flag)
        {
            Loger.Info("发送测试命令开始...");
            if (flag)
            {
                var testPluginCommamnd = new TestPluginCommamnd(MSOCommandDefine.TEST_SEND, text);
                var transferCommand = new SHPTransferCommand(SHPTransferCommand.CreateContextId(), MSOCommandDefine.MSO_PLUGIN_ID, testPluginCommamnd, 5000);
                this._shpDevOps.Net.SendCommandHost(transferCommand, hostInfo.Ip);
                Loger.Info("发送测试命令完成...");
            }
            else
            {
                var testPluginCommamnd = new TestPluginCommamnd(MSOCommandDefine.TEST_SEND_INTER, text);
                var transferCommand = new SHPTransferCommand(SHPTransferCommand.CreateContextId(), MSOCommandDefine.MSO_PLUGIN_ID, testPluginCommamnd, 5000);
                var result = this._shpDevOps.Net.SendInteractiveCommandHost(transferCommand, hostInfo.Ip);
                Loger.Info("发送测试命令完成");
            }

            return null;
        }
    }
}
