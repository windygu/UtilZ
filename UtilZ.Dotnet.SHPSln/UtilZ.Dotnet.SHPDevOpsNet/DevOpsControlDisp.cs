using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ice;

namespace UtilZ.Dotnet.SHPDevOpsNet
{
    public class DevOpsControlDisp : IDevOpsControlDisp_
    {
        private readonly Func<string, string> _sendCommand;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sendCommand">发送命令委托</param>
        public DevOpsControlDisp(Func<string, string> sendCommand)
        {
            if (sendCommand == null)
            {
                throw new ArgumentNullException(nameof(sendCommand));
            }

            this._sendCommand = sendCommand;
        }

        public override void SendCommand_async(AMD_IDevOpsControl_SendCommand cb__, string cmdStr, Current current__)
        {
            cb__.ice_response(this._sendCommand(cmdStr));
        }
    }
}
