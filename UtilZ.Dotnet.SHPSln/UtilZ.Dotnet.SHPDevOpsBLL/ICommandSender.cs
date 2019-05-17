using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPDevOpsBLL
{
    public interface ICommandSender
    {
        byte[] SendInteractiveCommandHost<T>(T command, string ip, int millisecondsTimeout) where T : ICommand;


        void SendCommandHost<T>(T command, string ip) where T : ICommand;
    }
}
