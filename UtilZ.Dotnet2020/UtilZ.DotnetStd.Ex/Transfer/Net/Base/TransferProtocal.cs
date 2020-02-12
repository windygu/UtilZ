using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.DotnetStd.Ex.Base;

namespace UtilZ.DotnetStd.Ex.Transfer.Net
{
    /// <summary>
    /// 传输协议
    /// </summary>
    public enum TransferProtocal : byte
    {
        /// <summary>
        /// udp
        /// </summary>
        [DisplayNameExAttribute("Udp")]
        Udp = 1,

        /// <summary>
        /// tcp
        /// </summary>
        [DisplayNameExAttribute("Tcp")]
        Tcp = 2
    }
}
