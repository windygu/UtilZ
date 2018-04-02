using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute.Winform;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Interface;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Excute;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait
{
    /// <summary>
    /// Winform异步执行对象创建工厂类
    /// </summary>
    public class PartAsynExcuteFactoryWinform : PartAsynExcuteFactoryBase
    {
        /// <summary>
        /// 静态构造函数初始化默认异步等待类型
        /// </summary>
        static PartAsynExcuteFactoryWinform()
        {
            _winformPartAsynExcuteType = WinformPartAsynExcuteTypeDefine.OpacityShadeDisableTab;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PartAsynExcuteFactoryWinform() : base()
        {

        }

        /// <summary>
        /// Winfrom异步执行对象创建类型
        /// </summary>
        private static int _winformPartAsynExcuteType;

        /// <summary>
        /// 获取或设置Winfrom异步执行对象创建类型
        /// </summary>
        public static int WinformPartAsynExcuteType
        {
            get { return _winformPartAsynExcuteType; }
            set { _winformPartAsynExcuteType = value; }
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
            int type = _winformPartAsynExcuteType;
            IAsynExcute<T, TContainer, TResult> asynExcute;
            if (type == WinformPartAsynExcuteTypeDefine.ShadeDisableControl)
            {
                asynExcute = new UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute.Winform.V1.WinformPartAsynExcuteV1<T, TContainer, TResult>();
            }
            else if (type == WinformPartAsynExcuteTypeDefine.OpacityDisableTab)
            {
                asynExcute = new UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute.Winform.V2.WinformPartAsynExcute2<T, TContainer, TResult>();
            }
            else if (type == WinformPartAsynExcuteTypeDefine.OpacityShadeDisableTab)
            {
                asynExcute = new UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute.Winform.V3.WinformPartAsynExcute3<T, TContainer, TResult>();
            }
            else if (type == WinformPartAsynExcuteTypeDefine.ScreenshotImgBackgrounDisableTab)
            {
                asynExcute = new UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute.Winform.V4.WinformPartAsynExcute4<T, TContainer, TResult>();
            }
            else if (type == WinformPartAsynExcuteTypeDefine.OpacityCustomerDisableTab)
            {
                asynExcute = new UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait.Excute.Winform.V5.WinformPartAsynExcute5<T, TContainer, TResult>();
            }
            else
            {
                throw new NotSupportedException(string.Format("不支持的异步执行对象创建类型{0}", type));
            }

            return asynExcute;
        }
    }
}
