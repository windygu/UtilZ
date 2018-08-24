using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UtilZ.Dotnet.SEx.Log.Config
{
    /// <summary>
    /// 重定向输出日志追加器配置
    /// </summary>
    [Serializable]
    public class RedirectAppendConfig : BaseConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ele">配置元素节点</param>
        public RedirectAppendConfig(XElement ele) : base(ele)
        {
            
            throw new NotImplementedException();
        }
    }
}
