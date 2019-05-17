using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class ServiceInfo : SHPBaseModel
    {
        [Browsable(false)]
        public long Id { get; set; }

        private string _name = string.Empty;
        [DisplayName("服务名称")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.Equals(_name, value))
                {
                    return;
                }

                _name = value;
                base.OnRaisePropertyChanged(nameof(Name));
            }
        }

        private string _des = string.Empty;
        [DisplayName("描述")]
        public string Des
        {
            get { return _des; }
            set
            {
                if (string.Equals(_des, value))
                {
                    return;
                }

                _des = value;
                base.OnRaisePropertyChanged(nameof(Des));
            }
        }

        private TransferProtocal _transferProtocal = TransferProtocal.Udp;
        /// <summary>
        /// 传输协议先默认UDP,至于 同时支持多种协议,理论上说支持有点难,主要是不好寻址
        /// </summary>
        [Browsable(false)]
        public TransferProtocal TransferProtocal
        {
            get { return _transferProtocal; }
            set
            {
                if (_transferProtocal == value)
                {
                    return;
                }

                _transferProtocal = value;
                base.OnRaisePropertyChanged(nameof(TransferProtocalText));
            }
        }
        [DisplayName("传输协议")]
        public string TransferProtocalText
        {
            get { return EnumEx.GetEnumItemDisplayName(_transferProtocal); }
        }

        private long _hostTypeId = 0;
        [Browsable(false)]
        public long HostTypeId
        {
            get { return _hostTypeId; }
            set
            {
                if (_hostTypeId == value)
                {
                    return;
                }

                _hostTypeId = value;
                base.OnRaisePropertyChanged(nameof(HostTypeText));
            }
        }

        [DisplayName("主机类型")]
        public string HostTypeText
        {
            get
            {
                var handler = GetHostTypeById;
                if (handler != null)
                {
                    var hostType = handler(_hostTypeId);
                    if (hostType != null)
                    {
                        return hostType.Name;
                    }
                }

                return string.Empty;
            }
        }
        public static Func<long, HostTypeItem> GetHostTypeById;

        /// <summary>
        /// 小于1无限,大于0按实际分配
        /// </summary>
        private int _serviceInsMaxCount = -1;
        [Browsable(false)]
        public int ServiceInsMaxCount
        {
            get { return _serviceInsMaxCount; }
            set
            {
                if (_serviceInsMaxCount == value)
                {
                    return;
                }

                _serviceInsMaxCount = value;
                base.OnRaisePropertyChanged(nameof(ServiceInsMaxCountText));
            }
        }
        [DisplayName("服务实例最大个数")]
        public string ServiceInsMaxCountText
        {
            get
            {
                string str;
                if (this._serviceInsMaxCount > 0)
                {
                    str = this._serviceInsMaxCount.ToString();
                }
                else
                {
                    str = "无限";
                }

                return str;
            }
        }

        private long _serviceMirrorId = 0;
        [Browsable(false)]
        public long ServiceMirrorId
        {
            get { return _serviceMirrorId; }
            set
            {
                if (_serviceMirrorId == value)
                {
                    return;
                }

                _serviceMirrorId = value;
                base.OnRaisePropertyChanged(nameof(ServiceMirrorInfo));
            }
        }

        [DisplayName("镜像版本")]
        public string ServiceMirrorInfo
        {
            get
            {
                var serviceMirrorInfo = this.GetServiceMirrorInfo();
                if (serviceMirrorInfo != null)
                {
                    return serviceMirrorInfo.Version.ToString();
                }

                return string.Empty;
            }
        }
        public static Func<long, ServiceMirrorInfo> GetServiceMirrorInfoByIdFunc;

        public ServiceMirrorInfo GetServiceMirrorInfo()
        {
            var handler = GetServiceMirrorInfoByIdFunc;
            if (handler != null)
            {
                return handler(_serviceMirrorId);
            }

            return null;
        }

        public ServiceInfo()
            : base()
        {

        }

        public void Update(ServiceInfo serviceTypeItem)
        {
            this.Name = serviceTypeItem._name;
            this.Des = serviceTypeItem._des;
            this.HostTypeId = serviceTypeItem._hostTypeId;
            this.ServiceMirrorId = serviceTypeItem._serviceMirrorId;
        }

        /// <summary>
        /// 允许部署服务实例返回true;不允许返回false
        /// </summary>
        /// <param name="deployedServiceInsCount"></param>
        /// <returns></returns>
        public bool AllowDeployNewServiceIns(int deployedServiceInsCount)
        {
            var serviceInsMaxCount = this._serviceInsMaxCount;
            if (serviceInsMaxCount > 0 && deployedServiceInsCount >= serviceInsMaxCount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
