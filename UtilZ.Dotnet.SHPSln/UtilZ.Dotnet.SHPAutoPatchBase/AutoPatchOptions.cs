using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPAutoPatchBase
{
    /// <summary>
    /// 升级选项
    /// </summary>
    public class AutoPatchOptions
    {
        /// <summary>
        /// 发起升级且升级过程中需要结束的进程ID
        /// </summary>
        public const string PROCESS_ID = "/p";

        /// <summary>
        /// 升级包路径
        /// </summary>
        public const string UPGRADE_PACKGE_FILE_PATH = "/u";

        /// <summary>
        /// 升级包类型
        /// </summary>
        public const string UPGRADE_PACKGE_TYPE = "/t";

        /// <summary>
        /// 解压目录
        /// </summary>
        public const string DIRECTORY = "/d";

        /// <summary>
        /// 目标程序路径
        /// </summary>
        public const string APP_EXE_FILE_PATH = "/f";
    }
}
