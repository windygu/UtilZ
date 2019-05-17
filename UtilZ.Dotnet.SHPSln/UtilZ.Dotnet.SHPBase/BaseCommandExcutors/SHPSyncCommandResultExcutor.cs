using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Commands;

namespace UtilZ.Dotnet.SHPBase.BaseCommandExcutors
{
    public class SHPSyncCommandResultExcutor : CommandExcutorBase<SHPSyncCommandResultCommand>
    {
        #region single instance
        private SHPSyncCommandResultExcutor()
            : base()
        {

        }

        private readonly static SHPSyncCommandResultExcutor _instance = new SHPSyncCommandResultExcutor();

        public static SHPSyncCommandResultExcutor Instance
        {
            get { return _instance; }
        }
        #endregion

        private readonly static List<int> _shpResultCommandList = new List<int>();

        public static void RegisterSHPSyncResponseCommand(IEnumerable<int> cmds)
        {
            if (cmds == null || cmds.Count() == 0)
            {
                return;
            }

            _shpResultCommandList.AddRange(cmds);
        }

        public static void RegisterSHPSyncResponseCommand(int cmd)
        {
            _shpResultCommandList.Add(cmd);
        }

        public static bool ContainsCmd(int cmd)
        {
            return _shpResultCommandList.Contains(cmd);
        }
    }
}
