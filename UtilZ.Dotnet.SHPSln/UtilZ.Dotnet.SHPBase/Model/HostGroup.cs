using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Model
{
    [Serializable]
    public class HostGroup
    {
        public long Id { get; set; }

        public long ParentId { get; set; }

        public string Name { get; set; }

        public string Des { get; set; }

        public HostGroup()
        {

        }

        public void Update(HostGroup hostGroup)
        {
            this.ParentId = hostGroup.ParentId;
            this.Name = hostGroup.Name;
            this.Des = hostGroup.Des;
        }
    }
}
