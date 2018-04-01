using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    ///  委托扩展方法类
    /// </summary>
    public static class DeletegateEx
    {
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventHandler">事件</param>
        /// <param name="sender">事件触发者</param>
        public static void OnRaise(this EventHandler eventHandler, object sender)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(sender, new EventArgs());
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="eventHandler">事件</param>
        /// <param name="sender">事件触发者</param>
        /// <param name="e">参数</param>
        public static void OnRaise<T>(this EventHandler<T> eventHandler, object sender, T e)
            where T : EventArgs
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T">事件参数值类型</typeparam>
        /// <param name="eventHandler">事件</param>
        /// <param name="sender">事件触发者</param>
        /// <param name="para">参数值</param>
        public static void OnRaise<T>(this EventHandler<TEventArgs<T>> eventHandler, object sender, T para)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(sender, new TEventArgs<T>(para));
            }
        }

        /// <summary>
        /// 触发委托
        /// </summary>
        /// <param name="eventHandler">委托</param>
        public static void OnRaise(this Action eventHandler)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// 触发委托
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="eventHandler">委托</param>
        /// <param name="p">参数</param>
        public static void OnRaise<T>(this Action<T> eventHandler, T p)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(p);
            }
        }

        /// <summary>
        /// 触发委托
        /// </summary>
        /// <typeparam name="T1">参数1类型</typeparam>
        /// <typeparam name="T2">参数2类型</typeparam>
        /// <param name="eventHandler">委托</param>
        /// <param name="p1">参数1</param>
        /// <param name="p2">参数2</param>
        public static void OnRaise<T1, T2>(this Action<T1, T2> eventHandler, T1 p1, T2 p2)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(p1, p2);
            }
        }

        /// <summary>
        /// 触发委托
        /// </summary>
        /// <typeparam name="T">委托入参类型</typeparam>
        /// <typeparam name="TResult">委托返回值类型</typeparam>
        /// <param name="eventHandler">委托</param>
        /// <param name="p">委托入参数</param>
        /// <returns>委托返回值</returns>
        public static TResult OnRaise<T, TResult>(this Func<T, TResult> eventHandler, T p)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                return handler(p);
            }
            else
            {
                return default(TResult);
            }
        }

        /// <summary>
        /// 触发委托
        /// </summary>
        /// <typeparam name="T1">参数1类型</typeparam>
        /// <typeparam name="T2">参数2类型</typeparam>
        /// <typeparam name="TResult">委托返回值类型</typeparam>
        /// <param name="eventHandler">委托</param>
        /// <param name="p1">参数1</param>
        /// <param name="p2">参数2</param>
        /// <returns>委托返回值</returns>
        public static TResult OnRaise<T1, T2, TResult>(this Func<T1, T2, TResult> eventHandler, T1 p1, T2 p2)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                return handler(p1, p2);
            }
            else
            {
                return default(TResult);
            }
        }
    }
}
