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
            IEnumerable<Person> persons
                = Enumerable.Range(0, 100)
                            .Select(index => new Person()
                            {
                                Age       = index,
                                FirstName = $"FirstName n°{index}",
                                LastName  = $"LastName n°{index}"
                            })
                            .ToList();

            using(var context = new SchoolDbContext())
            {
                // Delete
                context.Persons.Delete();

                // Bulk Insert
                context.Persons.BulkInsert(persons);

                // Bulk Merge
                var dbPersons = context.Persons.AsNoTracking()
                                               .Take(1000)
                                               .ToList();

                dbPersons.ForEach(p => {
                    p.Age += 10;
                });

                context.Persons.BulkMerge(dbPersons, p => new { p.FirstName, p.LastName });

                // Or Update
                context.Persons.Where(p => p.Id%2 == 0)
                               .SetValue(p => p.Age, 0)
                               .Update();
            }
        }
    }
}
