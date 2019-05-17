using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UtilZ.Dotnet.Ex.Transfer.Net;
using UtilZ.Dotnet.SHPBase.ServiceBasic.Command;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    /// <summary>
    /// 服务传输数据
    /// </summary>
    [Serializable]
    public class SHPServiceTransferData
    {
        /// <summary>
        /// 数据编码
        /// </summary>
        public int DataCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 获取数据标识[true:byte[]数据;false:文件]
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public IPEndPoint SrcEndPoint { get; set; }

        public SHPServiceTransferData()
        {

        }

        internal SHPServiceTransferData(int dataCode, byte[] data, IPEndPoint srcEndPoint)
        {
            this.DataCode = dataCode;
            this.Data = data;
            this.FilePath = null;
            this.Flag = true;
            this.SrcEndPoint = srcEndPoint;
        }

        internal SHPServiceTransferData(ReceiveDataItem receiveDataItem)
        {
            this.DataCode = 0;
            this.Data = null;
            this.FilePath = receiveDataItem.FilePath;
            this.Flag = false;
            this.SrcEndPoint = receiveDataItem.SrcEndPoint;
        }
    }
}
