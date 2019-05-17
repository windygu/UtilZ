using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base.TTLV;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class AddHostCommand : CommandBase
    {
        /// <summary>
        /// 运控ID
        /// </summary>
        [TTLVAttribute(101)]
        public long DOId { get; set; }

        [TTLVAttribute(102)]
        public long HostId { get; set; }

        [TTLVAttribute(103, typeof(TTLVPrimitiveCollectionConverter))]
        public List<int> NotAllocatePortList { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public AddHostCommand(long doId, long hostId, List<int> notAllocatePortList)
            : base(SHPCommandDefine.ADD_HOST_REQ)
        {
            this.DOId = doId;
            this.HostId = hostId;
            this.NotAllocatePortList = notAllocatePortList;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public AddHostCommand()
        {

        }
    }
}
