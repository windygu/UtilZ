using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.DBBase.Common;
using UtilZ.Dotnet.DBBase.Interfaces;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBBase.Core
{
    /// <summary>
    /// 数据库访问工厂管理类
    /// </summary>
    public class DBFactoryManager
    {
        private static readonly Type[] _dbFactoryTypes = new Type[] { typeof(IDBFactory) };

        /// <summary>
        /// 数据库连接信息集合[key:数据库访问工厂类型(Type),value:数据库访问工厂实例(IDBFactory)]
        /// </summary>
        private static readonly Hashtable _htFactorys = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object _monitor = new object();

        /// <summary>
        /// 已创建过后类型[key:dbFactoryName;value:Type]
        /// </summary>
        private static readonly Hashtable _htTypes = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 静态构造函数加载所有可用的数据库访问插件
        /// </summary>
        static DBFactoryManager()
        {
            try
            {
                Assembly dbiAssembly = typeof(DBFactoryManager).Assembly;
                var ignorAssemblyNames = new List<string>();
                ignorAssemblyNames.Add(dbiAssembly.GetName().Name);

                //string dir = Path.GetDirectoryName(dbiAssembly.Location);
                string dbpluginDir = Path.GetFullPath("DBPlugins");
                //if (!Directory.Exists(dbpluginDir))
                //{
                //    //插件目录不存在
                //    DBLog.OutLog("数据库访问插件目录不存在");
                //    return;
                //}

                string[] pluginDirs = Directory.GetDirectories(dbpluginDir);
                Dictionary<string, Assembly> assembliyDic = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(p => { return p.GetName().Name; });
                foreach (var pluginDir in pluginDirs)
                {
                    LoadDBPlugin(pluginDir, ignorAssemblyNames, assembliyDic);
                }
            }
            catch (Exception ex)
            {
                DBLog.OutLog(ex);
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //args.Name	"System.Data.SQLite, Version=1.0.109.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139"	string
            //args.RequestingAssembly.Location "F:\Project\Git\UtilZ\UtilZ.DotnetCore\TestE\bin\Debug\netcoreapp2.1\DBPlugins\SQLite\netstandard2.0\UtilZ.Dotnet.DBSQLite.dll"
            string reqAssemblyFileName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
            string reqAssemblyFileDir = Path.GetDirectoryName(args.RequestingAssembly.Location);
            string reqAssemblyFilePath = Path.Combine(reqAssemblyFileDir, reqAssemblyFileName);
            if (File.Exists(reqAssemblyFilePath))
            {
                return Assembly.LoadFile(reqAssemblyFilePath);
            }

            return null;
        }

        /// <summary>
        /// 加载所有可用的数据库访问插件
        /// </summary>
        /// <param name="pluginDir"></param>
        /// <param name="ignorAssemblyNames"></param>
        /// <param name="assembliyDic"></param>
        private static void LoadDBPlugin(string pluginDir, List<string> ignorAssemblyNames, Dictionary<string, Assembly> assembliyDic)
        {
            string[] dllFilePaths = Directory.GetFiles(pluginDir, "UtilZ.Dotnet.DB*.dll", SearchOption.AllDirectories);
            Assembly assembly;
            IDBFactory dbFactory;
            foreach (var dllFilePath in dllFilePaths)
            {
                try
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFilePath);
                    if (ignorAssemblyNames.Contains(an.Name))
                    {
                        continue;
                    }

                    if (assembliyDic.ContainsKey(an.Name))
                    {
                        assembly = assembliyDic[an.Name];
                    }
                    else
                    {
                        assembly = Assembly.LoadFile(dllFilePath);
                    }

                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        try
                        {
                            if (!TypeEx.ValidateCreateInstanceType(type, _dbFactoryTypes))
                            {
                                continue;
                            }

                            dbFactory = (IDBFactory)Activator.CreateInstance(type);
                            _htFactorys[type] = dbFactory;
                            return;
                        }
                        catch (Exception exi)
                        {
                            DBLog.OutLog(exi);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DBLog.OutLog(ex);
                }
            }
        }

        /// <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public static IDBAccess GetDBAccessByDBID(int dbid)
        {
            var dbConfig = ConfigManager.GetConfigByDBID(dbid);
            IDBFactory dbFactory = GetDBFactoryByDBFactoryName(dbConfig.DBFactory);
            return dbFactory.GetDBAccess(dbid);
        }

        /// <summary>
        /// 根据数据库访问对象创建工厂类型获取数据库访问工厂实例
        /// </summary>
        /// <param name="dbFactoryName">数据库访问对象创建工厂类型</param>
        /// <returns>数据库访问工厂实例</returns>
        private static IDBFactory GetDBFactoryByDBFactoryName(string dbFactoryName)
        {
            Type type = GetDBFactoryTypeByDBFactoryName(dbFactoryName);
            if (type == null)
            {
                throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
            }

            IDBFactory dbFactory = _htFactorys[type] as IDBFactory;
            if (dbFactory == null)
            {
                throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
            }

            return dbFactory;
        }

        private static Type GetDBFactoryTypeByDBFactoryName(string dbFactoryName)
        {
            if (string.IsNullOrWhiteSpace(dbFactoryName))
            {
                throw new ArgumentNullException(nameof(dbFactoryName));
            }

            Type type = _htTypes[dbFactoryName] as Type;
            if (type == null)
            {
                lock (_htTypes.SyncRoot)
                {
                    type = _htTypes[dbFactoryName] as Type;
                    if (type == null)
                    {
                        type = FindDBFactoryTypeByDBFactoryName(dbFactoryName);
                        _htTypes[dbFactoryName] = type;
                    }
                }
            }

            return type;
        }

        private static Type FindDBFactoryTypeByDBFactoryName(string dbFactoryName)
        {
            if (string.IsNullOrWhiteSpace(dbFactoryName))
            {
                throw new ArgumentNullException(nameof(dbFactoryName));
            }

            Type type = _htTypes[dbFactoryName] as Type;
            if (type == null)
            {
                string[] segs = dbFactoryName.Split(',');
                if (segs.Length < 2)
                {
                    throw new NotSupportedException(string.Format("不支持的格式{0}", dbFactoryName));
                }

                string assemblyFileName = segs[1].Trim();//程序集文件名称
                Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
                var dbFactoryAssemblys = (from assembly in assemblys where string.Equals(assembly.GetName().Name, assemblyFileName, StringComparison.OrdinalIgnoreCase) select assembly).ToArray();
                if (dbFactoryAssemblys.Count() == 0)
                {
                    throw new ArgumentException(string.Format("未找到定义工厂类型:{0}的程序集:{1}", dbFactoryName, assemblyFileName));
                }

                //遍历可能定义工厂类的程序集
                foreach (var dbFactoryAssembly in dbFactoryAssemblys)
                {
                    type = dbFactoryAssembly.GetType(segs[0].Trim(), false, true);
                    if (type != null && TypeEx.ValidateCreateInstanceType(type, _dbFactoryTypes))
                    {
                        //找到一个即可
                        break;
                    }
                }

                if (type == null)
                {
                    throw new ArgumentException(string.Format("未找到定义工厂类型:{0}", dbFactoryName));
                }
            }

            return type;
        }
    }
}
