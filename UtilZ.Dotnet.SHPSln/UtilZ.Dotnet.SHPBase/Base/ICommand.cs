using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令字
        /// </summary>
        Int32 Cmd { get; }

        /// <summary>
        /// 生成命令数据
        /// </summary>
        /// <returns>命令数据</returns>
        byte[] GenerateCommandData();

        /// <summary>
        /// 解析命令数据
        /// </summary>
        /// <param name="transferCommand">传输命令</param>
        void Parse(SHPTransferCommand transferCommand);
    }
}
