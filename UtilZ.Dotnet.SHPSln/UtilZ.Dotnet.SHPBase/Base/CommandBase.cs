using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;
using UtilZ.Dotnet.Ex.Base.TTLV;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 命令基类[TTLV_Tag范围(0-100,子类使用此范围以外的值)]
    /// </summary>
    [Serializable]
    public abstract class CommandBase : ICommand
    {
        public CommandBase()
        {

        }

        public CommandBase(Int32 cmd)
        {
            this.Cmd = cmd;
        }

        /// <summary>
        /// 命令字
        /// </summary>
        [TTLVAttribute(1)]
        public Int32 Cmd { get; protected set; }

        /// <summary>
        /// 生成命令数据
        /// </summary>
        /// <returns>命令数据</returns>
        public byte[] GenerateCommandData()
        {
            return TTLVHelper.Encoding(this);
        }

        /// <summary>
        /// 解析命令数据
        /// </summary>
        /// <param name="transferCommand">传输命令</param>
        public void Parse(SHPTransferCommand transferCommand)
        {
            TTLVHelper.Decoding(this, transferCommand.Data);
        }
    }
}
