### Query Builder

	The main purpose of this project is to generate queries using Expression Trees and Entity Framework mappings.
	It provides a set of extension methods on both DbSet<> and IQueryable<> types.


### Samples

```C#
using QueryBuilder.EntityFramework.SqlServer;
```

**Bulk Insert**
```C#
DbSet<Person> persons = dbContext.Persons;
dbSet.BulkInsert(newPersons);
```

**Bulk Merge**
```C#
DbSet<Person> dbSet = dbContext.Persons;
dbSet.BulkMerge(
	updatedPersons,
	person => new { person.FirstName, person.LastName } // Bulk merge performed using a composite key
);
```

**Delete**
```C#
DbSet<Person> dbSet = dbContext.Persons;
dbSet.Where(person => person.Age < 18)
     .Delete();
```

**Update**
```C#
DbSet<Person> dbSet = dbContext.Persons;
dbSet.Persons.Where(p => p.Id % 2 == 0)
             .SetValue(p => p.Age      , 0     )
             .SetValue(p => p.FirstName, "Even")
             .Update();
```