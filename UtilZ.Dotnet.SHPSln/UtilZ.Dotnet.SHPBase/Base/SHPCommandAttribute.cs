using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Base
{
    public class SHPCommandAttribute : Attribute
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 命令描述
        /// </summary>
        public string Des { get; private set; }

        public SHPCommandAttribute(string name, string des = null)
        {
            this.Name = name;
            this.Des = des;
        }
    }
}