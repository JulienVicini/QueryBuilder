using System.Collections.Generic;

namespace QueryBuilder.Samples.Entities
{
    public class Person
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Address> Addresses { get; set; }

        public List<Course> AttendedCourses { get; set; }

        public List<Course> GivenClasses { get; set; }
    }
}
