using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class DevOpsMigrateNotifyResCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public long HostId { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public DevOpsMigrateNotifyResCommand(long hostId)
            : base(SHPCommandDefine.DevOpsMigrateNotifyRes)
        {
            this.HostId = hostId;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public DevOpsMigrateNotifyResCommand()
        {

        }        
    }
}
