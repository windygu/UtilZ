using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPBase.Model
{
    /// <summary>
    /// SHP平台服务实例
    /// </summary>
    [Serializable]
    public class SHPServiceInsInfo : SHPBaseModel
    {
        [Browsable(false)]
        public long Id { get; set; }

        private List<long> _dataRouteId = new List<long>();
        [Browsable(false)]
        public List<long> DataRouteIdList
        {
            get { return _dataRouteId; }
            set
            {
                if (_dataRouteId == value)
                {
                    return;
                }

                _dataRouteId = value;
            }
        }

        private string _name = string.Empty;
        [DisplayName("服务实例名称")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                base.OnRaisePropertyChanged(nameof(Name));
            }
        }

        private long _dstServiceTypeId = 0;
        [Browsable(false)]
        public long ServiceId
        {
            get { return _dstServiceTypeId; }
            set
            {
                _dstServiceTypeId = value;
                base.OnRaisePropertyChanged(nameof(ServiceName));
            }
        }

        [DisplayName("服务名称")]
        public string ServiceName
        {
            get
            {
                var serviceInfo = this.GetServiceInfo();
                if (serviceInfo != null)
                {
                    return serviceInfo.Name;
                }

                return string.Empty;
            }
        }

        public static Func<long, ServiceInfo> GetServiceInfoByIdFunc;

        public static Func<long, HostInfo> GetHostInfoByIdFunc;

        public ServiceInfo GetServiceInfo()
        {
            var handler = GetServiceInfoByIdFunc;
            if (handler != null)
            {
                return handler(_dstServiceTypeId);
            }

            return null;
        }

        public HostInfo GetHostInfo()
        {
            var handler = GetHostInfoByIdFunc;
            if (handler != null)
            {
                return handler(_hostId);
            }

            return null;
        }

        private long _hostId = 0;
        [Browsable(false)]
        public long HostId
        {
            get { return _hostId; }
            set
            {
                if (_hostId == value)
                {
                    return;
                }

                _hostId = value;
                base.OnRaisePropertyChanged(nameof(ServiceEndPointText));
            }
        }


        private int _endPointPort = 0;
        [Browsable(false)]
        public int EndPointPort
        {
            get { return _endPointPort; }
            set
            {
                if (_endPointPort == value)
                {
                    return;
                }

                _endPointPort = value;
                base.OnRaisePropertyChanged(nameof(ServiceEndPointText));
            }
        }

        [DisplayName("终结点")]
        public string ServiceEndPointText
        {
            get
            {
                string serviceEndPointText;
                try
                {
                    HostInfo hostInfo = this.GetHostInfo();
                    if (hostInfo != null)
                    {
                        serviceEndPointText = $"{hostInfo.Name}({hostInfo.Ip}.{_endPointPort})";
                    }
                    else
                    {
                        //serviceEndPointText = $"{_hostId}.{_endPointPort}";
                        serviceEndPointText = "错误[主机不存在]";
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                    serviceEndPointText = $"{_hostId}.{_endPointPort}";
                }

                return serviceEndPointText;
            }
        }

        private ServiceInsStatus _status = ServiceInsStatus.OnLine;
        /// <summary>
        /// 状态[true:在线;false:离线]
        /// </summary>
        [Browsable(false)]
        public ServiceInsStatus Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                {
                    return;
                }

                _status = value;
                base.OnRaisePropertyChanged(StatusText);
            }
        }

        [DisplayName("状态")]
        public string StatusText
        {
            get
            {
                return EnumEx.GetEnumItemDisplayName(_status);
            }
        }

        private int _payloadValue = 0;
        public int PayloadValue
        {
            get { return _payloadValue; }
            set
            {
                if (_payloadValue == value)
                {
                    return;
                }

                _payloadValue = value;
                base.OnRaisePropertyChanged(nameof(PayloadValueText));
            }
        }
        [DisplayName("服务实例负载")]
        public string PayloadValueText
        {
            get { return _payloadValue.ToString(); }
        }

        public SHPServiceInsInfo()
           : base()
        {

        }

        public void Update(SHPServiceInsInfo serviceInsInfo)
        {
            this.DataRouteIdList = serviceInsInfo._dataRouteId;
            this.Name = serviceInsInfo._name;
            this.ServiceId = serviceInsInfo._dstServiceTypeId;
            this.HostId = serviceInsInfo._hostId;
            this.EndPointPort = serviceInsInfo._endPointPort;
        }

        public override string ToString()
        {
            var name = this.Name;
            var serviceEndPointText = this.ServiceEndPointText;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = this.MergeDataRouteAndEndPoint(name, serviceEndPointText);
            }
            else
            {
                var tmp = this.MergeDataRouteAndEndPoint(name, serviceEndPointText);
                name = $"{name}.{tmp}";
            }

            return name;
        }

        private string MergeDataRouteAndEndPoint(string name, string serviceEndPointText)
        {
            if (!string.IsNullOrWhiteSpace(serviceEndPointText))
            {
                name = serviceEndPointText;
            }

            return name;
        }
    }
}
