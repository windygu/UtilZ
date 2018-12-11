using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Constant;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBIBase.DBBase.Factory
{
    /// <summary>
    /// 数据库访问工厂管理类
    /// </summary>
    public class DBFactoryManager
    {
        /// <summary>
        /// 数据库连接信息集合[key:数据库访问工厂类型(Type),value:数据库访问工厂实例(DBFactoryBase)]
        /// </summary>
        private static readonly Hashtable _htFactorys = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object _monitor = new object();

        static DBFactoryManager()
        {
            Assembly dbiAssembly = typeof(DBFactoryManager).Assembly;
            var ignorAssemblyNames = new List<string>();
            ignorAssemblyNames.Add(typeof(UtilZ.Dotnet.Ex.Base.ObjectEx).Assembly.GetName().Name);
            ignorAssemblyNames.Add(dbiAssembly.GetName().Name);

            string dbPluginRootFullDir = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "DBPlugins");
            string[] pluginDirs = Directory.GetDirectories(dbPluginRootFullDir);
            Dictionary<string, Assembly> assembliyDic = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(p => { return p.GetName().Name; });
            Type dbFactoryBaseType = typeof(DBFactoryBase);

            AppDomain.CurrentDomain.AssemblyResolve += LoadDBAccesAssembly;
            foreach (var pluginDir in pluginDirs)
            {
                LoadDBPlugin(pluginDir, ignorAssemblyNames, assembliyDic, dbFactoryBaseType);
            }
            AppDomain.CurrentDomain.AssemblyResolve -= LoadDBAccesAssembly;
        }

        private static void LoadDBPlugin(string pluginDir, List<string> ignorAssemblyNames, Dictionary<string, Assembly> assembliyDic, Type dbFactoryBaseType)
        {
            string[] dllFilePaths = Directory.GetFiles(pluginDir, "UtilZ.Dotnet.*.dll", SearchOption.TopDirectoryOnly);
            Assembly assembly;
            DBFactoryBase dbFactory;

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
                        if (!type.IsClass)
                        {
                            continue;
                        }

                        if (type.IsAbstract)
                        {
                            continue;
                        }

                        if (!type.IsSubclassOf(dbFactoryBaseType))
                        {
                            continue;
                        }

                        dbFactory = Activator.CreateInstance(type) as DBFactoryBase;
                        if (dbFactory == null)
                        {
                            continue;
                        }

                        dbFactory.AttatchEFConfig();
                        _htFactorys[type] = dbFactory;
                        return;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static Assembly LoadDBAccesAssembly(object sender, ResolveEventArgs args)
        {
            try
            {
                string dbDllDir = Path.GetDirectoryName(args.RequestingAssembly.Location);
                string assemblyName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
                string assemblyFilePath = Path.Combine(dbDllDir, assemblyName);
                return Assembly.LoadFile(assemblyFilePath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数据库访问工厂实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问工厂实例</returns>
        public static DBFactoryBase GetDBFactory(int dbid)
        {
            var dbItem = ConfigManager.GetConfigItem(dbid);
            string dbFactoryName = dbItem.DBFactory;
            Type type = TypeEx.GetType(dbFactoryName);
            if (type == null)
            {
                throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
            }

            DBFactoryBase dbFactory = _htFactorys[type] as DBFactoryBase;
            if (dbFactory == null)
            {
                throw new ApplicationException(string.Format("获取名称为{0}数据库访问工厂实例失败", dbFactoryName));
            }

            return dbFactory;
        }
    }
}
