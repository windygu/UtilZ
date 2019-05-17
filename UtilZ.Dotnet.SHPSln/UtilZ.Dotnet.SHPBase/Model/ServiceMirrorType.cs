using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.SHPBase.Model
{
    public enum ServiceMirrorType : byte
    {
        [DisplayNameEx("Zip")]
        Zip,

        [DisplayNameEx("Rar")]
        Rar
    }
}
