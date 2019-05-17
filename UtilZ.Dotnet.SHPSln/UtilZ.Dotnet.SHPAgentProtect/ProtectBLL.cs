using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UtilZ.Dotnet.Ex.Log;

////////////////////////////////////////////////////////////////////
//                          _ooOoo_                               //
//                         o8888888o                              //
//                         88" . "88                              //
//                         (| ^_^ |)                              //
//                         O\  =  /O                              //
//                      ____/`---'\____                           //
//                    .'  \\|     |//  `.                         //
//                   /  \\|||  :  |||//  \                        //
//                  /  _||||| -:- |||||-  \                       //
//                  |   | \\\  -  /// |   |                       //
//                  | \_|  ''\---/''  |   |                       //
//                  \  .-\__  `-`  ___/-. /                       //
//                ___`. .'  /--.--\  `. . ___                     //
//              ."" '<  `.___\_<|>_/___.'  >'"".                  //
//            | | :  `- \`.;`\ _ /`;.`/ - ` : | |                 //
//            \  \ `-.   \_ __\ /__ _/   .-` /  /                 //
//      ========`-.____`-.___\_____/___.-`____.-'========         //
//                           `=---='                              //
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //
//         佛祖保佑       永无BUG     永不修改                    //
////////////////////////////////////////////////////////////////////

namespace UtilZ.Dotnet.SHPAgentProtect
{
    internal class ProtectBLL : IDisposable
    {
        private readonly Process _currentProcess;
        private readonly string _proAppFilePath;
        private Process _agentPro = null;

        public ProtectBLL(Process agentProcess)
        {
            this._currentProcess = Process.GetCurrentProcess();
            this._proAppFilePath = agentProcess.MainModule.FileName;
            this.MonitorProcess(agentProcess);
            this._agentPro = agentProcess;
        }

        private void MonitorProcess(Process pro)
        {
            if (pro == null)
            {
                return;
            }

            pro.EnableRaisingEvents = true;
            pro.Exited += Pro_Exited;
        }

        private void UnListenProcess(Process pro)
        {
            if (pro == null)
            {
                return;
            }

            pro.EnableRaisingEvents = false;
            pro.Exited -= Pro_Exited;
        }

        private void Pro_Exited(object sender, EventArgs e)
        {
            this.StartAgent();
        }

        private void StartAgent()
        {
            while (true)
            {
                try
                {
                    if (!File.Exists(this._proAppFilePath))
                    {
                        return;
                    }

                    var startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.FileName = this._proAppFilePath;
                    startInfo.Arguments = this._currentProcess.Id.ToString();
                    startInfo.WorkingDirectory = Path.GetDirectoryName(this._proAppFilePath);
                    Process pro = Process.Start(startInfo);

                    this.MonitorProcess(pro);
                    this._agentPro = pro;
                    break;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                    Thread.Sleep(1000);
                }
            }
        }

        public void Dispose()
        {
            try
            {
                this.UnListenProcess(this._agentPro);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }
        }
    }
}
