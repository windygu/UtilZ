using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
    public enum SHPCommandExcuteResult : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Sucess = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 2,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 3,

        /// <summary>
        /// 执行超时
        /// </summary>
        Timeout = 4,

        /// <summary>
        /// 不存在对应的插件
        /// </summary>
        NotExistPlugin = 5,

        /// <summary>
        /// 不存在对应的命令
        /// </summary>
        NotExistCommand = 6
    }
}
