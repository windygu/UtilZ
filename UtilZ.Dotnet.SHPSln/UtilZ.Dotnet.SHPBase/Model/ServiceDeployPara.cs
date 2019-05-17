using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Transfer.Net;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class ServiceDeployPara
    {
        /// <summary>
        /// 运控ID
        /// </summary>
        [TTLVAttribute(101)]
        public long DOId { get; set; }

        /// <summary>
        /// 服务实例Id
        /// </summary>
        [TTLVAttribute(102)]
        public long Id { get; set; }

        [TTLVAttribute(103)]
        public string ServiceInsName { get; set; }

        /// <summary>
        /// 传输协议
        /// </summary>
        [TTLVAttribute(104)]
        public TransferProtocal TransferProtocal { get; set; }

        [TTLVAttribute(105)]
        public ServiceMirrorType ServiceMirrorType { get; set; }

        /// <summary>
        /// 启动参数
        /// </summary>
        [TTLVAttribute(106)]
        public string Arguments { get; set; }

        /// <summary>
        /// 进程文件路径
        /// </summary>
        [TTLVAttribute(107)]
        public string AppProcessFilePath { get; set; }

        /// <summary>
        /// 启动文件路径
        /// </summary>
        [TTLVAttribute(108)]
        public string AppExeFilePath { get; set; }

        [TTLVAttribute(109)]
        public string BaseUrl { get; set; }

        [TTLVAttribute(110)]
        public string MirrorFilePath { get; set; }

        [TTLVAttribute(111)]
        public string FileServiceUsername { get; set; }

        [TTLVAttribute(112)]
        public string FileServicePassword { get; set; }

        /// <summary>
        /// 部署超时时长,System.Threading.Timeout.Infinite (-1)，表示无限期等待
        /// </summary>
        [TTLVAttribute(113)]
        public int MillisecondsTimeout { get; set; }

        [TTLVAttribute(114)]
        public List<int> NotAllocatePortList { get; set; }

        [TTLVAttribute(115)]
        public string RouteServiceUrl { get; set; }

        [TTLVAttribute(116)]
        public int StatusUploadIntervalMilliseconds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceDeployPara()
        {

        }
    }
}
