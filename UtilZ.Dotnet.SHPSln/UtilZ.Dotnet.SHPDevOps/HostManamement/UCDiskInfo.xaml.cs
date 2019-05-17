using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    /// <summary>
    /// UCDiskInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UCDiskInfo : UserControl
    {
        private readonly ObservableCollection<DiskUsageInfoItem> _disks = new ObservableCollection<DiskUsageInfoItem>();
        private readonly Dictionary<string, DiskUsageInfoItem> _diskDic = new Dictionary<string, DiskUsageInfoItem>();

        public UCDiskInfo()
        {
            InitializeComponent();

            this.DataContext = this._disks;
        }

        public void ShowDiskInfo(IEnumerable<HostDiskLoadItem> hostDiskLoadItems)
        {
            if (hostDiskLoadItems == null || hostDiskLoadItems.Count() == 0)
            {
                this.Clear();
                return;
            }

            var size = DiskUsageSizeConver.ConvertSize(listBox.ActualHeight);
            if (this._disks.Count == 0)
            {
                foreach (var hostDiskLoadItem in hostDiskLoadItems)
                {
                    this.AddDiskUsageInfoItem(hostDiskLoadItem, size);
                }
            }
            else
            {
                foreach (var hostDiskLoadItem in hostDiskLoadItems)
                {
                    if (this._diskDic.ContainsKey(hostDiskLoadItem.Name))
                    {
                        this._diskDic[hostDiskLoadItem.Name].Update(hostDiskLoadItem, size);
                    }
                    else
                    {
                        this.AddDiskUsageInfoItem(hostDiskLoadItem, size);
                    }
                }
            }
        }

        private void AddDiskUsageInfoItem(HostDiskLoadItem hostDiskLoadItem, double size)
        {
            var diskUsageInfoItem = new DiskUsageInfoItem(hostDiskLoadItem);
            diskUsageInfoItem.Update(hostDiskLoadItem, size);
            this._disks.Add(diskUsageInfoItem);
            this._diskDic[diskUsageInfoItem.Name] = diskUsageInfoItem;
        }

        public void Clear()
        {
            this._disks.Clear();
            this._diskDic.Clear();
        }
    }

    internal class DiskUsageInfoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnRaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string Name { get; private set; }
        public string DriveTypeText { get; private set; }
        public string DriveFormat { get; private set; }



        private double _size = 160;
        private long _useSpace;
        private long _totalSize;


        public double UsageSize
        {
            get
            {
                return _size - _size * _useSpace / _totalSize;
            }
        }

        public string UsageInfo
        {
            get
            {
                const long gb = 1073741824;
                var useGB = _useSpace / gb;
                var totalGb = _totalSize / gb;
                return string.Format("已使用({0}/{1})GB", useGB, totalGb);
            }
        }

        public DiskUsageInfoItem(HostDiskLoadItem item)
        {
            this.Name = item.Name;
            this.DriveTypeText = this.GetDriveTypeText(item.DriveType);
            this.DriveFormat = $"文件系统({item.DriveFormat})";
            this._totalSize = item.TotalSize;
        }

        private string GetDriveTypeText(DriveType driveType)
        {
            string driveTypeText;
            switch (driveType)
            {
                case System.IO.DriveType.NoRootDirectory:
                    driveTypeText = "没有根目录";
                    break;
                case System.IO.DriveType.Removable:
                    driveTypeText = "没有根目录";
                    break;
                case System.IO.DriveType.Fixed:
                    driveTypeText = "固定磁盘";
                    break;
                case System.IO.DriveType.Network:
                    driveTypeText = "网络驱动器";
                    break;
                case System.IO.DriveType.CDRom:
                    driveTypeText = "光盘设备";
                    break;
                case System.IO.DriveType.Ram:
                    driveTypeText = "RAM 磁盘";
                    break;
                case System.IO.DriveType.Unknown:
                default:
                    driveTypeText = "未知";
                    break;
            }

            return driveTypeText;
        }

        public void Update(HostDiskLoadItem item, double height)
        {
            this._size = height;
            this._useSpace = item.TotalSize - item.AvailableFreeSpace;
            this.OnRaisePropertyChanged(nameof(UsageSize));
            this.OnRaisePropertyChanged(nameof(UsageInfo));
        }
    }

    internal class DiskUsageSizeConver : IValueConverter
    {
        public DiskUsageSizeConver()
        {

        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertSize((double)value);
        }

        public static double ConvertSize(double size)
        {
            const double OFFSET = 22;
            if (size > OFFSET)
            {
                size = size - OFFSET;
            }

            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
