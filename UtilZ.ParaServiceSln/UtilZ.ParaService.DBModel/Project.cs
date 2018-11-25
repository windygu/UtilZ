namespace UtilZ.ParaService.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Project")]
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            //Group = new HashSet<ParaGroup>();
            //Module = new HashSet<ProjectModule>();
            //Para = new HashSet<Para>();
            //ParaValue = new HashSet<ParaValue>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Alias { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Des { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ParaGroup> Group { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProjectModule> Module { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Para> Para { get; set; }

        //public virtual ParaVersion ParaVersion { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ParaValue> ParaValue { get; set; }
    }
}
