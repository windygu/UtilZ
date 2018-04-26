using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Log.Config.Interface;
using UtilZ.Dotnet.Ex.Log.Model;

namespace UtilZ.Dotnet.Ex.Log.Layout
{
    /// <summary>
    /// 布局管理器
    /// </summary>
    public class LayoutManager
    {
        /// <summary>
        /// 静态构造函数,生成默认布局
        /// </summary>
        static LayoutManager()
        {
            //_defaultLayout = string.Format("时间:{0}\r\n级别:{1}\r\n线程:{2}\r\n事件ID:{3}\r\n日志:{4}\r\n堆栈:{5}", _TIME, _LEVEL, _THREAD, _EVENT, _CONTENT, _STACKTRACE);
            _defaultLayout = string.Format("{0} {1} {2}。堆栈:{3}", _TIME, _LEVEL, _CONTENT, _STACKTRACE);
        }

        #region 日志布局字段
        /// <summary>
        /// 时间
        /// </summary>
        private readonly static string _TIME = "%d";

        /// <summary>
        /// 时间
        /// </summary>
        public static string TIME { get { return _TIME; } }

        /// <summary>
        /// 日志级别
        /// </summary>
        private readonly static string _LEVEL = "%l";

        /// <summary>
        /// 日志级别
        /// </summary>
        public static string LEVEL { get { return _LEVEL; } }

        /// <summary>
        /// 事件ID
        /// </summary>
        private readonly static string _EVENT = "%i";

        /// <summary>
        /// 事件ID
        /// </summary>
        public static string EVENT { get { return _EVENT; } }

        /// <summary>
        /// 线程ID
        /// </summary>
        private readonly static string _THREAD = "%t";

        /// <summary>
        /// 线程ID
        /// </summary>
        public static string THREAD { get { return _THREAD; } }

        /// <summary>
        /// 内容
        /// </summary>
        private readonly static string _CONTENT = "%c";

        /// <summary>
        /// 内容
        /// </summary>
        public static string CONTENT { get { return _CONTENT; } }

        /// <summary>
        /// 堆栈
        /// </summary>
        private readonly static string _STACKTRACE = "%s";

        /// <summary>
        /// 堆栈
        /// </summary>
        public static string STACKTRACE { get { return _STACKTRACE; } }

        /// <summary>
        /// 扩展信息
        /// </summary>
        private readonly static string _EXTENTION = "%e";

        /// <summary>
        /// 扩展信息
        /// </summary>
        public static string EXTENTION { get { return _EXTENTION; } }
        #endregion

        /// <summary>
        /// 默认布局
        /// </summary>
        private readonly static string _defaultLayout = null;

        /// <summary>
        /// 布局一条日志文本记录
        /// </summary>
        /// <param name="item">日志信息对象</param>
        /// <param name="config">日志配置</param>
        /// <returns>日志文本记录</returns>
        public static string LayoutLog(LogItem item, IConfig config)
        {
            string logMsg = string.Empty;
            try
            {
                string layoutFormat = config.Layout;
                if (string.IsNullOrWhiteSpace(layoutFormat))
                {
                    //如果日志布局格式为空则采用默认日志布局
                    layoutFormat = _defaultLayout;
                }

                //是否显示分隔线
                if (config.SeparatorCount > 1)
                {
                    int separatorCount = config.SeparatorCount;
                    if (separatorCount <= 0)
                    {
                        separatorCount = 140;
                    }

                    layoutFormat = string.Format("{0}\r\n{1}", config.SeparatorLine, layoutFormat);
                }

                List<object> args = new List<object>();
                int index = 0;
                string tmp;
                //时间
                if (layoutFormat.Contains(_TIME))
                {
                    layoutFormat = layoutFormat.Replace(_TIME, string.Format("{{{0}}}", index++));
                    if (string.IsNullOrWhiteSpace(config.DateFormat))
                    {
                        tmp = item.Time.ToString(LogConstant.DateTimeFormat);
                    }
                    else
                    {
                        try
                        {
                            tmp = item.Time.ToString(config.DateFormat);
                        }
                        catch
                        {
                            tmp = item.Time.ToString(LogConstant.DateTimeFormat);
                        }
                    }

                    args.Add(tmp);
                }

                //日志级别
                if (layoutFormat.Contains(_LEVEL))
                {
                    layoutFormat = layoutFormat.Replace(_LEVEL, string.Format("{{{0}}}", index++));
                    args.Add(LogConstant.GetLogLevelName(item.Level));
                }

                //事件ID
                if (layoutFormat.Contains(_EVENT))
                {
                    layoutFormat = layoutFormat.Replace(_EVENT, string.Format("{{{0}}}", index++));
                    args.Add(item.EventID);
                }

                //线程
                if (layoutFormat.Contains(_THREAD))
                {
                    layoutFormat = layoutFormat.Replace(_THREAD, string.Format("{{{0}}}", index++));
                    if (string.IsNullOrWhiteSpace(item.ThreadName))
                    {
                        tmp = item.ThreadID.ToString();
                    }
                    else
                    {
                        tmp = item.ThreadName;
                    }

                    args.Add(tmp);
                }

                //内容
                if (layoutFormat.Contains(_CONTENT))
                {
                    layoutFormat = layoutFormat.Replace(_CONTENT, string.Format("{{{0}}}", index++));
                    args.Add(item.Content);
                }

                //扩展信息
                if (layoutFormat.Contains(_EXTENTION))
                {
                    layoutFormat = layoutFormat.Replace(_EXTENTION, string.Format("{{{0}}}", index++));
                    if (item.ExtendInfo != null)
                    {
                        tmp = item.ExtendInfo.ToString();
                    }
                    else
                    {
                        tmp = string.Empty;
                    }

                    args.Add(tmp);
                }

                //堆栈位置信息
                if (layoutFormat.Contains(_STACKTRACE))
                {
                    layoutFormat = layoutFormat.Replace(_STACKTRACE, string.Format("{{{0}}}", index++));
                    args.Add(item.StackTraceInfo);
                }

                //生成日志
                if (args.Count > 0)
                {
                    logMsg = string.Format(layoutFormat, args.ToArray());
                }
                else
                {
                    logMsg = item.Message;
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("LayoutManager", ex);
                logMsg = item.ToString();
            }

            return logMsg;
        }
    }
}
