﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Common;

namespace UtilZ.Dotnet.SHPBase.Commands.Host
{
    public class LogoutCommand : CommandBase
    {
        /// <summary>
        /// 构造函数-创建
        /// </summary>
        public LogoutCommand()
            : base(SHPCommandDefine.LOGOUT)
        {

        }
    }
}
