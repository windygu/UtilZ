using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.BaseCommandExcutors
{
    /// <summary>
    /// 命令执行器特性
    /// </summary>
    public class CommandExcutorAttribute : Attribute
    {
        /// <summary>
        /// 命令
        /// </summary>
        public int Cmd { get; private set; }

        public CommandExcutorType ExcutorType { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">命令</param>
        public CommandExcutorAttribute(int cmd, CommandExcutorType excutorType)
        {
            this.Cmd = cmd;
            this.ExcutorType = excutorType;
        }
    }
}
