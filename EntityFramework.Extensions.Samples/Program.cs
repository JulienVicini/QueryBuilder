﻿using EntityFramework.Extensions.Samples.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EntityFramework.Extensions.Samples
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
                context.Persons.BulkInsert(persons);

                context.Persons.Where(p => p.Id > 1000)
                               .Delete();


                //context.Persons.SetValue(p => p.Id  , 3            )
                //               .SetValue(p => p.Name, p => p.Id + 3)
                //               .Update();
                //context.Persons.Where(c => c.Id > 10000)
                //               .Delete();

                //context.Persons.BulkMerge(persons, d => new { d.Age, d.FirstName, d.LastName });
            }
        }
    }
}
