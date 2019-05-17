using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPAutoPatchBase;

namespace UtilZ.Dotnet.SHPAutoPatch
{
    internal class UpgradeArgsOptions
    {
        public int ProId { get; set; } = 0;

        public string UpgradePackgeFilePath { get; set; } = null;

        public int UpgradePackgeType { get; set; } = UpgradePackgeTypes.ZIP;

        public string DstDir { get; set; } = null;

        public string AppExeFilePath { get; set; } = null;

        public UpgradeArgsOptions()
        {

        }

        public bool Validate(StringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(UpgradePackgeFilePath) || string.IsNullOrWhiteSpace(DstDir))
            {
                sb.AppendLine("升级参数缺失,/u和/d参数必选项");
                return false;
            }

            return true;
        }
    }
}
