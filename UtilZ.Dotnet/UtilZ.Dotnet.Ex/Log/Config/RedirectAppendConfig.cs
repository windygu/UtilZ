using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UtilZ.Dotnet.Ex.Log.Config
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
        public RedirectAppendConfig() : base()
        {

        }

        /// <summary>
        /// 解析配置
        /// </summary>
        /// <param name="ele"></param>
        public override void Parse(XElement ele)
        {
            if (ele == null)
            {
                return;
            }

            base.Parse(ele);
        }
    }
}
