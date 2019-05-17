using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentModel
{
    [Serializable]
    public class DevOpsInfo
    {
        /// <summary>
        /// 运控标识ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 本机-运控对应的主机id
        /// </summary>
        public long HostId { get; set; }

        public IPEndPoint DevOpsIPEndPoint { get; set; }

        /// <summary>
        /// 上报主机状态间隔,单位/毫秒
        /// </summary>
        public int StatusUploadIntervalMilliseconds { get; set; }

        public DevOpsInfo(IPEndPoint devOpsIPEndPoint, AddHostCommand cmd)
        {
            this.Id = cmd.DOId;
            this.DevOpsIPEndPoint = devOpsIPEndPoint;
            this.HostId = cmd.HostId;
        }

        public DevOpsInfo()
        {

        }
    }
}
