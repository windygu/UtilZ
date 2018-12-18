using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UtilZ.ParaService.DBModel
{
    [Table("ModulePara")]
    public partial class ModulePara
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModulePara()
        {

        }

        public long ModuleID { get; set; }

        public long ParaID { get; set; }
    }
}
