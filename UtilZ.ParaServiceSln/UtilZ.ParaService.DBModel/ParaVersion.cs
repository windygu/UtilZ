namespace UtilZ.ParaService.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ParaVersion")]
    public partial class ParaVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProjectID { get; set; }

        public long Version { get; set; }

        public virtual Project Project { get; set; }
    }
}
