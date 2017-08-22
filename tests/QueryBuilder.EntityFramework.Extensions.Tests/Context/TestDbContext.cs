using System.Data.Entity;

namespace QueryBuilder.EntityFramework.Extensions.Tests.Context
{
    public class TestDbContext : DbContext
    {
        public DbSet<Children> Childrens { get; set; }

        public DbSet<Parent> Parents { get; set; }

        static TestDbContext()
        {
            Database.SetInitializer<TestDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Parent Configuration
            var parentConfig = modelBuilder.Entity<Parent>();

            parentConfig.ToTable("abc", "Prefix_Parent");

            parentConfig.HasKey(p => p.Id);
            parentConfig.Property(p => p.Id)
                .HasColumnName("P_Id");

            parentConfig.Property(p => p.FirstVariable)
                .HasColumnName("P_First");

            parentConfig.Property(p => p.SecondVariable)
                .HasColumnName("P_Second");

            // Child Configuration
            var childConfig = modelBuilder.Entity<Children>();

            childConfig.HasKey(c => c.Id);
            childConfig.Property(c => c.Id)
                .HasColumnName("C_Id");

            childConfig.Property(c => c.FirstVariable)
                .HasColumnName("C_First");

            childConfig.Property(c => c.ParentId)
                .HasColumnName("P_ParentId");

            childConfig.Property(c => c.SecondVariable)
                .HasColumnName("Second");

            childConfig.Property(c => c.ThirdVariable);

            childConfig.HasRequired(c => c.Parent)
                       .WithMany(p => p.Childrens)
                       .HasForeignKey(c => c.ParentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
