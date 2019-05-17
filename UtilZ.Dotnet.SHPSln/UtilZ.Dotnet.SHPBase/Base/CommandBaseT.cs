using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 泛型命令基类
    /// </summary>
    [Serializable]
    public class CommandBaseT<T> : CommandBase
    {
        [TTLVAttribute(101)]
        public T Value { get; set; }

        public CommandBaseT()
        {

        }

        public CommandBaseT(Int32 cmd, T value)
        {
            this.Cmd = cmd;
            this.Value = value;
        }
    }
}
