using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class ServiceRouteRemoveResPara
    {
        /// <summary>
        /// 服务实例id列表
        /// </summary>
        [TTLVAttribute(101)]
        public List<long> IdList { get; set; }

        [TTLVAttribute(102)]
        public bool Result { get; set; }

        [TTLVAttribute(103)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ServiceRouteRemoveResPara(List<long> idList)
        {
            this.IdList = idList;
            this.Result = true;
        }

        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public ServiceRouteRemoveResPara(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.Result = false;
        }

        /// <summary>
        /// 构造函数-序列化或解析
        /// </summary>
        public ServiceRouteRemoveResPara()
        {

        }
    }
}
