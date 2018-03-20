using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UtilZ.Lib.Base.Ex
{
    /// <summary>
    ///  Activator类型扩展方法类
    /// </summary>
    public static class ActivatorEx
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="typeFullName">类型名称[格式:类型名,程序集命名.例如:Oracle.ManagedDataAccess.Client.OracleConnection,Oracle.ManagedDataAccess, Version=4.121.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342]</param>
        /// <param name="args">构造函数参数</param>
        public static object CreateInstance(string typeFullName, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return null;
            }

            Type type = TypeEx.GetType(typeFullName);
            if (type == null)
            {
                return null;
            }

            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="typeFullName">类型名称[格式:类型名,程序集命名.例如:Oracle.ManagedDataAccess.Client.OracleConnection,Oracle.ManagedDataAccess, Version=4.121.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342]</param>
        /// <param name="appDomain">应用程序域,默认null为当前域</param>
        private static object CreateInstance_bk(string typeFullName, AppDomain appDomain = null)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                return null;
            }

            if (appDomain == null)
            {
                appDomain = AppDomain.CurrentDomain;
            }

            string[] segs = typeFullName.Split(',');
            if (segs.Length != 5)
            {
                throw new NotSupportedException(string.Format("不支持的格式{0}", typeFullName));
            }

            int index = typeFullName.IndexOf(',');
            if (index == -1 || typeFullName.Length == index + 1)
            {
                return null;
            }

            string assemblyName = string.Join(",", segs, 1, segs.Length - 1);
            System.Runtime.Remoting.ObjectHandle objHandle = Activator.CreateInstance(appDomain, assemblyName, segs[0]);
            return objHandle.Unwrap();
        }
    }
}
