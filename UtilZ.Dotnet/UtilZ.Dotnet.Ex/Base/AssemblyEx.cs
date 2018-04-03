using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// 程序集管理辅助类
    /// </summary>
    public static class AssemblyEx
    {
        /// <summary>
        /// 多线程操作锁
        /// </summary>
        private readonly static object _monitor = new object();

        /// <summary>
        /// 已加载的程序集集合,为了加快效率,避免每次都遍历[key:程序集名称;value:Assembly]
        /// </summary>
        private readonly static Dictionary<string, System.Reflection.Assembly> _loadedAssemblies = new Dictionary<string, System.Reflection.Assembly>();

        /// <summary>
        /// 是否内部加载数据库访问组件
        /// </summary>
        private static bool _enable = false;

        /// <summary>
        /// 待通查找的的程序集名称集合[key:程序集名称;value:程序集路径]
        /// </summary>
        private readonly static Dictionary<string, string> _dicPrepareLoadAssemblyNames = new Dictionary<string, string>();

        /// <summary>
        /// 获取待通过本辅助类查找的的程序集名称集合
        /// </summary>
        public static string[] PrepareLoadAssemblyNames
        {
            get
            {
                lock (_monitor)
                {
                    return _dicPrepareLoadAssemblyNames.Keys.ToArray();
                }
            }
        }

        /// <summary>
        /// 是否包含待通查找的的程序集[存在返回:true,否则返回false]
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>存在返回:true,否则返回false</returns>
        public static bool ContainsPrepareLoadAssembly(string assemblyName)
        {
            lock (_monitor)
            {
                return _dicPrepareLoadAssemblyNames.ContainsKey(assemblyName);
            }
        }

        /// <summary>
        /// 添加一个程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        public static void AddAssembly(System.Reflection.Assembly assembly)
        {
            lock (_monitor)
            {
                if (_loadedAssemblies.ContainsKey(assembly.FullName))
                {
                    return;
                }

                _loadedAssemblies.Add(assembly.FullName, assembly);
            }
        }

        /// <summary>
        /// 添加一个程序集集合
        /// </summary>
        /// <param name="assemblies">程序集集合</param>
        public static void AddAssembly(IEnumerable<System.Reflection.Assembly> assemblies)
        {
            lock (_monitor)
            {
                foreach (var assembly in assemblies)
                {
                    if (_loadedAssemblies.ContainsKey(assembly.FullName))
                    {
                        return;
                    }

                    _loadedAssemblies.Add(assembly.FullName, assembly);
                }
            }
        }

        /// <summary>
        /// 添加一个待通查找的的程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assemblyFullPath">程序集全路径</param>
        public static void AddPrepareLoadAssembly(string assemblyName, string assemblyFullPath)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => assemblyName));
            }

            if (string.IsNullOrEmpty(assemblyFullPath))
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => assemblyFullPath));
            }

            lock (_monitor)
            {
                if (_dicPrepareLoadAssemblyNames.ContainsKey(assemblyName))
                {
                    throw new ArgumentException(string.Format("已存在程序集:{0}", assemblyName));
                }

                if (!System.IO.File.Exists(assemblyFullPath))
                {
                    throw new System.IO.FileNotFoundException(string.Format("程序集:{0}不存在", assemblyFullPath));
                }

                _dicPrepareLoadAssemblyNames.Add(assemblyName, assemblyFullPath);
            }
        }

        /// <summary>
        /// 添加一个待通查找的的程序集名称集合
        /// </summary>
        /// <param name="assemblyNames">程序集名称集合</param>
        public static void AddPrepareLoadAssembly(Dictionary<string, string> assemblyNames)
        {
            lock (_monitor)
            {
                List<string> addedAssemblyNames = new List<string>();
                foreach (KeyValuePair<string, string> assemblyName in assemblyNames)
                {
                    if (_dicPrepareLoadAssemblyNames.ContainsKey(assemblyName.Key))
                    {
                        foreach (string removeAssemblyName in addedAssemblyNames)
                        {
                            _dicPrepareLoadAssemblyNames.Remove(removeAssemblyName);
                        }

                        throw new ArgumentException(string.Format("已存在程序集:{0}", assemblyName));
                    }

                    if (!System.IO.File.Exists(assemblyName.Value))
                    {
                        foreach (string removeAssemblyName in addedAssemblyNames)
                        {
                            _dicPrepareLoadAssemblyNames.Remove(removeAssemblyName);
                        }

                        throw new System.IO.FileNotFoundException(string.Format("程序集:{0}不存在", assemblyName.Value));
                    }

                    _dicPrepareLoadAssemblyNames.Add(assemblyName.Key, assemblyName.Value);
                    addedAssemblyNames.Add(assemblyName.Key);
                }
            }
        }

        /// <summary>
        /// 移除一个待通查找的的程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        public static void RemovePrepareLoadAssembly(string assemblyName)
        {
            lock (_monitor)
            {
                if (_dicPrepareLoadAssemblyNames.ContainsKey(assemblyName))
                {
                    _dicPrepareLoadAssemblyNames.Remove(assemblyName);
                }
                else
                {
                    throw new ArgumentException(string.Format("程序集:{0}不存在", assemblyName));
                }
            }
        }

        /// <summary>
        /// 获取或设置是否内部加载数据库访问组件,true:内部加载,false:外部加载
        /// </summary>
        public static bool Enable
        {
            get
            {
                lock (_monitor)
                {
                    return _enable;
                }
            }
            set
            {
                lock (_monitor)
                {
                    if (_enable == value)
                    {
                        return;
                    }

                    _enable = value;
                    if (_enable)
                    {
                        AppDomain.CurrentDomain.AssemblyResolve += LoadDBAccesAssembly;
                    }
                    else
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= LoadDBAccesAssembly;
                    }
                }
            }
        }

        /// <summary>
        /// 加载数组库访问程序集
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">需要的程序集名称信息参数</param>
        public static System.Reflection.Assembly LoadDBAccesAssembly(object sender, ResolveEventArgs args)
        {
            lock (_monitor)
            {
                //验证是否存在已查找过的集合中,如果已存在则直接取值返回
                if (_loadedAssemblies.ContainsKey(args.Name))
                {
                    return _loadedAssemblies[args.Name];
                }

                string assemblyFullPath;
                Assembly targetAssembly = FindAssembly(args.Name);
                if (targetAssembly == null)
                {
                    if (_dicPrepareLoadAssemblyNames.ContainsKey(args.Name))
                    {
                        //如果没在当前应用程序域中找到当前要查找的程序集,则通过程序集路径加载该程序集并添加到已查找过的集合中
                        assemblyFullPath = _dicPrepareLoadAssemblyNames[args.Name];
                        if (System.IO.File.Exists(assemblyFullPath))
                        {
                            targetAssembly = System.Reflection.Assembly.LoadFile(assemblyFullPath);
                        }
                        //else
                        //{
                        //    throw new System.IO.FileNotFoundException(string.Format("程序集:{0}不存在", assemblyFullPath));
                        //}
                    }
                }

                if (targetAssembly == null)
                {
                    string assemblyName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
                    //先找请求的程序集目录
                    assemblyFullPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(args.RequestingAssembly.Location), assemblyName);
                    if (!System.IO.File.Exists(assemblyName))
                    {
                        //如果请求的程序集目录中没有程序集,则在工作目录中查找
                        assemblyFullPath = System.IO.Path.Combine(Environment.CurrentDirectory, assemblyName);
                    }

                    if (System.IO.File.Exists(assemblyName))
                    {
                        targetAssembly = Assembly.LoadFile(assemblyName);
                    }
                }

                //添加到已加载的程序集字典集合中
                if (targetAssembly != null)
                {
                    _loadedAssemblies.Add(args.Name, targetAssembly);
                }

                return targetAssembly;
            }
        }

        /// <summary>
        /// 启用程序集查找功能调用无返回值委托
        /// </summary>
        /// <param name="action">委托</param>
        public static void ExcuteAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => action), "要执行的委托不能为null");
            }

            lock (_monitor)
            {
                bool resetFlag = false;
                if (!_enable)
                {
                    _enable = true;
                    resetFlag = true;
                    AppDomain.CurrentDomain.AssemblyResolve += LoadDBAccesAssembly;
                }

                try
                {
                    action();
                }
                finally
                {
                    if (resetFlag)
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= LoadDBAccesAssembly;
                        _enable = false;
                    }
                }
            }
        }

        /// <summary>
        /// 启用程序集查找功能调用带返回值委托
        /// </summary>
        /// <param name="function">委托</param>
        /// <returns>执行委托返回值</returns>
        public static T ExcuteFuction<T>(Func<T> function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => function), "要执行的委托不能为null");
            }

            lock (_monitor)
            {
                bool resetFlag = false;
                if (!_enable)
                {
                    _enable = true;
                    resetFlag = true;
                    AppDomain.CurrentDomain.AssemblyResolve += LoadDBAccesAssembly;
                }

                try
                {
                    return function();
                }
                finally
                {
                    if (resetFlag)
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= LoadDBAccesAssembly;
                        _enable = false;
                    }
                }
            }
        }

        #region 查找程序集
        /// <summary>
        /// 从当前应用程序域中查找指定名称的程序集[找到返回目标程序集,没找到返回null]
        /// </summary>
        /// <param name="assemblyFullName">程序集全名</param>
        /// <returns>找到返回目标程序集,没找到返回null</returns>
        public static Assembly FindAssembly(string assemblyFullName)
        {
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            return (from assembly in assemblys where assemblyFullName.Equals(assembly.FullName) select assembly).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// 获取程序集名称[获取失败或非.net程序集,则返回null]
        /// </summary>
        /// <param name="assemblyPath">程序集路径</param>
        /// <returns>程序集名称</returns>
        public static string GetAssemblyName(string assemblyPath)
        {
            try
            {
                return AssemblyName.GetAssemblyName(assemblyPath).FullName;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 判断dll文件是64位还是32位[true:64位;false:32位;.NET AnyCpu程序集判断为32位]
        /// </summary>
        /// <param name="dllFilePath">dll文件路径</param>
        /// <returns>[true:64位;false:32位;.NET AnyCpu程序集判断为32位]</returns>
        public static bool IsX64OrX86(string dllFilePath)
        {
            using (System.IO.FileStream fStream = new System.IO.FileStream(dllFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                System.IO.BinaryReader bReader = new System.IO.BinaryReader(fStream);
                if (bReader.ReadUInt16() != 23117) //check the MZ signature
                {
                    throw new Exception("不识别的dll");
                }

                fStream.Seek(0x3A, System.IO.SeekOrigin.Current); //seek to e_lfanew.
                fStream.Seek(bReader.ReadUInt32(), System.IO.SeekOrigin.Begin); //seek to the start of the NT header.
                if (bReader.ReadUInt32() != 17744) //check the PE\0\0 signature.
                {
                    throw new Exception("不识别的dll");
                }

                fStream.Seek(20, System.IO.SeekOrigin.Current); //seek past the file header,
                ushort architecture = bReader.ReadUInt16(); //read the magic number of the optional header.
                //523 64位    267 32位
                switch (architecture)
                {
                    case 523:
                        return true;
                    case 267:
                        return false;
                    default:
                        throw new Exception("不识别的dll类型");
                }
            }
        }

        /// <summary>
        /// 编码程序集[返回编码结果]
        /// </summary>
        /// <param name="sourceFilePaths">源码文件路径数组</param>
        /// <param name="referencedAssemblyFilePaths">所有引用程序集路径数组</param>
        /// <param name="outAssemblyFilePath">输出程序集路径</param>
        /// <returns>CompilerResults</returns>
        public static CompilerResults CompileAssembly(string[] sourceFilePaths, string[] referencedAssemblyFilePaths, string outAssemblyFilePath)
        {
            if (sourceFilePaths == null || sourceFilePaths.Length == 0)
            {
                throw new ArgumentNullException("sourceFilePaths");
            }

            string sourceFileExtension = System.IO.Path.GetExtension(sourceFilePaths[0].ToUpper(System.Globalization.CultureInfo.InvariantCulture));
            string extension;
            foreach (var sourceFilePath in sourceFilePaths)
            {
                extension = System.IO.Path.GetExtension(sourceFilePaths[0].ToUpper(System.Globalization.CultureInfo.InvariantCulture));
                if (!string.Equals(sourceFileExtension, extension))
                {
                    throw new ArgumentException("源文件扩展名必须一致");
                }
            }

            CodeDomProvider provider;
            switch (sourceFileExtension)
            {
                case ".CS":
                    provider = CodeDomProvider.CreateProvider("CSharp");
                    break;
                case ".VB":
                    provider = CodeDomProvider.CreateProvider("VisualBasic");
                    break;
                default:
                    throw new ArgumentException("源文件扩展名必须为.cs或.vb");
            }

            var cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.OutputAssembly = outAssemblyFilePath;
            cp.GenerateInMemory = false;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.AddRange(referencedAssemblyFilePaths);
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFilePaths);
            return cr;
        }

        /// <summary>
        /// 获取程序集GUID
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>程序集GUID</returns>
        public static string GetAssemblyGUID(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            return assembly.GetCustomAttributes(true).Where(t => t is GuidAttribute).Cast<GuidAttribute>().Select(t => t.Value).FirstOrDefault();
        }
    }
}
