using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.Base.Foundation
{
    /// <summary>
    /// 通用扩展类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// IPV4正则表达式
        /// </summary>
        public readonly static string IPV4Reg = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        /// <summary>
        /// 日期格式字符串
        /// </summary>
        public readonly static string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss";

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
        public static void OnRaise<T>(this EventHandler<EventTArgs<T>> eventHandler, object sender, T para)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(sender, new EventTArgs<T>(para));
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

        /// <summary>
        /// 字符串参数验证
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="name">参数名</param>
        /// <param name="allowWhiteSpace">是否允许空白字符[true:允许;false:不允许]</param>
        /// <param name="allowEmpty">是否允许为空字符串[true:允许;false:不允许]</param>
        public static void ParaValidateNull(this string para, string name, bool allowWhiteSpace = false, bool allowEmpty = false)
        {
            if (allowWhiteSpace)
            {
                if (allowEmpty)
                {
                    if (para == null)
                    {
                        throw new ArgumentNullException(name);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(para))
                    {
                        throw new ArgumentNullException(name);
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(para))
                {
                    throw new ArgumentNullException(name);
                }
            }
        }

        /// <summary>
        /// 参数验证
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="para">参数</param>
        /// <param name="name">参数名</param>
        public static void ParaValidateNull<T>(this T para, string name) where T : class
        {
            if (para == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// ConcurrentDictionary字典添加项[返回添加结果]
        /// </summary>
        /// <typeparam name="T">key类型</typeparam>
        /// <typeparam name="W">value类型</typeparam>
        /// <param name="dic">ConcurrentDictionary字典</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="repeatCount">添加失败重试次数</param>
        /// <returns>返回添加结果</returns>
        public static bool Add<T, W>(this ConcurrentDictionary<T, W> dic, T key, W value, int repeatCount = 0)
        {
            int currentRepeatCount = 0;
            bool ret = dic.TryAdd(key, value);
            while (!ret && currentRepeatCount++ < repeatCount)
            {
                System.Threading.Thread.Sleep(10);
                ret = dic.TryAdd(key, value);
            }

            return ret;
        }

        /// <summary>
        /// 获取永久时间片
        /// </summary>
        /// <returns>永久时间片</returns>
        public static TimeSpan GetForeverTimeSpan()
        {
            //将1000年当作为永久时间
            return new TimeSpan(365 * 1000, 0, 0, 0, 0);
        }

        /// <summary>
        /// 设置线程是否为后台线程
        /// </summary>
        /// <param name="thread">要设置的线程</param>
        /// <param name="isBackground">true:后台线程;false:前台线程</param>
        public static void SetThreadIsBackground(Thread thread, bool isBackground)
        {
            try
            {
                if (thread == null)
                {
                    return;
                }

                thread.IsBackground = isBackground;
            }
            catch
            { }
        }
    }
}
