using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class DeleteHostCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public long HostId { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public DeleteHostCommand(long hostId)
            : base(SHPCommandDefine.DELETE_HOST)
        {
            this.HostId = hostId;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public DeleteHostCommand()
        {

        }
    }
}
