using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.SHPBase.Model
{
    public enum HostStatus
    {
        [DisplayNameEx("在线")]
        OnLine,

        [DisplayNameEx("离线")]
        OffLine
    }
}
