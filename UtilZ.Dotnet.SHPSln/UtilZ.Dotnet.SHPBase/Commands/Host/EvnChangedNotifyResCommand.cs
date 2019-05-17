using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Base.TTLV;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class EvnChangedNotifyResCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public long HostId { get; set; }

        [TTLVAttribute(102, typeof(TTLVPrimitiveCollectionConverter))]
        public byte[] Data { get; set; } = null;

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public EvnChangedNotifyResCommand(HostHardInfo hostHardInfo)
            : base(SHPCommandDefine.EVN_CHANGED_NOTIFY_RES)
        {
            this.HostId = hostHardInfo.Id;
            this.Data = SerializeEx.BinarySerialize(hostHardInfo);
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public EvnChangedNotifyResCommand()
        {

        }
    }
}
