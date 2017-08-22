using System.Collections.Generic;

namespace QueryBuilder.Samples.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public List<Person> Students { get; set; }

        public int TeacherId { get; set; }
        public Person Teacher { get; set; }
    }
}