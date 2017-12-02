using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.DataStruct.Threading
{
    /// <summary>
    /// 扩展线程接口
    /// </summary>
    public interface IThreadEx
    {
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="obj">线程启动参数</param>
        void Start(object obj = null);

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <param name="isSycn">是否同步调用停止方法,同步调用会等线程结束后才退出本方法[true:同步;false:异步]</param>
        /// <param name="synMillisecondsTimeout">同步超时时间,-1表示无限期等待,单位/毫秒</param>
        /// <param name="throwOnfirtException">指示是否立即传播异常[默认为false]</param>
        void Stop(bool isSycn = false, int synMillisecondsTimeout = -1, bool throwOnfirtException = false);

        /// <summary>
        /// 终止线程
        /// </summary>
        void Abort();

        /// <summary>
        /// 线程执行完成事件
        /// </summary>
        event EventHandler<ThreadExCompletedArgs> Completed;

        /// <summary>
        /// 获取线程当前的状态
        /// </summary>
        System.Threading.ThreadState ThreadState { get; }

        /// <summary>
        /// 获取当前托管线程的唯一标识符
        /// </summary>
        int ManagedThreadId { get; }

        /// <summary>
        /// 当前线程是否正在运行
        /// </summary>
        bool IsRuning { get; }
    }
}
