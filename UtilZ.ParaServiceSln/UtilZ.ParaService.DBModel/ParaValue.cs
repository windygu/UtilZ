namespace UtilZ.ParaService.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ParaValue")]
    public partial class ParaValue
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ParaID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProjectID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Version { get; set; }

        [Key]
        [Column(Order = 3)]
        public string Value { get; set; }

        //public virtual Para Para { get; set; }

        //public virtual Project Project { get; set; }
    }
}
