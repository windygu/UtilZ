using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPBase.Monitor;
using UtilZ.Dotnet.SHPBase.ServiceBasic;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Model;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostStatusInfo
    {
        /// <summary>
        /// HostId
        /// </summary>
        public long Id { get; set; }

        public float CPU { get; set; }
        public ulong UseMemory { get; set; }
        public ulong TotalMemory { get; set; }
        public List<NetInfoLoadItem> Nets { get; set; }
        public List<HostDiskLoadItem> HostDiskInfos { get; set; }

        public List<ExtendInfo> ExtendStatusInfos { get; set; }

        public List<HostProcessInfoItem> ProcessInfoList { get; set; }

        public List<AppMonitorItem> AppMonitorItemList { get; set; }

        public List<ServiceStatusInfo> ServiceStatusInfoList { get; set; }

        public HostStatusInfo()
        {

        }
    }

    [Serializable]
    public class HostDiskLoadItem
    {
        public string DriveFormat { get; set; }
        public long AvailableFreeSpace { get; set; }
        public DriveType DriveType { get; set; }
        public string Name { get; set; }
        public long TotalSize { get; set; }

        public HostDiskLoadItem()
        {

        }
    }

    [Serializable]
    public class NetInfoLoadItem
    {
        /// <summary>
        /// 网卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public long Speed { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 发送
        /// </summary>
        public float Send { get; set; }

        /// <summary>
        /// 接收
        /// </summary>
        public float Receive { get; set; }

        //public float Total { get; set; }

        /// <summary>
        /// 万兆计算百分比
        /// </summary>
        /// <returns></returns>
        public float GetUsage()
        {
            if (Speed <= 0)
            {
                return 0;
            }

            //以万兆为参照,计算百分比
            return (Receive + Send) * 100 / 10000000000;
        }

        public NetInfoLoadItem()
        {

        }
    }
}
