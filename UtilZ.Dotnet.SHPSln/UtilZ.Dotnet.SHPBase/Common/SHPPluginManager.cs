using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Net;
using UtilZ.Dotnet.SHPBase.Plugin;
using UtilZ.Dotnet.SHPBase.Plugin.Base;

namespace UtilZ.Dotnet.SHPBase.Common
{
    public class SHPPluginManager<TD, TH>
        where TD : ISHPDevOpsBase
        where TH : ISHPHardBase
    {
        private readonly Dictionary<int, PluginInfo<TD>> _devOpsPluginDic = new Dictionary<int, PluginInfo<TD>>();

        private readonly Dictionary<int, PluginInfo<TH>> _hardPluginDic = new Dictionary<int, PluginInfo<TH>>();

        public SHPPluginManager()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadDBAccesAssembly;
        }


        private Assembly LoadDBAccesAssembly(object sender, ResolveEventArgs args)
        {
            if (args.RequestingAssembly == null)
            {
                return null;
            }

            string assemblyFileName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
            string assemblyFilePath = Path.Combine(Path.GetDirectoryName(args.RequestingAssembly.Location), assemblyFileName);
            return Assembly.LoadFile(assemblyFilePath);
        }

        public void LoadPlugin(string pluginDir, ISHPNet net)
        {
            Type tdInterfaceType = typeof(TD);
            Type thInterfaceType = typeof(TH);
            Type pluginAttributeType = typeof(SHPPluginAttribute);

            //加载外部命令
            string pluginFullDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pluginDir);
            if (!Directory.Exists(pluginFullDir))
            {
                Directory.CreateDirectory(pluginFullDir);
                return;
            }

            string[] dllFilePaths = Directory.GetFiles(pluginFullDir, "*.dll");
            string[] exeFilePaths = Directory.GetFiles(pluginFullDir, "*.exe");
            this.LoadPluginAssembly(net, dllFilePaths, tdInterfaceType, thInterfaceType, pluginAttributeType);
            this.LoadPluginAssembly(net, exeFilePaths, tdInterfaceType, thInterfaceType, pluginAttributeType);
        }

        private void LoadPluginAssembly(ISHPNet net, string[] assemblyFilePaths, Type tdInterfaceType, Type thInterfaceType, Type pluginAttributeType)
        {
            if (assemblyFilePaths == null || assemblyFilePaths.Length == 0)
            {
                return;
            }

            foreach (var assemblyFilePath in assemblyFilePaths)
            {
                try
                {
                    var assembly = AssemblyEx.FindAssembly(AssemblyEx.GetAssemblyName(assemblyFilePath));
                    if (assembly == null)
                    {
                        assembly = Assembly.LoadFile(assemblyFilePath);
                    }

                    Type[] types = assembly.GetExportedTypes();
                    this.PrimitiveLoadPlugin(net, types, tdInterfaceType, thInterfaceType, pluginAttributeType);
                }
                catch (BadImageFormatException)
                {
                    //不是有效的.net程序集,忽略
                    continue;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        private void PrimitiveLoadPlugin(ISHPNet net, Type[] types, Type tdInterfaceType, Type thInterfaceType, Type pluginAttributeType)
        {
            foreach (var type in types)
            {
                try
                {
                    if (!type.IsClass ||
                        type.IsAbstract ||
                        type.GetConstructors().Where(t => { return t.GetParameters().Length == 0; }).Count() == 0)
                    {
                        continue;
                    }

                    object[] attriObjs = type.GetCustomAttributes(pluginAttributeType, true);
                    if (attriObjs == null || attriObjs.Length == 0)
                    {
                        continue;
                    }

                    var pluginAttribute = (SHPPluginAttribute)attriObjs[0];

                    if (type.GetInterface(tdInterfaceType.FullName) != null)
                    {
                        //DevOps插件
                        var devOpsPluginObj = (TD)Activator.CreateInstance(type);
                        devOpsPluginObj.Load();
                        devOpsPluginObj.Net = net;
                        devOpsPluginObj.Loaded();
                        if (this._devOpsPluginDic.ContainsKey(pluginAttribute.Id))
                        {
                            Loger.Warn($"重复ID[{pluginAttribute.Id}]插件,类型[{type.FullName}],用新项覆盖已加载项");
                        }

                        this._devOpsPluginDic[pluginAttribute.Id] = new PluginInfo<TD>(devOpsPluginObj, pluginAttribute);
                        Loger.Info($"插件[{pluginAttribute.Id}]加载成功,类型[{type.FullName}]");
                    }

                    if (type.GetInterface(thInterfaceType.FullName) != null)
                    {
                        //Hard插件
                        var hardPluginObj = (TH)Activator.CreateInstance(type);
                        hardPluginObj.Load();
                        if (hardPluginObj.Enable)
                        {
                            hardPluginObj.Loaded();
                            if (this._hardPluginDic.ContainsKey(pluginAttribute.Id))
                            {
                                Loger.Warn($"重复ID[{pluginAttribute.Id}]插件,类型[{type.FullName}],用新项覆盖已加载项");
                            }

                            this._hardPluginDic[pluginAttribute.Id] = new PluginInfo<TH>(hardPluginObj, pluginAttribute);
                            Loger.Info($"插件[{pluginAttribute.Id}]加载成功,类型[{type.FullName}]");
                        }
                        else
                        {
                            Loger.Info($"插件[{pluginAttribute.Id}]不可用,忽略...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            }
        }

        public Dictionary<int, PluginInfo<TH>>.ValueCollection GetTHPlugins()
        {
            return this._hardPluginDic.Values;
        }

        public PluginInfo<TH> GetTHPluginById(Int32 id)
        {
            if (this._hardPluginDic.ContainsKey(id))
            {
                return this._hardPluginDic[id];
            }
            else
            {
                return null;
            }
        }

        public Dictionary<int, PluginInfo<TD>>.ValueCollection GetTDPlugins()
        {
            return this._devOpsPluginDic.Values;
        }

        public PluginInfo<TD> GetTDPluginById(Int32 id)
        {
            if (this._devOpsPluginDic.ContainsKey(id))
            {
                return this._devOpsPluginDic[id];
            }
            else
            {
                return null;
            }
        }
    }
}
