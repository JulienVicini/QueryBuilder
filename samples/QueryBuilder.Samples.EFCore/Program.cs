using Microsoft.EntityFrameworkCore;
using QueryBuilder.Samples.EFCore.Entities;
using QueryBuilder.EFCore.SqlServer;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Samples.EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new SampleDbContext())
            {
                dbContext.Database.EnsureCreated();

                // Delete
                dbContext.Persons.Delete();

                // Bulk Insert
                IEnumerable<Person> newPersons = CreatePersons(count: 100);
                dbContext.Persons.BulkInsert(newPersons);

                // Bulk Merge
                IEnumerable<Person> dbPersons = dbContext.Persons
                                                         .AsNoTracking()
                                                         .Take(1000)
                                                         .ToList();

                foreach (Person person in dbPersons)
                    person.Age += 10;

                dbContext.Persons.BulkMerge(dbPersons, p => new { p.FirstName, p.LastName });

                // Update
                dbContext.Persons.Where(p => p.Id % 2 == 0)
                                 .Update(p => new Person()
                                 {
                                     Age = p.Id / p.Age
                                 });
                // OR
                dbContext.Persons.SetValue(p => p.Age, p => p.Age + 1)
                                 .Update();

                // Delete with predicate
                dbContext.Persons.Where(p => p.Age > 50)
                                 .Delete();
            }
        }

        protected static IEnumerable<Person> CreatePersons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Person()
                {
                    Age = i,
                    FirstName = $"FirstName n°{i}",
                    LastName = $"LastName n°{i}"
                };
            }
        }
    }
}
