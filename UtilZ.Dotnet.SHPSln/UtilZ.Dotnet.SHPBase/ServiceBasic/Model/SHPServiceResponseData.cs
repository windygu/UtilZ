using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [Serializable]
    public class SHPServiceResponseData
    {
        public int DataCode { get; set; }

        public byte[] Data { get; set; }

        public SHPServiceResponseData()
        {

        }
    }
}
