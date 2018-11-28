namespace UtilZ.ParaService.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProjectModule")]
    public partial class ProjectModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectModule()
        {
            //Module1 = new HashSet<ProjectModule>();
            //Para = new HashSet<Para>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long ProjectID { get; set; }

        [Required]
        [StringLength(20)]
        public string Alias { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public long? ParentID { get; set; }

        [StringLength(50)]
        public string Des { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProjectModule> Module1 { get; set; }

        //public virtual ProjectModule Parent { get; set; }

        //public virtual Project Project { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Para> Para { get; set; }
    }
}
