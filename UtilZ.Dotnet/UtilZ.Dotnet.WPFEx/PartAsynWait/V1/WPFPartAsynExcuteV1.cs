using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DeskBase.PartAsynWait.Excute;
using UtilZ.Dotnet.DeskBase.PartAsynWait.Model;

namespace UtilZ.Dotnet.WPFEx.BaseControl.Common.PartAsynWait.Excute.WPF.V1
{
    /// <summary>
    /// WPF异步执行类
    /// </summary>
    /// <typeparam name="T">异步执行参数类型</typeparam>
    /// <typeparam name="TContainer">容器控件类型</typeparam>
    /// <typeparam name="TResult">异步执行返回值类型</typeparam>
    internal class WPFPartAsynExcuteV1<T, TContainer, TResult> : PartAsynExcuteBase<T, TContainer, TResult> where TContainer : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WPFPartAsynExcuteV1()
            : base()
        {

        }

        /// <summary>
        /// 执行异步委托
        /// </summary>
        /// <param name="asynWaitPara">异步等待执行参数</param>
        /// <param name="containerControl">容器控件</param>
        public override void Excute(PartAsynWaitPara<T, TResult> asynWaitPara, TContainer containerControl)
        {

        }
    }
}
