using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QueryBuilder.Samples.EFCore.Entities;
using QueryBuilder.Samples.EFCore.Helpers;

namespace QueryBuilder.Samples.EFCore
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        static SampleDbContext()
        {
            // TODO Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleDbContext>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(ConfigurationManager.SampleDbContextConnectionString, providerOptions => providerOptions.CommandTimeout(60))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurePersonEntity(modelBuilder.Entity<Person>());

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigurePersonEntity(EntityTypeBuilder<Person> personEntity)
        {
            personEntity.ToTable("user", "table");
            personEntity.HasKey(p => p.Id);
            personEntity.Property(p => p.Id)
                .ValueGeneratedOnAdd()
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
