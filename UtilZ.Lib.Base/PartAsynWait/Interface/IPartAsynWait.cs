using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.PartAsynWait.Interface
{
    /// <summary>
    /// 异步等待UI接口
    /// </summary>
    public interface IPartAsynWait
    {
        /// <summary>
        /// 获取是否已经取消
        /// </summary>
        bool IsCanceled { get; }

        /// <summary>
        /// 获取或设置提示标题
        /// </summary>
        string Caption { get; set; }

        /// <summary>
        /// 获取或设置提示内容
        /// </summary>
        string Hint { get; set; }

        /// <summary>
        /// 是否显示取消按钮
        /// </summary>
        bool IsShowCancel { get; set; }

        /// <summary>
        /// 获取或设置异步等待框背景色
        /// </summary>
        System.Drawing.Color AsynWaitBackground { get; set; }

        /// <summary>
        /// 取消操作事件
        /// </summary>
        event EventHandler Canceled;

        /// <summary>
        /// 取消操作
        /// </summary>
        void Cancel();

        /// <summary>
        /// 开始等待动画
        /// </summary>
        void StartAnimation();

        /// <summary>
        /// 停止等待动画
        /// </summary>
        void StopAnimation();

        /// <summary>
        /// 设置信息(保留接口),比如用来设置其它的什么进度条之类的
        /// </summary>
        /// <param name="para">参数</param>
        void SetInfo(object para);

        /// <summary>
        /// 重置异步等待框
        /// </summary>
        void Reset();
    }
}
