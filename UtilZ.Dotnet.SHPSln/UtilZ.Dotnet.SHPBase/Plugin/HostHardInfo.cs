using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostHardInfo
    {
        public int HostId { get; set; }
        public string OSFullName { get; set; }
        public string OSPlatform { get; set; }
        public string OSVersion { get; set; }
        public List<HostHardCpuItem> CpuInfos { get; set; }

        public HostHardMemory Memory { get; set; }

        public List<HostHardDiskItem> Disk { get; set; }

        public List<HostHardNetworkInterfaceItem> NetworkInterfaceItems { get; set; }

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
        public ulong Total { get; set; }
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
