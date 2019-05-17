using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Exceptions;

namespace UtilZ.Dotnet.SHPBase.BaseCommandExcutors
{
    public class SHPCommandExcutorManager
    {
        private readonly Dictionary<int, ICommandExcutor> _commandExcutorDic = new Dictionary<int, ICommandExcutor>();

        public void Init(Assembly assembly, Action<ICommandExcutor> advanceSetting)
        {
            string initExcutoeManagerType = new StackFrame(1).GetMethod().DeclaringType.FullName;
            Loger.Info($"[{initExcutoeManagerType}]初始化命令执行器开始...");

            if (advanceSetting != null)
            {
                advanceSetting(SHPSyncCommandResultExcutor.Instance);
            }

            var commandExcutorType = typeof(ICommandExcutor);
            var commandExcutorAttributeType = typeof(CommandExcutorAttribute);
            var types = assembly.GetTypes().Where(t =>
            {
                return t.IsClass //类型是类
                && !t.IsAbstract //非抽象类
                && t.GetInterface(commandExcutorType.FullName) != null //继承ICommandExcutor接口
                && t.GetConstructors().Where(c => { return c.GetParameters().Length == 0; }).Count() > 0;//有无参构造函数
            }).ToArray();

            foreach (var type in types)
            {
                try
                {
                    object[] attriObjs = type.GetCustomAttributes(commandExcutorAttributeType, true);
                    if (attriObjs.Length == 0)
                    {
                        Loger.Warn($"命令执行器类型[{type.FullName}]没有指定[{commandExcutorAttributeType.FullName}]特性,忽略");
                        continue;
                    }

                    var commandExcutorAttribute = (CommandExcutorAttribute)attriObjs[0];
                    var cmd = commandExcutorAttribute.Cmd;
                    var commandExcutor = (ICommandExcutor)Activator.CreateInstance(type);
                    commandExcutor.ExcutorType = commandExcutorAttribute.ExcutorType;

                    if (advanceSetting != null)
                    {
                        advanceSetting(commandExcutor);
                    }

                    if (this._commandExcutorDic.ContainsKey(cmd))
                    {
                        Loger.Warn($"重复的命令字[]执行器,使用新类型[{type.FullName}]覆盖旧类型[{this._commandExcutorDic[cmd].GetType().FullName}]");
                        this._commandExcutorDic[cmd] = commandExcutor;
                    }
                    else
                    {
                        this._commandExcutorDic.Add(cmd, commandExcutor);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error(ex, $"类型[{type.FullName}]创建实例异常");
                }
            }

            Loger.Info($"[{initExcutoeManagerType}]初始化命令执行器完成...");
        }

        public ICommandExcutor GetExcutorByCommand(int cmd)
        {
            if (SHPSyncCommandResultExcutor.ContainsCmd(cmd))
            {
                return SHPSyncCommandResultExcutor.Instance;
            }

            if (this._commandExcutorDic.ContainsKey(cmd))
            {
                return this._commandExcutorDic[cmd];
            }
            else
            {
                throw new SHPException($"不存在命令字[{cmd}]对应的执行器");
            }
        }
    }
}
