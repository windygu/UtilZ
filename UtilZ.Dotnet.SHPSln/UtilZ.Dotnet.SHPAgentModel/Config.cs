using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPAgentModel
{
    [Serializable]
    public class Config : SHPConfigBase
    {
        public Config()
            : base()
        {

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
        /// 主机状态上报超时时长,单位/毫秒
        /// </summary>
        public int HostStatusUploadTimeout { get; set; } = 1000;

        public string TmpDir { get; set; } = "Tmp";

        public bool UploadExtendStatus { get; set; } = true;
    }
}
