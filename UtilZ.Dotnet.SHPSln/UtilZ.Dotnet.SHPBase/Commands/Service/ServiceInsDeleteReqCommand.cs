using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Base.TTLV;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPBase.Commands.Service
{
    public class ServiceInsDeleteReqCommand : CommandBase
    {
        [TTLVAttribute(101)]
        public long DOID { get; set; }

        /// <summary>
        /// 服务实例停止超时时长,System.Threading.Timeout.Infinite (-1)，表示无限期等待
        /// </summary>
        [TTLVAttribute(102)]
        public int MillisecondsTimeout { get; set; }

        /// <summary>
        /// 服务实例id列表
        /// </summary>
        [TTLVAttribute(103, typeof(TTLVPrimitiveCollectionConverter))]
        public List<long> IdList { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="doId">运 控id</param>
        /// <param name="millisecondsTimeout">服务实例停止超时时长</param>
        /// <param name="idList">服务实例id列表</param>
        public ServiceInsDeleteReqCommand(long doId, int millisecondsTimeout, List<long> idList)
            : base(SHPCommandDefine.SERVICE_INS_DELETE_REQ)
        {
            this.IdList = idList;
            this.DOID = doId;
            this.MillisecondsTimeout = millisecondsTimeout;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ServiceInsDeleteReqCommand()
        {

        }
    }
}
