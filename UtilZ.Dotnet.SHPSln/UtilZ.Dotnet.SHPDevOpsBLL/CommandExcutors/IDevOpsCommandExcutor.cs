using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;

namespace UtilZ.Dotnet.SHPDevOpsBLL.CommandExcutors
{
    public interface IDevOpsCommandExcutor : ICommandExcutor
    {
        DevOpsBLL BLL { get; set; }
    }
}
