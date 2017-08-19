using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Queries.Statements.Values
{
    public class RangeConstantStatement<T>
        : IValueStatement
    {
        public IEnumerable<T> Values { get; private set; }

        public RangeConstantStatement(IEnumerable<T> values)
        {
            Values = values;
        }
    }
}
