using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Plugin.Base
{
    /// <summary>
    /// SHP插件特性
    /// </summary>
    public class SHPPluginAttribute : Attribute
    {
        /// <summary>
        /// 插件ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Des { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">插件ID</param>
        /// <param name="name">插件名称</param>
        /// <param name="des">插件描述</param>
        public SHPPluginAttribute(int id, string name, string des = null)
        {
            this.Id = id;
            this.Name = name;
            this.Des = des;
        }
    }
}
