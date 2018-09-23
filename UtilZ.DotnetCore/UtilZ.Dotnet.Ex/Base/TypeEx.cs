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

            Type type = TypeEx._htTypes[typeFullName] as Type;
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

        /// <summary>
        /// 验证类型是否可用于创建实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceTypes">类型是否需要实现指定的接口名称,不需要验证接口参数为null</param>
        /// <param name="constructorParameterTypes">构造函数参数类型集合,需要和目标构造函数参数顺序相同,构造函数无参数参数为null</param>
        /// <returns></returns>
        public static bool ValidateCreateInstanceType(Type type, IEnumerable<Type> interfaceTypes = null, IEnumerable<Type> constructorParameterTypes = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsClass)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (ValidateInterface(type, interfaceTypes))
            {
                return ValidateConstructor(type, constructorParameterTypes);
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateConstructor(Type type, IEnumerable<Type> constructorParameterTypes)
        {
            if (constructorParameterTypes != null && constructorParameterTypes.Count() > 0)
            {
                var constructors = type.GetConstructors().Where(t => { return t.GetParameters().Length == constructorParameterTypes.Count(); });
                if (constructors.Count() == 0)
                {
                    //所有带参构造函数参数个数不匹配
                    return false;
                }

                foreach (var constructor in constructors)
                {
                    //有一个带参构造函数完全匹配
                    if (PrimitiveValidateConstructor(constructor, constructorParameterTypes))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                //只要有一个无参构造函数即可
                return (type.GetConstructors().Where(t => { return t.GetParameters().Length == 0; }).Count() > 0);
            }
        }

        private static bool PrimitiveValidateConstructor(ConstructorInfo constructorInfo, IEnumerable<Type> constructorParameterTypes)
        {
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != constructorParameterTypes.ElementAt(i))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateInterface(Type type, IEnumerable<Type> interfaceTypes)
        {
            if (interfaceTypes != null && interfaceTypes.Count() > 0)
            {
                foreach (var interfaceTypeName in interfaceTypes)
                {
                    if (interfaceTypeName == null)
                    {
                        //某个接口类型为null,忽略验证
                        continue;
                    }

                    if (type.GetInterface(interfaceTypeName.FullName) == null)
                    {
                        //必须实现指定接口
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
