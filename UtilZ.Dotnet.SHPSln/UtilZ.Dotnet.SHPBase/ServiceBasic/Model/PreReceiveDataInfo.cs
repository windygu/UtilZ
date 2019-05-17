using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    /// <summary>
    /// 预接收数据信息
    /// </summary>
    public class PreReceiveDataInfo
    {
        /// <summary>
        /// 路由数据编码
        /// </summary>
        public int DataCode { get; private set; }

        /// <summary>
        /// 数据大小
        /// </summary>
        public long Size { get; private set; }

        internal PreReceiveDataInfo(int DataCode, long size)
        {
            this.DataCode = DataCode;
            this.Size = size;
        }
    }
}
