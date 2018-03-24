using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace UtilZ.Dotnet.Ex.Log.Model
{
    /// <summary>
    /// 日志信息类
    /// </summary>
    [Serializable]
    public class LogItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="thread">线程</param>
        /// <param name="skipFrames">调用堆栈跳过帧数</param>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public LogItem(DateTime time, Thread thread, int skipFrames, LogLevel level, string msg, Exception ex, string name, int eventID, object extendInfo)
        {
            this.Time = time;
            this.ThreadID = thread.ManagedThreadId;
            this.ThreadName = thread.Name;
            this.EventID = eventID;
            this.Level = level;
            this.StackTrace = new StackTrace(skipFrames, true);
            this.Message = msg;
            this.Exception = ex;
            this.Name = name;
            this.ExtendInfo = extendInfo;
            this._isAnalyzeStackTrace = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="thread">线程</param>
        /// <param name="stackTraceInfo">调用堆栈信息</param>
        /// <param name="level">日志级别</param>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="name">日志记录器名称</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="extendInfo">扩展信息</param>
        public LogItem(DateTime time, Thread thread, string stackTraceInfo, LogLevel level, string msg, Exception ex, string name, int eventID, object extendInfo)
        {
            this.Time = time;
            this.ThreadID = thread.ManagedThreadId;
            this.ThreadName = thread.Name;
            this.EventID = eventID;
            this.Level = level;
            this.StackTraceInfo = stackTraceInfo;
            this.Message = msg;
            this.Exception = ex;
            this.Name = name;
            this.ExtendInfo = extendInfo;
            this._isAnalyzeStackTrace = false;
        }

        /// <summary>
        /// 获取堆栈方法参数名称类型[true:代码方式false:系统堆栈方式(eg:List`string),默认为true]
        /// </summary>
        private static bool _getStackTraceMethodParameterNameType = true;

        /// <summary>
        /// 获取或设置获取堆栈方法参数名称类型[true:代码方式false:系统堆栈方式(eg:List`string),默认为true]
        /// </summary>
        public static bool GetStackTraceMethodParameterNameType
        {
            get { return LogItem._getStackTraceMethodParameterNameType; }
            set { LogItem._getStackTraceMethodParameterNameType = value; }
        }

        /// <summary>
        /// 日志项是否需要分析堆栈信息
        /// </summary>
        private readonly bool _isAnalyzeStackTrace;

        /// <summary>
        /// 日志项是否已分析过
        /// </summary>
        private bool _isAnalyzed = false;

        /// <summary>
        /// 获取日志项是否已分析过
        /// </summary>
        public bool IsAnalyzed
        {
            get { return _isAnalyzed; }
            set { _isAnalyzed = value; }
        }

        #region 属性
        /// <summary>
        /// 主键
        /// </summary>
        public object ID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// 线程ID
        /// </summary>
        public int ThreadID { get; private set; }

        /// <summary>
        /// 线程名称
        /// </summary>
        public string ThreadName { get; private set; }

        /// <summary>
        /// 日志记录器名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 事件ID
        /// </summary>
        public int EventID { get; private set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// 调用堆栈跟踪信息
        /// </summary>
        public StackTrace StackTrace { get; private set; }

        /// <summary>
        /// 日志信息对象
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public object ExtendInfo { get; private set; }

        /// <summary>
        /// 日志产生类名称
        /// </summary>
        public string Logger { get; private set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTraceInfo { get; private set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; private set; }
        #endregion

        /// <summary>
        /// 方法参数类型与参数名之间的间隔
        /// </summary>
        private const string MethodParameterTypeParameterNameSpacing = " ";

        /// <summary>
        /// 方法参数之间的间隔
        /// </summary>
        private const string MethodParameterSpacing = ", ";

        /// <summary>
        /// 日志处理
        /// </summary>
        public void LogProcess()
        {
            if (this._isAnalyzed)
            {
                return;
            }

            if (this._isAnalyzeStackTrace)
            {
                StackFrame sf = this.StackTrace.GetFrame(0);
                string fileName = sf.GetFileName();
                int lineNo = sf.GetFileLineNumber();
                //int colNo = sf.GetFileColumnNumber();

                MethodBase methodBase = sf.GetMethod();
                this.Logger = methodBase.DeclaringType.FullName;
                string methodName = methodBase.Name;
                ParameterInfo[] parameters = methodBase.GetParameters();
                StringBuilder sbParameter = new StringBuilder();

                if (parameters.Length > 0)
                {
                    bool getStackTraceMethodParameterNameType = _getStackTraceMethodParameterNameType;
                    StringBuilder sbGenericTypeParameter = new StringBuilder();
                    string parameterTypeName;
                    foreach (ParameterInfo parameter in parameters)
                    {
                        if (getStackTraceMethodParameterNameType)
                        {
                            if (parameter.ParameterType.IsGenericType)
                            {
                                try
                                {
                                    sbGenericTypeParameter.Clear();
                                    this.AppendGenericArgumentType(sbGenericTypeParameter, parameter.ParameterType);
                                    parameterTypeName = sbGenericTypeParameter.ToString();
                                }
                                catch (Exception ex)
                                {
                                    parameterTypeName = this.GetTypeNameStr(parameter.ParameterType);
                                    LogSysInnerLog.OnRaiseLog(null, ex);
                                }
                            }
                            else
                            {
                                parameterTypeName = this.GetTypeNameStr(parameter.ParameterType);
                            }
                        }
                        else
                        {
                            parameterTypeName = parameter.ParameterType.Name;
                        }

                        sbParameter.Append(parameterTypeName);
                        sbParameter.Append(MethodParameterTypeParameterNameSpacing);
                        sbParameter.Append(parameter.Name);
                        sbParameter.Append(MethodParameterSpacing);
                    }

                    sbParameter = sbParameter.Remove(sbParameter.Length - MethodParameterSpacing.Length, MethodParameterSpacing.Length);
                }

                //在 NTest.FTestLMQ.btnTest_Click(Object sender, EventArgs e) 位置 E:\Projects\Zhanghn\UtilitiesLib\NTest\FTestLMQ.cs:行号 88
                this.StackTraceInfo = string.Format(@"   在 {0}.{1}({2}) 位置 {3}:行号 {4}",
                    this.Logger, methodName, sbParameter.ToString(), fileName, lineNo);
            }
            else
            {
                this.Content = this.Exception.Message;
            }

            //拼接日志基本信息
            string content = this.Message;
            if (this.Exception != null)
            {
                if (string.IsNullOrEmpty(content))
                {
                    content = string.Format("{0}: {1}", this.Exception.GetType().FullName, this.Exception.Message);
                }
                else
                {
                    content = string.Format("{0}。{1}: {2}", content, this.Exception.GetType().FullName, this.Exception.Message);
                }
            }

            this.Content = content;
            this._isAnalyzed = true;
        }

        /// <summary>
        /// 追加泛型类型参数类型名称
        /// </summary>
        /// <param name="sbParameter">参数StringBuilder</param>
        /// <param name="parameterType">参数类型</param>
        private void AppendGenericArgumentType(StringBuilder sbParameter, Type parameterType)
        {
            sbParameter.Append(this.GetTypeNameStr(parameterType));
            sbParameter.Append('<');
            var genericArguments = parameterType.GetGenericArguments();
            int lastItemIndex = genericArguments.Length - 1;
            for (int i = 0; i < genericArguments.Length; i++)
            {
                var argsType = genericArguments[i];
                if (argsType.IsGenericType)
                {
                    this.AppendGenericArgumentType(sbParameter, argsType);
                }
                else
                {
                    sbParameter.Append(this.GetTypeNameStr(argsType));
                }

                if (i < lastItemIndex)
                {
                    sbParameter.Append(", ");
                }
            }

            sbParameter.Append('>');
        }

        /// <summary>
        /// 获取类型名称字符串
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称字符串</returns>
        private string GetTypeNameStr(Type type)
        {
            string typeNameStr;
            try
            {
                if (type.IsPrimitive)
                {
                    typeNameStr = type.Name;
                }
                else
                {
                    if (type.IsGenericType)
                    {
                        int splitIndex = type.Name.IndexOf('`');
                        if (splitIndex < 1)
                        {
                            typeNameStr = type.FullName;
                        }
                        else
                        {
                            typeNameStr = type.Name.Substring(0, splitIndex);
                        }
                    }
                    else
                    {
                        if (TypeCode.String == Type.GetTypeCode(type))
                        {
                            typeNameStr = type.Name;
                        }
                        else
                        {
                            typeNameStr = type.FullName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(null, ex);
                typeNameStr = type.FullName;
            }

            return typeNameStr;
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            string logMsg;
            try
            {
                logMsg = this.Message;
                if (string.IsNullOrWhiteSpace(logMsg))
                {
                    if (this.Exception != null)
                    {
                        logMsg = this.Exception.ToString();
                    }
                }
                else
                {
                    if (this.Exception != null)
                    {
                        logMsg = string.Format("{0},{1}", logMsg, this.Exception.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog("LogItem", ex);
                logMsg = "日志异常";
            }

            return logMsg;
        }
    }
}
