using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.Model
{
    public class ModuleParaGet
    {
        public List<ParaGroup> Groups { get; set; }

        public List<Para> AllParas { get; set; }

        public List<ModulePara> ModuleParas { get; set; }

        public ModuleParaGet()
        {

        }
    }
}
