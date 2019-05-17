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
    public class HostStatusInfoUploadCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public long HostId { get; set; }

        [TTLVAttribute(102, typeof(TTLVSerializeConverter), null, TTLVSerializeType.Json)]
        public HostStatusInfo StatusInfo { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public HostStatusInfoUploadCommand(HostStatusInfo hostStatusInfo)
            : base(SHPCommandDefine.UP_HOST_STATUS)
        {
            this.HostId = hostStatusInfo.Id;
            this.StatusInfo = hostStatusInfo;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public HostStatusInfoUploadCommand()
        {

        }
    }
}
