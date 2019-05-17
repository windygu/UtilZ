using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPDevOpsModel
{
    [Serializable]
    public class Config : SHPConfigBase
    {
        public Config()
            : base()
        {
            this.NetworkUsageLineColorList.AddRange(new Color[]
            {
Color.Lime,
Color.Brown,
Color.DeepSkyBlue,
Color.Fuchsia,
Color.OrangeRed,
Color.Red,
Color.Turquoise,
Color.Yellow,
Color.RoyalBlue,
Color.Purple,
Color.Magenta,
            });
        }

        private static Config _instance = null;

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (Config)_configBase;
                }

                return _instance;
            }
        }

        /// <summary>
        /// 运控标识,用于区分不同环境
        /// </summary>
        public long DevOpsId { get; set; } = 1;

        /// <summary>
        /// 下达命令默认超时时长,单位/毫秒
        /// </summary>
        public int SendCommandDefaultTimeout { get; set; } = 5000;

        /// <summary>
        /// 主机状态最大项数
        /// </summary>
        public ushort HostStatusMaxCount { get; set; } = 250;

        /// <summary>
        /// 主机离线判断超时时长,单位/毫秒
        /// </summary>
        public int HostTimeoutMillisecondsTimeout { get; set; } = 20000;

        public List<Color> NetworkUsageLineColorList { get; set; } = new List<Color>();

        /// <summary>
        /// 主机离线检测间隔,单位.毫秒
        /// </summary>
        public int HostOffLineCheckInterval { get; set; } = 2000;
        public string ServiceMirrorFtpUrl { get; set; } = @"ftp://192.168.10.241";
        public string FileServiceUserName { get; set; } = string.Empty;
        public string FileServicePassword { get; set; } = string.Empty;
        public string ServiceRouteUrl { get; set; } = "http://192.168.10.96:20001/";
        public int StatusUploadIntervalMilliseconds { get; set; } = 5000;
        public int ServiceInsStatusInfoCacheCount { get; set; } = 20;
        public int ServiceInsPaloadAnalyzeObjectCount { get; set; } = 3;
        public int ServiceInsOffLineMillisecondsTimeout { get; set; } = 10000;
        public int ServiceInsAvgMaxPayloadValue { get; set; } = 70;
        public int ServiceInsAvgMinPayloadValue { get; set; } = 30;
        public int DenyCount { get; set; } = 3;
    }
}
