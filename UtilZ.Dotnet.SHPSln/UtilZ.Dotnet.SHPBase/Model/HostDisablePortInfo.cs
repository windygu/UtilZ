using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.SHPBase.Base;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostDisablePortInfo : SHPBaseModel
    {
        [DisplayName("端口")]
        public int Port { get; set; }

        [DisplayName("描述")]
        public string Des { get; set; }

        public HostDisablePortInfo()
        {

        }
    }
}
