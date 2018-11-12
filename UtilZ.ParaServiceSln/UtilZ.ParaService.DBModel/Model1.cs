namespace UtilZ.ParaService.DBModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=ParaDBModole")
        {
        }

        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<Para> Para { get; set; }
        public virtual DbSet<ParaVersion> ParaVersion { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<ParaValue> ParaValue { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>()
                .Property(e => e.Alias)
                .IsFixedLength();

            modelBuilder.Entity<Module>()
                .HasMany(e => e.Module1)
                .WithOptional(e => e.Module2)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.Para)
                .WithMany(e => e.Module)
                .Map(m => m.ToTable("ModulePara").MapLeftKey("ModuleID").MapRightKey("ParaID"));

            modelBuilder.Entity<Para>()
                .Property(e => e.Key)
                .IsFixedLength();

            modelBuilder.Entity<Para>()
                .HasMany(e => e.ParaValue)
                .WithRequired(e => e.Para)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParaVersion>()
                .HasOptional(e => e.Project)
                .WithRequired(e => e.ParaVersion);

            modelBuilder.Entity<Project>()
                .Property(e => e.Alias)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Group)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Module)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Para)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.ParaValue)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);
        }
    }
}
