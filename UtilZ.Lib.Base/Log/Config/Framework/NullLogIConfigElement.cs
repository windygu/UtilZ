﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config.Core;

namespace UtilZ.Lib.Base.NLog.Config.Framework
{
    /// <summary>
    /// 空日志记录器配置类
    /// </summary>
    public class NullLogIConfigElement : BaseLogConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NullLogIConfigElement()
            : base()
        {

        }
    }
}
