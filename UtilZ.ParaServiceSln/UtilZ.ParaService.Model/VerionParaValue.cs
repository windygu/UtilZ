using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.Model
{
    public class VerionParaValue
    {
        public long Version { get; set; }

        public List<VerionParaValueItem> Items { get; set; } = new List<VerionParaValueItem>();

        public List<ParaGroup> ParaGroups { get; set; } = new List<ParaGroup>();

        public VerionParaValue()
        {

        }
    }
}
