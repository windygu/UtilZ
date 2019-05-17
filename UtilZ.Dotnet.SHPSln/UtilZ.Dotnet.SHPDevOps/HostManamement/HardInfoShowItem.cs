using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    internal class HardInfoShowItem
    {
        public string HardName { get; private set; }

        public Control Control { get; private set; }

        public HardInfoShowItem(string hardName, Control control)
        {
            this.HardName = hardName;
            this.Control = control;
        }

        public override string ToString()
        {
            return HardName;
        }
    }
}
