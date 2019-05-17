using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPDevOpsBLL.CommandExcutors
{
    public abstract class DevOpsAsynCommandExcutorBase<T> : CommandExcutorBase<T>, IDevOpsCommandExcutor
          where T : ICommand, new()
    {
        public DevOpsAsynCommandExcutorBase()
            : base()
        {

        }

        public DevOpsBLL BLL { get; set; }
    }
}
