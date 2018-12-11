using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    ///  Type类型扩展方法类
    /// </summary>
    public static class TypeEx
    {
        /// <summary>
        /// 已创建过后类型[key:类型名称;value:Type]
        /// </summary>
        private static readonly Hashtable _htTypes = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="typeFullName">类型名称[格式:类型名,程序集命名.例如:Oracle.ManagedDataAccess.Client.OracleConnection,Oracle.ManagedDataAccess, Version=4.121.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342]</param>
        public static Type GetType(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return null;
            }

            Type type = _htTypes[typeFullName] as Type;
            if (type == null)
            {
                string[] segs = typeFullName.Split(',');
                if (segs.Length < 2)
                {
                    throw new NotSupportedException(string.Format("不支持的格式{0}", typeFullName));
                }

                string assemblyFileName = segs[1].Trim();//程序集文件名称
                string assemblyPath;
                if (string.IsNullOrEmpty(Path.GetPathRoot(assemblyFileName)))
                {
                    //相对工作目录的路径
                    assemblyPath = Path.Combine(DirectoryInfoEx.CurrentAssemblyDirectory, assemblyFileName);
                }
                else
                {
                    //全路径
                    assemblyPath = assemblyFileName;
                }

                if (!File.Exists(assemblyPath))
                {
                    string srcExtension = Path.GetExtension(assemblyPath).ToLower();
                    List<string> extensions = new List<string> { ".dll", ".exe" };
                    if (extensions.Contains(srcExtension))
                    {
                        return null;
                    }

                    bool isFind = false;
                    string tmpAssemblyPath;
                    foreach (var extension in extensions)
                    {
                        tmpAssemblyPath = assemblyPath + extension;
                        if (File.Exists(tmpAssemblyPath))
                        {
                            assemblyPath = tmpAssemblyPath;
                            isFind = true;
                            break;
                        }
                    }

                    if (!isFind)
                    {
                        return null;
                    }
                }

                string assemblyName = AssemblyEx.GetAssemblyName(assemblyPath);
                Assembly assembly = AssemblyEx.FindAssembly(assemblyName);
                if (assembly == null)
                {
                    assembly = Assembly.LoadFile(assemblyPath);
                }

                type = assembly.GetType(segs[0].Trim(), false, true);
                TypeEx._htTypes[typeFullName] = type;
            }

            return type;
        }

        /// <summary>
        /// 确定当前的类型是继承自指定的接口[true:继承自接口;false:未继承自接口]
        /// </summary>
        /// <param name="type">当前的类型</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns>true:继承自接口;false:未继承自接口</returns>
        public static bool IsSubInterfaceOf(this Type type, Type interfaceType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }

            return type.GetInterface(interfaceType.FullName) != null;
        }
    }
}
