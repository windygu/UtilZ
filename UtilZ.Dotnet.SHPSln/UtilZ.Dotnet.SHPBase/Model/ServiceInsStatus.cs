using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.SHPBase.Model
{
    public enum ServiceInsStatus : byte
    {
        [DisplayNameEx("在线")]
        OnLine,

        [DisplayNameEx("离线")]
        OffLine,

        [DisplayNameEx("删除")]
        Delete
    }
}
