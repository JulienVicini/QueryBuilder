namespace QueryBuilder.Samples.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public string City { get; set; }

        public int Number { get; set; }

        public string Street { get; set; }

        public int ZipCode { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}