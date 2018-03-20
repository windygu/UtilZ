using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.PartAsynWait.Excute;

namespace UtilZ.Lib.BaseControl.Common.PartAsynWait.Excute.WPF
{
    /// <summary>
    /// WPF异步执行基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class WPFPartAsynExcuteBase<T, TContainer, TResult> : PartAsynExcuteBase<T, TContainer, TResult> where TContainer : System.Windows.Controls.Panel
    {
        /// <summary>
        /// 异步等待控件类型
        /// </summary>
        protected readonly static Type _asynControlType;

        /// <summary>
        /// 静态构造函数初始化
        /// </summary>
        static WPFPartAsynExcuteBase()
        {
            _asynControlType = typeof(System.Windows.Controls.UserControl);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WPFPartAsynExcuteBase()
            : base()
        {

        }
    }
}
