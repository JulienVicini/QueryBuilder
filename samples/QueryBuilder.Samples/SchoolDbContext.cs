using QueryBuilder.Samples.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace QueryBuilder.Samples
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        static SchoolDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SchoolDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigurePersonEntity(modelBuilder.Entity<Person>());

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigurePersonEntity(EntityTypeConfiguration<Person> personEntity)
        {
            personEntity.ToTable("user", "table");
            personEntity.HasKey(p => p.Id);
            personEntity.Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            personEntity.Property(p => p.FirstName)
                .HasColumnName("First_Name")
                .HasMaxLength(50)
                .IsRequired();

            personEntity.Property(p => p.LastName)
                .HasColumnName("Last_Name")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
