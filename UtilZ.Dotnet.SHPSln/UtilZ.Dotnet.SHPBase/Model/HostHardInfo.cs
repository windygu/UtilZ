using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostHardInfo
    {
        /// <summary>
        /// HostId
        /// </summary>
        public long Id { get; set; }
        public string OSFullName { get; set; }
        public string OSPlatform { get; set; }
        public string OSVersion { get; set; }
        public List<HostHardCpuItem> CpuInfos { get; set; }

        public HostHardMemory Memory { get; set; }

        public List<HostHardDiskItem> Disk { get; set; }

        public List<HostHardNetworkInterfaceItem> NetworkInterfaceItems { get; set; }

        public List<ExtendInfo> ExtendHardInfos { get; set; }

        public HostHardInfo()
        {

        }
    }

    [Serializable]
    public class HostHardNetworkInterfaceItem
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int NetworkInterfaceType { get; set; }

        public string Mac { get; set; }

        public long Speed { get; set; }

        public HostHardNetworkInterfaceItem()
        {

        }

    }

    [Serializable]
    public class HostHardDiskItem
    {
        public string Name { get; set; }
        public long Total { get; set; }

        public HostHardDiskItem(string name, long totalSize)
        {
            this.Name = name;
            this.Total = totalSize;
        }

        public HostHardDiskItem()
        {

        }


    }

    [Serializable]
    public class HostHardMemory
    {
        public ulong Total { get; set; }
    }

    [Serializable]
    public class HostHardCpuItem
    {
        public string Name { get; set; }
        public uint NumberOfCores { get; set; }
        public uint NumberOfLogicalProcessors { get; set; }

        public HostHardCpuItem()
        {

        }
    }
}
