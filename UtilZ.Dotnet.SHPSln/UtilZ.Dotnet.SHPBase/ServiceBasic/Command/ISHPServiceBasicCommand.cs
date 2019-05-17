using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public interface ISHPServiceBasicCommand
    {
        byte Cmd { get; }

        byte[] ToBytes();

        void Parse(byte[] buffer);
    }
}
