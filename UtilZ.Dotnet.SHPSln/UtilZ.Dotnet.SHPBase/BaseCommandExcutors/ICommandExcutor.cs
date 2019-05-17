using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;
using UtilZ.Dotnet.SHPBase.Net;

namespace UtilZ.Dotnet.SHPBase.BaseCommandExcutors
{
    public interface ICommandExcutor
    {
        CommandExcutorType ExcutorType { get; set; }

        ISHPNet Net { get; set; }

        void ProCommand(SHPTransferCommand transferCommand);
    }
}
