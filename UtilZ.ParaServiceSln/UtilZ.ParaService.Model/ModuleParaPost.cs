using System;
using System.Collections.Generic;
using System.Text;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.Model
{
    public class ModuleParaPost
    {
        public long ModuleId { get; set; }

        public List<long> ParaIds { get; set; } = new List<long>();

        public ModuleParaPost()
        {

        }

        //public List<ModulePara> ToModulePara()
        //{
        //    var moduleParas = new List<ModulePara>();
        //    foreach (var paraId in this.ParaIds)
        //    {
        //        moduleParas.Add(new ModulePara() { ModuleID = this.ModuleId, ParaID = paraId });
        //    }

        //    return moduleParas;
        //}
    }
}
