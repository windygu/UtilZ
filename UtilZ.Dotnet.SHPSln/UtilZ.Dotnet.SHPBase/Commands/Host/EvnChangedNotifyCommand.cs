using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class EvnChangedNotifyCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public long DevOpsId { get; set; }

        [TTLVAttribute(102)]
        public long HostId { get; set; }

        [TTLVAttribute(103)]
        public int StatusUploadIntervalMilliseconds { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public EvnChangedNotifyCommand(long devOpsId, long hostId, int statusUploadIntervalMilliseconds)
            : base(SHPCommandDefine.EVN_CHANGED_NOTIFY)
        {
            this.DevOpsId = devOpsId;
            this.HostId = hostId;
            this.StatusUploadIntervalMilliseconds = statusUploadIntervalMilliseconds;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public EvnChangedNotifyCommand()
        {

        }
    }
}
