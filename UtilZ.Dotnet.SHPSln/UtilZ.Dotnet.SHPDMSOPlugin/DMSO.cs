using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Net;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;
using UtilZ.Dotnet.SHPBase.Plugin.PluginDBase;
using UtilZ.Dotnet.SHPMSOPluginBase;

namespace UtilZ.Dotnet.SHPDMSOPlugin
{
    /// <summary>
    /// 测轨DevOps端插件
    /// </summary>
    [SHPPluginAttribute(MSOCommandDefine.MSO_PLUGIN_ID, "测轨", "测轨D插件")]
    public class DMSO : SHPDDevOpsBase
    {
        private readonly MSOBLL _bll;
        private readonly UCMSO _control;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DMSO()
        {
            this._bll = new MSOBLL(this);
            this._control = new UCMSO(this._bll);
        }

        public override void Loaded()
        {
            base.Loaded();

            SHPSyncCommandResultExcutor.RegisterSHPSyncResponseCommand(MSOCommandDefine.TEST_SEND_INTER_RES);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="transferCommand"></param>
        /// <returns></returns>
        protected override void PrimitiveExcuteCommand(SHPTransferCommand transferCommand)
        {

            //transferCommand.Data
        }

        /// <summary>
        /// 获取运控控件
        /// </summary>
        /// <returns>运控控件</returns>
        public override Control GetDevOpsControl()
        {
            return this._control;
        }
    }
}
