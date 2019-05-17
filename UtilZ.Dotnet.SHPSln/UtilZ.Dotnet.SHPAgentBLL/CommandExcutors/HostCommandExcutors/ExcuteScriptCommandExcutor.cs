using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPAgentModel;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Commands.Host;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors.HostCommandExcutors
{
    [CommandExcutorAttribute(SHPCommandDefine.EXCUTE_SCRPT_REQ, CommandExcutorType.Asyn)]
    public class ExcuteScriptCommandExcutor : AgentCommandExcutorBase<ExcuteScriptCommand>
    {
        public ExcuteScriptCommandExcutor()
            : base()
        {

        }

        protected override void ProAsynCommand(SHPTransferCommand transferCommand, ExcuteScriptCommand cmd)
        {
            string cmdName = base.GetCommandName(transferCommand.Cmd);
            SHPCommandExcuteResult result;
            string info;
            try
            {
                switch (cmd.ScriptType)
                {
                    case ScriptType.Bat:
                        info = this.ExcuteBat(transferCommand.Timeout, cmd);
                        break;
                    case ScriptType.Python:
                        info = this.ExcutePython(transferCommand.Timeout, cmd);
                        break;
                    case ScriptType.Javascript:
                        info = this.ExcuteJavaScript(transferCommand.Timeout, cmd);
                        break;
                    case ScriptType.Ruby:
                        info = this.ExcuteRuby(transferCommand.Timeout, cmd);
                        break;
                    default:
                        throw new NotSupportedException("不支持的脚本");
                }

                result = SHPCommandExcuteResult.Sucess;
                Loger.Info($"执行{cmdName}命令完成,{info}");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                info = ex.Message;
                result = SHPCommandExcuteResult.Exception;
            }

            var resTransferCommand = new SHPTransferCommand(transferCommand, SHPCommandDefine.EXCUTE_SCRPT_RES, result, SHPResult.GetBytes(info));
            base.SendResponse(resTransferCommand, transferCommand.SrcEndPoint);
            Loger.Info($"响应{cmdName}命令结果完成...");
        }

        private string ExcuteRuby(int timeout, ExcuteScriptCommand excuteScriptCommand)
        {
            //思路:使用IronRuby库
            throw new NotImplementedException();
        }

        private string ExcuteJavaScript(int timeout, ExcuteScriptCommand excuteScriptCommand)
        {
            object obj = JavascriptEx.Eval(excuteScriptCommand.Content);
            return obj.ToString();
        }

        private string ExcutePython(int timeout, ExcuteScriptCommand excuteScriptCommand)
        {
            //思路:使用IronPython库
            throw new NotImplementedException();
        }

        private string ExcuteBat(int timeout, ExcuteScriptCommand excuteScriptCommand)
        {
            string scriptFilePath = Path.GetFullPath(Path.Combine(Config.Instance.TmpDir, TimeEx.GetTimestamp() + ".bat"));
            DirectoryInfoEx.CheckFilePathDirectory(scriptFilePath);
            File.WriteAllText(scriptFilePath, excuteScriptCommand.Content, Encoding.Default);
            try
            {
                return ProcessEx.SynExcuteCmd(scriptFilePath, null, timeout);
            }
            finally
            {
                try
                {
                    File.Delete(scriptFilePath);
                }
                catch
                { }
            }
        }
    }
}
