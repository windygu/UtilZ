using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public enum ScriptType : byte
    {
        /// <summary>
        /// 执行批处理
        /// </summary>
        [DisplayNameEx("批处理")]
        Bat,

        /// <summary>
        /// 执行python脚本
        /// </summary>
        [DisplayNameEx("Python")]
        Python,

        /// <summary>
        /// 执行js脚本
        /// </summary>
        [DisplayNameEx("Javascript")]
        Javascript,

        /// <summary>
        /// 执行ruby脚本
        /// </summary>
        [DisplayNameEx("Ruby")]
        Ruby
    }
}
