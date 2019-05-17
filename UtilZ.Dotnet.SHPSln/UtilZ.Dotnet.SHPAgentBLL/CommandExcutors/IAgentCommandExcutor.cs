using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors
{
    public interface IAgentCommandExcutor : ICommandExcutor
    { 
        AgentBLL BLL { get; set; }
    }
}
