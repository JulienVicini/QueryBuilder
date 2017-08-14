using EntityFramework.Extensions.Samples.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFramework.Extensions.Samples
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureAddressEntity(modelBuilder.Entity<Address>());
            ConfigureCourseEntity(modelBuilder.Entity<Course>());
            ConfigurePersonEntity(modelBuilder.Entity<Person>());

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureAddressEntity(EntityTypeConfiguration<Address> addressEntity)
        {
            addressEntity.HasKey(a => a.Id);
            addressEntity.Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            addressEntity.Property(a => a.City).HasMaxLength(50).IsRequired();
            addressEntity.Property(a => a.Street).HasMaxLength(50).IsRequired();

            addressEntity.HasRequired(addr => addr.Person)
                .WithMany(person => person.Addresses)
                .HasForeignKey(addr => addr.PersonId);
        }

        private void ConfigureCourseEntity(EntityTypeConfiguration<Course> courseEntity)
        {
            courseEntity.HasKey(c => c.Id);
            courseEntity.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            courseEntity.Property(c => c.Topic)
                .HasMaxLength(200)
                .IsRequired();

            courseEntity.HasRequired(c => c.Teacher)
                .WithMany(person => person.GivenClasses)
                .HasForeignKey(c => c.TeacherId);

            courseEntity.HasMany(c => c.Students)
                .WithMany(person => person.AttendedCourses)
                .Map(m =>
                {
                    m.ToTable("PersonsCourses");
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("CourseId");
                });
        }

        private void ConfigurePersonEntity(EntityTypeConfiguration<Person> personEntity)
        {
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
