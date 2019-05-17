using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class DataRouteInfo : SHPBaseModel
    {
        [Browsable(false)]
        public long Id { get; set; }

        private int _dataCode = 0;
        [DisplayName("数据编码")]
        public int DataCode
        {
            get { return _dataCode; }
            set
            {
                if (_dataCode == value)
                {
                    return;
                }

                _dataCode = value;
                base.OnRaisePropertyChanged(nameof(DataCode));
            }
        }

        private string _name = string.Empty;
        [DisplayName("路由名称")]
        public string Name
        {
            get { return _name; }
            set
            {
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

        private long _serviceInfoId = 0;
        [Browsable(false)]
        public long ServiceInfoId
        {
            get { return _serviceInfoId; }
            set
            {
                _serviceInfoId = value;
                base.OnRaisePropertyChanged(nameof(ServiceName));
            }
        }

        [DisplayName("目标服务")]
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

        public ServiceInfo GetServiceInfo()
        {
            var handler = GetServiceInfoByIdFunc;
            if (handler != null)
            {
                return handler(_serviceInfoId);
            }

            return null;
        }

        public DataRouteInfo()
           : base()
        {

        }

        public void Update(DataRouteInfo dataRouteItem)
        {
            this.DataCode = dataRouteItem._dataCode;
            this.Des = dataRouteItem._des;
            this.ServiceInfoId = dataRouteItem._serviceInfoId;
        }

        public override string ToString()
        {
            var name = this.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                name = this._dataCode.ToString();
            }
            else
            {
                name = $"{name}({this._dataCode})";
            }

            return name;
        }
    }
}
