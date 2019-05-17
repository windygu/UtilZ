using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.ServiceBasic.Command
{
    public abstract class SHPServiceBasicCommandBase : ISHPServiceBasicCommand
    {
        public SHPServiceBasicCommandBase()
        {

        }        

        protected byte _cmd = 0;
        public byte Cmd
        {
            get { return _cmd; }
        }

        public abstract byte[] ToBytes();

        public abstract void Parse(byte[] buffer);
    }
}
