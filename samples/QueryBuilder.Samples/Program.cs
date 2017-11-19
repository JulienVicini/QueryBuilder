using QueryBuilder.EntityFramework.SqlServer;
using QueryBuilder.Samples.Entities;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var dbContext = new SampleDbContext())
            {
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

                foreach(Person person in dbPersons)
                    person.Age += 10;

                dbContext.Persons.BulkMerge(dbPersons, p => new { p.FirstName, p.LastName });

                // Update
                dbContext.Persons.Where(p => p.Id % 2 == 0)
                                 .SetValue(p => p.Age, 0)
                                 // TODO Does not work so far .SetValue(p => p.Age, p => p.Age / p.Id)
                                 .Update();

                // Delete with predicate
                dbContext.Persons.Where(p => p.Age == 0)
                                 .Delete();
            }
        }

        protected static IEnumerable<Person> CreatePersons(int count)
        {
            for(int i =0; i < count; i++)
            {
                yield return new Person() {
                    Age       = i,
                    FirstName = $"FirstName n°{i}",
                    LastName  = $"LastName n°{i}"
                };
            }
        }
    }
}
