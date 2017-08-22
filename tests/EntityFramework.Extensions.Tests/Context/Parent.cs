using System.Collections.Generic;

namespace EntityFramework.Extensions.Tests.Context
{
    public class Parent
    {
        public int Id { get; set; }

        public string FirstVariable { get; set; }

        public double SecondVariable { get; set; }

        public ICollection<Children> Childrens { get; set; }
    }
}
