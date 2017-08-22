using System;

namespace QueryBuilder.EntityFramework.Extensions.Tests.Context
{
    public class Children
    {
        public int Id { get; set; }

        public DateTime FirstVariable { get; set; }

        public int SecondVariable { get; set; }

        public string ThirdVariable { get; set; }

        public int ParentId { get; set; }

        public Parent Parent { get; set; }
    }
}