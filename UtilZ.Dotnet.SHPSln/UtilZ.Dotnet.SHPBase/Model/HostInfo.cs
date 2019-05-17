using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostInfo
    {
        /// <summary>
        /// HostId
        /// </summary>
        public long Id { get; set; }
        public string Name { get; set; }

        public string Ip { get; set; }

        public long HostGoupId { get; set; }

        public long HostTypeId { get; set; }

        /// <summary>
        /// 主机状态[true:在线;false:离线]
        /// </summary>
        private HostStatus _status = HostStatus.OffLine;
        public HostStatus Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                {
                    return;
                }

                _status = value;
            }
        }

        private List<HostDisablePortInfo> _hostDisablePortInfoList = new List<HostDisablePortInfo>();
        public List<HostDisablePortInfo> HostDisablePortInfoList
        {
            get
            {
                return _hostDisablePortInfoList;
            }
            set
            {
                _hostDisablePortInfoList = value;
            }
        }

        public HostInfo()
        {

        }

        public void Update(HostInfo hostInfo)
        {
            this.Name = hostInfo.Name;
            this.Ip = hostInfo.Ip;
            this.HostGoupId = hostInfo.HostGoupId;
            this.HostTypeId = hostInfo.HostTypeId;
        }

        public override string ToString()
        {
            return $"{this.Name}({this.Ip})";
        }
    }
}
