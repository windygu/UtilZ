using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.BaseCommandExcutors;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPAgentBLL.CommandExcutors
{
    public abstract class AgentCommandExcutorBase<T> : CommandExcutorBase<T>, IAgentCommandExcutor
        where T : ICommand, new()
    {
        public AgentCommandExcutorBase()
            : base()
        {

        }

        public AgentBLL BLL { get; set; }
    }
}
