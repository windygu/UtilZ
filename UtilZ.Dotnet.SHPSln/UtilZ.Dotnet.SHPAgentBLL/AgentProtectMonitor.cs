using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Compress;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace UtilZ.Dotnet.SHPAgentBLL
{
    internal class AgentProtectMonitor
    {
        private readonly string _protectAppPath;
        private readonly int _agentProtectProcessId;
        private readonly Process _currentProcess;
        private Process _agentProtectPro = null;

        public AgentProtectMonitor(string protectAppPath, int agentProtectProcessId)
        {
            this._protectAppPath = protectAppPath;
            this._agentProtectProcessId = agentProtectProcessId;
            this._currentProcess = Process.GetCurrentProcess();
        }

        internal void Start()
        {
            Task.Factory.StartNew(this.PrimitiveStartAgentProtectApp);
        }

        private void PrimitiveStartAgentProtectApp()
        {
            try
            {
                try
                {
                    this._agentProtectPro = Process.GetProcessById(this._agentProtectProcessId);
                    this.MonitorProcess(this._agentProtectPro);
                }
                catch
                {
                    this._agentProtectPro = ProcessEx.FindProcessByFilePath(this._protectAppPath).FirstOrDefault();
                    if (this._agentProtectPro == null)
                    {
                        this.AgentProtectStart(this._protectAppPath);
                    }
                    else
                    {
                        this.MonitorProcess(this._agentProtectPro);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, $"启动[{this._protectAppPath}]程序异常");
            }
        }

        private void AgentProtectStart(string agentProtectExeFilePath)
        {
            if (File.Exists(agentProtectExeFilePath))
            {
                //var agentVer = this._currentProcess.MainModule.FileVersionInfo;
                var agentVer = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                var agentProtectVer = FileVersionInfo.GetVersionInfo(agentProtectExeFilePath);
                if (agentProtectVer.ProductMajorPart < agentVer.ProductMajorPart ||
                    agentProtectVer.ProductMinorPart < agentVer.ProductMinorPart ||
                    agentProtectVer.ProductBuildPart < agentVer.ProductBuildPart ||
                    agentProtectVer.ProductPrivatePart < agentVer.ProductPrivatePart)
                {
                    if (!this.UnCompressAgentProtectApp(agentProtectExeFilePath))
                    {
                        Loger.Warn("解压保护程序失败,使用旧版本保护程序");
                        //Loger.Warn("解压保护程序失败,启动保护程序失败");
                        //return;
                    }
                }
            }
            else
            {
                if (this.UnCompressAgentProtectApp(agentProtectExeFilePath))
                {
                    if (!File.Exists(agentProtectExeFilePath))
                    {
                        Loger.Error($"[{agentProtectExeFilePath}]不存在");
                        return;
                    }
                }
                else
                {
                    Loger.Warn("解压保护程序失败,启动保护程序失败");
                    return;
                }
            }

            this.StartAgentProtect(agentProtectExeFilePath);
        }

        private void StartAgentProtect(string agentProtectExeFilePath)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = agentProtectExeFilePath;
            startInfo.Arguments = $"{ this._currentProcess.Id}";
            startInfo.WorkingDirectory = Path.GetDirectoryName(agentProtectExeFilePath);
            startInfo.Verb = "runas";
            this._agentProtectPro = Process.Start(startInfo);
            this.MonitorProcess(this._agentProtectPro);
        }

        private bool UnCompressAgentProtectApp(string agentProtectExeFilePath)
        {
            try
            {
                var fileInfo = new FileInfo(agentProtectExeFilePath);
                const string agentProtectAppZipFileName = "AgentProtect.zip";
                string agentProtectAppZipFilePath = Path.Combine(fileInfo.Directory.Parent.FullName, agentProtectAppZipFileName);

                if (File.Exists(agentProtectAppZipFilePath))
                {
                    CompressHelper.DeCompressZip(agentProtectAppZipFilePath, fileInfo.Directory.FullName);
                    return true;
                }
                else
                {
                    Loger.Error($"保护程序包[{agentProtectAppZipFileName}]不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "解压保护程序异常");
                return false;
            }
        }

        private void AgentProtectPro_Exited(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var agentProtectPro = (Process)sender;
                    if (agentProtectPro != null)
                    {
                        agentProtectPro.Exited -= this.AgentProtectPro_Exited;
                    }
                }
                finally
                {
                    this.StartAgentProtect(this._protectAppPath);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void PrimitiveKillAgentProtectPro(Process pro)
        {
            this.UnListenProcess(pro);
            if (pro.HasExited)
            {
                return;
            }

            pro.Kill();
        }

        private void MonitorProcess(Process pro)
        {
            if (pro == null)
            {
                return;
            }

            pro.EnableRaisingEvents = true;
            pro.Exited += AgentProtectPro_Exited;
        }

        private void UnListenProcess(Process pro)
        {
            if (pro == null)
            {
                return;
            }

            pro.EnableRaisingEvents = false;
            pro.Exited -= AgentProtectPro_Exited;
        }

        public void Stop()
        {
            try
            {
                if (this._agentProtectPro != null)
                {
                    this.PrimitiveKillAgentProtectPro(this._agentProtectPro);
                }
                else
                {
                    Process[] pros = Process.GetProcesses();
                    foreach (var pro in pros)
                    {
                        try
                        {
                            if (string.Equals(this._protectAppPath, pro.MainModule.FileName, StringComparison.OrdinalIgnoreCase))
                            {
                                this.PrimitiveKillAgentProtectPro(pro);
                            }
                        }
                        catch
                        { }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "Dispose异常");
            }

            this._agentProtectPro = null;
        }
    }
}
