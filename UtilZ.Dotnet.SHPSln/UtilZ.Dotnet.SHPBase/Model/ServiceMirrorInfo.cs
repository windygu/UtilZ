using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class ServiceMirrorInfo : SHPBaseModel
    {
        [Browsable(false)]
        public long Id { get; set; }

        [Browsable(false)]
        public long ServiceInfoId { get; set; }

        private string _mirrorFilePath = string.Empty;
        [Browsable(false)]
        public string MirrorFilePath
        {
            get { return _mirrorFilePath; }
            set
            {
                if (string.Equals(_mirrorFilePath, value))
                {
                    return;
                }

                _mirrorFilePath = value;
                base.OnRaisePropertyChanged(nameof(MirrorFilePath));
            }
        }

        private ServiceMirrorType _serviceMirrorType = ServiceMirrorType.Zip;
        [Browsable(false)]
        public ServiceMirrorType ServiceMirrorType
        {
            get { return _serviceMirrorType; }
            set
            {
                if (_serviceMirrorType == value)
                {
                    return;
                }

                _serviceMirrorType = value;
                base.OnRaisePropertyChanged(nameof(ServiceMirrorTypeText));
            }
        }
        [DisplayName("镜像类型")]
        public string ServiceMirrorTypeText
        {
            get { return EnumEx.GetEnumItemDisplayName(_serviceMirrorType); }
        }

        private string _arguments = string.Empty;
        [DisplayName("启动参数")]
        public string Arguments
        {
            get { return _arguments; }
            set
            {
                _arguments = value;
                base.OnRaisePropertyChanged(nameof(Arguments));
            }
        }

        private string _appProcessFilePath = string.Empty;
        [DisplayName("进程文件路径")]
        public string AppProcessFilePath
        {
            get { return _appProcessFilePath; }
            set
            {
                _appProcessFilePath = value;
                base.OnRaisePropertyChanged(nameof(AppProcessFilePath));
            }
        }

        private string _appExeFilePath = string.Empty;
        [DisplayName("启动文件路径")]
        public string AppExeFilePath
        {
            get { return _appExeFilePath; }
            set
            {
                _appExeFilePath = value;
                base.OnRaisePropertyChanged(nameof(AppExeFilePath));
            }
        }

        private int _version = 0;
        [DisplayName("版本")]
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version == value)
                {
                    return;
                }

                _version = value;
                base.OnRaisePropertyChanged(nameof(Version));
            }
        }

        private string _des = string.Empty;
        [Browsable(false)]
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

        private int _deployMillisecondsTimeout = 60000;
        [DisplayName("部署超时时长")]
        public int DeployMillisecondsTimeout
        {
            get { return _deployMillisecondsTimeout; }
            set
            {
                if (_deployMillisecondsTimeout == value)
                {
                    return;
                }

                _deployMillisecondsTimeout = value;
                base.OnRaisePropertyChanged(nameof(DeployMillisecondsTimeout));
            }
        }

        public ServiceMirrorInfo()
        {

        }

        public void Update(ServiceMirrorInfo serviceMirrorInfo)
        {
            this.ServiceInfoId = serviceMirrorInfo.ServiceInfoId;
            this.MirrorFilePath = serviceMirrorInfo._mirrorFilePath;
            this.Arguments = serviceMirrorInfo._arguments;
            this.AppProcessFilePath = serviceMirrorInfo._appProcessFilePath;
            this.AppExeFilePath = serviceMirrorInfo._appExeFilePath;
            this.Version = serviceMirrorInfo._version;
            this.Des = serviceMirrorInfo._des;
            this.DeployMillisecondsTimeout = serviceMirrorInfo._deployMillisecondsTimeout;
        }
    }
}
