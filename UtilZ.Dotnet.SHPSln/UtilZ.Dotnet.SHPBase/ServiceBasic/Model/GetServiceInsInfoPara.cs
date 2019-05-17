using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Model
{
    [DataContract]
    public class GetServiceInsInfoPara
    {
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// 构造函数-解析
        /// </summary>
        public GetServiceInsInfoPara()
        {

        }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        /// <param name="filePath"></param>
        public GetServiceInsInfoPara(string filePath)
        {
            this.FilePath = filePath;
        }
    }
}
