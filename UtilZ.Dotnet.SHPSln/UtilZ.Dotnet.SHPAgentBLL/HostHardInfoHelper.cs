using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    internal class HostHardInfoHelper
    {
        public static HostHardInfo GetHostHardInfo()
        {
            var hostHardInfo = new HostHardInfo();

            var computerInfo = new ComputerInfo();
            hostHardInfo.OSFullName = computerInfo.OSFullName;
            hostHardInfo.OSPlatform = computerInfo.OSPlatform;
            hostHardInfo.OSVersion = computerInfo.OSVersion;
            hostHardInfo.CpuInfos = GetCpuInfos();
            hostHardInfo.Memory = GetMemoryInfo(computerInfo);
            hostHardInfo.Disk = GetDisk();
            hostHardInfo.NetworkInterfaceItems = GetNetworkInterfaceItems();
            return hostHardInfo;
        }

        private static List<HostHardDiskItem> GetDisk()
        {
            var diskList = new List<HostHardDiskItem>();
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            long totalSize;
            foreach (var driveInfo in driveInfos)
            {
                try
                {
                    if (driveInfo.IsReady)
                    {
                        totalSize = driveInfo.TotalSize;
                    }
                    else
                    {
                        totalSize = 0;
                    }

                    diskList.Add(new HostHardDiskItem(driveInfo.Name, totalSize));
                }
                catch (IOException)
                { }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }

            return diskList;
        }

        private static List<HostHardNetworkInterfaceItem> GetNetworkInterfaceItems()
        {
            var items = new List<HostHardNetworkInterfaceItem>();
            var nets = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (var net in nets)
            {
                var item = new HostHardNetworkInterfaceItem();
                item.Description = net.Description;
                item.Name = net.Name;
                item.Mac = net.GetPhysicalAddress().ToString();
                item.NetworkInterfaceType = (int)net.NetworkInterfaceType;
                item.Speed = net.Speed;
                items.Add(item);
            }

            return items;
        }

        private static HostHardMemory GetMemoryInfo(ComputerInfo computerInfo)
        {
            return new HostHardMemory() { Total = computerInfo.TotalPhysicalMemory };
        }

        private static List<HostHardCpuItem> GetCpuInfos()
        {
            var items = new List<HostHardCpuItem>();
            using (var searcher = new ManagementObjectSearcher("select * from Win32_Processor"))
            {
                using (var moc = searcher.Get())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        //StringBuilder sb = new StringBuilder();
                        //sb.AppendLine("Environment.ProcessorCount       " + Environment.ProcessorCount.ToString());

                        //foreach (PropertyData pd in mo.Properties)
                        //{
                        //    sb.Append(pd.Name);
                        //    sb.Append("             ");
                        //    var o = mo[pd.Name];
                        //    sb.AppendLine(o != null ? o.ToString() : string.Empty);
                        //}

                        //var str = sb.ToString();

                        var item = new HostHardCpuItem();
                        item.Name = mo["Name"].ToString();
                        item.NumberOfCores = Convert.ToUInt32(mo["NumberOfCores"]);
                        item.NumberOfLogicalProcessors = Convert.ToUInt32(mo["NumberOfLogicalProcessors"]);
                        items.Add(item);
                    }

                    return items;
                }
            }
        }
    }
}
