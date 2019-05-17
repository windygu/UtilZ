using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Compress;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.SHPAutoPatchBase;

namespace UtilZ.Dotnet.SHPAutoPatch
{
    internal class UpgradeHelper
    {
        private readonly StringBuilder _sbLog = new StringBuilder();

        public void ExcuteUpgrade(string[] args)
        {
            try
            {
                this._sbLog.AppendLine("升级参数:");
                this._sbLog.Append(string.Join(",", args));
                var option = this.ParseArgs(args);

                if (option.Validate(this._sbLog))
                {
                    this.Upgrade(option);
                    this._sbLog.AppendLine("升级成功");
                }
            }
            catch (Exception ex)
            {
                this._sbLog.AppendLine($"升级异常,{ex.ToString()}");
            }
        }

        public void WriteUpgradeLog()
        {
            try
            {
                var upgradeLogFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Upgrade.log");
                File.WriteAllText(upgradeLogFilePath, this._sbLog.ToString());
            }
            catch
            { }
        }

        private void Upgrade(UpgradeArgsOptions option)
        {
            //杀死发起升级的进程
            ProcessEx.KillProcessTreeById(option.ProId);

            //解压
            UnCompress(option);

            //启动升级程序
            StartApp(option.AppExeFilePath);
        }

        private void StartApp(string appFilePath)
        {
            if (string.IsNullOrWhiteSpace(appFilePath) || !File.Exists(appFilePath))
            {
                this._sbLog.AppendLine($"启动程序文件选项[{appFilePath}]为空或不存在");
                return;
            }

            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = appFilePath;
            startInfo.Arguments = string.Empty;
            var agentProtectPro = Process.Start(startInfo);
        }

        private void UnCompress(UpgradeArgsOptions option)
        {
            switch (option.UpgradePackgeType)
            {
                case UpgradePackgeTypes.ZIP:
                    CompressHelper.DeCompressZip(option.UpgradePackgeFilePath, option.DstDir);
                    break;
                case UpgradePackgeTypes.RAR:
                    CompressHelper.DecompressRar(option.UpgradePackgeFilePath, option.DstDir, true);
                    break;
                default:
                    throw new NotSupportedException($"不支持的升级包格式[{option.UpgradePackgeType}]");
            }
        }

        private UpgradeArgsOptions ParseArgs(string[] args)
        {
            var option = new UpgradeArgsOptions();
            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 1 >= args.Length)
                {
                    break;
                }

                var optionName = args[i].Trim().Replace('-', '/').Replace('\\', '/').ToLower();
                switch (optionName)
                {
                    case AutoPatchOptions.PROCESS_ID://升级程序主进程ID
                        option.ProId = int.Parse(args[i + 1].Trim());
                        break;
                    case AutoPatchOptions.UPGRADE_PACKGE_FILE_PATH://升级包路径
                        option.UpgradePackgeFilePath = args[i + 1].Trim();
                        break;
                    case AutoPatchOptions.UPGRADE_PACKGE_TYPE://升级包类型
                        option.UpgradePackgeType = int.Parse(args[i + 1].Trim());
                        break;
                    case AutoPatchOptions.DIRECTORY://解压目录
                        option.DstDir = args[i + 1].Trim();
                        break;
                    case AutoPatchOptions.APP_EXE_FILE_PATH://目标程序路径
                        option.AppExeFilePath = args[i + 1].Trim();
                        break;
                    default:
                        this._sbLog.AppendLine($"未知参数[{args[i]},{args[i + 1]}]");
                        break;
                }
            }

            return option;
        }
    }
}
