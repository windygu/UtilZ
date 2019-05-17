using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class ExtendInfo
    {
        /// <summary>
        /// 插件Id
        /// </summary>
        public int Id { get; set; }

        public string Info { get; set; }

        public ExtendInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">插件Id</param>
        /// <param name="info">信息</param>
        public ExtendInfo(int id, string info)
        {
            this.Id = id;
            this.Info = info;
        }
    }
}
