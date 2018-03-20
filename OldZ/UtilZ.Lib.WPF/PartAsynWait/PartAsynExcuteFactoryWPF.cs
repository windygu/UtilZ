using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.PartAsynWait.Excute;
using UtilZ.Lib.Base.PartAsynWait.Interface;

namespace UtilZ.Lib.WPF.PartAsynWait
{
    /// <summary>
    /// WPF异步执行对象创建工厂类
    /// </summary>
    public class PartAsynExcuteFactoryWPF : PartAsynExcuteFactoryBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PartAsynExcuteFactoryWPF() : base()
        {

        }

        /// <summary>
        /// WPF异步执行对象创建类型
        /// </summary>
        private static int _WPFPartAsynExcuteType;

        /// <summary>
        /// 获取或设置WPF异步执行对象创建类型
        /// </summary>
        public static int WPFPartAsynExcuteType
        {
            get { return _WPFPartAsynExcuteType; }
            set { _WPFPartAsynExcuteType = value; }
        }

        /// <summary>
        /// 创建异步执行对象
        /// </summary>
        /// <typeparam name="T">异步执行参数类型</typeparam>
        /// <typeparam name="TContainer">容器控件类型</typeparam>
        /// <typeparam name="TResult">异步执行返回值类型</typeparam>
        /// <returns>异步执行对象</returns>
        public override IAsynExcute<T, TContainer, TResult> CreateExcute<T, TContainer, TResult>()
        {
            throw new NotImplementedException();
        }
    }
}
