﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.Dotnet.SEx.Log.Model
{
    /// <summary>
    /// 内部日志输出事件参数
    /// </summary>
    public class InnerLogOutputArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ex">异常</param>
        public InnerLogOutputArgs(Exception ex)
        {
            this.SEx = ex;
        }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception SEx { get; private set; }
    }
}